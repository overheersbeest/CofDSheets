using System;
using System.Xml.Serialization;

namespace CofD_Sheet_WPF.Models.Components
{
	[Serializable]
	public class Resource : BaseComponent
	{
		[XmlIgnore]
		const int maxPerRow = 15;

		[XmlIgnore]
		const float separatorProportion = 2F;

		[XmlAttribute]
		public int maxValue { get; set; } = 10;

		[XmlAttribute]
		public int currentValue { get; set; } = 0;
		
		public Resource() : base("SimpleComponent")
		{
			currentValue = maxValue;
		}

		public Resource(string componentName) : base(componentName)
		{
			currentValue = maxValue;
		}
	}
}
