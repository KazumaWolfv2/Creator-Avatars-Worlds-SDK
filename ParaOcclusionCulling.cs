using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Jobs;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x0200002C RID: 44
[AddComponentMenu("ParaSpace/Para Occlusion Culling")]
[Serializable]
public class ParaOcclusionCulling : MonoBehaviour
{
	// Token: 0x17000007 RID: 7
	// (get) Token: 0x0600004E RID: 78 RVA: 0x0000383B File Offset: 0x00001A3B
	// (set) Token: 0x0600004F RID: 79 RVA: 0x00003843 File Offset: 0x00001A43
	public bool EnabledDepthMap
	{
		get
		{
			return this._enabledDepthMap;
		}
		set
		{
			this._enabledDepthMap = value;
			if (this.depthMap == null && this._enabledDepthMap)
			{
				this.createDepthMap();
			}
		}
	}

	// Token: 0x06000050 RID: 80
	[DllImport("libSDOC.qti.dll")]
	private static extern bool sdocInit(uint width, uint height, float near);

	// Token: 0x06000051 RID: 81
	[DllImport("libSDOC.qti.dll")]
	private static extern bool sdocSync(uint id, byte[] bytes);

	// Token: 0x06000052 RID: 82
	[DllImport("libSDOC.qti.dll")]
	private static extern bool sdocSet(uint id, uint value);

	// Token: 0x06000053 RID: 83
	[DllImport("libSDOC.qti.dll")]
	private static extern bool sdocStartNewFrame(float[] viewPos, float[] viewDir, float[] viewProj);

	// Token: 0x06000054 RID: 84
	[DllImport("libSDOC.qti.dll")]
	private static extern void sdocRenderOccluder(float[] vertices, ushort[] indices, uint nVert, uint nIdx, float[] modelAABB, float[] localToWorld);

	// Token: 0x06000055 RID: 85
	[DllImport("libSDOC.qti.dll")]
	private static extern bool sdocQueryOccludees(float[] bbox, uint nMesh, byte[] results);

	// Token: 0x06000056 RID: 86
	[DllImport("libSDOC.qti.dll")]
	private static extern void sdocRenderBakedOccluder(int[] compressedModel);

	// Token: 0x06000057 RID: 87 RVA: 0x00003868 File Offset: 0x00001A68
	public void Awake()
	{
		this.CameraPosition = new NativeArray<float>(3, 4, 1);
		this.CameraDirection = new NativeArray<float>(3, 4, 1);
		this.CameraViewProj = new NativeArray<float>(16, 4, 1);
	}

	// Token: 0x06000058 RID: 88 RVA: 0x00003898 File Offset: 0x00001A98
	public void JobAllocate()
	{
		if (this.meshes == null || this.meshes.Length == 0)
		{
			this.invalidOccluders = true;
			return;
		}
		Mesh mesh = this.meshes[0];
		if (mesh == null || mesh.vertexCount == 0)
		{
			this.invalidOccluders = true;
			return;
		}
		this.job = default(ParaOcclusionCullingJob);
		ParaOcclusionCulling.OccluderData occluderData = this.convertToOccluder(mesh);
		this.occluders.Add(occluderData);
		this.job.vertices = occluderData.vertices;
		this.job.nVert = occluderData.vertexCount;
		this.job.indices = occluderData.indices;
		this.job.nIdx = occluderData.indexCount;
		this.job.modelAABB = occluderData.modelAABB;
		this.job.localToWorld = occluderData.localToWorld;
		this.job.viewPos = this.CameraPosition;
		this.job.viewDir = this.CameraDirection;
		this.job.viewProj = this.CameraViewProj;
		Singleton<ParaOcclusionCullingManager>.instance.Reset();
	}

