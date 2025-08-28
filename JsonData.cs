using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;

namespace LitJson
{
	// Token: 0x0200004C RID: 76
	public class JsonData : IJsonWrapper, IList, ICollection, IEnumerable, IOrderedDictionary, IDictionary, IEquatable<JsonData>
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00005658 File Offset: 0x00003858
		public int Count
		{
			get
			{
				return this.EnsureCollection().Count;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00005665 File Offset: 0x00003865
		public bool IsArray
		{
			get
			{
				return this.type == JsonType.Array;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00005670 File Offset: 0x00003870
		public bool IsBoolean
		{
			get
			{
				return this.type == JsonType.Boolean;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000BF RID: 191 RVA: 0x0000567B File Offset: 0x0000387B
		public bool IsDouble
		{
			get
			{
				return this.type == JsonType.Double;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00005686 File Offset: 0x00003886
		public bool IsInt
		{
			get
			{
				return this.type == JsonType.Int;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00005691 File Offset: 0x00003891
		public bool IsLong
		{
			get
			{
				return this.type == JsonType.Long;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x0000569C File Offset: 0x0000389C
		public bool IsObject
		{
			get
			{
				return this.type == JsonType.Object;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x000056A7 File Offset: 0x000038A7
		public bool IsString
		{
			get
			{
				return this.type == JsonType.String;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x000056B2 File Offset: 0x000038B2
		public ICollection<string> Keys
		{
			get
			{
				this.EnsureDictionary();
				return this.inst_object.Keys;
			}
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x000056C6 File Offset: 0x000038C6
		public bool ContainsKey(string key)
		{
			this.EnsureDictionary();
			return this.inst_object.Keys.Contains(key);
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x000056E0 File Offset: 0x000038E0
		int ICollection.Count
		{
			get
			{
				return this.Count;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x000056E8 File Offset: 0x000038E8
		bool ICollection.IsSynchronized
		{
			get
			{
				return this.EnsureCollection().IsSynchronized;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x000056F5 File Offset: 0x000038F5
		object ICollection.SyncRoot
		{
			get
			{
				return this.EnsureCollection().SyncRoot;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x00005702 File Offset: 0x00003902
		bool IDictionary.IsFixedSize
		{
			get
			{
				return this.EnsureDictionary().IsFixedSize;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000CA RID: 202 RVA: 0x0000570F File Offset: 0x0000390F
		bool IDictionary.IsReadOnly
		{
			get
			{
				return this.EnsureDictionary().IsReadOnly;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000CB RID: 203 RVA: 0x0000571C File Offset: 0x0000391C
		ICollection IDictionary.Keys
		{
			get
			{
				this.EnsureDictionary();
				IList<string> list = new List<string>();
				foreach (KeyValuePair<string, JsonData> keyValuePair in this.object_list)
				{
					list.Add(keyValuePair.Key);
				}
				return (ICollection)list;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000CC RID: 204 RVA: 0x00005784 File Offset: 0x00003984
		ICollection IDictionary.Values
		{
			get
			{
				this.EnsureDictionary();
				IList<JsonData> list = new List<JsonData>();
				foreach (KeyValuePair<string, JsonData> keyValuePair in this.object_list)
				{
					list.Add(keyValuePair.Value);
				}
				return (ICollection)list;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000CD RID: 205 RVA: 0x000057EC File Offset: 0x000039EC
		bool IJsonWrapper.IsArray
		{
			get
			{
				return this.IsArray;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000CE RID: 206 RVA: 0x000057F4 File Offset: 0x000039F4
		bool IJsonWrapper.IsBoolean
		{
			get
			{
				return this.IsBoolean;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000CF RID: 207 RVA: 0x000057FC File Offset: 0x000039FC
		bool IJsonWrapper.IsDouble
		{
			get
			{
				return this.IsDouble;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x00005804 File Offset: 0x00003A04
		bool IJsonWrapper.IsInt
		{
			get
			{
				return this.IsInt;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x0000580C File Offset: 0x00003A0C
		bool IJsonWrapper.IsLong
		{
			get
			{
				return this.IsLong;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x00005814 File Offset: 0x00003A14
		bool IJsonWrapper.IsObject
		{
			get
			{
				return this.IsObject;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x0000581C File Offset: 0x00003A1C
		bool IJsonWrapper.IsString
		{
			get
			{
				return this.IsString;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00005824 File Offset: 0x00003A24
		bool IList.IsFixedSize
		{
			get
			{
				return this.EnsureList().IsFixedSize;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x00005831 File Offset: 0x00003A31
		bool IList.IsReadOnly
		{
			get
			{
				return this.EnsureList().IsReadOnly;
			}
		}

		// Token: 0x1700002A RID: 42
		object IDictionary.this[object key]
		{
			get
			{
				return this.EnsureDictionary()[key];
			}
			set
			{
				if (!(key is string))
				{
					throw new ArgumentException("The key has to be a string");
				}
				JsonData jsonData = this.ToJsonData(value);
				this[(string)key] = jsonData;
			}
		}

		// Token: 0x1700002B RID: 43
		object IOrderedDictionary.this[int idx]
		{
			get
			{
				this.EnsureDictionary();
				return this.object_list[idx].Value;
			}
			set
			{
				this.EnsureDictionary();
				JsonData jsonData = this.ToJsonData(value);
				KeyValuePair<string, JsonData> keyValuePair = this.object_list[idx];
				this.inst_object[keyValuePair.Key] = jsonData;
				KeyValuePair<string, JsonData> keyValuePair2 = new KeyValuePair<string, JsonData>(keyValuePair.Key, jsonData);
				this.object_list[idx] = keyValuePair2;
			}
		}

		// Token: 0x1700002C RID: 44
		object IList.this[int index]
		{
			get
			{
				return this.EnsureList()[index];
			}
			set
			{
				this.EnsureList();
				JsonData jsonData = this.ToJsonData(value);
				this[index] = jsonData;
			}
		}

		// Token: 0x1700002D RID: 45
		public JsonData this[string prop_name]
		{
			get
			{
				this.EnsureDictionary();
				return this.inst_object[prop_name];
			}
			set
			{
				this.EnsureDictionary();
				KeyValuePair<string, JsonData> keyValuePair = new KeyValuePair<string, JsonData>(prop_name, value);
				if (this.inst_object.ContainsKey(prop_name))
				{
					for (int i = 0; i < this.object_list.Count; i++)
					{
						if (this.object_list[i].Key == prop_name)
						{
							this.object_list[i] = keyValuePair;
							break;
						}
					}
				}
				else
				{
					this.object_list.Add(keyValuePair);
				}
				this.inst_object[prop_name] = value;
				this.json = null;
			}
		}

		// Token: 0x1700002E RID: 46
		public JsonData this[int index]
		{
			get
			{
				this.EnsureCollection();
				if (this.type == JsonType.Array)
				{
					return this.inst_array[index];
				}
				return this.object_list[index].Value;
			}
			set
			{
				this.EnsureCollection();
				if (this.type == JsonType.Array)
				{
					this.inst_array[index] = value;
				}
				else
				{
					KeyValuePair<string, JsonData> keyValuePair = this.object_list[index];
					KeyValuePair<string, JsonData> keyValuePair2 = new KeyValuePair<string, JsonData>(keyValuePair.Key, value);
					this.object_list[index] = keyValuePair2;
					this.inst_object[keyValuePair.Key] = value;
				}
				this.json = null;
			}
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x000020AC File Offset: 0x000002AC
		public JsonData()
		{
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00005A8F File Offset: 0x00003C8F
		public JsonData(bool boolean)
		{
			this.type = JsonType.Boolean;
			this.inst_boolean = boolean;
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00005AA5 File Offset: 0x00003CA5
		public JsonData(double number)
		{
			this.type = JsonType.Double;
			this.inst_double = number;
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00005ABB File Offset: 0x00003CBB
		public JsonData(int number)
		{
			this.type = JsonType.Int;
			this.inst_int = number;
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00005AD1 File Offset: 0x00003CD1
		public JsonData(long number)
		{
			this.type = JsonType.Long;
			this.inst_long = number;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00005AE8 File Offset: 0x00003CE8
		public JsonData(object obj)
		{
			if (obj is bool)
			{
				this.type = JsonType.Boolean;
				this.inst_boolean = (bool)obj;
				return;
			}
			if (obj is double)
			{
				this.type = JsonType.Double;
				this.inst_double = (double)obj;
				return;
			}
			if (obj is int)
			{
				this.type = JsonType.Int;
				this.inst_int = (int)obj;
				return;
			}
			if (obj is long)
			{
				this.type = JsonType.Long;
				this.inst_long = (long)obj;
				return;
			}
			if (obj is string)
			{
				this.type = JsonType.String;
				this.inst_string = (string)obj;
				return;
			}
			throw new ArgumentException("Unable to wrap the given object with JsonData");
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00005B91 File Offset: 0x00003D91
		public JsonData(string str)
		{
			this.type = JsonType.String;
			this.inst_string = str;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00005BA7 File Offset: 0x00003DA7
		public static implicit operator JsonData(bool data)
		{
			return new JsonData(data);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00005BAF File Offset: 0x00003DAF
		public static implicit operator JsonData(double data)
		{
			return new JsonData(data);
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00005BB7 File Offset: 0x00003DB7
		public static implicit operator JsonData(int data)
		{
			return new JsonData(data);
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00005BBF File Offset: 0x00003DBF
		public static implicit operator JsonData(long data)
		{
			return new JsonData(data);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00005BC7 File Offset: 0x00003DC7
		public static implicit operator JsonData(string data)
		{
			return new JsonData(data);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00005BCF File Offset: 0x00003DCF
		public static explicit operator bool(JsonData data)
		{
			if (data.type != JsonType.Boolean)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold a double");
			}
			return data.inst_boolean;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00005BEB File Offset: 0x00003DEB
		public static explicit operator double(JsonData data)
		{
			if (data.type != JsonType.Double)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold a double");
			}
			return data.inst_double;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00005C07 File Offset: 0x00003E07
		public static explicit operator int(JsonData data)
		{
			if (data.type != JsonType.Int && data.type != JsonType.Long)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold an int");
			}
			if (data.type != JsonType.Int)
			{
				return (int)data.inst_long;
			}
			return data.inst_int;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00005C3D File Offset: 0x00003E3D
		public static explicit operator long(JsonData data)
		{
			if (data.type != JsonType.Long && data.type != JsonType.Int)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold a long");
			}
			if (data.type != JsonType.Long)
			{
				return (long)data.inst_int;
			}
			return data.inst_long;
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00005C73 File Offset: 0x00003E73
		public static explicit operator string(JsonData data)
		{
			if (data.type != JsonType.String)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold a string");
			}
			return data.inst_string;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00005C8F File Offset: 0x00003E8F
		void ICollection.CopyTo(Array array, int index)
		{
			this.EnsureCollection().CopyTo(array, index);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00005CA0 File Offset: 0x00003EA0
		void IDictionary.Add(object key, object value)
		{
			JsonData jsonData = this.ToJsonData(value);
			this.EnsureDictionary().Add(key, jsonData);
			KeyValuePair<string, JsonData> keyValuePair = new KeyValuePair<string, JsonData>((string)key, jsonData);
			this.object_list.Add(keyValuePair);
			this.json = null;
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00005CE3 File Offset: 0x00003EE3
		void IDictionary.Clear()
		{
			this.EnsureDictionary().Clear();
			this.object_list.Clear();
			this.json = null;
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00005D02 File Offset: 0x00003F02
		bool IDictionary.Contains(object key)
		{
			return this.EnsureDictionary().Contains(key);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00005D10 File Offset: 0x00003F10
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return ((IOrderedDictionary)this).GetEnumerator();
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00005D18 File Offset: 0x00003F18
		void IDictionary.Remove(object key)
		{
			this.EnsureDictionary().Remove(key);
			for (int i = 0; i < this.object_list.Count; i++)
			{
				if (this.object_list[i].Key == (string)key)
				{
					this.object_list.RemoveAt(i);
					break;
				}
			}
			this.json = null;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00005D7D File Offset: 0x00003F7D
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.EnsureCollection().GetEnumerator();
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00005D8A File Offset: 0x00003F8A
		bool IJsonWrapper.GetBoolean()
		{
			if (this.type != JsonType.Boolean)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a boolean");
			}
			return this.inst_boolean;
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00005DA6 File Offset: 0x00003FA6
		double IJsonWrapper.GetDouble()
		{
			if (this.type != JsonType.Double)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a double");
			}
			return this.inst_double;
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00005DC2 File Offset: 0x00003FC2
		int IJsonWrapper.GetInt()
		{
			if (this.type != JsonType.Int)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold an int");
			}
			return this.inst_int;
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00005DDE File Offset: 0x00003FDE
		long IJsonWrapper.GetLong()
		{
			if (this.type != JsonType.Long)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a long");
			}
			return this.inst_long;
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00005DFA File Offset: 0x00003FFA
		string IJsonWrapper.GetString()
		{
			if (this.type != JsonType.String)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a string");
			}
			return this.inst_string;
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00005E16 File Offset: 0x00004016
		void IJsonWrapper.SetBoolean(bool val)
		{
			this.type = JsonType.Boolean;
			this.inst_boolean = val;
			this.json = null;
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00005E2D File Offset: 0x0000402D
		void IJsonWrapper.SetDouble(double val)
		{
			this.type = JsonType.Double;
			this.inst_double = val;
			this.json = null;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00005E44 File Offset: 0x00004044
		void IJsonWrapper.SetInt(int val)
		{
			this.type = JsonType.Int;
			this.inst_int = val;
			this.json = null;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00005E5B File Offset: 0x0000405B
		void IJsonWrapper.SetLong(long val)
		{
			this.type = JsonType.Long;
			this.inst_long = val;
			this.json = null;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00005E72 File Offset: 0x00004072
		void IJsonWrapper.SetString(string val)
		{
			this.type = JsonType.String;
			this.inst_string = val;
			this.json = null;
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00005E89 File Offset: 0x00004089
		string IJsonWrapper.ToJson()
		{
			return this.ToJson();
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00005E91 File Offset: 0x00004091
		void IJsonWrapper.ToJson(JsonWriter writer)
		{
			this.ToJson(writer);
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00005E9A File Offset: 0x0000409A
		int IList.Add(object value)
		{
			return this.Add(value);
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00005EA3 File Offset: 0x000040A3
		void IList.Clear()
		{
			this.EnsureList().Clear();
			this.json = null;
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00005EB7 File Offset: 0x000040B7
		bool IList.Contains(object value)
		{
			return this.EnsureList().Contains(value);
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00005EC5 File Offset: 0x000040C5
		int IList.IndexOf(object value)
		{
			return this.EnsureList().IndexOf(value);
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00005ED3 File Offset: 0x000040D3
		void IList.Insert(int index, object value)
		{
			this.EnsureList().Insert(index, value);
			this.json = null;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00005EE9 File Offset: 0x000040E9
		void IList.Remove(object value)
		{
			this.EnsureList().Remove(value);
			this.json = null;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00005EFE File Offset: 0x000040FE
		void IList.RemoveAt(int index)
		{
			this.EnsureList().RemoveAt(index);
			this.json = null;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00005F13 File Offset: 0x00004113
		IDictionaryEnumerator IOrderedDictionary.GetEnumerator()
		{
			this.EnsureDictionary();
			return new OrderedDictionaryEnumerator(this.object_list.GetEnumerator());
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00005F2C File Offset: 0x0000412C
		void IOrderedDictionary.Insert(int idx, object key, object value)
		{
			string text = (string)key;
			JsonData jsonData = this.ToJsonData(value);
			this[text] = jsonData;
			KeyValuePair<string, JsonData> keyValuePair = new KeyValuePair<string, JsonData>(text, jsonData);
			this.object_list.Insert(idx, keyValuePair);
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00005F68 File Offset: 0x00004168
		void IOrderedDictionary.RemoveAt(int idx)
		{
			this.EnsureDictionary();
			this.inst_object.Remove(this.object_list[idx].Key);
			this.object_list.RemoveAt(idx);
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00005FA8 File Offset: 0x000041A8
		private ICollection EnsureCollection()
		{
			if (this.type == JsonType.Array)
			{
				return (ICollection)this.inst_array;
			}
			if (this.type == JsonType.Object)
			{
				return (ICollection)this.inst_object;
			}
			throw new InvalidOperationException("The JsonData instance has to be initialized first");
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00005FE0 File Offset: 0x000041E0
		private IDictionary EnsureDictionary()
		{
			if (this.type == JsonType.Object)
			{
				return (IDictionary)this.inst_object;
			}
			if (this.type != JsonType.None)
			{
				throw new InvalidOperationException("Instance of JsonData is not a dictionary");
			}
			this.type = JsonType.Object;
			this.inst_object = new Dictionary<string, JsonData>();
			this.object_list = new List<KeyValuePair<string, JsonData>>();
			return (IDictionary)this.inst_object;
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00006040 File Offset: 0x00004240
		private IList EnsureList()
		{
			if (this.type == JsonType.Array)
			{
				return (IList)this.inst_array;
			}
			if (this.type != JsonType.None)
			{
				throw new InvalidOperationException("Instance of JsonData is not a list");
			}
			this.type = JsonType.Array;
			this.inst_array = new List<JsonData>();
			return (IList)this.inst_array;
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00006092 File Offset: 0x00004292
		private JsonData ToJsonData(object obj)
		{
			if (obj == null)
			{
				return null;
			}
			if (obj is JsonData)
			{
				return (JsonData)obj;
			}
			return new JsonData(obj);
		}

		// Token: 0x06000112 RID: 274 RVA: 0x000060B0 File Offset: 0x000042B0
		private static void WriteJson(IJsonWrapper obj, JsonWriter writer)
		{
			if (obj == null)
			{
				writer.Write(null);
				return;
			}
			if (obj.IsString)
			{
				writer.Write(obj.GetString());
				return;
			}
			if (obj.IsBoolean)
			{
				writer.Write(obj.GetBoolean());
				return;
			}
			if (obj.IsDouble)
			{
				writer.Write(obj.GetDouble());
				return;
			}
			if (obj.IsInt)
			{
				writer.Write(obj.GetInt());
				return;
			}
			if (obj.IsLong)
			{
				writer.Write(obj.GetLong());
				return;
			}
			if (obj.IsArray)
			{
				writer.WriteArrayStart();
				foreach (object obj2 in obj)
				{
					JsonData.WriteJson((JsonData)obj2, writer);
				}
				writer.WriteArrayEnd();
				return;
			}
			if (obj.IsObject)
			{
				writer.WriteObjectStart();
				foreach (object obj3 in obj)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj3;
					writer.WritePropertyName((string)dictionaryEntry.Key);
					JsonData.WriteJson((JsonData)dictionaryEntry.Value, writer);
				}
				writer.WriteObjectEnd();
				return;
			}
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00006200 File Offset: 0x00004400
		public int Add(object value)
		{
			JsonData jsonData = this.ToJsonData(value);
			this.json = null;
			return this.EnsureList().Add(jsonData);
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00006228 File Offset: 0x00004428
		public bool Remove(object obj)
		{
			this.json = null;
			if (this.IsObject)
			{
				JsonData jsonData = null;
				if (this.inst_object.TryGetValue((string)obj, out jsonData))
				{
					return this.inst_object.Remove((string)obj) && this.object_list.Remove(new KeyValuePair<string, JsonData>((string)obj, jsonData));
				}
				throw new KeyNotFoundException("The specified key was not found in the JsonData object.");
			}
			else
			{
				if (this.IsArray)
				{
					return this.inst_array.Remove(this.ToJsonData(obj));
				}
				throw new InvalidOperationException("Instance of JsonData is not an object or a list.");
			}
		}

		// Token: 0x06000115 RID: 277 RVA: 0x000062B8 File Offset: 0x000044B8
		public void Clear()
		{
			if (this.IsObject)
			{
				((IDictionary)this).Clear();
				return;
			}
			if (this.IsArray)
			{
				((IList)this).Clear();
				return;
			}
		}

		// Token: 0x06000116 RID: 278 RVA: 0x000062D8 File Offset: 0x000044D8
		public bool Equals(JsonData x)
		{
			if (x == null)
			{
				return false;
			}
			if (x.type != this.type && ((x.type != JsonType.Int && x.type != JsonType.Long) || (this.type != JsonType.Int && this.type != JsonType.Long)))
			{
				return false;
			}
			switch (this.type)
			{
			case JsonType.None:
				return true;
			case JsonType.Object:
				return this.inst_object.Equals(x.inst_object);
			case JsonType.Array:
				return this.inst_array.Equals(x.inst_array);
			case JsonType.String:
				return this.inst_string.Equals(x.inst_string);
			case JsonType.Int:
				if (x.IsLong)
				{
					return x.inst_long >= -2147483648L && x.inst_long <= 2147483647L && this.inst_int.Equals((int)x.inst_long);
				}
				return this.inst_int.Equals(x.inst_int);
			case JsonType.Long:
				if (x.IsInt)
				{
					return this.inst_long >= -2147483648L && this.inst_long <= 2147483647L && x.inst_int.Equals((int)this.inst_long);
				}
				return this.inst_long.Equals(x.inst_long);
			case JsonType.Double:
				return this.inst_double.Equals(x.inst_double);
			case JsonType.Boolean:
				return this.inst_boolean.Equals(x.inst_boolean);
			default:
				return false;
			}
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00006443 File Offset: 0x00004643
		public JsonType GetJsonType()
		{
			return this.type;
		}

		// Token: 0x06000118 RID: 280 RVA: 0x0000644C File Offset: 0x0000464C
		public void SetJsonType(JsonType type)
		{
			if (this.type == type)
			{
				return;
			}
			switch (type)
			{
			case JsonType.Object:
				this.inst_object = new Dictionary<string, JsonData>();
				this.object_list = new List<KeyValuePair<string, JsonData>>();
				break;
			case JsonType.Array:
				this.inst_array = new List<JsonData>();
				break;
			case JsonType.String:
				this.inst_string = null;
				break;
			case JsonType.Int:
				this.inst_int = 0;
				break;
			case JsonType.Long:
				this.inst_long = 0L;
				break;
			case JsonType.Double:
				this.inst_double = 0.0;
				break;
			case JsonType.Boolean:
				this.inst_boolean = false;
				break;
			}
			this.type = type;
		}

		// Token: 0x06000119 RID: 281 RVA: 0x000064EC File Offset: 0x000046EC
		public string ToJson()
		{
			if (this.json != null)
			{
				return this.json;
			}
			StringWriter stringWriter = new StringWriter();
			JsonData.WriteJson(this, new JsonWriter(stringWriter)
			{
				Validate = false
			});
			this.json = stringWriter.ToString();
			return this.json;
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00006538 File Offset: 0x00004738
		public void ToJson(JsonWriter writer)
		{
			bool validate = writer.Validate;
			writer.Validate = false;
			JsonData.WriteJson(this, writer);
			writer.Validate = validate;
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00006564 File Offset: 0x00004764
		public override string ToString()
		{
			switch (this.type)
			{
			case JsonType.Object:
				return "JsonData object";
			case JsonType.Array:
				return "JsonData array";
			case JsonType.String:
				return this.inst_string;
			case JsonType.Int:
				return this.inst_int.ToString();
			case JsonType.Long:
				return this.inst_long.ToString();
			case JsonType.Double:
				return this.inst_double.ToString();
			case JsonType.Boolean:
				return this.inst_boolean.ToString();
			default:
				return "Uninitialized JsonData";
			}
		}

		// Token: 0x04000199 RID: 409
		private IList<JsonData> inst_array;

		// Token: 0x0400019A RID: 410
		private bool inst_boolean;

		// Token: 0x0400019B RID: 411
		private double inst_double;

		// Token: 0x0400019C RID: 412
		private int inst_int;

		// Token: 0x0400019D RID: 413
		private long inst_long;

		// Token: 0x0400019E RID: 414
		private IDictionary<string, JsonData> inst_object;

		// Token: 0x0400019F RID: 415
		private string inst_string;

		// Token: 0x040001A0 RID: 416
		private string json;

		// Token: 0x040001A1 RID: 417
		private JsonType type;

		// Token: 0x040001A2 RID: 418
		private IList<KeyValuePair<string, JsonData>> object_list;
	}
}
