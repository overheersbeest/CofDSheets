using System;
using System.Xml.Serialization;

namespace CofD_Sheet_WPF.Models.Components
{
	[Serializable]
	public class Health : BaseComponent
	{
		[XmlIgnore]
		const int maxPerRow = 15;

		[XmlIgnore]
		const float separatorProportion = 2F;

		[XmlAttribute]
		public int maxValue = 10;

		[XmlAttribute]
		public int aggrivated = 0;

		[XmlAttribute]
		public int lethal = 0;

		[XmlAttribute]
		public int bashing = 0;
		
		public Health() : base("HealthComponent", ColumnId.Undefined)
		{ }

		public Health(string componentName, ColumnId _column) : base(componentName, _column)
		{ }
	}
}
