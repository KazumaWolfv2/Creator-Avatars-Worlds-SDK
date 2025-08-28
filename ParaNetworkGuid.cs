using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000029 RID: 41
[DisallowMultipleComponent]
public class ParaNetworkGuid : MonoBehaviour
{
	// Token: 0x0600003D RID: 61 RVA: 0x00003470 File Offset: 0x00001670
	private void OnValidate()
	{
		if (!Application.isPlaying && Application.isEditor && !Application.isBatchMode)
		{
			this.RefreshGUID();
			return;
		}
		if (Application.isBatchMode)
		{
			this.RefreshGUID();
			Debug.Log(string.Format("network vv guid gameObject:{0} guid:{1}", ParaNetworkGuid.ComputeGoPath(base.gameObject), this.NetGUID));
		}
	}

	// Token: 0x0600003E RID: 62 RVA: 0x000034CB File Offset: 0x000016CB
	public string GetGuidStr()
	{
		return this.NetGUID.ToString();
	}

	// Token: 0x0600003F RID: 63 RVA: 0x000034DE File Offset: 0x000016DE
	public bool IsValid()
	{
		return this.NetGUID.IsValid;
	}

	// Token: 0x06000040 RID: 64 RVA: 0x000034EC File Offset: 0x000016EC
	[ContextMenu("Refresh GUID")]
	public void RefreshGUID()
	{
		ulong num = XXHash.Hash64(ParaNetworkGuid.ComputeGoPath(base.gameObject));
		ulong num2 = XXHash.Hash64(base.gameObject.name);
		this.NetGUID = new ParaNetworkGuid.NetworkGUID(num, num2);
	}

	// Token: 0x06000041 RID: 65 RVA: 0x00003528 File Offset: 0x00001728
	public static string GameObjectHierarchyName(GameObject go)
	{
		if (go == null)
		{
			return string.Empty;
		}
		string text = go.name;
		Transform transform = go.transform.parent;
		while (transform != null)
		{
			text = transform.name + "=>" + text;
			transform = transform.parent;
		}
		return text;
	}

	// Token: 0x06000042 RID: 66 RVA: 0x0000357C File Offset: 0x0000177C
	public static string ComputeGoPath(GameObject go)
	{
		return ParaNetworkGuid.<ComputeGoPath>g__GameObjectHierarchyPath|8_0(go);
	}

	// Token: 0x06000043 RID: 67 RVA: 0x00003584 File Offset: 0x00001784
	[ContextMenu("Check Scene GUID")]
	private void DoCheckObjectsGUIDIsValid()
	{
		ParaNetworkGuid.CheckObjectsGUIDIsValid();
	}

	// Token: 0x06000044 RID: 68 RVA: 0x0000358C File Offset: 0x0000178C
	private static int GetTransformIndex(Transform t)
	{
		Transform parent = t.parent;
		if (parent == null)
		{
			return -1;
		}
		for (int i = 0; i < parent.childCount; i++)
		{
			if (t == parent.GetChild(i))
			{
				return i;
			}
		}
		throw new Exception("error unexpected");
	}

	// Token: 0x06000045 RID: 69 RVA: 0x000035D8 File Offset: 0x000017D8
	public static bool CheckObjectsGUIDIsValid()
	{
		ParaNetworkGuid[] array = Object.FindObjectsOfType<ParaNetworkGuid>(true);
		Dictionary<ParaNetworkGuid.NetworkGUID, GameObject> dictionary = new Dictionary<ParaNetworkGuid.NetworkGUID, GameObject>();
		bool flag = false;
		foreach (ParaNetworkGuid paraNetworkGuid in array)
		{
			GameObject gameObject;
			if (dictionary.TryGetValue(paraNetworkGuid.NetGUID, out gameObject))
			{
				paraNetworkGuid.RefreshGUID();
				flag = true;
				Debug.Log(string.Concat(new string[]
				{
					"guid check error gameObject:",
					ParaNetworkGuid.GameObjectHierarchyName(paraNetworkGuid.gameObject),
					" already contain guid ",
					paraNetworkGuid.NetGUID.ToString(),
					" other object:",
					ParaNetworkGuid.GameObjectHierarchyName(gameObject)
				}));
			}
			else
			{
				dictionary.Add(paraNetworkGuid.NetGUID, paraNetworkGuid.gameObject);
			}
		}
		Debug.Log("check guid complete");
		return flag;
	}

	// Token: 0x06000047 RID: 71 RVA: 0x000036A0 File Offset: 0x000018A0
	[CompilerGenerated]
	internal static string <ComputeGoPath>g__GameObjectHierarchyPath|8_0(GameObject go)
	{
		if (go == null)
		{
			return string.Empty;
		}
		string text = string.Format("{0}_p{1}", go.name, ParaNetworkGuid.GetTransformIndex(go.transform));
		Transform transform = go.transform.parent;
		while (transform != null)
		{
			text = string.Format("{0}=>{1}_p({2})", text, transform.name, ParaNetworkGuid.GetTransformIndex(transform));
			transform = transform.parent;
		}
		return text;
	}

