using System;
using System.Collections;
using System.Collections.Specialized;

namespace LitJson
{
	// Token: 0x02000058 RID: 88
	public class JsonMockWrapper : IJsonWrapper, IList, ICollection, IEnumerable, IOrderedDictionary, IDictionary
	{
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000166 RID: 358 RVA: 0x00007E28 File Offset: 0x00006028
		public bool IsArray
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000167 RID: 359 RVA: 0x00007E28 File Offset: 0x00006028
		public bool IsBoolean
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000168 RID: 360 RVA: 0x00007E28 File Offset: 0x00006028
		public bool IsDouble
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000169 RID: 361 RVA: 0x00007E28 File Offset: 0x00006028
		public bool IsInt
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600016A RID: 362 RVA: 0x00007E28 File Offset: 0x00006028
		public bool IsLong
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600016B RID: 363 RVA: 0x00007E28 File Offset: 0x00006028
		public bool IsObject
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600016C RID: 364 RVA: 0x00007E28 File Offset: 0x00006028
		public bool IsString
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00007E28 File Offset: 0x00006028
		public bool GetBoolean()
		{
			return false;
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00007E2B File Offset: 0x0000602B
		public double GetDouble()
		{
			return 0.0;
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00007E28 File Offset: 0x00006028
		public int GetInt()
		{
			return 0;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00007E28 File Offset: 0x00006028
		public JsonType GetJsonType()
		{
			return JsonType.None;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00007E36 File Offset: 0x00006036
		public long GetLong()
		{
			return 0L;
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00007E3A File Offset: 0x0000603A
		public string GetString()
		{
			return "";
		}

		// Token: 0x06000173 RID: 371 RVA: 0x000030B5 File Offset: 0x000012B5
		public void SetBoolean(bool val)
		{
		}

		// Token: 0x06000174 RID: 372 RVA: 0x000030B5 File Offset: 0x000012B5
		public void SetDouble(double val)
		{
		}

		// Token: 0x06000175 RID: 373 RVA: 0x000030B5 File Offset: 0x000012B5
		public void SetInt(int val)
		{
		}

		// Token: 0x06000176 RID: 374 RVA: 0x000030B5 File Offset: 0x000012B5
		public void SetJsonType(JsonType type)
		{
		}

		// Token: 0x06000177 RID: 375 RVA: 0x000030B5 File Offset: 0x000012B5
		public void SetLong(long val)
		{
		}

		// Token: 0x06000178 RID: 376 RVA: 0x000030B5 File Offset: 0x000012B5
		public void SetString(string val)
		{
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00007E3A File Offset: 0x0000603A
		public string ToJson()
		{
			return "";
		}

		// Token: 0x0600017A RID: 378 RVA: 0x000030B5 File Offset: 0x000012B5
		public void ToJson(JsonWriter writer)
		{
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600017B RID: 379 RVA: 0x00007E41 File Offset: 0x00006041
		bool IList.IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600017C RID: 380 RVA: 0x00007E41 File Offset: 0x00006041
		bool IList.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000042 RID: 66
		object IList.this[int index]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00007E28 File Offset: 0x00006028
		int IList.Add(object value)
		{
			return 0;
		}

		// Token: 0x06000180 RID: 384 RVA: 0x000030B5 File Offset: 0x000012B5
		void IList.Clear()
		{
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00007E28 File Offset: 0x00006028
		bool IList.Contains(object value)
		{
			return false;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00007E44 File Offset: 0x00006044
		int IList.IndexOf(object value)
		{
			return -1;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x000030B5 File Offset: 0x000012B5
		void IList.Insert(int i, object v)
		{
		}

		// Token: 0x06000184 RID: 388 RVA: 0x000030B5 File Offset: 0x000012B5
		void IList.Remove(object value)
		{
		}

		// Token: 0x06000185 RID: 389 RVA: 0x000030B5 File Offset: 0x000012B5
		void IList.RemoveAt(int index)
		{
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000186 RID: 390 RVA: 0x00007E28 File Offset: 0x00006028
		int ICollection.Count
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000187 RID: 391 RVA: 0x00007E28 File Offset: 0x00006028
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000188 RID: 392 RVA: 0x00002923 File Offset: 0x00000B23
		object ICollection.SyncRoot
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000189 RID: 393 RVA: 0x000030B5 File Offset: 0x000012B5
		void ICollection.CopyTo(Array array, int index)
		{
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00002923 File Offset: 0x00000B23
		IEnumerator IEnumerable.GetEnumerator()
		{
			return null;
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600018B RID: 395 RVA: 0x00007E41 File Offset: 0x00006041
		bool IDictionary.IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600018C RID: 396 RVA: 0x00007E41 File Offset: 0x00006041
		bool IDictionary.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600018D RID: 397 RVA: 0x00002923 File Offset: 0x00000B23
		ICollection IDictionary.Keys
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600018E RID: 398 RVA: 0x00002923 File Offset: 0x00000B23
		ICollection IDictionary.Values
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700004A RID: 74
		object IDictionary.this[object key]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x06000191 RID: 401 RVA: 0x000030B5 File Offset: 0x000012B5
		void IDictionary.Add(object k, object v)
		{
		}

		// Token: 0x06000192 RID: 402 RVA: 0x000030B5 File Offset: 0x000012B5
		void IDictionary.Clear()
		{
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00007E28 File Offset: 0x00006028
		bool IDictionary.Contains(object key)
		{
			return false;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x000030B5 File Offset: 0x000012B5
		void IDictionary.Remove(object key)
		{
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00002923 File Offset: 0x00000B23
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return null;
		}

		// Token: 0x1700004B RID: 75
		object IOrderedDictionary.this[int idx]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00002923 File Offset: 0x00000B23
		IDictionaryEnumerator IOrderedDictionary.GetEnumerator()
		{
			return null;
		}

		// Token: 0x06000199 RID: 409 RVA: 0x000030B5 File Offset: 0x000012B5
		void IOrderedDictionary.Insert(int i, object k, object v)
		{
		}

		// Token: 0x0600019A RID: 410 RVA: 0x000030B5 File Offset: 0x000012B5
		void IOrderedDictionary.RemoveAt(int i)
		{
		}
	}
}
