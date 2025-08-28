using System;

// Token: 0x02000046 RID: 70
public abstract class Singleton<T> where T : class, new()
{
	// Token: 0x17000009 RID: 9
	// (get) Token: 0x06000088 RID: 136 RVA: 0x00004EB8 File Offset: 0x000030B8
	// (set) Token: 0x06000089 RID: 137 RVA: 0x00004EBF File Offset: 0x000030BF
	public static T instance
	{
		get
		{
			return Singleton<T>._instance;
		}
		set
		{
			Singleton<T>._instance = value;
		}
	}

	// Token: 0x0400017B RID: 379
	private static T _instance = new T();
}