	// Token: 0x06000059 RID: 89 RVA: 0x000039A4 File Offset: 0x00001BA4
	private ParaOcclusionCulling.OccluderData convertToOccluder(Mesh mesh)
	{
		Mesh.MeshDataArray meshDataArray = Mesh.AcquireReadOnlyMeshData(mesh);
		Mesh.MeshData meshData = meshDataArray[0];
		NativeArray<Vector3> nativeArray = new NativeArray<Vector3>(meshData.vertexCount, 3, 1);
		meshData.GetVertices(nativeArray);
		NativeArray<ushort> indexData = meshData.GetIndexData<ushort>();
		int length = indexData.Length;
		NativeArray<float> nativeArray2 = new NativeArray<float>(length * 3, 4, 1);
		NativeArray<ushort> nativeArray3 = new NativeArray<ushort>(length, 4, 1);
		ushort num = 0;
		while ((int)num < length)
		{
			Vector3 vector = nativeArray[(int)num];
			nativeArray2[(int)(num * 3)] = vector.x;
			nativeArray2[(int)(num * 3 + 1)] = vector.y;
			nativeArray2[(int)(num * 3 + 2)] = vector.z;
			nativeArray3[(int)num] = num;
			num += 1;
		}
		ParaOcclusionCulling.OccluderData occluderData = default(ParaOcclusionCulling.OccluderData);
		if (nativeArray.IsCreated)
		{
			nativeArray.Dispose();
		}
		if (indexData.IsCreated)
		{
			indexData.Dispose();
		}
		meshDataArray.Dispose();
		occluderData.vertices = nativeArray2;
		occluderData.indices = nativeArray3;
		occluderData.vertexCount = (uint)(length * 3);
		occluderData.indexCount = (uint)length;
		Bounds bounds = mesh.bounds;
		occluderData.modelAABB = new NativeArray<float>(6, 4, 1);
		occluderData.modelAABB[0] = bounds.min.x;
		occluderData.modelAABB[1] = bounds.min.y;
		occluderData.modelAABB[2] = bounds.min.z;
		occluderData.modelAABB[3] = bounds.max.x;
		occluderData.modelAABB[4] = bounds.max.y;
		occluderData.modelAABB[5] = bounds.max.z;
		occluderData.localToWorld = new NativeArray<float>(16, 4, 1);
		occluderData.localToWorld[0] = (occluderData.localToWorld[5] = (occluderData.localToWorld[10] = (occluderData.localToWorld[15] = 1f)));
		occluderData.localToWorld[1] = (occluderData.localToWorld[2] = (occluderData.localToWorld[3] = 0f));
		occluderData.localToWorld[4] = (occluderData.localToWorld[6] = (occluderData.localToWorld[7] = 0f));
		occluderData.localToWorld[8] = (occluderData.localToWorld[9] = (occluderData.localToWorld[11] = 0f));
		occluderData.localToWorld[12] = (occluderData.localToWorld[13] = (occluderData.localToWorld[14] = 0f));
		return occluderData;
	}

	// Token: 0x0600005A RID: 90 RVA: 0x00003CA0 File Offset: 0x00001EA0
	public void Start()
	{
		this.JobAllocate();
		RenderPipelineManager.beginCameraRendering += this.BeginCamera;
		RenderPipelineManager.endCameraRendering += this.EndCamera;
		if (!ParaOcclusionCulling.sdocInit(256U, 152U, 0.05f))
		{
			Debug.LogError("SDOC initialize failed.");
			return;
		}
		Debug.Log("SDOC initialize success.");
		ParaOcclusionCulling.sdocSet(20U, 1U);
		if (ParaOcclusionCulling.sdocSet(21U, 0U))
		{
			Debug.Log("SDOC set double side success");
		}
		ParaOcclusionCulling.sdocSet(260U, 1U);
		this.sdocInitialized = true;
		Singleton<ParaOcclusionCullingManager>.instance.Enable();
	}

	// Token: 0x0600005B RID: 91 RVA: 0x00003D3C File Offset: 0x00001F3C
	public void Update()
	{
		if (!Singleton<ParaOcclusionCullingManager>.instance.enable)
		{
			return;
		}
		this.EnabledDepthMap = Singleton<ParaOcclusionCullingManager>.instance.enableDepthMap;
		if (!this.Enabled && this.jobStarted && this.handle.IsCompleted)
		{
			this.handle.Complete();
		}
	}

