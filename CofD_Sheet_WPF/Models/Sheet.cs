﻿using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace CofD_Sheet_WPF.Models
{
	public enum SheetType
	{
		Mortal,
		Mage,
		Werewolf,
		Spirit
	}

	[XmlRoot]
	public class Sheet : ViewModelBase
	{
		public Sheet()
		{ }

		public Sheet(SheetType type)
		{
			fields.Add(new Field("Name", ""));
			fields.Add(new Field("Player", ""));
			fields.Add(new Field("Chronicle", ""));
			fields.Add(new Field("Concept", ""));
			switch (type)
			{
				case SheetType.Mortal:
					fields.Add(new Field("Vice", ""));
					fields.Add(new Field("Virtue", ""));
					//components.Add(new AttributesComponent("Mental_Attributes", new List<string> { "Intelligence", "Wits", "Resolve" }, 0));
					//components.Add(new AttributesComponent("Physical_Attributes", new List<string> { "Strength", "Dexterity", "Stamina" }, 1));
					//components.Add(new AttributesComponent("Social_Attributes", new List<string> { "Presence", "Manipulation", "Composure" }, 2));
					//
					//components.Add(new SkillsComponent("Mental_Skills", new List<string> { "Academics", "Computer", "Crafts", "Investigation", "Medicine", "Occult", "Politics", "Science" }, 0));
					//components.Add(new SkillsComponent("Physical_Skills", new List<string> { "Athletics", "Brawl", "Drive", "Firearms", "Larceny", "Stealth", "Survival", "Weaponry" }, 0));
					//components.Add(new SkillsComponent("Social_Skills", new List<string> { "Animal_Ken", "Empathy", "Expression", "Intimidation", "Persuasion", "Socialize", "Streetwise", "Subterfuge" }, 0));
					//
					//components.Add(new MeritComponent("Merits", "merit", true, new List<string>(), 1));
					//components.Add(new ExperienceComponent("Experience", "Beats", 1));
					//
					//components.Add(new HealthComponent("Health", 2));
					//components.Add(new SimpleComponent("Willpower", 2));
					//components.Add(new StatComponent("Integrity", 2));
					//components.Add(new AspirationsComponent("Aspirations", 3, 2));
					break;
				case SheetType.Mage:
					//components.Add(new AttributesComponent("Mental_Attributes", new List<string> { "Intelligence", "Wits", "Resolve" }, 0));
					//components.Add(new AttributesComponent("Physical_Attributes", new List<string> { "Strength", "Dexterity", "Stamina" }, 1));
					//components.Add(new AttributesComponent("Social_Attributes", new List<string> { "Presence", "Manipulation", "Composure" }, 2));
					//
					//components.Add(new SkillsComponent("Mental_Skills", new List<string> { "Academics", "Computer", "Crafts", "Investigation", "Medicine", "Occult", "Politics", "Science" }, 0));
					//components.Add(new SkillsComponent("Physical_Skills", new List<string> { "Athletics", "Brawl", "Drive", "Firearms", "Larceny", "Stealth", "Survival", "Weaponry" }, 0));
					//components.Add(new SkillsComponent("Social_Skills", new List<string> { "Animal_Ken", "Empathy", "Expression", "Intimidation", "Persuasion", "Socialize", "Streetwise", "Subterfuge" }, 0));
					//
					//components.Add(new MeritComponent("Merits", "merit", true, new List<string>(), 1));
					//components.Add(new MeritComponent("Arcana", "arcanum", false, new List<string> { "Death", "Fate", "Forces", "Life", "Matter", "Mind", "Prime", "Space", "Spirit", "Time" }, 1));
					//components.Add(new ExperienceComponent("Experience", "Beats", 1));
					//components.Add(new ExperienceComponent("Arcane_Experience", "Arcane_Beats", 1));
					//
					//components.Add(new HealthComponent("Health", 2));
					//components.Add(new SimpleComponent("Willpower", 2));
					//components.Add(new StatComponent("Gnosis", 2));
					//components.Add(new SimpleComponent("Mana", 2));
					//components.Add(new StatComponent("Wisdom", 2));
					//components.Add(new AspirationsComponent("Aspirations", 3, 2));
					//components.Add(new AspirationsComponent("Obsessions", 1, 2));
					break;
				case SheetType.Werewolf:
					//components.Add(new AttributesComponent("Mental_Attributes", new List<string> { "Intelligence", "Wits", "Resolve" }, 0));
					//components.Add(new AttributesComponent("Physical_Attributes", new List<string> { "Strength", "Dexterity", "Stamina" }, 1));
					//components.Add(new AttributesComponent("Social_Attributes", new List<string> { "Presence", "Manipulation", "Composure" }, 2));
					//
					//components.Add(new SkillsComponent("Mental_Skills", new List<string> { "Academics", "Computer", "Crafts", "Investigation", "Medicine", "Occult", "Politics", "Science" }, 0));
					//components.Add(new SkillsComponent("Physical_Skills", new List<string> { "Athletics", "Brawl", "Drive", "Firearms", "Larceny", "Stealth", "Survival", "Weaponry" }, 0));
					//components.Add(new SkillsComponent("Social_Skills", new List<string> { "Animal_Ken", "Empathy", "Expression", "Intimidation", "Persuasion", "Socialize", "Streetwise", "Subterfuge" }, 0));
					//
					//components.Add(new MeritComponent("Merits", "merit", true, new List<string>(), 1));
					//components.Add(new MeritComponent("Renown", "renown", false, new List<string> { "Cunning", "Glory", "Honor", "Purity", "Wisdom" }, 1));
					//components.Add(new ExperienceComponent("Experience", "Beats", 1));
					//
					//components.Add(new HealthComponent("Health", 2));
					//components.Add(new SimpleComponent("Willpower", 2));
					//components.Add(new StatComponent("Primal_Urge", 2));
					//components.Add(new SimpleComponent("Essence", 2));
					//components.Add(new StatComponent("Harmony", 2));
					//components.Add(new AspirationsComponent("Aspirations", 3, 2));
					break;
				case SheetType.Spirit:
					//components.Add(new AttributesComponent("Attributes", new List<string> { "Power", "Finesse", "Resistance" }, 0));
					//
					//components.Add(new MeritComponent("Influences", "influence", true, new List<string>(), 1));
					//
					//components.Add(new HealthComponent("Corpus", 2));
					//components.Add(new SimpleComponent("Willpower", 2));
					//components.Add(new StatComponent("Rank", 2));
					//components.Add(new SimpleComponent("Essence", 2));
					//components.Add(new AspirationsComponent("Aspirations", 3, 2));
					break;
			}
		}

		[XmlArray]
		List<Field> fields = new List<Field>();

		[XmlIgnore]
		public List<Field> Fields
		{
			get
			{
				return fields;
			}
			set
			{
				fields = value;
				//OnPropertyChanged("fields");
			}
		}

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
			}
		}
	}
}
