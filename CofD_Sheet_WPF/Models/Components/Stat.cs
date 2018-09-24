﻿using System;
using System.Xml.Serialization;

namespace CofD_Sheet_WPF.Models.Components
{
	[Serializable]
	public class Stat : IComponent
	{
		[XmlIgnore]
		const int maxPerRow = 15;

		[XmlIgnore]
		const float separatorProportion = 2F;

		[XmlAttribute]
		public int maxValue = 10;

		[XmlAttribute]
		public int currentValue = 0;
		
		public Stat() : base("StatComponent", ColumnId.Undefined)
		{ }

		public Stat(string componentName, ColumnId _column) : base(componentName, _column)
		{ }
	}
}