using System;
using System.IO;
using System.Text;

namespace LitJson
{
	// Token: 0x0200005F RID: 95
	internal class Lexer
	{
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x000090BF File Offset: 0x000072BF
		// (set) Token: 0x060001DA RID: 474 RVA: 0x000090C7 File Offset: 0x000072C7
		public bool AllowComments
		{
			get
			{
				return this.allow_comments;
			}
			set
			{
				this.allow_comments = value;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060001DB RID: 475 RVA: 0x000090D0 File Offset: 0x000072D0
		// (set) Token: 0x060001DC RID: 476 RVA: 0x000090D8 File Offset: 0x000072D8
		public bool AllowSingleQuotedStrings
		{
			get
			{
				return this.allow_single_quoted_strings;
			}
			set
			{
				this.allow_single_quoted_strings = value;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060001DD RID: 477 RVA: 0x000090E1 File Offset: 0x000072E1
		public bool EndOfInput
		{
			get
			{
				return this.end_of_input;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060001DE RID: 478 RVA: 0x000090E9 File Offset: 0x000072E9
		public int Token
		{
			get
			{
				return this.token;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060001DF RID: 479 RVA: 0x000090F1 File Offset: 0x000072F1
		public string StringValue
		{
			get
			{
				return this.string_value;
			}
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x000090F9 File Offset: 0x000072F9
		static Lexer()
		{
			Lexer.PopulateFsmTables(out Lexer.fsm_handler_table, out Lexer.fsm_return_table);
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000910C File Offset: 0x0000730C
		public Lexer(TextReader reader)
		{
			this.allow_comments = true;
			this.allow_single_quoted_strings = true;
			this.input_buffer = 0;
			this.string_buffer = new StringBuilder(128);
			this.state = 1;
			this.end_of_input = false;
			this.reader = reader;
			this.fsm_context = new FsmContext();
			this.fsm_context.L = this;
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00009170 File Offset: 0x00007370
		private static int HexValue(int digit)
		{
			switch (digit)
			{
			case 65:
				break;
			case 66:
				return 11;
			case 67:
				return 12;
			case 68:
				return 13;
			case 69:
				return 14;
			case 70:
				return 15;
			default:
				switch (digit)
				{
				case 97:
					break;
				case 98:
					return 11;
				case 99:
					return 12;
				case 100:
					return 13;
				case 101:
					return 14;
				case 102:
					return 15;
				default:
					return digit - 48;
				}
				break;
			}
			return 10;
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x000091D8 File Offset: 0x000073D8
		private static void PopulateFsmTables(out Lexer.StateHandler[] fsm_handler_table, out int[] fsm_return_table)
		{
			fsm_handler_table = new Lexer.StateHandler[]
			{
				new Lexer.StateHandler(Lexer.State1),
				new Lexer.StateHandler(Lexer.State2),
				new Lexer.StateHandler(Lexer.State3),
				new Lexer.StateHandler(Lexer.State4),
				new Lexer.StateHandler(Lexer.State5),
				new Lexer.StateHandler(Lexer.State6),
				new Lexer.StateHandler(Lexer.State7),
				new Lexer.StateHandler(Lexer.State8),
				new Lexer.StateHandler(Lexer.State9),
				new Lexer.StateHandler(Lexer.State10),
				new Lexer.StateHandler(Lexer.State11),
				new Lexer.StateHandler(Lexer.State12),
				new Lexer.StateHandler(Lexer.State13),
				new Lexer.StateHandler(Lexer.State14),
				new Lexer.StateHandler(Lexer.State15),
				new Lexer.StateHandler(Lexer.State16),
				new Lexer.StateHandler(Lexer.State17),
				new Lexer.StateHandler(Lexer.State18),
				new Lexer.StateHandler(Lexer.State19),
				new Lexer.StateHandler(Lexer.State20),
				new Lexer.StateHandler(Lexer.State21),
				new Lexer.StateHandler(Lexer.State22),
				new Lexer.StateHandler(Lexer.State23),
				new Lexer.StateHandler(Lexer.State24),
				new Lexer.StateHandler(Lexer.State25),
				new Lexer.StateHandler(Lexer.State26),
				new Lexer.StateHandler(Lexer.State27),
				new Lexer.StateHandler(Lexer.State28)
			};
			fsm_return_table = new int[]
			{
				65542, 0, 65537, 65537, 0, 65537, 0, 65537, 0, 0,
				65538, 0, 0, 0, 65539, 0, 0, 65540, 65541, 65542,
				0, 0, 65541, 65542, 0, 0, 0, 0
			};
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x000093BC File Offset: 0x000075BC
		private static char ProcessEscChar(int esc_char)
		{
			if (esc_char <= 92)
			{
				if (esc_char <= 39)
				{
					if (esc_char != 34 && esc_char != 39)
					{
						return '?';
					}
				}
				else if (esc_char != 47 && esc_char != 92)
				{
					return '?';
				}
				return Convert.ToChar(esc_char);
			}
			if (esc_char <= 102)
			{
				if (esc_char == 98)
				{
					return '\b';
				}
				if (esc_char == 102)
				{
					return '\f';
				}
			}
			else
			{
				if (esc_char == 110)
				{
					return '\n';
				}
				if (esc_char == 114)
				{
					return '\r';
				}
				if (esc_char == 116)
				{
					return '\t';
				}
			}
			return '?';
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x00009424 File Offset: 0x00007624
		private static bool State1(FsmContext ctx)
		{
			while (ctx.L.GetChar())
			{
				if (ctx.L.input_char != 32 && (ctx.L.input_char < 9 || ctx.L.input_char > 13))
				{
					if (ctx.L.input_char >= 49 && ctx.L.input_char <= 57)
					{
						ctx.L.string_buffer.Append((char)ctx.L.input_char);
						ctx.NextState = 3;
						return true;
					}
					int num = ctx.L.input_char;
					if (num <= 91)
					{
						if (num <= 39)
						{
							if (num == 34)
							{
								ctx.NextState = 19;
								ctx.Return = true;
								return true;
							}
							if (num != 39)
							{
								return false;
							}
							if (!ctx.L.allow_single_quoted_strings)
							{
								return false;
							}
							ctx.L.input_char = 34;
							ctx.NextState = 23;
							ctx.Return = true;
							return true;
						}
						else
						{
							switch (num)
							{
							case 44:
								break;
							case 45:
								ctx.L.string_buffer.Append((char)ctx.L.input_char);
								ctx.NextState = 2;
								return true;
							case 46:
								return false;
							case 47:
								if (!ctx.L.allow_comments)
								{
									return false;
								}
								ctx.NextState = 25;
								return true;
							case 48:
								ctx.L.string_buffer.Append((char)ctx.L.input_char);
								ctx.NextState = 4;
								return true;
							default:
								if (num != 58 && num != 91)
								{
									return false;
								}
								break;
							}
						}
					}
					else if (num <= 110)
					{
						if (num != 93)
						{
							if (num == 102)
							{
								ctx.NextState = 12;
								return true;
							}
							if (num != 110)
							{
								return false;
							}
							ctx.NextState = 16;
							return true;
						}
					}
					else
					{
						if (num == 116)
						{
							ctx.NextState = 9;
							return true;
						}
						if (num != 123 && num != 125)
						{
							return false;
						}
					}
					ctx.NextState = 1;
					ctx.Return = true;
					return true;
				}
			}
			return true;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000961C File Offset: 0x0000781C
		private static bool State2(FsmContext ctx)
		{
			ctx.L.GetChar();
			if (ctx.L.input_char >= 49 && ctx.L.input_char <= 57)
			{
				ctx.L.string_buffer.Append((char)ctx.L.input_char);
				ctx.NextState = 3;
				return true;
			}
			if (ctx.L.input_char == 48)
			{
				ctx.L.string_buffer.Append((char)ctx.L.input_char);
				ctx.NextState = 4;
				return true;
			}
			return false;
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x000096B0 File Offset: 0x000078B0
		private static bool State3(FsmContext ctx)
		{
			while (ctx.L.GetChar())
			{
				if (ctx.L.input_char >= 48 && ctx.L.input_char <= 57)
				{
					ctx.L.string_buffer.Append((char)ctx.L.input_char);
				}
				else
				{
					if (ctx.L.input_char == 32 || (ctx.L.input_char >= 9 && ctx.L.input_char <= 13))
					{
						ctx.Return = true;
						ctx.NextState = 1;
						return true;
					}
					int num = ctx.L.input_char;
					if (num <= 69)
					{
						if (num != 44)
						{
							if (num == 46)
							{
								ctx.L.string_buffer.Append((char)ctx.L.input_char);
								ctx.NextState = 5;
								return true;
							}
							if (num != 69)
							{
								return false;
							}
							goto IL_00F4;
						}
					}
					else if (num != 93)
					{
						if (num == 101)
						{
							goto IL_00F4;
						}
						if (num != 125)
						{
							return false;
						}
					}
					ctx.L.UngetChar();
					ctx.Return = true;
					ctx.NextState = 1;
					return true;
					IL_00F4:
					ctx.L.string_buffer.Append((char)ctx.L.input_char);
					ctx.NextState = 7;
					return true;
				}
			}
			return true;
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x000097EC File Offset: 0x000079EC
		private static bool State4(FsmContext ctx)
		{
			ctx.L.GetChar();
			if (ctx.L.input_char == 32 || (ctx.L.input_char >= 9 && ctx.L.input_char <= 13))
			{
				ctx.Return = true;
				ctx.NextState = 1;
				return true;
			}
			int num = ctx.L.input_char;
			if (num <= 69)
			{
				if (num != 44)
				{
					if (num == 46)
					{
						ctx.L.string_buffer.Append((char)ctx.L.input_char);
						ctx.NextState = 5;
						return true;
					}
					if (num != 69)
					{
						return false;
					}
					goto IL_00BB;
				}
			}
			else if (num != 93)
			{
				if (num == 101)
				{
					goto IL_00BB;
				}
				if (num != 125)
				{
					return false;
				}
			}
			ctx.L.UngetChar();
			ctx.Return = true;
			ctx.NextState = 1;
			return true;
			IL_00BB:
			ctx.L.string_buffer.Append((char)ctx.L.input_char);
			ctx.NextState = 7;
			return true;
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x000098DC File Offset: 0x00007ADC
		private static bool State5(FsmContext ctx)
		{
			ctx.L.GetChar();
			if (ctx.L.input_char >= 48 && ctx.L.input_char <= 57)
			{
				ctx.L.string_buffer.Append((char)ctx.L.input_char);
				ctx.NextState = 6;
				return true;
			}
			return false;
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0000993C File Offset: 0x00007B3C
		private static bool State6(FsmContext ctx)
		{
			while (ctx.L.GetChar())
			{
				if (ctx.L.input_char >= 48 && ctx.L.input_char <= 57)
				{
					ctx.L.string_buffer.Append((char)ctx.L.input_char);
				}
				else
				{
					if (ctx.L.input_char == 32 || (ctx.L.input_char >= 9 && ctx.L.input_char <= 13))
					{
						ctx.Return = true;
						ctx.NextState = 1;
						return true;
					}
					int num = ctx.L.input_char;
					if (num <= 69)
					{
						if (num != 44)
						{
							if (num != 69)
							{
								return false;
							}
							goto IL_00C9;
						}
					}
					else if (num != 93)
					{
						if (num == 101)
						{
							goto IL_00C9;
						}
						if (num != 125)
						{
							return false;
						}
					}
					ctx.L.UngetChar();
					ctx.Return = true;
					ctx.NextState = 1;
					return true;
					IL_00C9:
					ctx.L.string_buffer.Append((char)ctx.L.input_char);
					ctx.NextState = 7;
					return true;
				}
			}
			return true;
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00009A4C File Offset: 0x00007C4C
		private static bool State7(FsmContext ctx)
		{
			ctx.L.GetChar();
			if (ctx.L.input_char >= 48 && ctx.L.input_char <= 57)
			{
				ctx.L.string_buffer.Append((char)ctx.L.input_char);
				ctx.NextState = 8;
				return true;
			}
			int num = ctx.L.input_char;
			if (num == 43 || num == 45)
			{
				ctx.L.string_buffer.Append((char)ctx.L.input_char);
				ctx.NextState = 8;
				return true;
			}
			return false;
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00009AE8 File Offset: 0x00007CE8
		private static bool State8(FsmContext ctx)
		{
			while (ctx.L.GetChar())
			{
				if (ctx.L.input_char >= 48 && ctx.L.input_char <= 57)
				{
					ctx.L.string_buffer.Append((char)ctx.L.input_char);
				}
				else
				{
					if (ctx.L.input_char == 32 || (ctx.L.input_char >= 9 && ctx.L.input_char <= 13))
					{
						ctx.Return = true;
						ctx.NextState = 1;
						return true;
					}
					int num = ctx.L.input_char;
					if (num == 44 || num == 93 || num == 125)
					{
						ctx.L.UngetChar();
						ctx.Return = true;
						ctx.NextState = 1;
						return true;
					}
					return false;
				}
			}
			return true;
		}

		// Token: 0x060001ED RID: 493 RVA: 0x00009BBD File Offset: 0x00007DBD
		private static bool State9(FsmContext ctx)
		{
			ctx.L.GetChar();
			if (ctx.L.input_char == 114)
			{
				ctx.NextState = 10;
				return true;
			}
			return false;
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00009BE5 File Offset: 0x00007DE5
		private static bool State10(FsmContext ctx)
		{
			ctx.L.GetChar();
			if (ctx.L.input_char == 117)
			{
				ctx.NextState = 11;
				return true;
			}
			return false;
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00009C0D File Offset: 0x00007E0D
		private static bool State11(FsmContext ctx)
		{
			ctx.L.GetChar();
			if (ctx.L.input_char == 101)
			{
				ctx.Return = true;
				ctx.NextState = 1;
				return true;
			}
			return false;
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00009C3B File Offset: 0x00007E3B
		private static bool State12(FsmContext ctx)
		{
			ctx.L.GetChar();
			if (ctx.L.input_char == 97)
			{
				ctx.NextState = 13;
				return true;
			}
			return false;
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00009C63 File Offset: 0x00007E63
		private static bool State13(FsmContext ctx)
		{
			ctx.L.GetChar();
			if (ctx.L.input_char == 108)
			{
				ctx.NextState = 14;
				return true;
			}
			return false;
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00009C8B File Offset: 0x00007E8B
		private static bool State14(FsmContext ctx)
		{
			ctx.L.GetChar();
			if (ctx.L.input_char == 115)
			{
				ctx.NextState = 15;
				return true;
			}
			return false;
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00009C0D File Offset: 0x00007E0D
		private static bool State15(FsmContext ctx)
		{
			ctx.L.GetChar();
			if (ctx.L.input_char == 101)
			{
				ctx.Return = true;
				ctx.NextState = 1;
				return true;
			}
			return false;
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00009CB3 File Offset: 0x00007EB3
		private static bool State16(FsmContext ctx)
		{
			ctx.L.GetChar();
			if (ctx.L.input_char == 117)
			{
				ctx.NextState = 17;
				return true;
			}
			return false;
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x00009CDB File Offset: 0x00007EDB
		private static bool State17(FsmContext ctx)
		{
			ctx.L.GetChar();
			if (ctx.L.input_char == 108)
			{
				ctx.NextState = 18;
				return true;
			}
			return false;
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x00009D03 File Offset: 0x00007F03
		private static bool State18(FsmContext ctx)
		{
			ctx.L.GetChar();
			if (ctx.L.input_char == 108)
			{
				ctx.Return = true;
				ctx.NextState = 1;
				return true;
			}
			return false;
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x00009D34 File Offset: 0x00007F34
		private static bool State19(FsmContext ctx)
		{
			while (ctx.L.GetChar())
			{
				int num = ctx.L.input_char;
				if (num == 34)
				{
					ctx.L.UngetChar();
					ctx.Return = true;
					ctx.NextState = 20;
					return true;
				}
				if (num == 92)
				{
					ctx.StateStack = 19;
					ctx.NextState = 21;
					return true;
				}
				ctx.L.string_buffer.Append((char)ctx.L.input_char);
			}
			return true;
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00009DB4 File Offset: 0x00007FB4
		private static bool State20(FsmContext ctx)
		{
			ctx.L.GetChar();
			if (ctx.L.input_char == 34)
			{
				ctx.Return = true;
				ctx.NextState = 1;
				return true;
			}
			return false;
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x00009DE4 File Offset: 0x00007FE4
		private static bool State21(FsmContext ctx)
		{
			ctx.L.GetChar();
			int num = ctx.L.input_char;
			if (num <= 92)
			{
				if (num <= 39)
				{
					if (num != 34 && num != 39)
					{
						return false;
					}
				}
				else if (num != 47 && num != 92)
				{
					return false;
				}
			}
			else if (num <= 102)
			{
				if (num != 98 && num != 102)
				{
					return false;
				}
			}
			else if (num != 110)
			{
				switch (num)
				{
				case 114:
				case 116:
					break;
				case 115:
					return false;
				case 117:
					ctx.NextState = 22;
					return true;
				default:
					return false;
				}
			}
			ctx.L.string_buffer.Append(Lexer.ProcessEscChar(ctx.L.input_char));
			ctx.NextState = ctx.StateStack;
			return true;
		}

		// Token: 0x060001FA RID: 506 RVA: 0x00009E98 File Offset: 0x00008098
		private static bool State22(FsmContext ctx)
		{
			int num = 0;
			int num2 = 4096;
			ctx.L.unichar = 0;
			while (ctx.L.GetChar())
			{
				if ((ctx.L.input_char < 48 || ctx.L.input_char > 57) && (ctx.L.input_char < 65 || ctx.L.input_char > 70) && (ctx.L.input_char < 97 || ctx.L.input_char > 102))
				{
					return false;
				}
				ctx.L.unichar += Lexer.HexValue(ctx.L.input_char) * num2;
				num++;
				num2 /= 16;
				if (num == 4)
				{
					ctx.L.string_buffer.Append(Convert.ToChar(ctx.L.unichar));
					ctx.NextState = ctx.StateStack;
					return true;
				}
			}
			return true;
		}

		// Token: 0x060001FB RID: 507 RVA: 0x00009F8C File Offset: 0x0000818C
		private static bool State23(FsmContext ctx)
		{
			while (ctx.L.GetChar())
			{
				int num = ctx.L.input_char;
				if (num == 39)
				{
					ctx.L.UngetChar();
					ctx.Return = true;
					ctx.NextState = 24;
					return true;
				}
				if (num == 92)
				{
					ctx.StateStack = 23;
					ctx.NextState = 21;
					return true;
				}
				ctx.L.string_buffer.Append((char)ctx.L.input_char);
			}
			return true;
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000A00C File Offset: 0x0000820C
		private static bool State24(FsmContext ctx)
		{
			ctx.L.GetChar();
			if (ctx.L.input_char == 39)
			{
				ctx.L.input_char = 34;
				ctx.Return = true;
				ctx.NextState = 1;
				return true;
			}
			return false;
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000A048 File Offset: 0x00008248
		private static bool State25(FsmContext ctx)
		{
			ctx.L.GetChar();
			int num = ctx.L.input_char;
			if (num == 42)
			{
				ctx.NextState = 27;
				return true;
			}
			if (num != 47)
			{
				return false;
			}
			ctx.NextState = 26;
			return true;
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000A08E File Offset: 0x0000828E
		private static bool State26(FsmContext ctx)
		{
			while (ctx.L.GetChar())
			{
				if (ctx.L.input_char == 10)
				{
					ctx.NextState = 1;
					return true;
				}
			}
			return true;
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000A0B8 File Offset: 0x000082B8
		private static bool State27(FsmContext ctx)
		{
			while (ctx.L.GetChar())
			{
				if (ctx.L.input_char == 42)
				{
					ctx.NextState = 28;
					return true;
				}
			}
			return true;
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000A0E4 File Offset: 0x000082E4
		private static bool State28(FsmContext ctx)
		{
			while (ctx.L.GetChar())
			{
				if (ctx.L.input_char != 42)
				{
					if (ctx.L.input_char == 47)
					{
						ctx.NextState = 1;
						return true;
					}
					ctx.NextState = 27;
					return true;
				}
			}
			return true;
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000A134 File Offset: 0x00008334
		private bool GetChar()
		{
			if ((this.input_char = this.NextChar()) != -1)
			{
				return true;
			}
			this.end_of_input = true;
			return false;
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000A15D File Offset: 0x0000835D
		private int NextChar()
		{
			if (this.input_buffer != 0)
			{
				int num = this.input_buffer;
				this.input_buffer = 0;
				return num;
			}
			return this.reader.Read();
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000A180 File Offset: 0x00008380
		public bool NextToken()
		{
			this.fsm_context.Return = false;
			while (Lexer.fsm_handler_table[this.state - 1](this.fsm_context))
			{
				if (this.end_of_input)
				{
					return false;
				}
				if (this.fsm_context.Return)
				{
					this.string_value = this.string_buffer.ToString();
					this.string_buffer.Remove(0, this.string_buffer.Length);
					this.token = Lexer.fsm_return_table[this.state - 1];
					if (this.token == 65542)
					{
						this.token = this.input_char;
					}
					this.state = this.fsm_context.NextState;
					return true;
				}
				this.state = this.fsm_context.NextState;
			}
			throw new JsonException(this.input_char);
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000A255 File Offset: 0x00008455
		private void UngetChar()
		{
			this.input_buffer = this.input_char;
		}

		// Token: 0x040001F4 RID: 500
		private static readonly int[] fsm_return_table;

		// Token: 0x040001F5 RID: 501
		private static readonly Lexer.StateHandler[] fsm_handler_table;

		// Token: 0x040001F6 RID: 502
		private bool allow_comments;

		// Token: 0x040001F7 RID: 503
		private bool allow_single_quoted_strings;

		// Token: 0x040001F8 RID: 504
		private bool end_of_input;

		// Token: 0x040001F9 RID: 505
		private FsmContext fsm_context;

		// Token: 0x040001FA RID: 506
		private int input_buffer;

		// Token: 0x040001FB RID: 507
		private int input_char;

		// Token: 0x040001FC RID: 508
		private TextReader reader;

		// Token: 0x040001FD RID: 509
		private int state;

		// Token: 0x040001FE RID: 510
		private StringBuilder string_buffer;

		// Token: 0x040001FF RID: 511
		private string string_value;

		// Token: 0x04000200 RID: 512
		private int token;

		// Token: 0x04000201 RID: 513
		private int unichar;

		// Token: 0x0200008E RID: 142
		// (Invoke) Token: 0x0600029F RID: 671
		private delegate bool StateHandler(FsmContext ctx);
	}
}
