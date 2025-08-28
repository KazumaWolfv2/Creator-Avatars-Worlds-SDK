using System;
using UnityEngine;

// Token: 0x02000040 RID: 64
[ExecuteAlways]
public class ArtRotation : MonoBehaviour
{
	// Token: 0x06000079 RID: 121 RVA: 0x00004B28 File Offset: 0x00002D28
	private void LateUpdate()
	{
		switch (this.m_axis)
		{
		case ArtRotation.AxisDirection.Up:
		{
			for (int i = 0; i < this.m_clouds.Length; i++)
			{
				if (!(null == this.m_clouds[i].transform))
				{
					this.m_clouds[i].transform.Rotate(new Vector3(0f, this.m_clouds[i].speed * Time.deltaTime * 10f, 0f));
				}
			}
			return;
		}
		case ArtRotation.AxisDirection.Right:
		{
			for (int j = 0; j < this.m_clouds.Length; j++)
			{
				if (!(null == this.m_clouds[j].transform))
				{
					this.m_clouds[j].transform.Rotate(new Vector3(this.m_clouds[j].speed * Time.deltaTime * 10f, 0f, 0f));
				}
			}
			return;
		}
		case ArtRotation.AxisDirection.Forward:
		{
			for (int k = 0; k < this.m_clouds.Length; k++)
			{
				if (!(null == this.m_clouds[k].transform))
				{
					this.m_clouds[k].transform.Rotate(new Vector3(0f, 0f, this.m_clouds[k].speed * Time.deltaTime * 10f));
				}
			}
			return;
		}
		default:
			return;
		}
	}

	// Token: 0x04000178 RID: 376
	public ArtRotation.AxisDirection m_axis = ArtRotation.AxisDirection.Forward;

	// Token: 0x04000179 RID: 377
	public ArtRotation.CloudsSpeed[] m_clouds;

	// Token: 0x02000086 RID: 134
	public enum AxisDirection
	{
		// Token: 0x040002FE RID: 766
		Up,
		// Token: 0x040002FF RID: 767
		Right,
		// Token: 0x04000300 RID: 768
		Forward
	}

	// Token: 0x02000087 RID: 135
	[Serializable]
	public struct CloudsSpeed
	{
		// Token: 0x04000301 RID: 769
		public Transform transform;

		// Token: 0x04000302 RID: 770
		public float speed;
	}
}
