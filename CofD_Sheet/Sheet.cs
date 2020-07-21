using CofD_Sheet.Modifyables;
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
		Vampire,
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
		public bool ChangedSinceSave
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
					components.Add(new TraitsComponent("Mental_Attributes", false, false, "attribute", true, new List<string> { "Intelligence", "Wits", "Resolve" }, 5, ColumnId.Left));
					components.Add(new TraitsComponent("Physical_Attributes", false, false, "attribute", true, new List<string> { "Strength", "Dexterity", "Stamina" }, 5, ColumnId.Middle));
					components.Add(new TraitsComponent("Social_Attributes", false, false, "attribute", true, new List<string> { "Presence", "Manipulation", "Composure" }, 5, ColumnId.Right));

					components.Add(new TraitsComponent("Mental_Skills", true, false, "skill", true, new List<string> { "Academics", "Computer", "Crafts", "Investigation", "Medicine", "Occult", "Politics", "Science" }, 5, ColumnId.Left));
					components.Add(new TraitsComponent("Physical_Skills", true, false, "skill", true, new List<string> { "Athletics", "Brawl", "Drive", "Firearms", "Larceny", "Stealth", "Survival", "Weaponry" }, 5, ColumnId.Left));
					components.Add(new TraitsComponent("Social_Skills", true, false, "skill", true, new List<string> { "Animal_Ken", "Empathy", "Expression", "Intimidation", "Persuasion", "Socialize", "Streetwise", "Subterfuge" }, 5, ColumnId.Left));

					components.Add(new TraitsComponent("Merits", false, true, "merit", false, new List<string>(), 5, ColumnId.Middle));
					components.Add(new AdvantagesComponent("Advantages", GetGetAdvantages(type), ColumnId.Middle));
					components.Add(new ExperienceComponent("Experience", "Beats", ColumnId.Middle));

					components.Add(new HealthComponent("Health", ColumnId.Right));
					components.Add(new ResourceComponent("Willpower", ColumnId.Right));
					components.Add(new StatComponent("Integrity", ColumnId.Right));
					components.Add(new TraitsComponent("Conditions", false, true, "condition", false, new List<string>(), 0, ColumnId.Right));
					components.Add(new AspirationsComponent("Aspirations", 3, ColumnId.Right));
					components.Add(new TraitsComponent("Equipment", false, true, "equipment", false, new List<string>(), 0, ColumnId.Right));
					break;
				case SheetType.Mage:
					components.Add(new TraitsComponent("Mental_Attributes", false, false, "attribute", true, new List<string> { "Intelligence", "Wits", "Resolve" }, 5, ColumnId.Left));
					components.Add(new TraitsComponent("Physical_Attributes", false, false, "attribute", true, new List<string> { "Strength", "Dexterity", "Stamina" }, 5, ColumnId.Middle));
					components.Add(new TraitsComponent("Social_Attributes", false, false, "attribute", true, new List<string> { "Presence", "Manipulation", "Composure" }, 5, ColumnId.Right));

					components.Add(new TraitsComponent("Mental_Skills", true, false, "skill", true, new List<string> { "Academics", "Computer", "Crafts", "Investigation", "Medicine", "Occult", "Politics", "Science" }, 5, ColumnId.Left));
					components.Add(new TraitsComponent("Physical_Skills", true, false, "skill", true, new List<string> { "Athletics", "Brawl", "Drive", "Firearms", "Larceny", "Stealth", "Survival", "Weaponry" }, 5, ColumnId.Left));
					components.Add(new TraitsComponent("Social_Skills", true, false, "skill", true, new List<string> { "Animal_Ken", "Empathy", "Expression", "Intimidation", "Persuasion", "Socialize", "Streetwise", "Subterfuge" }, 5, ColumnId.Left));

					components.Add(new TraitsComponent("Merits", false, true, "merit", false, new List<string>(), 5, ColumnId.Middle));
					components.Add(new TraitsComponent("Arcana", false, false, "arcanum", false, new List<string> { "Death", "Fate", "Forces", "Life", "Matter", "Mind", "Prime", "Space", "Spirit", "Time" }, 5, ColumnId.Middle));
					components.Add(new AdvantagesComponent("Advantages", GetGetAdvantages(type), ColumnId.Middle));
					components.Add(new ExperienceComponent("Experience", "Beats", ColumnId.Middle));
					components.Add(new ExperienceComponent("Arcane_Experience", "Arcane_Beats", ColumnId.Middle));

					components.Add(new HealthComponent("Health", ColumnId.Right));
					components.Add(new ResourceComponent("Willpower", ColumnId.Right));
					components.Add(new StatComponent("Gnosis", ColumnId.Right));
					components.Add(new ResourceComponent("Mana", ColumnId.Right));
					components.Add(new StatComponent("Wisdom", ColumnId.Right));
					components.Add(new TraitsComponent("Conditions", false, true, "condition", false, new List<string>(), 0, ColumnId.Right));
					components.Add(new AspirationsComponent("Aspirations", 3, ColumnId.Right));
					components.Add(new AspirationsComponent("Obsessions", 1, ColumnId.Right));
					components.Add(new TraitsComponent("Equipment", false, true, "equipment", false, new List<string>(), 0, ColumnId.Right));
					break;
				case SheetType.Werewolf:
					components.Add(new TraitsComponent("Mental_Attributes", false, false, "attribute", true, new List<string> { "Intelligence", "Wits", "Resolve" }, 5, ColumnId.Left));
					components.Add(new TraitsComponent("Physical_Attributes", false, false, "attribute", true, new List<string> { "Strength", "Dexterity", "Stamina" }, 5, ColumnId.Middle));
					components.Add(new TraitsComponent("Social_Attributes", false, false, "attribute", true, new List<string> { "Presence", "Manipulation", "Composure" }, 5, ColumnId.Right));

					components.Add(new TraitsComponent("Mental_Skills", true, false, "skill", true, new List<string> { "Academics", "Computer", "Crafts", "Investigation", "Medicine", "Occult", "Politics", "Science" }, 5, ColumnId.Left));
					components.Add(new TraitsComponent("Physical_Skills", true, false, "skill", true, new List<string> { "Athletics", "Brawl", "Drive", "Firearms", "Larceny", "Stealth", "Survival", "Weaponry" }, 5, ColumnId.Left));
					components.Add(new TraitsComponent("Social_Skills", true, false, "skill", true, new List<string> { "Animal_Ken", "Empathy", "Expression", "Intimidation", "Persuasion", "Socialize", "Streetwise", "Subterfuge" }, 5, ColumnId.Left));
					components.Add(GetWerewolfFormModSetComponent());

					components.Add(new TraitsComponent("Merits", false, true, "merit", false, new List<string>(), 5, ColumnId.Middle));
					components.Add(new TraitsComponent("Renown", false, false, "renown", false, new List<string> { "Cunning", "Glory", "Honor", "Purity", "Wisdom" }, 5, ColumnId.Middle));
					components.Add(new AdvantagesComponent("Advantages", GetGetAdvantages(type), ColumnId.Middle));
					components.Add(new ExperienceComponent("Experience", "Beats", ColumnId.Middle));
					components.Add(new TraitsComponent("Rites", false, true, "rite", false, new List<string>(), 0, ColumnId.Middle));

					components.Add(new HealthComponent("Health", ColumnId.Right));
					components.Add(new ResourceComponent("Willpower", ColumnId.Right));
					components.Add(new StatComponent("Primal_Urge", ColumnId.Right));
					components.Add(new ResourceComponent("Essence", ColumnId.Right));
					components.Add(new StatComponent("Harmony", ColumnId.Right));
					components.Add(new TraitsComponent("Conditions", false, true, "condition", false, new List<string>(), 0, ColumnId.Right));
					components.Add(new AspirationsComponent("Aspirations", 4, ColumnId.Right));
					components.Add(new TraitsComponent("Gifts", false, true, "gift", false, new List<string>(), 0, ColumnId.Right));
					components.Add(new TraitsComponent("Equipment", false, true, "equipment", false, new List<string>(), 0, ColumnId.Right));
					break;
				case SheetType.Vampire:
					components.Add(new TraitsComponent("Mental_Attributes", false, false, "attribute", true, new List<string> { "Intelligence", "Wits", "Resolve" }, 5, ColumnId.Left));
					components.Add(new TraitsComponent("Physical_Attributes", false, false, "attribute", true, new List<string> { "Strength", "Dexterity", "Stamina" }, 5, ColumnId.Middle));
					components.Add(new TraitsComponent("Social_Attributes", false, false, "attribute", true, new List<string> { "Presence", "Manipulation", "Composure" }, 5, ColumnId.Right));

					components.Add(new TraitsComponent("Mental_Skills", true, false, "skill", true, new List<string> { "Academics", "Computer", "Crafts", "Investigation", "Medicine", "Occult", "Politics", "Science" }, 5, ColumnId.Left));
					components.Add(new TraitsComponent("Physical_Skills", true, false, "skill", true, new List<string> { "Athletics", "Brawl", "Drive", "Firearms", "Larceny", "Stealth", "Survival", "Weaponry" }, 5, ColumnId.Left));
					components.Add(new TraitsComponent("Social_Skills", true, false, "skill", true, new List<string> { "Animal_Ken", "Empathy", "Expression", "Intimidation", "Persuasion", "Socialize", "Streetwise", "Subterfuge" }, 5, ColumnId.Left));

					components.Add(new TraitsComponent("Merits", false, true, "merit", false, new List<string>(), 5, ColumnId.Middle));
					components.Add(new TraitsComponent("Disciplines", false, true, "discipline", false, new List<string>(), 5, ColumnId.Middle));
					components.Add(new AdvantagesComponent("Advantages", GetGetAdvantages(type), ColumnId.Middle));
					components.Add(new TraitsComponent("Banes", false, true, "bane", false, new List<string>(), 0, ColumnId.Middle));
					components.Add(new ExperienceComponent("Experience", "Beats", ColumnId.Middle));

					components.Add(new HealthComponent("Health", ColumnId.Right));
					components.Add(new ResourceComponent("Willpower", ColumnId.Right));
					components.Add(new StatComponent("Blood Potency", ColumnId.Right));
					components.Add(new ResourceComponent("Vitae", ColumnId.Right));
					components.Add(new StatComponent("Humanity", ColumnId.Right));
					components.Add(new TraitsComponent("Conditions", false, true, "condition", false, new List<string>(), 0, ColumnId.Right));
					components.Add(new AspirationsComponent("Aspirations", 3, ColumnId.Right));
					components.Add(new TraitsComponent("Equipment", false, true, "equipment", false, new List<string>(), 0, ColumnId.Right));
					break;
				case SheetType.WolfBlooded:
					components.Add(new TraitsComponent("Mental_Attributes", false, false, "attribute", true, new List<string> { "Intelligence", "Wits", "Resolve" }, 5, ColumnId.Left));
					components.Add(new TraitsComponent("Physical_Attributes", false, false, "attribute", true, new List<string> { "Strength", "Dexterity", "Stamina" }, 5, ColumnId.Middle));
					components.Add(new TraitsComponent("Social_Attributes", false, false, "attribute", true, new List<string> { "Presence", "Manipulation", "Composure" }, 5, ColumnId.Right));

					components.Add(new TraitsComponent("Mental_Skills", true, false, "skill", true, new List<string> { "Academics", "Computer", "Crafts", "Investigation", "Medicine", "Occult", "Politics", "Science" }, 5, ColumnId.Left));
					components.Add(new TraitsComponent("Physical_Skills", true, false, "skill", true, new List<string> { "Athletics", "Brawl", "Drive", "Firearms", "Larceny", "Stealth", "Survival", "Weaponry" }, 5, ColumnId.Left));
					components.Add(new TraitsComponent("Social_Skills", true, false, "skill", true, new List<string> { "Animal_Ken", "Empathy", "Expression", "Intimidation", "Persuasion", "Socialize", "Streetwise", "Subterfuge" }, 5, ColumnId.Left));

					components.Add(new TraitsComponent("Merits", false, true, "merit", false, new List<string>(), 5, ColumnId.Middle));
					components.Add(new TraitsComponent("Tells", false, true, "tell", false, new List<string>(), 0, ColumnId.Middle));
					components.Add(new AdvantagesComponent("Advantages", GetGetAdvantages(type), ColumnId.Middle));
					components.Add(new ExperienceComponent("Experience", "Beats", ColumnId.Middle));

					components.Add(new HealthComponent("Health", ColumnId.Right));
					components.Add(new ResourceComponent("Willpower", ColumnId.Right));
					components.Add(new StatComponent("Integrity", ColumnId.Right));
					components.Add(new TraitsComponent("Conditions", false, true, "condition", false, new List<string>(), 0, ColumnId.Right));
					components.Add(new AspirationsComponent("Aspirations", 3, ColumnId.Right));
					components.Add(new TraitsComponent("Equipment", false, true, "equipment", false, new List<string>(), 0, ColumnId.Right));
					break;
				case SheetType.Spirit:
					components.Add(new TraitsComponent("Attributes", false, false, "attribute", true, new List<string> { "Power", "Finesse", "Resistance" }, 15, ColumnId.Left));

					components.Add(new TraitsComponent("Influences", false, true, "influence", false, new List<string>(), 5, ColumnId.Middle));
					components.Add(new AdvantagesComponent("Advantages", GetGetAdvantages(type), ColumnId.Middle));

					components.Add(new HealthComponent("Corpus", ColumnId.Right));
					components.Add(new ResourceComponent("Willpower", ColumnId.Right));
					components.Add(new ResourceComponent("Essence", ColumnId.Right));
					components.Add(new StatComponent("Rank", ColumnId.Right));
					components.Add(new TraitsComponent("Conditions", false, true, "condition", false, new List<string>(), 0, ColumnId.Right));
					components.Add(new AspirationsComponent("Aspirations", 3, ColumnId.Right));
					break;
			}
		}

		public void RefreshModifications()
		{
			List<ModificationSetComponent> modSetComponents = new List<ModificationSetComponent>();
			foreach (ISheetComponent component in components)
			{
				if (component is ModificationSetComponent modComponent)
				{
					modSetComponents.Add(modComponent);
				}
				else
				{
					component.ResetModifications();
				}
			}
			foreach (ModificationSetComponent modSetComponent in modSetComponents)
			{
				modSetComponent.sets[modSetComponent.ActiveIndex].Apply();
			}
			foreach (ISheetComponent component in components)
			{
				component.OnModificationsComplete();
			}
		}

		private ModificationSetComponent GetWerewolfFormModSetComponent()
		{
			ModificationSetComponent FormsComponent = new ModificationSetComponent("Forms", new List<string> { "Hishu", "Dalu", "Gauru", "Urshul", "Urhan" }, ColumnId.Left);
			//hishu
			//default, no modifications made

			//dalu
			FormsComponent.sets[1].modifications.Add(new ModificationSetComponent.IntModification(new List<string>() { "Physical_Attributes", "Strength" }, 1, IntModificationType.Delta));
			FormsComponent.sets[1].modifications.Add(new ModificationSetComponent.IntModification(new List<string>() { "Physical_Attributes", "Stamina" }, 1, IntModificationType.Delta));
			FormsComponent.sets[1].modifications.Add(new ModificationSetComponent.IntModification(new List<string>() { "Social_Attributes", "Manipulation" }, -1, IntModificationType.Delta));
			FormsComponent.sets[1].modifications.Add(new ModificationSetComponent.IntModification(new List<string>() { "Health", "MaxValue" }, 2, IntModificationType.Delta));
			FormsComponent.sets[1].modifications.Add(new ModificationSetComponent.IntModification(new List<string>() { "Advantages", "Size" }, 1, IntModificationType.Delta));
			FormsComponent.sets[1].modifications.Add(new ModificationSetComponent.IntModification(new List<string>() { "Advantages", "Speed" }, 1, IntModificationType.Delta));
			FormsComponent.sets[1].modifications.Add(new ModificationSetComponent.IntModification(new List<string>() { "Advantages", "Perception" }, 2, IntModificationType.Delta));

			//gauru
			FormsComponent.sets[2].modifications.Add(new ModificationSetComponent.IntModification(new List<string>() { "Physical_Attributes", "Strength" }, 3, IntModificationType.Delta));
			FormsComponent.sets[2].modifications.Add(new ModificationSetComponent.IntModification(new List<string>() { "Physical_Attributes", "Dexterity" }, 1, IntModificationType.Delta));
			FormsComponent.sets[2].modifications.Add(new ModificationSetComponent.IntModification(new List<string>() { "Physical_Attributes", "Stamina" }, 2, IntModificationType.Delta));
			FormsComponent.sets[2].modifications.Add(new ModificationSetComponent.IntModification(new List<string>() { "Health", "MaxValue" }, 4, IntModificationType.Delta));
			FormsComponent.sets[2].modifications.Add(new ModificationSetComponent.IntModification(new List<string>() { "Advantages", "Size" }, 2, IntModificationType.Delta));
			FormsComponent.sets[2].modifications.Add(new ModificationSetComponent.IntModification(new List<string>() { "Advantages", "Speed" }, 4, IntModificationType.Delta));
			FormsComponent.sets[2].modifications.Add(new ModificationSetComponent.IntModification(new List<string>() { "Advantages", "Perception" }, 3, IntModificationType.Delta));
			FormsComponent.sets[2].modifications.Add(new ModificationSetComponent.IntModification(new List<string>() { "Advantages", "Initiative Mod" }, 1, IntModificationType.Delta));

			//urshul
			FormsComponent.sets[3].modifications.Add(new ModificationSetComponent.IntModification(new List<string>() { "Physical_Attributes", "Strength" }, 2, IntModificationType.Delta));
			FormsComponent.sets[3].modifications.Add(new ModificationSetComponent.IntModification(new List<string>() { "Physical_Attributes", "Dexterity" }, 2, IntModificationType.Delta));
			FormsComponent.sets[3].modifications.Add(new ModificationSetComponent.IntModification(new List<string>() { "Physical_Attributes", "Stamina" }, 2, IntModificationType.Delta));
			FormsComponent.sets[3].modifications.Add(new ModificationSetComponent.IntModification(new List<string>() { "Social_Attributes", "Manipulation" }, -1, IntModificationType.Delta));
			FormsComponent.sets[3].modifications.Add(new ModificationSetComponent.IntModification(new List<string>() { "Health", "MaxValue" }, 3, IntModificationType.Delta));
			FormsComponent.sets[3].modifications.Add(new ModificationSetComponent.IntModification(new List<string>() { "Advantages", "Size" }, 1, IntModificationType.Delta));
			FormsComponent.sets[3].modifications.Add(new ModificationSetComponent.IntModification(new List<string>() { "Advantages", "Speed" }, 7, IntModificationType.Delta));
			FormsComponent.sets[3].modifications.Add(new ModificationSetComponent.IntModification(new List<string>() { "Advantages", "Perception" }, 3, IntModificationType.Delta));
			FormsComponent.sets[3].modifications.Add(new ModificationSetComponent.IntModification(new List<string>() { "Advantages", "Initiative Mod" }, 2, IntModificationType.Delta));

			//urhan
			FormsComponent.sets[4].modifications.Add(new ModificationSetComponent.IntModification(new List<string>() { "Physical_Attributes", "Dexterity" }, 2, IntModificationType.Delta));
			FormsComponent.sets[4].modifications.Add(new ModificationSetComponent.IntModification(new List<string>() { "Physical_Attributes", "Stamina" }, 1, IntModificationType.Delta));
			FormsComponent.sets[4].modifications.Add(new ModificationSetComponent.IntModification(new List<string>() { "Social_Attributes", "Manipulation" }, -1, IntModificationType.Delta));
			FormsComponent.sets[4].modifications.Add(new ModificationSetComponent.IntModification(new List<string>() { "Advantages", "Size" }, -1, IntModificationType.Delta));
			FormsComponent.sets[4].modifications.Add(new ModificationSetComponent.IntModification(new List<string>() { "Advantages", "Speed" }, 5, IntModificationType.Delta));
			FormsComponent.sets[4].modifications.Add(new ModificationSetComponent.IntModification(new List<string>() { "Advantages", "Perception" }, 4, IntModificationType.Delta));
			FormsComponent.sets[4].modifications.Add(new ModificationSetComponent.IntModification(new List<string>() { "Advantages", "Initiative Mod" }, 2, IntModificationType.Delta));

			return FormsComponent;
		}

		private List<AdvantagesComponent.Advantage> GetGetAdvantages(SheetType type)
		{
			List<AdvantagesComponent.Advantage> retVal = new List<AdvantagesComponent.Advantage>();
			switch (type)
			{
				case SheetType.Mortal:
				case SheetType.WolfBlooded:
				case SheetType.Mage:
					retVal.Add(new AdvantagesComponent.StringAdvantage("Virtue"));
					retVal.Add(new AdvantagesComponent.StringAdvantage("Vice"));
					break;
				case SheetType.Werewolf:
					retVal.Add(new AdvantagesComponent.StringAdvantage("Bone"));
					retVal.Add(new AdvantagesComponent.StringAdvantage("Blood"));
					break;
				case SheetType.Vampire:
					retVal.Add(new AdvantagesComponent.StringAdvantage("Mask"));
					retVal.Add(new AdvantagesComponent.StringAdvantage("Dirge"));
					break;
				case SheetType.Spirit:
					retVal.Add(new AdvantagesComponent.StringAdvantage("Ban"));
					retVal.Add(new AdvantagesComponent.StringAdvantage("Bane"));
					break;
			}

			retVal.Add(new AdvantagesComponent.NumericAdvantage("Size"));
			retVal.Add(new AdvantagesComponent.NumericAdvantage("Speed"));
			retVal.Add(new AdvantagesComponent.NumericAdvantage("Defense"));
			retVal.Add(new AdvantagesComponent.ArmorAdvantage("Armor"));
			retVal.Add(new AdvantagesComponent.NumericAdvantage("Initiative Mod"));

			return retVal;
		}
	}
}
