using System;
using System.Collections.Generic;

namespace LitJson
{
	// Token: 0x02000051 RID: 81
	internal struct ObjectMetadata
	{
		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000130 RID: 304 RVA: 0x00006754 File Offset: 0x00004954
		// (set) Token: 0x06000131 RID: 305 RVA: 0x00006775 File Offset: 0x00004975
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

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000132 RID: 306 RVA: 0x0000677E File Offset: 0x0000497E
		// (set) Token: 0x06000133 RID: 307 RVA: 0x00006786 File Offset: 0x00004986
		public bool IsDictionary
		{
			get
			{
				return this.is_dictionary;
			}
			set
			{
				this.is_dictionary = value;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000134 RID: 308 RVA: 0x0000678F File Offset: 0x0000498F
		// (set) Token: 0x06000135 RID: 309 RVA: 0x00006797 File Offset: 0x00004997
		public IDictionary<string, PropertyMetadata> Properties
		{
			get
			{
				return this.properties;
			}
			set
			{
				this.properties = value;
			}
		}

		// Token: 0x040001AA RID: 426
		private Type element_type;

		// Token: 0x040001AB RID: 427
		private bool is_dictionary;

		// Token: 0x040001AC RID: 428
		private IDictionary<string, PropertyMetadata> properties;
	}
}
