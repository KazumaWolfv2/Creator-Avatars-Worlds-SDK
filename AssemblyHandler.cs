using System;
using System.Reflection;

// Token: 0x0200001D RID: 29
public class AssemblyHandler
{
	// Token: 0x060000AD RID: 173 RVA: 0x00007348 File Offset: 0x00005548
	public static Type GetType(string typeFullName, string assemblyName)
	{
		if (assemblyName == null)
		{
			return Type.GetType(typeFullName);
		}
		foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
		{
			if (assembly.FullName.Split(',', StringSplitOptions.None)[0].Trim() == assemblyName.Trim())
			{
				return assembly.GetType(typeFullName);
			}
		}
		Assembly assembly2 = Assembly.LoadWithPartialName(assemblyName);
		if (assembly2 != null)
		{
			return assembly2.GetType(typeFullName);
		}
		return null;
	}
}
