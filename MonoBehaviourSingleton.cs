using System;
using UnityEngine;

// Token: 0x02000043 RID: 67
public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	// Token: 0x17000008 RID: 8
	// (get) Token: 0x0600007E RID: 126 RVA: 0x00004DE0 File Offset: 0x00002FE0
	public static T Instance
	{
		get
		{
			if (MonoBehaviourSingleton<T>._instance == null)
			{
				Type typeFromHandle = typeof(T);
				MonoBehaviourSingleton<T>._instance = (T)((object)Object.FindObjectOfType(typeFromHandle));
				if (MonoBehaviourSingleton<T>._instance == null)
				{
					Type type = typeFromHandle;
					Debug.LogError(((type != null) ? type.ToString() : null) + " can not find instance!");
				}
			}
			return MonoBehaviourSingleton<T>._instance;
		}
	}

	// Token: 0x0600007F RID: 127 RVA: 0x00004E4D File Offset: 0x0000304D
	public static bool IsExist()
	{
		return MonoBehaviourSingleton<T>._instance != null;
	}

	// Token: 0x06000080 RID: 128 RVA: 0x00004E5F File Offset: 0x0000305F
	protected virtual void Awake()
	{
		if (MonoBehaviourSingleton<T>.Instance != this)
		{
			Type typeFromHandle = typeof(T);
			Debug.LogError(((typeFromHandle != null) ? typeFromHandle.ToString() : null) + " is already existed!");
		}
	}

	// Token: 0x0400017A RID: 378
	private static T _instance;
}
