using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CofD_Sheet_WPF.Models.Components
{
	[Serializable]
	public class Attributes : BaseComponent
	{
		public class Attribute
		{
			public Attribute()
			{ }
			public Attribute(string _name)
			{
				this.name = _name;
			}
			public Attribute(string _name, int _value)
			{
				this.name = _name;
				this.currentValue = _value;
			}

			[XmlAttribute]
			public string name { get; set; } = "Attribute";

			[XmlAttribute]
			public int currentValue { get; set; } = 1;
		}

		[XmlIgnore]
		const int maxDotsPerRow = 10;

		[XmlIgnore]
		const int nameLabelWidth = 100;

		[XmlAttribute]
		public int maxValue { get; set; } = 5;

		[XmlArray]
		public List<Attribute> attributes { get; set; } = new List<Attribute>();

		public Attributes() : base("AttributesComponent", ColumnId.Undefined)
		{ }

		public Attributes(string componentName, List<string> attributeNames, ColumnId _column) : base(componentName, _column)
		{
			for (int i = 0; i < attributeNames.Count; i++)
			{
				attributes.Add(new Attribute(attributeNames[i]));
			}
		}
	}
}
