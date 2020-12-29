using CofD_Sheet.Modifyables;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace CofD_Sheet.Modifications
{
	[Serializable]
	public enum IntModificationType
	{
		[XmlEnum(Name = "Delta")]
		Delta,
		[XmlEnum(Name = "Absolute")]
		Absolute
	}

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

			return ModificationFunctions.ParseIntQuery(query, sheet);
		}
	}
}
