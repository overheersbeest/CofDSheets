using CofD_Sheet.Modifications;
using CofD_Sheet.Modifyables;
using CofD_Sheet.Sheet_Components;
using System;
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

		[XmlElement]
		public ModificationSetComponent.ModificationSet DefaultModSet = new ModificationSetComponent.ModificationSet();

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

		[XmlIgnore]
		public bool allowRefreshingMods = true;

		public Sheet()
		{ }

		public Sheet(SheetType type)
		{
			switch (type)
			{
				case SheetType.Mortal:
					AddMortalSkillsAndAttributes();

					components.Add(new TraitsComponent("Merits", true, "subtype", true, "merit", false, new List<string>(), 0, 5, ColumnId.Middle));
					components.Add(new AdvantagesComponent("Advantages", GetGetAdvantages(type), ColumnId.Middle));
					components.Add(new ExperienceComponent("Experience", "Beats", ColumnId.Middle));

					components.Add(new HealthComponent("Health", ColumnId.Right));
					components.Add(new ResourceComponent("Willpower", true, true, 0, 0, ColumnId.Right));
					components.Add(new ResourceComponent("Integrity", false, false, 7, 10, ColumnId.Right));
					components.Add(new TraitsComponent("Conditions", true, "effect", true, "condition", false, new List<string>(), 0, 0, ColumnId.Right));
					components.Add(new AspirationsComponent("Aspirations", 3, ColumnId.Right));
					components.Add(new TraitsComponent("Equipment", false, "note", true, "equipment", false, new List<string>(), 0, 0, ColumnId.Right));
					break;
				case SheetType.Mage:
					AddMortalSkillsAndAttributes();

					components.Add(new TraitsComponent("Merits", true, "subtype", true, "merit", false, new List<string>(), 0, 5, ColumnId.Middle));
					components.Add(new TraitsComponent("Arcana", false, "note", false, "arcanum", false, new List<string> { "Death", "Fate", "Forces", "Life", "Matter", "Mind", "Prime", "Space", "Spirit", "Time" }, 0, 5, ColumnId.Middle));
					components.Add(new AdvantagesComponent("Advantages", GetGetAdvantages(type), ColumnId.Middle));
					components.Add(new ExperienceComponent("Experience", "Beats", ColumnId.Middle));
					components.Add(new ExperienceComponent("Arcane_Experience", "Arcane_Beats", ColumnId.Middle));

					components.Add(new HealthComponent("Health", ColumnId.Right));
					components.Add(new ResourceComponent("Willpower", true, true, 0, 0, ColumnId.Right));
					components.Add(new ResourceComponent("Gnosis", false, false, 1, 10, ColumnId.Right));
					components.Add(new ResourceComponent("Mana", true, true, 0, 10, ColumnId.Right));
					components.Add(new ResourceComponent("Wisdom", false, false, 7, 10, ColumnId.Right));
					components.Add(new TraitsComponent("Conditions", true, "effect", true, "condition", false, new List<string>(), 0, 0, ColumnId.Right));
					components.Add(new AspirationsComponent("Aspirations", 3, ColumnId.Right));
					components.Add(new AspirationsComponent("Obsessions", 1, ColumnId.Right));
					components.Add(new TraitsComponent("Equipment", false, "note", true, "equipment", false, new List<string>(), 0, 0, ColumnId.Right));
					break;
				case SheetType.Werewolf:
					AddMortalSkillsAndAttributes();

					components.Add(GetWerewolfFormModSetComponent());

					components.Add(new TraitsComponent("Merits", true, "subtype", true, "merit", false, new List<string>(), 0, 5, ColumnId.Middle));
					components.Add(new TraitsComponent("Renown", false, "note", false, "renown", false, new List<string> { "Cunning", "Glory", "Honor", "Purity", "Wisdom" }, 0, 5, ColumnId.Middle));
					components.Add(new AdvantagesComponent("Advantages", GetGetAdvantages(type), ColumnId.Middle));
					components.Add(new ExperienceComponent("Experience", "Beats", ColumnId.Middle));
					components.Add(new TraitsComponent("Rites", true, "symbol", true, "rite", false, new List<string>(), 0, 0, ColumnId.Middle));

					components.Add(new HealthComponent("Health", ColumnId.Right));
					components.Add(new ResourceComponent("Willpower", true, true, 0, 0, ColumnId.Right));
					components.Add(new ResourceComponent("Primal_Urge", false, false, 1, 10, ColumnId.Right));
					components.Add(new ResourceComponent("Essence", true, true, 0, 10, ColumnId.Right));
					components.Add(new ResourceComponent("Harmony", false, false, 7, 10, ColumnId.Right));
					components.Add(new TraitsComponent("Conditions", true, "effect", true, "condition", false, new List<string>(), 0, 0, ColumnId.Right));
					components.Add(new AspirationsComponent("Aspirations", 4, ColumnId.Right));
					components.Add(new TraitsComponent("Gifts", true, "facet", true, "gift", false, new List<string>(), 0, 0, ColumnId.Right));
					components.Add(new TraitsComponent("Equipment", true, "note", true, "equipment", false, new List<string>(), 0, 0, ColumnId.Right));
					break;
				case SheetType.Vampire:
					AddMortalSkillsAndAttributes();

					components.Add(new TraitsComponent("Merits", true, "subtype", true, "merit", false, new List<string>(), 0, 5, ColumnId.Middle));
					components.Add(new TraitsComponent("Disciplines", false, "note", true, "discipline", false, new List<string>(), 0, 5, ColumnId.Middle));
					components.Add(new AdvantagesComponent("Advantages", GetGetAdvantages(type), ColumnId.Middle));
					components.Add(new TraitsComponent("Banes", false, "note", true, "bane", false, new List<string>(), 0, 0, ColumnId.Middle));
					components.Add(new ExperienceComponent("Experience", "Beats", ColumnId.Middle));

					components.Add(new HealthComponent("Health", ColumnId.Right));
					components.Add(new ResourceComponent("Willpower", true, true, 0, 0, ColumnId.Right));
					components.Add(new ResourceComponent("Blood Potency", false, false, 1, 10, ColumnId.Right));
					components.Add(new ResourceComponent("Vitae", true, true, 0, 10, ColumnId.Right));
					components.Add(new ResourceComponent("Humanity", false, false, 7, 10, ColumnId.Right));
					components.Add(new TraitsComponent("Conditions", true, "effect", true, "condition", false, new List<string>(), 0, 0, ColumnId.Right));
					components.Add(new AspirationsComponent("Aspirations", 3, ColumnId.Right));
					components.Add(new TraitsComponent("Equipment", false, "note", true, "equipment", false, new List<string>(), 0, 0, ColumnId.Right));
					break;
				case SheetType.WolfBlooded:
					AddMortalSkillsAndAttributes();

					components.Add(new TraitsComponent("Merits", true, "subtype", true, "merit", false, new List<string>(), 0, 5, ColumnId.Middle));
					components.Add(new TraitsComponent("Tells", false, "note", true, "tell", false, new List<string>(), 0, 0, ColumnId.Middle));
					components.Add(new AdvantagesComponent("Advantages", GetGetAdvantages(type), ColumnId.Middle));
					components.Add(new ExperienceComponent("Experience", "Beats", ColumnId.Middle));
					components.Add(new TraitsComponent("Rites", true, "symbol", true, "rite", false, new List<string>(), 0, 0, ColumnId.Middle));

					components.Add(new HealthComponent("Health", ColumnId.Right));
					components.Add(new ResourceComponent("Willpower", true, true, 0, 0, ColumnId.Right));
					components.Add(new ResourceComponent("Integrity", false, false, 7, 10, ColumnId.Right));
					components.Add(new TraitsComponent("Conditions", true, "effect", true, "condition", false, new List<string>(), 0, 0, ColumnId.Right));
					components.Add(new AspirationsComponent("Aspirations", 3, ColumnId.Right));
					components.Add(new TraitsComponent("Equipment", false, "note", true, "equipment", false, new List<string>(), 0, 0, ColumnId.Right));
					break;
				case SheetType.Spirit:
					components.Add(new TraitsComponent("Attributes", false, "note", false, "attribute", true, new List<string> { "Power", "Finesse", "Resistance" }, 1, 15, ColumnId.Left));

					components.Add(new TraitsComponent("Influences", false, "note", true, "influence", false, new List<string>(), 0, 5, ColumnId.Middle));
					components.Add(new AdvantagesComponent("Advantages", GetGetAdvantages(type), ColumnId.Middle));

					components.Add(new HealthComponent("Corpus", ColumnId.Right));
					components.Add(new ResourceComponent("Willpower", true, true, 0, 0, ColumnId.Right));
					components.Add(new ResourceComponent("Essence", true, true, 0, 10, ColumnId.Right));
					components.Add(new ResourceComponent("Rank", false, false, 0, 5, ColumnId.Right));
					components.Add(new TraitsComponent("Conditions", true, "effect", true, "condition", false, new List<string>(), 0, 0, ColumnId.Right));
					components.Add(new AspirationsComponent("Aspirations", 3, ColumnId.Right));
					break;
			}
			SetDefaultModSet(type);
		}

		public void RefreshModifications(bool supressDrawing = true)
		{
			if (!allowRefreshingMods)
			{
				return;
			}
			allowRefreshingMods = false;
			if (supressDrawing)
			{
				Form1.instance.drawingHelper.SuspendDrawing();
			}

			List<ModificationSetComponent.ModificationSet> modSets = new List<ModificationSetComponent.ModificationSet>();
			foreach (ISheetComponent component in components)
			{
				if (component is ModificationSetComponent modComponent)
				{
					modSets.Add(modComponent.sets[modComponent.ActiveIndex]);
				}
				else
				{
					component.ResetModifications();
				}
			}
			//apply the Default Mod Set last, so it gets the correct values for its formulas
			modSets.Add(DefaultModSet);

			foreach (ModificationSetComponent.ModificationSet modSet in modSets)
			{
				modSet.Apply();
			}
			foreach (ISheetComponent component in components)
			{
				component.OnModificationsComplete();
			}

			if (supressDrawing)
			{
				Form1.instance.drawingHelper.ResumeDrawing(true);
			}
			allowRefreshingMods = true;
		}

		public int QueryInt(string query)
		{
			List<string> path = new List<string>(query.Split('.'));
			if (path.Count > 0)
			{
				foreach (ISheetComponent component in components)
				{
					if (component.name == path[0])
					{
						return component.QueryInt(path);
					}
				}
			}

			throw new Exception("Query could not find component: " + query);
		}

		private void AddMortalSkillsAndAttributes()
		{
			components.Add(new TraitsComponent("Mental_Attributes", false, "note", false, "attribute", true, new List<string> { "Intelligence", "Wits", "Resolve" }, 1, 5, ColumnId.Left));
			components.Add(new TraitsComponent("Physical_Attributes", false, "note", false, "attribute", true, new List<string> { "Strength", "Dexterity", "Stamina" }, 1, 5, ColumnId.Middle));
			components.Add(new TraitsComponent("Social_Attributes", false, "note", false, "attribute", true, new List<string> { "Presence", "Manipulation", "Composure" }, 1, 5, ColumnId.Right));

			components.Add(new TraitsComponent("Mental_Skills", true, "specialty", false, "skill", true, new List<string> { "Academics", "Computer", "Crafts", "Investigation", "Medicine", "Occult", "Politics", "Science" }, 0, 5, ColumnId.Left));
			components.Add(new TraitsComponent("Physical_Skills", true, "specialty", false, "skill", true, new List<string> { "Athletics", "Brawl", "Drive", "Firearms", "Larceny", "Stealth", "Survival", "Weaponry" }, 0, 5, ColumnId.Left));
			components.Add(new TraitsComponent("Social_Skills", true, "specialty", false, "skill", true, new List<string> { "Animal_Ken", "Empathy", "Expression", "Intimidation", "Persuasion", "Socialize", "Streetwise", "Subterfuge" }, 0, 5, ColumnId.Left));
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
					retVal.Add(new AdvantagesComponent.StringAdvantage("Blood"));
					retVal.Add(new AdvantagesComponent.StringAdvantage("Bone"));
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

			retVal.Add(new AdvantagesComponent.NumericAdvantage("Size", 5));
			retVal.Add(new AdvantagesComponent.NumericAdvantage("Speed"));
			retVal.Add(new AdvantagesComponent.NumericAdvantage("Defense"));
			retVal.Add(new AdvantagesComponent.ArmorAdvantage("Armor"));
			retVal.Add(new AdvantagesComponent.NumericAdvantage("Initiative_Mod"));

			if (type == SheetType.Werewolf)
			{
				retVal.Add(new AdvantagesComponent.NumericAdvantage("Wolf_Senses"));
			}

			return retVal;
		}

		private ModificationSetComponent GetWerewolfFormModSetComponent()
		{
			ModificationSetComponent FormsComponent = new ModificationSetComponent("Forms", new List<string> { "Hishu", "Dalu", "Gauru", "Urshul", "Urhan" }, ColumnId.Left);
			//hishu
			FormsComponent.sets[0].modifications.Add(new IntModification(new List<string>() { "Advantages", "Wolf_Senses" }, 1, "", IntModificationType.Delta));

			//dalu
			FormsComponent.sets[1].modifications.Add(new IntModification(new List<string>() { "Physical_Attributes", "Strength" }, 1, "", IntModificationType.Delta));
			FormsComponent.sets[1].modifications.Add(new IntModification(new List<string>() { "Physical_Attributes", "Stamina" }, 1, "", IntModificationType.Delta));
			FormsComponent.sets[1].modifications.Add(new IntModification(new List<string>() { "Social_Attributes", "Manipulation" }, -1, "", IntModificationType.Delta));
			FormsComponent.sets[1].modifications.Add(new IntModification(new List<string>() { "Advantages", "Size" }, 1, "", IntModificationType.Delta));
			FormsComponent.sets[1].modifications.Add(new IntModification(new List<string>() { "Advantages", "Wolf_Senses" }, 2, "", IntModificationType.Delta));

			//gauru
			FormsComponent.sets[2].modifications.Add(new IntModification(new List<string>() { "Physical_Attributes", "Strength" }, 3, "", IntModificationType.Delta));
			FormsComponent.sets[2].modifications.Add(new IntModification(new List<string>() { "Physical_Attributes", "Dexterity" }, 1, "", IntModificationType.Delta));
			FormsComponent.sets[2].modifications.Add(new IntModification(new List<string>() { "Physical_Attributes", "Stamina" }, 2, "", IntModificationType.Delta));
			FormsComponent.sets[2].modifications.Add(new IntModification(new List<string>() { "Advantages", "Size" }, 2, "", IntModificationType.Delta));
			FormsComponent.sets[2].modifications.Add(new IntModification(new List<string>() { "Advantages", "Wolf_Senses" }, 3, "", IntModificationType.Delta));

			//urshul
			FormsComponent.sets[3].modifications.Add(new IntModification(new List<string>() { "Physical_Attributes", "Strength" }, 2, "", IntModificationType.Delta));
			FormsComponent.sets[3].modifications.Add(new IntModification(new List<string>() { "Physical_Attributes", "Dexterity" }, 2, "", IntModificationType.Delta));
			FormsComponent.sets[3].modifications.Add(new IntModification(new List<string>() { "Physical_Attributes", "Stamina" }, 2, "", IntModificationType.Delta));
			FormsComponent.sets[3].modifications.Add(new IntModification(new List<string>() { "Social_Attributes", "Manipulation" }, -1, "", IntModificationType.Delta));
			FormsComponent.sets[3].modifications.Add(new IntModification(new List<string>() { "Advantages", "Size" }, 1, "", IntModificationType.Delta));
			FormsComponent.sets[3].modifications.Add(new IntModification(new List<string>() { "Advantages", "Speed" }, 2, "", IntModificationType.Delta));
			FormsComponent.sets[3].modifications.Add(new IntModification(new List<string>() { "Advantages", "Wolf_Senses" }, 3, "", IntModificationType.Delta));

			//urhan
			FormsComponent.sets[4].modifications.Add(new IntModification(new List<string>() { "Physical_Attributes", "Dexterity" }, 2, "", IntModificationType.Delta));
			FormsComponent.sets[4].modifications.Add(new IntModification(new List<string>() { "Physical_Attributes", "Stamina" }, 1, "", IntModificationType.Delta));
			FormsComponent.sets[4].modifications.Add(new IntModification(new List<string>() { "Social_Attributes", "Manipulation" }, -1, "", IntModificationType.Delta));
			FormsComponent.sets[4].modifications.Add(new IntModification(new List<string>() { "Advantages", "Size" }, -1, "", IntModificationType.Delta));
			FormsComponent.sets[4].modifications.Add(new IntModification(new List<string>() { "Advantages", "Speed" }, 4, "", IntModificationType.Delta));
			FormsComponent.sets[4].modifications.Add(new IntModification(new List<string>() { "Advantages", "Wolf_Senses" }, 4, "", IntModificationType.Delta));

			return FormsComponent;
		}

		private void SetDefaultModSet(SheetType type)
		{
			DefaultModSet = new ModificationSetComponent.ModificationSet("DefaultModSet");
			switch (type)
			{
				case SheetType.Mortal:
				case SheetType.Mage:
				case SheetType.Werewolf:
				case SheetType.Vampire:
				case SheetType.WolfBlooded:
					DefaultModSet.modifications.Add(new IntModification(new List<string>() { "Health", "MaxValue" }, 0, "(Advantages.Size+Physical_Attributes.Stamina)", IntModificationType.Delta));
					DefaultModSet.modifications.Add(new IntModification(new List<string>() { "Willpower", "MaxValue" }, 0, "[Min,10,(Mental_Attributes.Resolve+Social_Attributes.Composure)]", IntModificationType.Delta));
					DefaultModSet.modifications.Add(new IntModification(new List<string>() { "Advantages", "Initiative_Mod" }, 0, "(Physical_Attributes.Dexterity+Social_Attributes.Composure)", IntModificationType.Delta));
					DefaultModSet.modifications.Add(new IntModification(new List<string>() { "Advantages", "Defense" }, 0, "([Min,Physical_Attributes.Dexterity,Mental_Attributes.Wits]+Physical_Skills.Athletics)", IntModificationType.Delta));
					DefaultModSet.modifications.Add(new IntModification(new List<string>() { "Advantages", "Speed" }, 0, "(Advantages.Size+(Physical_Attributes.Strength+Physical_Attributes.Dexterity))", IntModificationType.Delta));
					break;
				case SheetType.Spirit:
					DefaultModSet.modifications.Add(new IntModification(new List<string>() { "Corpus", "MaxValue" }, 0, "(Advantages.Size+Attributes.Resistance)", IntModificationType.Delta));
					DefaultModSet.modifications.Add(new IntModification(new List<string>() { "Willpower", "MaxValue" }, 0, "[Min,10,(Attributes.Finesse+Attributes.Resistance)]", IntModificationType.Delta));
					DefaultModSet.modifications.Add(new IntModification(new List<string>() { "Advantages", "Initiative_Mod" }, 0, "(Attributes.Finesse+Attributes.Resistance)", IntModificationType.Delta));
					DefaultModSet.modifications.Add(new IntModification(new List<string>() { "Advantages", "Defense" }, 0, "[Min,Attributes.Power,Attributes.Finesse]", IntModificationType.Delta));
					DefaultModSet.modifications.Add(new IntModification(new List<string>() { "Advantages", "Speed" }, 0, "(Attributes.Power+Attributes.Finesse)", IntModificationType.Delta));
					break;
			}
		}
	}
}
