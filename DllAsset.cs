using System;
using System.Collections.Generic;

// Token: 0x0200005F RID: 95
[Serializable]
public class DllAsset
{
	// Token: 0x060002CF RID: 719 RVA: 0x00011D84 File Offset: 0x0000FF84
	public bool IsExist(string guid)
	{
		using (List<DllInfo>.Enumerator enumerator = this.InfoList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (string.Equals(enumerator.Current.Guid, guid))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060002D0 RID: 720 RVA: 0x00011DE4 File Offset: 0x0000FFE4
	public bool IsExistPath(string path)
	{
		using (List<DllInfo>.Enumerator enumerator = this.InfoList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (string.Equals(enumerator.Current.Path, path))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060002D1 RID: 721 RVA: 0x00011E44 File Offset: 0x00010044
	public string GetGuid(string path)
	{
		foreach (DllInfo dllInfo in this.InfoList)
		{
			if (dllInfo.Path.Equals(path))
			{
				return dllInfo.Guid;
			}
		}
		return null;
	}

	// Token: 0x04000181 RID: 385
	public List<DllInfo> InfoList;
}
