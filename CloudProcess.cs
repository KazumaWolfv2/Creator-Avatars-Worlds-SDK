using System;
using System.Collections.Generic;

// Token: 0x02000070 RID: 112
public class CloudProcess
{
	// Token: 0x060003EE RID: 1006 RVA: 0x0001B2D6 File Offset: 0x000194D6
	public void Add(string name, string key)
	{
		this.NameList.Add(name);
		this.KeyList.Add(key);
	}

	// Token: 0x060003EF RID: 1007 RVA: 0x0001B2F0 File Offset: 0x000194F0
	public string GetKey(int index)
	{
		return this.KeyList[index];
	}

	// Token: 0x060003F0 RID: 1008 RVA: 0x0001B2FE File Offset: 0x000194FE
	public string[] GetNameArray()
	{
		return this.NameList.ToArray();
	}

	// Token: 0x04000220 RID: 544
	private List<string> NameList = new List<string>();

	// Token: 0x04000221 RID: 545
	private List<string> KeyList = new List<string>();
}
