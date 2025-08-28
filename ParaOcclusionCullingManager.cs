using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

// Token: 0x0200002E RID: 46
public class ParaOcclusionCullingManager : Singleton<ParaOcclusionCullingManager>
{
	// Token: 0x06000069 RID: 105 RVA: 0x000043F0 File Offset: 0x000025F0
	public ParaOcclusionCullingManager()
	{
		this.renderers = new WeakReference<Renderer>[65535];
	}

	// Token: 0x0600006A RID: 106 RVA: 0x00004454 File Offset: 0x00002654
	public void Enable()
	{
		if (this.allocated)
		{
			return;
		}
		this.curResults = new NativeArray<byte>(65535, 4, 1);
		this.prevResults = new NativeArray<byte>(65535, 4, 1);
		this.meshBounds = new NativeArray<float>(393210, 4, 1);
		this.sharedMeshBounds = new NativeArray<float>(393210, 4, 1);
		this.meshCount = 0U;
		this.allocated = true;
	}

	// Token: 0x0600006B RID: 107 RVA: 0x000044C0 File Offset: 0x000026C0
	public void Disable()
	{
		this.renderers = new WeakReference<Renderer>[65535];
		if (!this.allocated)
		{
			return;
		}
		this.curResults.Dispose();
		this.prevResults.Dispose();
		this.meshBounds.Dispose();
		this.sharedMeshBounds.Dispose();
		this.allocated = false;
	}

	// Token: 0x0600006C RID: 108 RVA: 0x00004519 File Offset: 0x00002719
	public void Reset()
	{
		this.freeSet.Clear();
		this.freeStack.Clear();
		this.renderers = new WeakReference<Renderer>[65535];
		this.meshCount = 0U;
	}

	// Token: 0x0600006D RID: 109 RVA: 0x00004548 File Offset: 0x00002748
	public void Dispose()
	{
		this.ForceVisible();
		this.renderers = new WeakReference<Renderer>[65535];
		this.curResults.Dispose();
		this.prevResults.Dispose();
		this.meshBounds.Dispose();
		this.sharedMeshBounds.Dispose();
	}

	// Token: 0x0600006E RID: 110 RVA: 0x00004598 File Offset: 0x00002798
	public void Swap()
	{
		NativeArray<byte> nativeArray = this.prevResults;
		NativeArray<byte> nativeArray2 = this.curResults;
		this.curResults = nativeArray;
		this.prevResults = nativeArray2;
	}

	// Token: 0x0600006F RID: 111 RVA: 0x000045C4 File Offset: 0x000027C4
	public int AddObject(Renderer renderer, bool staticOccluder = false)
	{
		if (!this.allocated)
		{
			this.Enable();
		}
		if (renderer == null)
		{
			return -1;
		}
		if (this.freeStack.Count <= 0 && this.meshCount >= 65535U)
		{
			return -1;
		}
		int num;
		if (this.freeStack.Count > 0)
		{
			num = this.freeStack.Pop();
			this.freeSet.Remove(num);
		}
		else
		{
			uint num2 = this.meshCount;
			this.meshCount = num2 + 1U;
			num = (int)num2;
		}
		this.renderers[num] = new WeakReference<Renderer>(renderer);
		this.staticOccluders[num] = (staticOccluder ? 1 : 0);
		Bounds bounds = renderer.bounds;
		int num3 = num * 6;
		this.meshBounds[num3] = bounds.min.x;
		this.meshBounds[num3 + 1] = bounds.min.y;
		this.meshBounds[num3 + 2] = bounds.min.z;
		this.meshBounds[num3 + 3] = bounds.max.x;
		this.meshBounds[num3 + 4] = bounds.max.y;
		this.meshBounds[num3 + 5] = bounds.max.z;
		return num;
	}

	// Token: 0x06000070 RID: 112 RVA: 0x00004708 File Offset: 0x00002908
	public void UpdateVisibility()
	{
		if (!this.enable)
		{
			this.ForceVisible();
			return;
		}
		this.culledCount = 0;
		this.flipCount = 0;
		int num = 0;
		while ((long)num < (long)((ulong)this.meshCount))
		{
			if (!this.freeSet.Contains(num))
			{
				Renderer renderer;
				if (this.renderers[num].TryGetTarget(out renderer))
				{
					if (renderer != null)
					{
						bool flag = this.staticOccluders[num] == 1;
						this.culledCount += ((this.curResults[num] == 1) ? 1 : 0);
						if (this.prevResults[num] != this.curResults[num])
						{
							this.updateRendererVisibility(renderer, this.curResults[num] == 0, flag);
							this.flipCount++;
						}
						if (!flag)
						{
							this.updateBounds(num, renderer.bounds);
						}
					}
					else
					{
						this.free(num);
					}
				}
				else
				{
					this.free(num);
				}
			}
			num++;
		}
		NativeArray<float>.Copy(this.meshBounds, this.sharedMeshBounds);
		if (this.ShowStatistic && this.meshCount != 0U)
		{
			this.UpdateStatisticInfo();
		}
	}

