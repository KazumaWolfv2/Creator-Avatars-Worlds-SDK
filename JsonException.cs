using System;

namespace LitJson
{
	// Token: 0x0200004E RID: 78
	public class JsonException : ApplicationException
	{
		// Token: 0x06000123 RID: 291 RVA: 0x00006689 File Offset: 0x00004889
		public JsonException()
		{
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00006691 File Offset: 0x00004891
		internal JsonException(ParserToken token)
			: base(string.Format("Invalid token '{0}' in input string", token))
		{
		}

		// Token: 0x06000125 RID: 293 RVA: 0x000066A9 File Offset: 0x000048A9
		internal JsonException(ParserToken token, Exception inner_exception)
			: base(string.Format("Invalid token '{0}' in input string", token), inner_exception)
		{
		}

		// Token: 0x06000126 RID: 294 RVA: 0x000066C2 File Offset: 0x000048C2
		internal JsonException(int c)
			: base(string.Format("Invalid character '{0}' in input string", (char)c))
		{
		}

		// Token: 0x06000127 RID: 295 RVA: 0x000066DB File Offset: 0x000048DB
		internal JsonException(int c, Exception inner_exception)
			: base(string.Format("Invalid character '{0}' in input string", (char)c), inner_exception)
		{
		}

		// Token: 0x06000128 RID: 296 RVA: 0x000066F5 File Offset: 0x000048F5
		public JsonException(string message)
			: base(message)
		{
		}

		// Token: 0x06000129 RID: 297 RVA: 0x000066FE File Offset: 0x000048FE
		public JsonException(string message, Exception inner_exception)
			: base(message, inner_exception)
		{
		}
	}
}
