using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace LitJson
{
	// Token: 0x0200005A RID: 90
	public class JsonReader
	{
		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600019C RID: 412 RVA: 0x00007E47 File Offset: 0x00006047
		// (set) Token: 0x0600019D RID: 413 RVA: 0x00007E54 File Offset: 0x00006054
		public bool AllowComments
		{
			get
			{
				return this.lexer.AllowComments;
			}
			set
			{
				this.lexer.AllowComments = value;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600019E RID: 414 RVA: 0x00007E62 File Offset: 0x00006062
		// (set) Token: 0x0600019F RID: 415 RVA: 0x00007E6F File Offset: 0x0000606F
		public bool AllowSingleQuotedStrings
		{
			get
			{
				return this.lexer.AllowSingleQuotedStrings;
			}
			set
			{
				this.lexer.AllowSingleQuotedStrings = value;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x00007E7D File Offset: 0x0000607D
		// (set) Token: 0x060001A1 RID: 417 RVA: 0x00007E85 File Offset: 0x00006085
		public bool SkipNonMembers
		{
			get
			{
				return this.skip_non_members;
			}
			set
			{
				this.skip_non_members = value;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x00007E8E File Offset: 0x0000608E
		public bool EndOfInput
		{
			get
			{
				return this.end_of_input;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x00007E96 File Offset: 0x00006096
		public bool EndOfJson
		{
			get
			{
				return this.end_of_json;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x00007E9E File Offset: 0x0000609E
		public JsonToken Token
		{
			get
			{
				return this.token;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x00007EA6 File Offset: 0x000060A6
		public object Value
		{
			get
			{
				return this.token_value;
			}
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00007EBA File Offset: 0x000060BA
		public JsonReader(string json_text)
			: this(new StringReader(json_text), true)
		{
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00007EC9 File Offset: 0x000060C9
		public JsonReader(TextReader reader)
			: this(reader, false)
		{
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00007ED4 File Offset: 0x000060D4
		private JsonReader(TextReader reader, bool owned)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			this.parser_in_string = false;
			this.parser_return = false;
			this.read_started = false;
			this.automaton_stack = new Stack<int>();
			this.automaton_stack.Push(65553);
			this.automaton_stack.Push(65543);
			this.lexer = new Lexer(reader);
			this.end_of_input = false;
			this.end_of_json = false;
			this.skip_non_members = true;
			this.reader = reader;
			this.reader_is_owned = owned;
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00007F64 File Offset: 0x00006164
		private static IDictionary<int, IDictionary<int, int[]>> PopulateParseTable()
		{
			IDictionary<int, IDictionary<int, int[]>> dictionary = new Dictionary<int, IDictionary<int, int[]>>();
			JsonReader.TableAddRow(dictionary, ParserToken.Array);
			JsonReader.TableAddCol(dictionary, ParserToken.Array, 91, new int[] { 91, 65549 });
			JsonReader.TableAddRow(dictionary, ParserToken.ArrayPrime);
			JsonReader.TableAddCol(dictionary, ParserToken.ArrayPrime, 34, new int[] { 65550, 65551, 93 });
			JsonReader.TableAddCol(dictionary, ParserToken.ArrayPrime, 91, new int[] { 65550, 65551, 93 });
			JsonReader.TableAddCol(dictionary, ParserToken.ArrayPrime, 93, new int[] { 93 });
			JsonReader.TableAddCol(dictionary, ParserToken.ArrayPrime, 123, new int[] { 65550, 65551, 93 });
			JsonReader.TableAddCol(dictionary, ParserToken.ArrayPrime, 65537, new int[] { 65550, 65551, 93 });
			JsonReader.TableAddCol(dictionary, ParserToken.ArrayPrime, 65538, new int[] { 65550, 65551, 93 });
			JsonReader.TableAddCol(dictionary, ParserToken.ArrayPrime, 65539, new int[] { 65550, 65551, 93 });
			JsonReader.TableAddCol(dictionary, ParserToken.ArrayPrime, 65540, new int[] { 65550, 65551, 93 });
			JsonReader.TableAddRow(dictionary, ParserToken.Object);
			JsonReader.TableAddCol(dictionary, ParserToken.Object, 123, new int[] { 123, 65545 });
			JsonReader.TableAddRow(dictionary, ParserToken.ObjectPrime);
			JsonReader.TableAddCol(dictionary, ParserToken.ObjectPrime, 34, new int[] { 65546, 65547, 125 });
			JsonReader.TableAddCol(dictionary, ParserToken.ObjectPrime, 125, new int[] { 125 });
			JsonReader.TableAddRow(dictionary, ParserToken.Pair);
			JsonReader.TableAddCol(dictionary, ParserToken.Pair, 34, new int[] { 65552, 58, 65550 });
			JsonReader.TableAddRow(dictionary, ParserToken.PairRest);
			JsonReader.TableAddCol(dictionary, ParserToken.PairRest, 44, new int[] { 44, 65546, 65547 });
			JsonReader.TableAddCol(dictionary, ParserToken.PairRest, 125, new int[] { 65554 });
			JsonReader.TableAddRow(dictionary, ParserToken.String);
			JsonReader.TableAddCol(dictionary, ParserToken.String, 34, new int[] { 34, 65541, 34 });
			JsonReader.TableAddRow(dictionary, ParserToken.Text);
			JsonReader.TableAddCol(dictionary, ParserToken.Text, 91, new int[] { 65548 });
			JsonReader.TableAddCol(dictionary, ParserToken.Text, 123, new int[] { 65544 });
			JsonReader.TableAddRow(dictionary, ParserToken.Value);
			JsonReader.TableAddCol(dictionary, ParserToken.Value, 34, new int[] { 65552 });
			JsonReader.TableAddCol(dictionary, ParserToken.Value, 91, new int[] { 65548 });
			JsonReader.TableAddCol(dictionary, ParserToken.Value, 123, new int[] { 65544 });
			JsonReader.TableAddCol(dictionary, ParserToken.Value, 65537, new int[] { 65537 });
			JsonReader.TableAddCol(dictionary, ParserToken.Value, 65538, new int[] { 65538 });
			JsonReader.TableAddCol(dictionary, ParserToken.Value, 65539, new int[] { 65539 });
			JsonReader.TableAddCol(dictionary, ParserToken.Value, 65540, new int[] { 65540 });
			JsonReader.TableAddRow(dictionary, ParserToken.ValueRest);
			JsonReader.TableAddCol(dictionary, ParserToken.ValueRest, 44, new int[] { 44, 65550, 65551 });
			JsonReader.TableAddCol(dictionary, ParserToken.ValueRest, 93, new int[] { 65554 });
			return dictionary;
		}

		// Token: 0x060001AB RID: 427 RVA: 0x000082FF File Offset: 0x000064FF
		private static void TableAddCol(IDictionary<int, IDictionary<int, int[]>> parse_table, ParserToken row, int col, params int[] symbols)
		{
			parse_table[(int)row].Add(col, symbols);
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0000830F File Offset: 0x0000650F
		private static void TableAddRow(IDictionary<int, IDictionary<int, int[]>> parse_table, ParserToken rule)
		{
			parse_table.Add((int)rule, new Dictionary<int, int[]>());
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00008320 File Offset: 0x00006520
		private void ProcessNumber(string number)
		{
			double num;
			if ((number.IndexOf('.') != -1 || number.IndexOf('e') != -1 || number.IndexOf('E') != -1) && double.TryParse(number, NumberStyles.Any, CultureInfo.InvariantCulture, out num))
			{
				this.token = JsonToken.Double;
				this.token_value = num;
				return;
			}
			int num2;
			if (int.TryParse(number, NumberStyles.Integer, CultureInfo.InvariantCulture, out num2))
			{
				this.token = JsonToken.Int;
				this.token_value = num2;
				return;
			}
			long num3;
			if (long.TryParse(number, NumberStyles.Integer, CultureInfo.InvariantCulture, out num3))
			{
				this.token = JsonToken.Long;
				this.token_value = num3;
				return;
			}
			ulong num4;
			if (ulong.TryParse(number, NumberStyles.Integer, CultureInfo.InvariantCulture, out num4))
			{
				this.token = JsonToken.Long;
				this.token_value = num4;
				return;
			}
			this.token = JsonToken.Int;
			this.token_value = 0;
		}

		// Token: 0x060001AE RID: 430 RVA: 0x000083F8 File Offset: 0x000065F8
		private void ProcessSymbol()
		{
			if (this.current_symbol == 91)
			{
				this.token = JsonToken.ArrayStart;
				this.parser_return = true;
				return;
			}
			if (this.current_symbol == 93)
			{
				this.token = JsonToken.ArrayEnd;
				this.parser_return = true;
				return;
			}
			if (this.current_symbol == 123)
			{
				this.token = JsonToken.ObjectStart;
				this.parser_return = true;
				return;
			}
			if (this.current_symbol == 125)
			{
				this.token = JsonToken.ObjectEnd;
				this.parser_return = true;
				return;
			}
			if (this.current_symbol == 34)
			{
				if (this.parser_in_string)
				{
					this.parser_in_string = false;
					this.parser_return = true;
					return;
				}
				if (this.token == JsonToken.None)
				{
					this.token = JsonToken.String;
				}
				this.parser_in_string = true;
				return;
			}
			else
			{
				if (this.current_symbol == 65541)
				{
					this.token_value = this.lexer.StringValue;
					return;
				}
				if (this.current_symbol == 65539)
				{
					this.token = JsonToken.Boolean;
					this.token_value = false;
					this.parser_return = true;
					return;
				}
				if (this.current_symbol == 65540)
				{
					this.token = JsonToken.Null;
					this.parser_return = true;
					return;
				}
				if (this.current_symbol == 65537)
				{
					this.ProcessNumber(this.lexer.StringValue);
					this.parser_return = true;
					return;
				}
				if (this.current_symbol == 65546)
				{
					this.token = JsonToken.PropertyName;
					return;
				}
				if (this.current_symbol == 65538)
				{
					this.token = JsonToken.Boolean;
					this.token_value = true;
					this.parser_return = true;
				}
				return;
			}
		}

		// Token: 0x060001AF RID: 431 RVA: 0x0000856A File Offset: 0x0000676A
		private bool ReadToken()
		{
			if (this.end_of_input)
			{
				return false;
			}
			this.lexer.NextToken();
			if (this.lexer.EndOfInput)
			{
				this.Close();
				return false;
			}
			this.current_input = this.lexer.Token;
			return true;
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x000085AC File Offset: 0x000067AC
		public void Close()
		{
			if (this.end_of_input)
			{
				return;
			}
			this.end_of_input = true;
			this.end_of_json = true;
			if (this.reader_is_owned)
			{
				using (this.reader)
				{
				}
			}
			this.reader = null;
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00008604 File Offset: 0x00006804
		public bool Read()
		{
			if (this.end_of_input)
			{
				return false;
			}
			if (this.end_of_json)
			{
				this.end_of_json = false;
				this.automaton_stack.Clear();
				this.automaton_stack.Push(65553);
				this.automaton_stack.Push(65543);
			}
			this.parser_in_string = false;
			this.parser_return = false;
			this.token = JsonToken.None;
			this.token_value = null;
			if (!this.read_started)
			{
				this.read_started = true;
				if (!this.ReadToken())
				{
					return false;
				}
			}
			while (!this.parser_return)
			{
				this.current_symbol = this.automaton_stack.Pop();
				this.ProcessSymbol();
				if (this.current_symbol == this.current_input)
				{
					if (!this.ReadToken())
					{
						if (this.automaton_stack.Peek() != 65553)
						{
							throw new JsonException("Input doesn't evaluate to proper JSON text");
						}
						return this.parser_return;
					}
				}
				else
				{
					int[] array;
					try
					{
						array = JsonReader.parse_table[this.current_symbol][this.current_input];
					}
					catch (KeyNotFoundException ex)
					{
						throw new JsonException((ParserToken)this.current_input, ex);
					}
					if (array[0] != 65554)
					{
						for (int i = array.Length - 1; i >= 0; i--)
						{
							this.automaton_stack.Push(array[i]);
						}
					}
				}
			}
			if (this.automaton_stack.Peek() == 65553)
			{
				this.end_of_json = true;
			}
			return true;
		}

		// Token: 0x040001CA RID: 458
		private static readonly IDictionary<int, IDictionary<int, int[]>> parse_table = JsonReader.PopulateParseTable();

		// Token: 0x040001CB RID: 459
		private Stack<int> automaton_stack;

		// Token: 0x040001CC RID: 460
		private int current_input;

		// Token: 0x040001CD RID: 461
		private int current_symbol;

		// Token: 0x040001CE RID: 462
		private bool end_of_json;

		// Token: 0x040001CF RID: 463
		private bool end_of_input;

		// Token: 0x040001D0 RID: 464
		private Lexer lexer;

		// Token: 0x040001D1 RID: 465
		private bool parser_in_string;

		// Token: 0x040001D2 RID: 466
		private bool parser_return;

		// Token: 0x040001D3 RID: 467
		private bool read_started;

		// Token: 0x040001D4 RID: 468
		private TextReader reader;

		// Token: 0x040001D5 RID: 469
		private bool reader_is_owned;

		// Token: 0x040001D6 RID: 470
		private bool skip_non_members;

		// Token: 0x040001D7 RID: 471
		private object token_value;

		// Token: 0x040001D8 RID: 472
		private JsonToken token;
	}
}