	// Token: 0x06000071 RID: 113 RVA: 0x00004830 File Offset: 0x00002A30
	private void UpdateStatisticInfo()
	{
		this.StatisticInfo = string.Format("Enabled: {0}\nCulledCount: {1}/{2} {3}%\nFlipCount: {4}", new object[]
		{
			this.enable,
			this.culledCount,
			this.meshCount,
			((float)this.culledCount * 100f / this.meshCount).ToString("F"),
			this.flipCount
		});
	}

	// Token: 0x06000072 RID: 114 RVA: 0x000048B4 File Offset: 0x00002AB4
	public void ForceVisible()
	{
		int num = 0;
		while ((long)num < (long)((ulong)this.meshCount))
		{
			if (!this.freeSet.Contains(num))
			{
				WeakReference<Renderer> weakReference = this.renderers[num];
				bool flag = this.staticOccluders[num] == 1;
				Renderer renderer;
				if (weakReference.TryGetTarget(out renderer))
				{
					this.updateRendererVisibility(renderer, true, flag);
				}
				else
				{
					this.free(num);
				}
				this.prevResults[num] = 2;
			}
			num++;
		}
		this.flipCount = 0;
		this.culledCount = 0;
		this.UpdateStatisticInfo();
	}

	// Token: 0x06000073 RID: 115 RVA: 0x00004932 File Offset: 0x00002B32
	public void free(int id)
	{
		this.freeStack.Push(id);
		this.freeSet.Add(id);
	}

	// Token: 0x06000074 RID: 116 RVA: 0x00004950 File Offset: 0x00002B50
	private void updateRendererVisibility(Renderer renderer, bool value, bool isStatic)
	{
		if (renderer == null)
		{
			return;
		}
		Material[] materials = renderer.materials;
		for (int i = 0; i < materials.Length; i++)
		{
			materials[i].SetShaderPassEnabled("UniversalForward", value);
		}
	}

	// Token: 0x06000075 RID: 117 RVA: 0x0000498C File Offset: 0x00002B8C
	private void updateBounds(int id, Bounds bounds)
	{
		int num = id * 6;
		this.meshBounds[num] = bounds.min.x;
		this.meshBounds[num + 1] = bounds.min.y;
		this.meshBounds[num + 2] = bounds.min.z;
		this.meshBounds[num + 3] = bounds.max.x;
		this.meshBounds[num + 4] = bounds.max.y;
		this.meshBounds[num + 5] = bounds.max.z;
	}

	// Token: 0x04000113 RID: 275
	public bool ShowStatistic = true;

	// Token: 0x04000114 RID: 276
	public string StatisticInfo = "";

	// Token: 0x04000115 RID: 277
	public bool enable = true;

	// Token: 0x04000116 RID: 278
	public bool enableDepthMap;

	// Token: 0x04000117 RID: 279
	public const int MAX_OCCLUDEE_COUNT = 65535;

	// Token: 0x04000118 RID: 280
	public uint meshCount;

	// Token: 0x04000119 RID: 281
	public NativeArray<byte> curResults;

	// Token: 0x0400011A RID: 282
	public NativeArray<byte> prevResults;

	// Token: 0x0400011B RID: 283
	public NativeArray<float> meshBounds;

	// Token: 0x0400011C RID: 284
	public NativeArray<float> sharedMeshBounds;

	// Token: 0x0400011D RID: 285
	private byte[] staticOccluders = new byte[65535];

	// Token: 0x0400011E RID: 286
	private WeakReference<Renderer>[] renderers;

	// Token: 0x0400011F RID: 287
	private Stack<int> freeStack = new Stack<int>();

	// Token: 0x04000120 RID: 288
	private HashSet<int> freeSet = new HashSet<int>();

	// Token: 0x04000121 RID: 289
	public Camera camera;

	// Token: 0x04000122 RID: 290
	private int culledCount;

	// Token: 0x04000123 RID: 291
	private int flipCount;

	// Token: 0x04000124 RID: 292
	private bool allocated;
}
