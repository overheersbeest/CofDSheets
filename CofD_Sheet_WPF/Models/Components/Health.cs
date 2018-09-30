using System;
using System.Xml.Serialization;

namespace CofD_Sheet_WPF.Models.Components
{
	[Serializable]
	public class Health : BaseComponent
	{
		[XmlIgnore]
		const int maxPerRow = 15;
		
		[XmlAttribute]
		public int maxValue { get; set; } = 10;

		[XmlAttribute]
		public int aggrivated { get; set; } = 0;

		[XmlAttribute]
		public int lethal { get; set; } = 0;

		[XmlAttribute]
		public int bashing { get; set; } = 0;
		
		public Health() : base("HealthComponent")
		{ }

		public Health(string componentName) : base(componentName)
		{ }
	}
}
