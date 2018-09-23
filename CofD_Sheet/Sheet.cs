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
		WolfBlooded,
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
					components.Add(new AttributesComponent("Mental_Attributes", new List<string>{"Intelligence", "Wits", "Resolve"}, ColumnId.Left));
					components.Add(new AttributesComponent("Physical_Attributes", new List<string>{"Strength", "Dexterity", "Stamina"}, ColumnId.Middle));
					components.Add(new AttributesComponent("Social_Attributes", new List<string>{"Presence", "Manipulation", "Composure"}, ColumnId.Right));

					components.Add(new SkillsComponent("Mental_Skills", new List<string> { "Academics", "Computer", "Crafts", "Investigation", "Medicine", "Occult", "Politics", "Science" }, ColumnId.Left));
					components.Add(new SkillsComponent("Physical_Skills", new List<string> { "Athletics", "Brawl", "Drive", "Firearms", "Larceny", "Stealth", "Survival", "Weaponry" }, ColumnId.Left));
					components.Add(new SkillsComponent("Social_Skills", new List<string> { "Animal_Ken", "Empathy", "Expression", "Intimidation", "Persuasion", "Socialize", "Streetwise", "Subterfuge" }, ColumnId.Left));

					components.Add(new MeritComponent("Merits", "merit", true, new List<string>(), 5, ColumnId.Middle));
					components.Add(new AdvantagesComponent("Advantages", new List<string> {"Virtue", "Vice", "Size", "Speed", "Defense", "Armor", "Initiative Mod"}, ColumnId.Middle));
					components.Add(new ExperienceComponent("Experience", "Beats", ColumnId.Middle));

					components.Add(new HealthComponent("Health", ColumnId.Right));
					components.Add(new SimpleComponent("Willpower", ColumnId.Right));
					components.Add(new StatComponent("Integrity", ColumnId.Right));
					components.Add(new MeritComponent("Conditions", "condition", true, new List<string>(), 0, ColumnId.Right));
					components.Add(new AspirationsComponent("Aspirations", 3, ColumnId.Right));
					break;
				case SheetType.Mage:
					components.Add(new AttributesComponent("Mental_Attributes", new List<string> { "Intelligence", "Wits", "Resolve" }, ColumnId.Left));
					components.Add(new AttributesComponent("Physical_Attributes", new List<string> { "Strength", "Dexterity", "Stamina" }, ColumnId.Middle));
					components.Add(new AttributesComponent("Social_Attributes", new List<string> { "Presence", "Manipulation", "Composure" }, ColumnId.Right));

					components.Add(new SkillsComponent("Mental_Skills", new List<string> { "Academics", "Computer", "Crafts", "Investigation", "Medicine", "Occult", "Politics", "Science" }, ColumnId.Left));
					components.Add(new SkillsComponent("Physical_Skills", new List<string> { "Athletics", "Brawl", "Drive", "Firearms", "Larceny", "Stealth", "Survival", "Weaponry" }, ColumnId.Left));
					components.Add(new SkillsComponent("Social_Skills", new List<string> { "Animal_Ken", "Empathy", "Expression", "Intimidation", "Persuasion", "Socialize", "Streetwise", "Subterfuge" }, ColumnId.Left));

					components.Add(new MeritComponent("Merits", "merit", true, new List<string>(), 5, ColumnId.Middle));
					components.Add(new MeritComponent("Arcana", "arcanum", false, new List<string> { "Death", "Fate", "Forces", "Life", "Matter", "Mind", "Prime", "Space", "Spirit", "Time" }, 5, ColumnId.Middle));
					components.Add(new AdvantagesComponent("Advantages", new List<string> { "Virtue", "Vice", "Size", "Speed", "Defense", "Armor", "Initiative Mod" }, ColumnId.Middle));
					components.Add(new ExperienceComponent("Experience", "Beats", ColumnId.Middle));
					components.Add(new ExperienceComponent("Arcane_Experience", "Arcane_Beats", ColumnId.Middle));

					components.Add(new HealthComponent("Health", ColumnId.Right));
					components.Add(new SimpleComponent("Willpower", ColumnId.Right));
					components.Add(new StatComponent("Gnosis", ColumnId.Right));
					components.Add(new SimpleComponent("Mana", ColumnId.Right));
					components.Add(new StatComponent("Wisdom", ColumnId.Right));
					components.Add(new MeritComponent("Conditions", "condition", true, new List<string>(), 0, ColumnId.Right));
					components.Add(new AspirationsComponent("Aspirations", 3, ColumnId.Right));
					components.Add(new AspirationsComponent("Obsessions", 1, ColumnId.Right));
					break;
				case SheetType.Werewolf:
					components.Add(new AttributesComponent("Mental_Attributes", new List<string> { "Intelligence", "Wits", "Resolve" }, ColumnId.Left));
					components.Add(new AttributesComponent("Physical_Attributes", new List<string> { "Strength", "Dexterity", "Stamina" }, ColumnId.Middle));
					components.Add(new AttributesComponent("Social_Attributes", new List<string> { "Presence", "Manipulation", "Composure" }, ColumnId.Right));

					components.Add(new SkillsComponent("Mental_Skills", new List<string> { "Academics", "Computer", "Crafts", "Investigation", "Medicine", "Occult", "Politics", "Science" }, ColumnId.Left));
					components.Add(new SkillsComponent("Physical_Skills", new List<string> { "Athletics", "Brawl", "Drive", "Firearms", "Larceny", "Stealth", "Survival", "Weaponry" }, ColumnId.Left));
					components.Add(new SkillsComponent("Social_Skills", new List<string> { "Animal_Ken", "Empathy", "Expression", "Intimidation", "Persuasion", "Socialize", "Streetwise", "Subterfuge" }, ColumnId.Left));

					components.Add(new MeritComponent("Merits", "merit", true, new List<string>(), 5, ColumnId.Middle));
					components.Add(new MeritComponent("Renown", "renown", false, new List<string> { "Cunning", "Glory", "Honor", "Purity", "Wisdom"}, 5, ColumnId.Middle));
					components.Add(new AdvantagesComponent("Advantages", new List<string> { "Bone", "Blood", "Size", "Speed", "Defense", "Armor", "Initiative Mod" }, ColumnId.Middle));
					components.Add(new ExperienceComponent("Experience", "Beats", ColumnId.Middle));

					components.Add(new HealthComponent("Health", ColumnId.Right));
					components.Add(new SimpleComponent("Willpower", ColumnId.Right));
					components.Add(new StatComponent("Primal_Urge", ColumnId.Right));
					components.Add(new SimpleComponent("Essence", ColumnId.Right));
					components.Add(new StatComponent("Harmony", ColumnId.Right));
					components.Add(new MeritComponent("Conditions", "condition", true, new List<string>(), 0, ColumnId.Right));
					components.Add(new AspirationsComponent("Aspirations", 3, ColumnId.Right));
					break;
				case SheetType.WolfBlooded:
					components.Add(new AttributesComponent("Mental_Attributes", new List<string> { "Intelligence", "Wits", "Resolve" }, ColumnId.Left));
					components.Add(new AttributesComponent("Physical_Attributes", new List<string> { "Strength", "Dexterity", "Stamina" }, ColumnId.Middle));
					components.Add(new AttributesComponent("Social_Attributes", new List<string> { "Presence", "Manipulation", "Composure" }, ColumnId.Right));

					components.Add(new SkillsComponent("Mental_Skills", new List<string> { "Academics", "Computer", "Crafts", "Investigation", "Medicine", "Occult", "Politics", "Science" }, ColumnId.Left));
					components.Add(new SkillsComponent("Physical_Skills", new List<string> { "Athletics", "Brawl", "Drive", "Firearms", "Larceny", "Stealth", "Survival", "Weaponry" }, ColumnId.Left));
					components.Add(new SkillsComponent("Social_Skills", new List<string> { "Animal_Ken", "Empathy", "Expression", "Intimidation", "Persuasion", "Socialize", "Streetwise", "Subterfuge" }, ColumnId.Left));

					components.Add(new MeritComponent("Merits", "merit", true, new List<string>(), 5, ColumnId.Middle));
					components.Add(new MeritComponent("Tells", "tell", true, new List<string>(), 0, ColumnId.Middle));
					components.Add(new AdvantagesComponent("Advantages", new List<string> { "Virtue", "Vice", "Size", "Speed", "Defense", "Armor", "Initiative Mod" }, ColumnId.Middle));
					components.Add(new ExperienceComponent("Experience", "Beats", ColumnId.Middle));

					components.Add(new HealthComponent("Health", ColumnId.Right));
					components.Add(new SimpleComponent("Willpower", ColumnId.Right));
					components.Add(new StatComponent("Integrity", ColumnId.Right));
					components.Add(new MeritComponent("Conditions", "condition", true, new List<string>(), 0, ColumnId.Right));
					components.Add(new AspirationsComponent("Aspirations", 3, ColumnId.Right));
					break;
				case SheetType.Spirit:
					components.Add(new AttributesComponent("Attributes", new List<string> { "Power", "Finesse", "Resistance" }, ColumnId.Left));

					components.Add(new MeritComponent("Influences", "influence", true, new List<string>(), 5, ColumnId.Middle));
					components.Add(new AdvantagesComponent("Advantages", new List<string> { "Ban", "Bane", "Size", "Speed", "Defense", "Armor", "Initiative Mod" }, ColumnId.Middle));

					components.Add(new HealthComponent("Corpus", ColumnId.Right));
					components.Add(new SimpleComponent("Willpower", ColumnId.Right));
					components.Add(new SimpleComponent("Essence", ColumnId.Right));
					components.Add(new StatComponent("Rank", ColumnId.Right));
					components.Add(new MeritComponent("Conditions", "condition", true, new List<string>(), 0, ColumnId.Right));
					components.Add(new AspirationsComponent("Aspirations", 3, ColumnId.Right));
					break;
			}
		}
	}
}
