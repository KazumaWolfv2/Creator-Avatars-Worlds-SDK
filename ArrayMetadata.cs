using System;

namespace LitJson
{
	// Token: 0x02000050 RID: 80
	internal struct ArrayMetadata
	{
		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600012A RID: 298 RVA: 0x00006708 File Offset: 0x00004908
		// (set) Token: 0x0600012B RID: 299 RVA: 0x00006729 File Offset: 0x00004929
		public Type ElementType
		{
			get
			{
				if (this.element_type == null)
				{
					return typeof(JsonData);
				}
				return this.element_type;
			}
			set
			{
				this.element_type = value;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600012C RID: 300 RVA: 0x00006732 File Offset: 0x00004932
		// (set) Token: 0x0600012D RID: 301 RVA: 0x0000673A File Offset: 0x0000493A
		public bool IsArray
		{
			get
			{
				return this.is_array;
			}
			set
			{
				this.is_array = value;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600012E RID: 302 RVA: 0x00006743 File Offset: 0x00004943
		// (set) Token: 0x0600012F RID: 303 RVA: 0x0000674B File Offset: 0x0000494B
		public bool IsList
		{
			get
			{
				return this.is_list;
			}
			set
			{
				this.is_list = value;
			}
		}

		// Token: 0x040001A7 RID: 423
		private Type element_type;

		// Token: 0x040001A8 RID: 424
		private bool is_array;

		// Token: 0x040001A9 RID: 425
		private bool is_list;
	}
}
