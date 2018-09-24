using System;
using System.Xml.Serialization;

namespace CofD_Sheet_WPF.Models.Components
{
	[Serializable]
	public class Experience : IComponent
	{
		[XmlIgnore]
		const int maxPerRow = 10;

		[XmlIgnore]
		const float separatorProportion = 2F;

		[XmlAttribute]
		public string beatName = "Beats";

		[XmlAttribute]
		public int maxBeats = 5;

		[XmlAttribute]
		public int beats = 0;

		[XmlAttribute]
		public int experience = 0;
		
		public Experience() : base("ExperienceComponent", ColumnId.Undefined)
		{ }

		public Experience(string majorName, string minorName, ColumnId _column) : base(majorName, _column)
		{
			beatName = minorName;
		}
	}
}
