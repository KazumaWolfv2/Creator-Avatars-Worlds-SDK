using System;
using System.Collections.Generic;
using System.Linq;

// Token: 0x0200003F RID: 63
internal static class MyExtensionMethods
{
	// Token: 0x060001CA RID: 458 RVA: 0x0000C299 File Offset: 0x0000A499
	public static IOrderedEnumerable<T> Order<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector, bool ascending)
	{
		if (ascending)
		{
			return source.OrderBy(selector);
		}
		return source.OrderByDescending(selector);
	}

	// Token: 0x060001CB RID: 459 RVA: 0x0000C2AD File Offset: 0x0000A4AD
	public static IOrderedEnumerable<T> ThenBy<T, TKey>(this IOrderedEnumerable<T> source, Func<T, TKey> selector, bool ascending)
	{
		if (ascending)
		{
			return source.ThenBy(selector);
		}
		return source.ThenByDescending(selector);
	}
}