	// Token: 0x0600005C RID: 92 RVA: 0x00003D8E File Offset: 0x00001F8E
	public void SetOccluders(Mesh[] meshes)
	{
		this.meshes = meshes;
	}

	// Token: 0x0600005D RID: 93 RVA: 0x00003D98 File Offset: 0x00001F98
	private void updateCamera(Camera camera)
	{
		Vector3 position = camera.transform.position;
		Vector3 forward = camera.transform.forward;
		Matrix4x4 worldToCameraMatrix = camera.worldToCameraMatrix;
		Matrix4x4 projectionMatrix = camera.projectionMatrix;
		this.CameraPosition[0] = position.x;
		this.CameraPosition[1] = position.y;
		this.CameraPosition[2] = position.z;
		this.CameraDirection[0] = forward.x;
		this.CameraDirection[1] = forward.y;
		this.CameraDirection[2] = forward.z;
		Matrix4x4 matrix4x = projectionMatrix * worldToCameraMatrix;
		NativeArray<float> cameraViewProj = this.CameraViewProj;
		cameraViewProj[0] = matrix4x.m00;
		cameraViewProj[1] = matrix4x.m10;
		cameraViewProj[2] = matrix4x.m20;
		cameraViewProj[3] = matrix4x.m30;
		cameraViewProj[4] = matrix4x.m01;
		cameraViewProj[5] = matrix4x.m11;
		cameraViewProj[6] = matrix4x.m21;
		cameraViewProj[7] = matrix4x.m31;
		cameraViewProj[8] = matrix4x.m02;
		cameraViewProj[9] = matrix4x.m12;
		cameraViewProj[10] = matrix4x.m22;
		cameraViewProj[11] = matrix4x.m32;
		cameraViewProj[12] = matrix4x.m03;
		cameraViewProj[13] = matrix4x.m13;
		cameraViewProj[14] = matrix4x.m23;
		cameraViewProj[15] = matrix4x.m33;
	}

	// Token: 0x0600005E RID: 94 RVA: 0x00003F2C File Offset: 0x0000212C
	private void BeginCamera(ScriptableRenderContext context, Camera camera)
	{
		if (SceneView.lastActiveSceneView == null)
		{
			return;
		}
		if (SceneView.lastActiveSceneView.camera == camera)
		{
			return;
		}
		if (camera.orthographic)
		{
			return;
		}
		ParaOcclusionCullingManager instance = Singleton<ParaOcclusionCullingManager>.instance;
		if (!(camera.name == "FrameCacheCamera") && !(camera.name == "TPCamera"))
		{
			if (this.Enabled)
			{
				instance.ForceVisible();
			}
			this.Enabled = false;
			if (this.jobStarted)
			{
				this.handle.Complete();
			}
			this.frame_count = 0U;
			return;
		}
		this.frame_count += 1U;
		if (!this.Enabled && this.frame_count >= this.defer_frame_count)
		{
			this.Enabled = true;
		}
		if (!this.sdocInitialized || !this.Enabled || this.invalidOccluders)
		{
			return;
		}
		if (!this.handle.IsCompleted)
		{
			return;
		}
		if (this.jobStarted)
		{
			this.handle.Complete();
			instance.UpdateVisibility();
			instance.Swap();
			this.jobStarted = false;
			return;
		}
		this.updateCamera(camera);
		this.job.nMesh = instance.meshCount;
		this.job.bbox = instance.sharedMeshBounds;
		this.job.results = instance.curResults;
		this.handle = IJobExtensions.Schedule<ParaOcclusionCullingJob>(this.job, default(JobHandle));
		this.jobStarted = true;
	}

	// Token: 0x0600005F RID: 95 RVA: 0x00004094 File Offset: 0x00002294
	private void EndCamera(ScriptableRenderContext context, Camera camera)
	{
		if (SceneView.lastActiveSceneView == null)
		{
			return;
		}
		if (SceneView.lastActiveSceneView.camera == camera)
		{
			return;
		}
		if (!this.Enabled || !this._enabledDepthMap || !this.depthMap)
		{
			return;
		}
		ParaOcclusionCulling.sdocSync(252U, this.depthData);
		this.depthMap.SetPixelData<byte>(this.depthData, 0, 0);
		this.depthMap.Apply();
		Shader.SetGlobalTexture("_MainTex", this.depthMap);
		Graphics.Blit(this.depthMap, camera.targetTexture);
	}

