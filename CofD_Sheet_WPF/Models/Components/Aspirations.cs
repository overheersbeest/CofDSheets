using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CofD_Sheet_WPF.Models.Components
{
	[Serializable]
	public class Aspirations : BaseComponent
	{
		[XmlAttribute]
		public int maxAspirations = 3;

		[XmlArray]
		public List<string> aspirations = new List<string>();
		
		public Aspirations() : base("AspirationsComponent", ColumnId.Undefined)
		{ }

		public Aspirations(string componentName, int amountAllowed, ColumnId _column) : base(componentName, _column)
		{
			maxAspirations = amountAllowed;
			aspirations = new List<string>(new string[maxAspirations]);
		}
	}
}
