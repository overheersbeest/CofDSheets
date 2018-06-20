using CofD_Sheet.Sheet_Components;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace CofD_Sheet
{
	public enum SheetType
	{
		Mortal,
		Mage,
		Werewolf,
		Spirit
	}

	[XmlRoot]
	public class Sheet
	{
		[XmlAttribute]
		public string name = "";

		[XmlAttribute]
		public string player = "";

		[XmlAttribute]
		public string chronicle = "";
		
		[XmlArray]
		//[XmlArrayItem("AspirationsComponent", typeof(AspirationsComponent))]
		//[XmlArrayItem("AttributesComponent", typeof(AttributesComponent))]
		//[XmlArrayItem("ExperienceComponent", typeof(ExperienceComponent))]
		//[XmlArrayItem("HealthComponent", typeof(HealthComponent))]
		//[XmlArrayItem("SimpleComponent", typeof(SimpleComponent))]
		//[XmlArrayItem("SkillsComponent", typeof(SkillsComponent))]
		//[XmlArrayItem("StatComponent", typeof(StatComponent))]
		public List<ISheetComponent> components = new List<ISheetComponent>();

		[XmlIgnore]
		public bool _changedSinceSave = false;
		[XmlIgnore]
		public bool changedSinceSave
		{
			get { return this._changedSinceSave; }
			set
			{
				this._changedSinceSave = value;
				if (Form1.instance != null)
				{
					Form1.instance.RefreshFormTitle();
				}
			}
		}


		public Sheet()
		{ }

		public Sheet(SheetType type)
		{
			switch (type)
			{
				case SheetType.Mortal:
					components.Add(new AttributesComponent("Mental_Attributes", new List<string>{"Intelligence", "Wits", "Resolve"}));
					components.Add(new AttributesComponent("Physical_Attributes", new List<string>{"Strength", "Dexterity", "Stamina"}));
					components.Add(new AttributesComponent("Social_Attributes", new List<string>{"Presence", "Manipulation", "Composure"}));
					components.Add(new SkillsComponent("Mental_Skills", new List<string> { "Academics", "Computer", "Crafts", "Investigation", "Medicine", "Occult", "Politics", "Science" }));
					components.Add(new SkillsComponent("Physical_Skills", new List<string> { "Athletics", "Brawl", "Drive", "Firearms", "Larceny", "Stealth", "Survival", "Weaponry" }));
					components.Add(new SkillsComponent("Social_Skills", new List<string> { "Animal_Ken", "Empathy", "Expression", "Intimidation", "Persuasion", "Socialize", "Streetwise", "Subterfuge" }));
					components.Add(new HealthComponent("Health"));
					components.Add(new SimpleComponent("Willpower"));
					components.Add(new StatComponent("Integrity"));
					components.Add(new ExperienceComponent("Experience", "Beats"));
					components.Add(new AspirationsComponent("Aspirations"));
					break;
				case SheetType.Mage:
					components.Add(new HealthComponent("Health"));
					components.Add(new SimpleComponent("Willpower"));
					components.Add(new SimpleComponent("Mana"));
					components.Add(new StatComponent("Wisdom"));
					components.Add(new StatComponent("Gnosis"));
					components.Add(new ExperienceComponent("Experience", "Beats"));
					components.Add(new ExperienceComponent("Arcane_Experience", "Arcane_Beats"));
					components.Add(new AspirationsComponent("Aspirations"));
					components.Add(new AspirationsComponent("Obsessions", 1));
					break;
				case SheetType.Werewolf:
					components.Add(new HealthComponent("Health"));
					components.Add(new SimpleComponent("Willpower"));
					components.Add(new SimpleComponent("Essence"));
					components.Add(new StatComponent("Harmony"));
					components.Add(new StatComponent("Primal_Urge"));
					components.Add(new ExperienceComponent("Experience", "Beats"));
					components.Add(new AspirationsComponent("Aspirations"));
					break;
				case SheetType.Spirit:
					components.Add(new HealthComponent("Corpus"));
					components.Add(new SimpleComponent("Willpower"));
					components.Add(new SimpleComponent("Essence"));
					components.Add(new StatComponent("Rank"));
					components.Add(new AspirationsComponent("Aspirations"));
					break;
			}
		}
	}
}