	// Token: 0x06000060 RID: 96 RVA: 0x00004130 File Offset: 0x00002330
	public void OnDestroy()
	{
		if (this.jobStarted)
		{
			this.handle.Complete();
			this.jobStarted = false;
		}
		RenderPipelineManager.beginCameraRendering -= this.BeginCamera;
		RenderPipelineManager.endCameraRendering -= this.EndCamera;
		this.sdocCamera = null;
		if (ParaOcclusionCulling.sdocSet(240U, 1U))
		{
			Debug.Log("SDOC destroy success.");
		}
		else
		{
			Debug.LogWarning("SDOC Destroy failed.");
		}
		foreach (ParaOcclusionCulling.OccluderData occluderData in this.occluders)
		{
			NativeArray<float> nativeArray = occluderData.vertices;
			nativeArray.Dispose();
			NativeArray<ushort> indices = occluderData.indices;
			indices.Dispose();
			nativeArray = occluderData.localToWorld;
			nativeArray.Dispose();
			nativeArray = occluderData.modelAABB;
			nativeArray.Dispose();
		}
		this.CameraPosition.Dispose();
		this.CameraDirection.Dispose();
		this.CameraViewProj.Dispose();
		Singleton<ParaOcclusionCullingManager>.instance.Disable();
	}

	// Token: 0x06000061 RID: 97 RVA: 0x00004248 File Offset: 0x00002448
	public void OnGUI()
	{
		ParaOcclusionCullingManager instance = Singleton<ParaOcclusionCullingManager>.instance;
		if (instance != null && instance.ShowStatistic)
		{
			GUILayout.BeginArea(new Rect(300f, 300f, 320f, 240f));
			GUILayout.Label(instance.StatisticInfo, Array.Empty<GUILayoutOption>());
			GUILayout.EndArea();
		}
	}

	// Token: 0x06000062 RID: 98 RVA: 0x00004299 File Offset: 0x00002499
	public void OnApplicationQuit()
	{
		if (this.jobStarted)
		{
			this.handle.Complete();
			this.jobStarted = false;
		}
		if (this.depthMap)
		{
			Object.Destroy(this.depthMap);
		}
	}

	// Token: 0x06000063 RID: 99 RVA: 0x000042CD File Offset: 0x000024CD
	private void createDepthMap()
	{
		if (this.depthMap != null)
		{
			return;
		}
		this.depthMap = new Texture2D(256, 152, 63, false);
		this.depthData = new byte[38912];
	}

	// Token: 0x040000D8 RID: 216
	public bool Enabled;

	// Token: 0x040000D9 RID: 217
	private bool _enabledDepthMap;

	// Token: 0x040000DA RID: 218
	private const uint SDOC_RenderMode_Full = 0U;

	// Token: 0x040000DB RID: 219
	private const uint SDOC_RenderMode_Coherent = 1U;

	// Token: 0x040000DC RID: 220
	private const uint SDOC_RenderMode_CoherentFast = 2U;

	// Token: 0x040000DD RID: 221
	private const uint SDOC_RenderMode_ToggleRenderType = 12340U;

	// Token: 0x040000DE RID: 222
	private const uint SDOC_SetCCW = 20U;

	// Token: 0x040000DF RID: 223
	private const uint SDOC_FlipFaceIfNegScaled = 21U;

	// Token: 0x040000E0 RID: 224
	private const uint SDOC_Get_IsSameCamera = 99U;

	// Token: 0x040000E1 RID: 225
	private const uint SDOC_Set_UsePrevDepthBuffer = 100U;

	// Token: 0x040000E2 RID: 226
	private const uint SDOC_RenderMode = 101U;

	// Token: 0x040000E3 RID: 227
	private const uint SDOC_Get_Version = 212U;

