﻿using System;
using System.Xml.Serialization;

namespace CofD_Sheet_WPF.Models.Components
{
	[Serializable]
	public class Resource : IComponent
	{
		[XmlIgnore]
		const int maxPerRow = 15;

		[XmlIgnore]
		const float separatorProportion = 2F;

		[XmlAttribute]
		public int maxValue = 10;

		[XmlAttribute]
		public int currentValue = 0;
		
		public Resource() : base("SimpleComponent", ColumnId.Undefined)
		{
			currentValue = maxValue;
		}

		public Resource(string componentName, ColumnId _column) : base(componentName, _column)
		{
			currentValue = maxValue;
		}
	}
}