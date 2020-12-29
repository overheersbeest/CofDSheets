using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CofD_Sheet_WPF.Models.Components
{
	[Serializable]
	public class Skills : BaseComponent
	{
		public class Skill
		{
			public Skill()
			{ }
			public Skill(string _name)
			{
				this.name = _name;
			}
			public Skill(string _name, int _value)
			{
				this.name = _name;
				this.currentValue = _value;
			}

			[XmlAttribute]
			public string name { get; set; } = "Skill";

			[XmlAttribute]
			public int currentValue { get; set; } = 0;

			[XmlElement]
			public List<String> specialties { get; set; } = new List<String>();
		}

		[XmlIgnore]
		const int maxDotsPerRow = 10;
		
		[XmlAttribute]
		public int maxValue { get; set; } = 5;

		[XmlArray]
		public List<Skill> skills { get; set; } = new List<Skill>();

		public Skills() : base("SkillsComponent")
		{ }

		public Skills(string componentName, List<string> skillNames) : base(componentName)
		{
			for (int i = 0; i < skillNames.Count; ++i)
			{
				skills.Add(new Skill(skillNames[i]));
			}
		}
	}
}