using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CofD_Sheet_WPF.Models.Components
{
	[Serializable]
	public class Merits : IComponent
	{
		public class Merit
		{
			public Merit()
			{ }
			public Merit(string _name)
			{
				this.name = _name;
			}
			public Merit(string _name, int _value)
			{
				this.name = _name;
				this.currentValue = _value;
			}

			[XmlAttribute]
			public string name = "Merit";

			[XmlAttribute]
			public int currentValue = 0;
		}

		[XmlIgnore]
		const int maxDotsPerRow = 10;

		[XmlIgnore]
		const int nameLabelWidth = 180;

		[XmlAttribute]
		public int maxValue = 5;

		[XmlAttribute]
		public string singularName = "merit";
		
		[XmlArray]
		public List<Merit> merits = new List<Merit>();

		public Merits() : base("SkillsComponent", ColumnId.Undefined)
		{ }

		public Merits(string componentName, string _singularName, List<string> meritNames, int _maxValue, ColumnId _column) : base(componentName, _column)
		{
			this.singularName = _singularName;
			this.maxValue = _maxValue;
			foreach (string meritName in meritNames)
			{
				merits.Add(new Merit(meritName));
			}
		}
	}
}