	// Token: 0x040000E4 RID: 228
	private const uint SDOC_Set_CoherentModeSmallRotateDotAngleThreshold = 213U;

	// Token: 0x040000E5 RID: 229
	private const uint SDOC_Set_CoherentModeLargeRotateDotAngleThreshold = 214U;

	// Token: 0x040000E6 RID: 230
	private const uint SDOC_Set_CoherentModeCameraDistanceNearThreshold = 215U;

	// Token: 0x040000E7 RID: 231
	private const uint SDOC_Reset_DepthMapWidthAndHeight = 220U;

	// Token: 0x040000E8 RID: 232
	private const uint SDOC_DestroySDOC = 240U;

	// Token: 0x040000E9 RID: 233
	private const uint SDOC_Get_DepthBufferWidthHeight = 250U;

	// Token: 0x040000EA RID: 234
	private const uint SDOC_Set_FrameCaptureOutputPath = 251U;

	// Token: 0x040000EB RID: 235
	private const uint SDOC_Get_DepthMap = 252U;

	// Token: 0x040000EC RID: 236
	private const uint SDOC_Save_DepthMap = 256U;

	// Token: 0x040000ED RID: 237
	private const uint SDOC_Save_DepthMapPath = 257U;

	// Token: 0x040000EE RID: 238
	private const uint SDOC_ShowCulled = 260U;

	// Token: 0x040000EF RID: 239
	private const uint SDOC_ShowOccludeeInDepthMap = 261U;

	// Token: 0x040000F0 RID: 240
	private const uint SDOC_Get_MemoryUsed = 270U;

	// Token: 0x040000F1 RID: 241
	private const uint SDOC_SetPrintLogInGame = 300U;

	// Token: 0x040000F2 RID: 242
	private const uint SDOC_Get_Log = 301U;

	// Token: 0x040000F3 RID: 243
	private const uint SDOC_CaptureFrame = 400U;

	// Token: 0x040000F4 RID: 244
	public const string libSDOC = "libSDOC.qti.dll";

	// Token: 0x040000F5 RID: 245
	private bool sdocInitialized;

	// Token: 0x040000F6 RID: 246
	private Camera sdocCamera;

	// Token: 0x040000F7 RID: 247
	private Texture2D depthMap;

	// Token: 0x040000F8 RID: 248
	private byte[] depthData;

	// Token: 0x040000F9 RID: 249
	private NativeArray<float> CameraPosition;

	// Token: 0x040000FA RID: 250
	private NativeArray<float> CameraDirection;

	// Token: 0x040000FB RID: 251
	private NativeArray<float> CameraViewProj;

	// Token: 0x040000FC RID: 252
	[SerializeField]
	public Mesh[] meshes;

	// Token: 0x040000FD RID: 253
	private bool invalidOccluders;

	// Token: 0x040000FE RID: 254
	private List<ParaOcclusionCulling.OccluderData> occluders = new List<ParaOcclusionCulling.OccluderData>();

	// Token: 0x040000FF RID: 255
	private ParaOcclusionCullingJob job;

	// Token: 0x04000100 RID: 256
	private JobHandle handle;

	// Token: 0x04000101 RID: 257
	private bool jobStarted;

	// Token: 0x04000102 RID: 258
	private uint frame_count = 10U;

	// Token: 0x04000103 RID: 259
	private uint defer_frame_count = 10U;

	// Token: 0x04000104 RID: 260
	private const int OCWidth = 256;

	// Token: 0x04000105 RID: 261
	private const int OCHeight = 152;

	// Token: 0x02000085 RID: 133
	[Serializable]
	public struct OccluderData
	{
		// Token: 0x040002F7 RID: 759
		public NativeArray<float> vertices;

		// Token: 0x040002F8 RID: 760
		public uint vertexCount;

		// Token: 0x040002F9 RID: 761
		public NativeArray<ushort> indices;

		// Token: 0x040002FA RID: 762
		public uint indexCount;

		// Token: 0x040002FB RID: 763
		public NativeArray<float> modelAABB;

		// Token: 0x040002FC RID: 764
		public NativeArray<float> localToWorld;
	}
}
