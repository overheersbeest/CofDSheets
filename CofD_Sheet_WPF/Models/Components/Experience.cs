using System;
using System.Xml.Serialization;

namespace CofD_Sheet_WPF.Models.Components
{
	[Serializable]
	public class Experience : BaseComponent
	{
		[XmlIgnore]
		const int maxPerRow = 10;
		
		[XmlAttribute]
		public string beatName { get; set; } = "Beats";

		[XmlAttribute]
		public int maxBeats { get; set; } = 5;

		[XmlAttribute]
		public int beats { get; set; } = 0;

		[XmlAttribute]
		public int experience { get; set; } = 0;
		
		public Experience() : base("ExperienceComponent")
		{ }

		public Experience(string majorName, string minorName) : base(majorName)
		{
			beatName = minorName;
		}
	}
}
