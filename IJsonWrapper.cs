using System;
using System.Collections;
using System.Collections.Specialized;

namespace LitJson
{
	// Token: 0x0200004B RID: 75
	public interface IJsonWrapper : IList, ICollection, IEnumerable, IOrderedDictionary, IDictionary
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x060000A7 RID: 167
		bool IsArray { get; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x060000A8 RID: 168
		bool IsBoolean { get; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x060000A9 RID: 169
		bool IsDouble { get; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060000AA RID: 170
		bool IsInt { get; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060000AB RID: 171
		bool IsLong { get; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060000AC RID: 172
		bool IsObject { get; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060000AD RID: 173
		bool IsString { get; }

		// Token: 0x060000AE RID: 174
		bool GetBoolean();

		// Token: 0x060000AF RID: 175
		double GetDouble();

		// Token: 0x060000B0 RID: 176
		int GetInt();

		// Token: 0x060000B1 RID: 177
		JsonType GetJsonType();

		// Token: 0x060000B2 RID: 178
		long GetLong();

		// Token: 0x060000B3 RID: 179
		string GetString();

		// Token: 0x060000B4 RID: 180
		void SetBoolean(bool val);

		// Token: 0x060000B5 RID: 181
		void SetDouble(double val);

		// Token: 0x060000B6 RID: 182
		void SetInt(int val);

		// Token: 0x060000B7 RID: 183
		void SetJsonType(JsonType type);

		// Token: 0x060000B8 RID: 184
		void SetLong(long val);

		// Token: 0x060000B9 RID: 185
		void SetString(string val);

		// Token: 0x060000BA RID: 186
		string ToJson();

		// Token: 0x060000BB RID: 187
		void ToJson(JsonWriter writer);
	}
}
