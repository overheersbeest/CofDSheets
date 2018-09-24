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
			public string name = "Skill";

			[XmlAttribute]
			public int currentValue = 0;

			[XmlElement]
			public List<String> specialties = new List<String>();
		}

		[XmlIgnore]
		const int maxDotsPerRow = 10;

		[XmlIgnore]
		const int nameLabelWidth = 180;

		[XmlAttribute]
		public int maxValue = 5;

		[XmlArray]
		public List<Skill> skills = new List<Skill>();

		public Skills() : base("SkillsComponent", ColumnId.Undefined)
		{ }

		public Skills(string componentName, List<string> skillNames, ColumnId _column) : base(componentName, _column)
		{
			for (int i = 0; i < skillNames.Count; i++)
			{
				skills.Add(new Skill(skillNames[i]));
			}
		}
	}
}