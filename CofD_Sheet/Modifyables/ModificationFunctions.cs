using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CofD_Sheet.Modifyables
{
	class ModificationFunctions
	{
		private static readonly Regex isNumericRegex = new Regex(@"^\d+$");

		public static int ParseIntQuery(string queryToParse, Sheet sheet)
		{
			//we're enforcing prefix/suffix characters around every operation, so we don't have to worry about order of operation
			//operations are surrounded with parentheses ('(', ')')
			//functions are surrounded with brackets ('[', ']')
			//if the query doesn't start with either, we know it's a direct query, and will pass it through to the sheet to get our value
			if (queryToParse.StartsWith("("))
			{
				//operation
				int parenthesesDepth = 0;
				//ignore first and last character, those are the parentheses
				for (int i = 1; i < queryToParse.Length - 1; ++i)
				{
					if (queryToParse[i] == '(')
					{
						++parenthesesDepth;
					}
					else if (queryToParse[i] == ')')
					{
						--parenthesesDepth;
					}
					else if (parenthesesDepth == 0)
					{
						//operations are all one character large, so that makes it easy to find
						if (queryToParse[i] == '+')
						{
							return ParseIntQuery(queryToParse.Substring(1, i - 1), sheet) + ParseIntQuery(queryToParse.Substring(i + 1, (queryToParse.Length - i) - 2), sheet);
						}
						else if (queryToParse[i] == '-')
						{
							return ParseIntQuery(queryToParse.Substring(1, i - 1), sheet) - ParseIntQuery(queryToParse.Substring(i + 1, (queryToParse.Length - i) - 2), sheet);
						}
						else if (queryToParse[i] == '*')
						{
							return ParseIntQuery(queryToParse.Substring(1, i - 1), sheet) * ParseIntQuery(queryToParse.Substring(i + 1, (queryToParse.Length - i) - 2), sheet);
						}
						else if (queryToParse[i] == '/')
						{
							return ParseIntQuery(queryToParse.Substring(1, i - 1), sheet) / ParseIntQuery(queryToParse.Substring(i + 1, (queryToParse.Length - i) - 2), sheet);
						}
						else if (queryToParse[i] == '^')
						{
							return (int)Math.Pow(ParseIntQuery(queryToParse.Substring(1, i - 1), sheet), ParseIntQuery(queryToParse.Substring(i + 1, (queryToParse.Length - i) - 2), sheet));
						}
					}
				}
			}
			else if (queryToParse.StartsWith("["))
			{
				//function
				//first argument is the function name, following ones are the actual arguments for the function
				int parenthesesDepth = 0;
				int argumentStartIndex = 1;
				List<string> arguments = new List<string>();
				//get all arguments
				//ignore first and last character, those are the brackets
				for (int i = 1; i < queryToParse.Length - 1; ++i)
				{
					if (queryToParse[i] == '('
						|| queryToParse[i] == '[')
					{
						++parenthesesDepth;
					}
					else if (queryToParse[i] == ')'
						|| queryToParse[i] == ']')
					{
						--parenthesesDepth;
					}
					else if (parenthesesDepth == 0)
					{
						//arguments are split with commas, 
						if (queryToParse[i] == ',')
						{
							arguments.Add(queryToParse.Substring(argumentStartIndex, i - argumentStartIndex));
							argumentStartIndex = i + 1;
						}
					}
				}
				arguments.Add(queryToParse.Substring(argumentStartIndex, queryToParse.Length - argumentStartIndex - 1));

				//parse arguments
				if (String.Equals(arguments[0], "Min", StringComparison.OrdinalIgnoreCase))
				{
					int minValue = ParseIntQuery(arguments[1], sheet);
					for (int i = 2; i < arguments.Count; ++i)
					{
						minValue = Math.Min(minValue, ParseIntQuery(arguments[i], sheet));
					}
					return minValue;
				}
				else if (String.Equals(arguments[0], "Max", StringComparison.OrdinalIgnoreCase))
				{
					int maxValue = ParseIntQuery(arguments[1], sheet);
					for (int i = 2; i < arguments.Count; ++i)
					{
						maxValue = Math.Max(maxValue, ParseIntQuery(arguments[i], sheet));
					}
					return maxValue;
				}
			}
			else
			{
				//direct query, could be either a standard value, or a query to the sheet
				if (isNumericRegex.IsMatch(queryToParse))
				{
					int.TryParse(queryToParse, out int number);
					return number;
				}
				else
				{
					return sheet.QueryInt(queryToParse);
				}
			}

			return 0;
		}

		public static string ParseStringQuery(string queryToParse, Sheet sheet)
		{
			//we're enforcing prefix/suffix characters around every operation, so we don't have to worry about order of operation
			//operations are surrounded with parentheses ('(', ')')
			//functions are surrounded with brackets ('[', ']')
			//literal strings  are surrounded with quotes ('"', '"')
			//if the query doesn't start with any of those, we know it's a direct query, and will pass it through to the sheet to get our value
			if (queryToParse.StartsWith("("))
			{
				//operation
				int parenthesesDepth = 0;
				bool isInLiteral = false;
				//ignore first and last character, those are the parentheses
				for (int i = 1; i < queryToParse.Length - 1; ++i)
				{
					if (queryToParse[i] == '"')
					{
						isInLiteral = !isInLiteral;
					}
					else if (!isInLiteral)
					{
						if (queryToParse[i] == '(')
						{
							++parenthesesDepth;
						}
						else if (queryToParse[i] == ')')
						{
							--parenthesesDepth;
						}
						else if (parenthesesDepth == 0)
						{
							//operations are all one character large, so that makes it easy to find
							if (queryToParse[i] == '+')
							{
								return ParseStringQuery(queryToParse.Substring(1, i - 1), sheet) + ParseStringQuery(queryToParse.Substring(i + 1, (queryToParse.Length - i) - 2), sheet);
							}
						}
					}
				}
				return "";
			}
			else if (queryToParse.StartsWith("["))
			{
				//function
				//first argument is the function name, following ones are the actual arguments for the function
				int parenthesesDepth = 0;
				bool isInLiteral = false;
				int argumentStartIndex = 1;
				List<string> arguments = new List<string>();
				//get all arguments
				//ignore first and last character, those are the brackets
				for (int i = 1; i < queryToParse.Length - 1; ++i)
				{
					if (queryToParse[i] == '"')
					{
						isInLiteral = !isInLiteral;
					}
					else if (!isInLiteral)
					{
						if (queryToParse[i] == '('
						|| queryToParse[i] == '[')
						{
							++parenthesesDepth;
						}
						else if (queryToParse[i] == ')'
							|| queryToParse[i] == ']')
						{
							--parenthesesDepth;
						}
						else if (parenthesesDepth == 0)
						{
							//arguments are split with commas, 
							if (queryToParse[i] == ',')
							{
								arguments.Add(queryToParse.Substring(argumentStartIndex, i - argumentStartIndex));
								argumentStartIndex = i + 1;
							}
						}
					}
				}
				arguments.Add(queryToParse.Substring(argumentStartIndex, queryToParse.Length - argumentStartIndex - 1));

				//parse arguments

				if (String.Equals(arguments[0], "IntToString", StringComparison.OrdinalIgnoreCase))
				{
					int intValue = ParseIntQuery(arguments[1], sheet);
					return intValue.ToString();
				}
				else if (String.Equals(arguments[0], "Shortest", StringComparison.OrdinalIgnoreCase))
				{
					string bestValue = ParseStringQuery(arguments[1], sheet);
					for (int i = 2; i < arguments.Count; ++i)
					{
						string nextValue = ParseStringQuery(arguments[i], sheet);
						if (nextValue.Length < bestValue.Length)
						{
							bestValue = nextValue;
						}
					}
					return bestValue;
				}
				else if (String.Equals(arguments[0], "Longest", StringComparison.OrdinalIgnoreCase))
				{
					string bestValue = ParseStringQuery(arguments[1], sheet);
					for (int i = 2; i < arguments.Count; ++i)
					{
						string nextValue = ParseStringQuery(arguments[i], sheet);
						if (nextValue.Length > bestValue.Length)
						{
							bestValue = nextValue;
						}
					}
					return bestValue;
				}
				else
				{
					return "";
				}
			}
			else if (queryToParse.StartsWith("\""))
			{
				//literal string
				return queryToParse.Substring(1, queryToParse.Length - 2);
			}
			else
			{
				//direct query, query the sheet
				return sheet.QueryString(queryToParse);
			}
		}
	}
}