	// Token: 0x040000D4 RID: 212
	[HideInInspector]
	[SerializeField]
	public ParaNetworkGuid.NetworkGUID NetGUID;

	// Token: 0x040000D5 RID: 213
	[SerializeField]
	public bool IsLocal;

	// Token: 0x02000084 RID: 132
	[Serializable]
	public struct NetworkGUID : IEquatable<ParaNetworkGuid.NetworkGUID>, IComparable<ParaNetworkGuid.NetworkGUID>
	{
		// Token: 0x06000260 RID: 608 RVA: 0x000104A7 File Offset: 0x0000E6A7
		public NetworkGUID(byte[] guid)
		{
			this.n1 = BitConverter.ToUInt64(guid, 0);
			this.n2 = BitConverter.ToUInt64(guid, 8);
		}

		// Token: 0x06000261 RID: 609 RVA: 0x000104C3 File Offset: 0x0000E6C3
		public NetworkGUID(ulong[] guid)
		{
			this.n1 = guid[0];
			this.n2 = guid[1];
		}

		// Token: 0x06000262 RID: 610 RVA: 0x000104D7 File Offset: 0x0000E6D7
		public NetworkGUID(ulong a, ulong b)
		{
			this.n1 = a;
			this.n2 = b;
		}

		// Token: 0x06000263 RID: 611 RVA: 0x000104E8 File Offset: 0x0000E6E8
		public NetworkGUID(string guid)
		{
			Guid guid2 = new Guid(guid);
			byte[] array = guid2.ToByteArray();
			this.n1 = BitConverter.ToUInt64(array, 0);
			this.n2 = BitConverter.ToUInt64(array, 8);
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000264 RID: 612 RVA: 0x0001051F File Offset: 0x0000E71F
		public bool IsValid
		{
			get
			{
				return this.n1 != 0UL || this.n2 > 0UL;
			}
		}

		// Token: 0x06000265 RID: 613 RVA: 0x00010535 File Offset: 0x0000E735
		public static bool operator ==(ParaNetworkGuid.NetworkGUID a, ParaNetworkGuid.NetworkGUID b)
		{
			return a.n1 == b.n1 && a.n2 == b.n2;
		}

		// Token: 0x06000266 RID: 614 RVA: 0x00010555 File Offset: 0x0000E755
		public static bool operator !=(ParaNetworkGuid.NetworkGUID a, ParaNetworkGuid.NetworkGUID b)
		{
			return !(a == b);
		}

		// Token: 0x06000267 RID: 615 RVA: 0x00010535 File Offset: 0x0000E735
		public bool Equals(ParaNetworkGuid.NetworkGUID other)
		{
			return this.n1 == other.n1 && this.n2 == other.n2;
		}

		// Token: 0x06000268 RID: 616 RVA: 0x00010564 File Offset: 0x0000E764
		public override bool Equals(object obj)
		{
			if (obj is ParaNetworkGuid.NetworkGUID)
			{
				ParaNetworkGuid.NetworkGUID networkGUID = (ParaNetworkGuid.NetworkGUID)obj;
				return this.Equals(networkGUID);
			}
			return false;
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0001058C File Offset: 0x0000E78C
		public override string ToString()
		{
			BitConverter.TryWriteBytes(ParaNetworkGuid.NetworkGUID.cache, this.n1);
			BitConverter.TryWriteBytes(new Span<byte>(ParaNetworkGuid.NetworkGUID.cache, 8, 8), this.n2);
			Guid guid = new Guid(ParaNetworkGuid.NetworkGUID.cache);
			return guid.ToString();
		}

		// Token: 0x0600026A RID: 618 RVA: 0x000105E0 File Offset: 0x0000E7E0
		public override int GetHashCode()
		{
			BitConverter.TryWriteBytes(ParaNetworkGuid.NetworkGUID.cache, this.n1);
			BitConverter.TryWriteBytes(new Span<byte>(ParaNetworkGuid.NetworkGUID.cache, 8, 8), this.n2);
			Guid guid = new Guid(ParaNetworkGuid.NetworkGUID.cache);
			return guid.GetHashCode();
		}

		// Token: 0x0600026B RID: 619 RVA: 0x00010634 File Offset: 0x0000E834
		public int CompareTo(ParaNetworkGuid.NetworkGUID other)
		{
			long num = (long)(this.n1 - other.n1);
			if (num == 0L)
			{
				num = (long)(this.n2 - other.n2);
				if (num == 0L)
				{
					return 0;
				}
			}
			if (num < 0L)
			{
				return -1;
			}
			return 1;
		}

		// Token: 0x040002F4 RID: 756
		[SerializeField]
		public ulong n1;

		// Token: 0x040002F5 RID: 757
		[SerializeField]
		public ulong n2;

		// Token: 0x040002F6 RID: 758
		private static byte[] cache = new byte[16];
	}
}
