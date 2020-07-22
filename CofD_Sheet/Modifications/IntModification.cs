using CofD_Sheet.Modifyables;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace CofD_Sheet.Modifications
{
	public class IntModification : Modification
	{
		public IntModification()
		{ }
		public IntModification(List<string> _path, int _baseValue, string _query, IntModificationType _modType) : base(_path)
		{
			modType = _modType;
			baseValue = _baseValue;
			query = Regex.Replace(_query, @"\s+", "");//no whitespace allowed, to make things easier for us during querying
		}

		[XmlAttribute]
		public IntModificationType modType = IntModificationType.Delta;

		[XmlAttribute]
		public int baseValue = 0;

		[XmlAttribute]
		public string query = "";

		[XmlIgnore]
		private static readonly Regex isNumericRegex = new Regex(@"^\d+$");

		public int GetValue(Sheet sheet)
		{
			return baseValue + GetQueryValue(sheet);
		}

		public int GetQueryValue(Sheet sheet)
		{
			if (query.Length == 0)
			{
				return 0;
			}

			return ParseQuery(query, sheet);
		}

		private int ParseQuery(string queryToParse, Sheet sheet)
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
							return ParseQuery(queryToParse.Substring(1, i - 1), sheet) + ParseQuery(queryToParse.Substring(i + 1, (queryToParse.Length - i) - 2), sheet);
						}
						else if (queryToParse[i] == '-')
						{
							return ParseQuery(queryToParse.Substring(1, i - 1), sheet) - ParseQuery(queryToParse.Substring(i + 1, (queryToParse.Length - i) - 2), sheet);
						}
						else if (queryToParse[i] == '*')
						{
							return ParseQuery(queryToParse.Substring(1, i - 1), sheet) * ParseQuery(queryToParse.Substring(i + 1, (queryToParse.Length - i) - 2), sheet);
						}
						else if (queryToParse[i] == '/')
						{
							return ParseQuery(queryToParse.Substring(1, i - 1), sheet) / ParseQuery(queryToParse.Substring(i + 1, (queryToParse.Length - i) - 2), sheet);
						}
						else if (queryToParse[i] == '^')
						{
							return (int)Math.Pow(ParseQuery(queryToParse.Substring(1, i - 1), sheet), ParseQuery(queryToParse.Substring(i + 1, (queryToParse.Length - i) - 2), sheet));
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
				if (String.Compare(arguments[0], "min", true) == 0)
				{
					int minValue = ParseQuery(arguments[1], sheet);
					for (int i = 2; i < arguments.Count; ++i)
					{
						minValue = Math.Min(minValue, ParseQuery(arguments[i], sheet));
					}
					return minValue;
				}
				else if (String.Compare(arguments[0], "max", true) == 0)
				{
					int maxValue = ParseQuery(arguments[1], sheet);
					for (int i = 2; i < arguments.Count; ++i)
					{
						maxValue = Math.Max(maxValue, ParseQuery(arguments[i], sheet));
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
	}
}
