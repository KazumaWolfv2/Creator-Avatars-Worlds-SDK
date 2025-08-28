using System;
using System.Collections;
using System.Collections.Generic;

namespace LitJson
{
	// Token: 0x0200004D RID: 77
	internal class OrderedDictionaryEnumerator : IDictionaryEnumerator, IEnumerator
	{
		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600011C RID: 284 RVA: 0x000065E6 File Offset: 0x000047E6
		public object Current
		{
			get
			{
				return this.Entry;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600011D RID: 285 RVA: 0x000065F4 File Offset: 0x000047F4
		public DictionaryEntry Entry
		{
			get
			{
				KeyValuePair<string, JsonData> keyValuePair = this.list_enumerator.Current;
				return new DictionaryEntry(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600011E RID: 286 RVA: 0x00006620 File Offset: 0x00004820
		public object Key
		{
			get
			{
				KeyValuePair<string, JsonData> keyValuePair = this.list_enumerator.Current;
				return keyValuePair.Key;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600011F RID: 287 RVA: 0x00006640 File Offset: 0x00004840
		public object Value
		{
			get
			{
				KeyValuePair<string, JsonData> keyValuePair = this.list_enumerator.Current;
				return keyValuePair.Value;
			}
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00006660 File Offset: 0x00004860
		public OrderedDictionaryEnumerator(IEnumerator<KeyValuePair<string, JsonData>> enumerator)
		{
			this.list_enumerator = enumerator;
		}

		// Token: 0x06000121 RID: 289 RVA: 0x0000666F File Offset: 0x0000486F
		public bool MoveNext()
		{
			return this.list_enumerator.MoveNext();
		}

		// Token: 0x06000122 RID: 290 RVA: 0x0000667C File Offset: 0x0000487C
		public void Reset()
		{
			this.list_enumerator.Reset();
		}

		// Token: 0x040001A3 RID: 419
		private IEnumerator<KeyValuePair<string, JsonData>> list_enumerator;
	}
}
