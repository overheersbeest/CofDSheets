using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CofD_Sheet_WPF.Models.Components
{
	[Serializable]
	public class Merits : BaseComponent
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
			public string name { get; set; } = "Merit";

			[XmlAttribute]
			public int currentValue { get; set; } = 0;
		}

		[XmlIgnore]
		const int maxDotsPerRow = 10;
		
		[XmlAttribute]
		public int maxValue { get; set; } = 5;

		[XmlAttribute]
		public string singularName { get; set; } = "merit";
		
		[XmlArray]
		public List<Merit> merits { get; set; } = new List<Merit>();

		public Merits() : base("SkillsComponent")
		{ }

		public Merits(string componentName, string _singularName, List<string> meritNames, int _maxValue) : base(componentName)
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
