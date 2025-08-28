using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace LitJson
{
	// Token: 0x0200005D RID: 93
	public class JsonWriter
	{
		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x00008770 File Offset: 0x00006970
		// (set) Token: 0x060001B4 RID: 436 RVA: 0x00008778 File Offset: 0x00006978
		public int IndentValue
		{
			get
			{
				return this.indent_value;
			}
			set
			{
				this.indentation = this.indentation / this.indent_value * value;
				this.indent_value = value;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x00008796 File Offset: 0x00006996
		// (set) Token: 0x060001B6 RID: 438 RVA: 0x0000879E File Offset: 0x0000699E
		public bool PrettyPrint
		{
			get
			{
				return this.pretty_print;
			}
			set
			{
				this.pretty_print = value;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x000087A7 File Offset: 0x000069A7
		public TextWriter TextWriter
		{
			get
			{
				return this.writer;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x000087AF File Offset: 0x000069AF
		// (set) Token: 0x060001B9 RID: 441 RVA: 0x000087B7 File Offset: 0x000069B7
		public bool Validate
		{
			get
			{
				return this.validate;
			}
			set
			{
				this.validate = value;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001BA RID: 442 RVA: 0x000087C0 File Offset: 0x000069C0
		// (set) Token: 0x060001BB RID: 443 RVA: 0x000087C8 File Offset: 0x000069C8
		public bool LowerCaseProperties
		{
			get
			{
				return this.lower_case_properties;
			}
			set
			{
				this.lower_case_properties = value;
			}
		}

		// Token: 0x060001BD RID: 445 RVA: 0x000087DD File Offset: 0x000069DD
		public JsonWriter()
		{
			this.inst_string_builder = new StringBuilder();
			this.writer = new StringWriter(this.inst_string_builder);
			this.Init();
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00008807 File Offset: 0x00006A07
		public JsonWriter(StringBuilder sb)
			: this(new StringWriter(sb))
		{
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00008815 File Offset: 0x00006A15
		public JsonWriter(TextWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			this.writer = writer;
			this.Init();
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00008838 File Offset: 0x00006A38
		private void DoValidation(Condition cond)
		{
			if (!this.context.ExpectingValue)
			{
				this.context.Count++;
			}
			if (!this.validate)
			{
				return;
			}
			if (this.has_reached_end)
			{
				throw new JsonException("A complete JSON symbol has already been written");
			}
			switch (cond)
			{
			case Condition.InArray:
				if (!this.context.InArray)
				{
					throw new JsonException("Can't close an array here");
				}
				break;
			case Condition.InObject:
				if (!this.context.InObject || this.context.ExpectingValue)
				{
					throw new JsonException("Can't close an object here");
				}
				break;
			case Condition.NotAProperty:
				if (this.context.InObject && !this.context.ExpectingValue)
				{
					throw new JsonException("Expected a property");
				}
				break;
			case Condition.Property:
				if (!this.context.InObject || this.context.ExpectingValue)
				{
					throw new JsonException("Can't add a property here");
				}
				break;
			case Condition.Value:
				if (!this.context.InArray && (!this.context.InObject || !this.context.ExpectingValue))
				{
					throw new JsonException("Can't add a value here");
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0000895C File Offset: 0x00006B5C
		private void Init()
		{
			this.has_reached_end = false;
			this.hex_seq = new char[4];
			this.indentation = 0;
			this.indent_value = 4;
			this.pretty_print = false;
			this.validate = true;
			this.lower_case_properties = false;
			this.ctx_stack = new Stack<WriterContext>();
			this.context = new WriterContext();
			this.ctx_stack.Push(this.context);
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x000089C8 File Offset: 0x00006BC8
		private static void IntToHex(int n, char[] hex)
		{
			for (int i = 0; i < 4; i++)
			{
				int num = n % 16;
				if (num < 10)
				{
					hex[3 - i] = (char)(48 + num);
				}
				else
				{
					hex[3 - i] = (char)(65 + (num - 10));
				}
				n >>= 4;
			}
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00008A09 File Offset: 0x00006C09
		private void Indent()
		{
			if (this.pretty_print)
			{
				this.indentation += this.indent_value;
			}
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00008A28 File Offset: 0x00006C28
		private void Put(string str)
		{
			if (this.pretty_print && !this.context.ExpectingValue)
			{
				for (int i = 0; i < this.indentation; i++)
				{
					this.writer.Write(' ');
				}
			}
			this.writer.Write(str);
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x00008A74 File Offset: 0x00006C74
		private void PutNewline()
		{
			this.PutNewline(true);
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00008A80 File Offset: 0x00006C80
		private void PutNewline(bool add_comma)
		{
			if (add_comma && !this.context.ExpectingValue && this.context.Count > 1)
			{
				this.writer.Write(',');
			}
			if (this.pretty_print && !this.context.ExpectingValue)
			{
				this.writer.Write(Environment.NewLine);
			}
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x00008AE0 File Offset: 0x00006CE0
		private void PutString(string str)
		{
			this.Put(string.Empty);
			this.writer.Write('"');
			int length = str.Length;
			int i = 0;
			while (i < length)
			{
				char c = str[i];
				switch (c)
				{
				case '\b':
					this.writer.Write("\\b");
					break;
				case '\t':
					this.writer.Write("\\t");
					break;
				case '\n':
					this.writer.Write("\\n");
					break;
				case '\v':
					goto IL_00E4;
				case '\f':
					this.writer.Write("\\f");
					break;
				case '\r':
					this.writer.Write("\\r");
					break;
				default:
					if (c != '"' && c != '\\')
					{
						goto IL_00E4;
					}
					this.writer.Write('\\');
					this.writer.Write(str[i]);
					break;
				}
				IL_0141:
				i++;
				continue;
				IL_00E4:
				if (str[i] >= ' ' && str[i] <= '~')
				{
					this.writer.Write(str[i]);
					goto IL_0141;
				}
				JsonWriter.IntToHex((int)str[i], this.hex_seq);
				this.writer.Write("\\u");
				this.writer.Write(this.hex_seq);
				goto IL_0141;
			}
			this.writer.Write('"');
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x00008C46 File Offset: 0x00006E46
		private void Unindent()
		{
			if (this.pretty_print)
			{
				this.indentation -= this.indent_value;
			}
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00008C63 File Offset: 0x00006E63
		public override string ToString()
		{
			if (this.inst_string_builder == null)
			{
				return string.Empty;
			}
			return this.inst_string_builder.ToString();
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00008C80 File Offset: 0x00006E80
		public void Reset()
		{
			this.has_reached_end = false;
			this.ctx_stack.Clear();
			this.context = new WriterContext();
			this.ctx_stack.Push(this.context);
			if (this.inst_string_builder != null)
			{
				this.inst_string_builder.Remove(0, this.inst_string_builder.Length);
			}
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00008CDB File Offset: 0x00006EDB
		public void Write(bool boolean)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(boolean ? "true" : "false");
			this.context.ExpectingValue = false;
		}

		// Token: 0x060001CC RID: 460 RVA: 0x00008D0B File Offset: 0x00006F0B
		public void Write(decimal number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(Convert.ToString(number, JsonWriter.number_format));
			this.context.ExpectingValue = false;
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00008D38 File Offset: 0x00006F38
		public void Write(double number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			string text = Convert.ToString(number, JsonWriter.number_format);
			this.Put(text);
			if (text.IndexOf('.') == -1 && text.IndexOf('E') == -1)
			{
				this.writer.Write(".0");
			}
			this.context.ExpectingValue = false;
		}

		// Token: 0x060001CE RID: 462 RVA: 0x00008D98 File Offset: 0x00006F98
		public void Write(float number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			string text = Convert.ToString(number, JsonWriter.number_format);
			this.Put(text);
			this.context.ExpectingValue = false;
		}

		// Token: 0x060001CF RID: 463 RVA: 0x00008DD1 File Offset: 0x00006FD1
		public void Write(int number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(Convert.ToString(number, JsonWriter.number_format));
			this.context.ExpectingValue = false;
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x00008DFD File Offset: 0x00006FFD
		public void Write(long number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(Convert.ToString(number, JsonWriter.number_format));
			this.context.ExpectingValue = false;
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x00008E29 File Offset: 0x00007029
		public void Write(string str)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			if (str == null)
			{
				this.Put("null");
			}
			else
			{
				this.PutString(str);
			}
			this.context.ExpectingValue = false;
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00008E5B File Offset: 0x0000705B
		[CLSCompliant(false)]
		public void Write(ulong number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(Convert.ToString(number, JsonWriter.number_format));
			this.context.ExpectingValue = false;
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00008E88 File Offset: 0x00007088
		public void WriteArrayEnd()
		{
			this.DoValidation(Condition.InArray);
			this.PutNewline(false);
			this.ctx_stack.Pop();
			if (this.ctx_stack.Count == 1)
			{
				this.has_reached_end = true;
			}
			else
			{
				this.context = this.ctx_stack.Peek();
				this.context.ExpectingValue = false;
			}
			this.Unindent();
			this.Put("]");
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x00008EF4 File Offset: 0x000070F4
		public void WriteArrayStart()
		{
			this.DoValidation(Condition.NotAProperty);
			this.PutNewline();
			this.Put("[");
			this.context = new WriterContext();
			this.context.InArray = true;
			this.ctx_stack.Push(this.context);
			this.Indent();
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x00008F48 File Offset: 0x00007148
		public void WriteObjectEnd()
		{
			this.DoValidation(Condition.InObject);
			this.PutNewline(false);
			this.ctx_stack.Pop();
			if (this.ctx_stack.Count == 1)
			{
				this.has_reached_end = true;
			}
			else
			{
				this.context = this.ctx_stack.Peek();
				this.context.ExpectingValue = false;
			}
			this.Unindent();
			this.Put("}");
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x00008FB4 File Offset: 0x000071B4
		public void WriteObjectStart()
		{
			this.DoValidation(Condition.NotAProperty);
			this.PutNewline();
			this.Put("{");
			this.context = new WriterContext();
			this.context.InObject = true;
			this.ctx_stack.Push(this.context);
			this.Indent();
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x00009008 File Offset: 0x00007208
		public void WritePropertyName(string property_name)
		{
			this.DoValidation(Condition.Property);
			this.PutNewline();
			string text = ((property_name == null || !this.lower_case_properties) ? property_name : property_name.ToLowerInvariant());
			this.PutString(text);
			if (this.pretty_print)
			{
				if (text.Length > this.context.Padding)
				{
					this.context.Padding = text.Length;
				}
				for (int i = this.context.Padding - text.Length; i >= 0; i--)
				{
					this.writer.Write(' ');
				}
				this.writer.Write(": ");
			}
			else
			{
				this.writer.Write(':');
			}
			this.context.ExpectingValue = true;
		}

		// Token: 0x040001E4 RID: 484
		private static readonly NumberFormatInfo number_format = NumberFormatInfo.InvariantInfo;

		// Token: 0x040001E5 RID: 485
		private WriterContext context;

		// Token: 0x040001E6 RID: 486
		private Stack<WriterContext> ctx_stack;

		// Token: 0x040001E7 RID: 487
		private bool has_reached_end;

		// Token: 0x040001E8 RID: 488
		private char[] hex_seq;

		// Token: 0x040001E9 RID: 489
		private int indentation;

		// Token: 0x040001EA RID: 490
		private int indent_value;

		// Token: 0x040001EB RID: 491
		private StringBuilder inst_string_builder;

		// Token: 0x040001EC RID: 492
		private bool pretty_print;

		// Token: 0x040001ED RID: 493
		private bool validate;

		// Token: 0x040001EE RID: 494
		private bool lower_case_properties;

		// Token: 0x040001EF RID: 495
		private TextWriter writer;
	}
}
