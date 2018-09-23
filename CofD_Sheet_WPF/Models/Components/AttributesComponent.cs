using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;
using System.Xml.Serialization;

namespace CofD_Sheet_WPF.Models
{
	[Serializable]
	public class AttributesComponent : ISheetComponent
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
			public string name = "Attribute";

			[XmlAttribute]
			public int currentValue = 1;
		}

		[XmlIgnore]
		const int maxDotsPerRow = 10;

		[XmlIgnore]
		const int nameLabelWidth = 100;

		[XmlAttribute]
		public int maxValue = 5;

		[XmlArray]
		public List<Attribute> attributes = new List<Attribute>();

		public AttributesComponent() : base("AttributesComponent", -1)
		{ }

		public AttributesComponent(string componentName, List<string> attributeNames, int componentColumnIndex) : base(componentName, componentColumnIndex)
		{
			for (int i = 0; i < attributeNames.Count; i++)
			{
				attributes.Add(new Attribute(attributeNames[i]));
			}
		}
		
		//void valueChanged(object sender, EventArgs e)
		//{
		//	foreach (Attribute attribute in attributes)
		//	{
		//		for (int i = 0; i < attribute.pips.Count; i++)
		//		{
		//			if (sender == attribute.pips[i])
		//			{
		//				if (attribute.currentValue == i + 1)
		//				{
		//					//when clicking the last pip, reduce value by 1
		//					attribute.currentValue = i;
		//				}
		//				else
		//				{
		//					attribute.currentValue = i + 1;
		//				}
		//			}
		//		}
		//	}
		//	onValueChanged();
		//}
		//
		//void onValueChanged()
		//{
		//	foreach (Attribute attribute in attributes)
		//	{
		//		for (int i = 0; i < attribute.pips.Count; i++)
		//		{
		//			attribute.pips[i].Checked = i < attribute.currentValue;
		//		}
		//	}
		//
		//	onComponentChanged();
		//}
	}
}
