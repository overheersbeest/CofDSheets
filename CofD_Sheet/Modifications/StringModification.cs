using CofD_Sheet.Modifyables;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace CofD_Sheet.Modifications
{
	[Serializable]
	public enum StringModificationState
	{
		[XmlEnum(Name = "None")]
		None,
		[XmlEnum(Name = "Additive")]
		Additive,
		[XmlEnum(Name = "Replace")]
		Replace
	}

	[Serializable]
	public enum StringModificationType
	{
		[XmlEnum(Name = "Prefix")]
		Prefix,
		[XmlEnum(Name = "Suffix")]
		Suffix,
		[XmlEnum(Name = "Replace")]
		Replace,
		[XmlEnum(Name = "AddElement")]
		AddElement
	}

	[Serializable]
	public class StringModification : Modification
	{
		public StringModification() : base()
		{ }
		public StringModification(List<string> _path, string _value, string _query, StringModificationType _modType) : base(_path)
		{
			modType = _modType;
			value = _value;
			query = Regex.Replace(_query, @"\s+", "");//no whitespace allowed, to make things easier for us during querying
		}

		[XmlAttribute]
		public StringModificationType modType = StringModificationType.Replace;

		[XmlAttribute]
		public string value = "";

		[XmlAttribute]
		public string query = "";

		public string GetValue(Sheet sheet)
		{
			if (query.Length > 0)
			{
				return GetQueryValue(sheet);
			}
			else
			{
				return value;
			}
		}

		public string GetQueryValue(Sheet sheet)
		{
			if (query.Length == 0)
			{
				return "";
			}

			return ModificationFunctions.ParseStringQuery(query, sheet);
		}
	}
}
