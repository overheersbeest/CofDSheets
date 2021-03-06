﻿using CofD_Sheet_WPF.Models.Components;
using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace CofD_Sheet_WPF.Models
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
	public class Sheet : ObservableObject
	{
		public Sheet()
		{ }

		public Sheet(SheetType type)
		{
			leftFields.Add(new Field("Name", ""));
			leftFields.Add(new Field("Player", ""));
			leftFields.Add(new Field("Chronicle", ""));
			switch (type)
			{
				case SheetType.Mortal:
					middleFields.Add(new Field("Virtue", ""));
					middleFields.Add(new Field("Vice", ""));
					rightFields.Add(new Field("Faction", ""));
					rightFields.Add(new Field("Group Name", ""));
					rightFields.Add(new Field("Age", ""));

					leftComponents.Add(new Attributes("Mental Attributes", new List<string> { "Intelligence", "Wits", "Resolve" }));
					leftComponents.Add(new Skills("Mental Skills", new List<string> { "Academics", "Computer", "Crafts", "Investigation", "Medicine", "Occult", "Politics", "Science" }));
					break;
				case SheetType.Mage:
					middleFields.Add(new Field("Virtue", ""));
					middleFields.Add(new Field("Vice", ""));
					rightFields.Add(new Field("Path", ""));
					rightFields.Add(new Field("Order", ""));
					rightFields.Add(new Field("Legacy", ""));
					break;
				case SheetType.Werewolf:
					middleFields.Add(new Field("Blood", ""));
					middleFields.Add(new Field("Bone", ""));
					rightFields.Add(new Field("Auspice", ""));
					rightFields.Add(new Field("Tribe", ""));
					rightFields.Add(new Field("Lodge", ""));
					break;
				case SheetType.Vampire:
					middleFields.Add(new Field("Mask", ""));
					middleFields.Add(new Field("Dirge", ""));
					rightFields.Add(new Field("Clan", ""));
					rightFields.Add(new Field("Bloodline", ""));
					rightFields.Add(new Field("Covenant", ""));
					break;
				case SheetType.WolfBlooded:
					middleFields.Add(new Field("Virtue", ""));
					middleFields.Add(new Field("Vice", ""));
					rightFields.Add(new Field("Tribe", ""));
					rightFields.Add(new Field("Pack", ""));
					rightFields.Add(new Field("Age", ""));
					break;
				case SheetType.Spirit:
					middleFields.Add(new Field("Ban", ""));
					middleFields.Add(new Field("Bane", ""));
					rightFields.Add(new Field("Court", ""));
					rightFields.Add(new Field("Brood", ""));
					rightFields.Add(new Field("Rank", ""));
					break;
			}
			middleFields.Add(new Field("Concept", ""));
		}

		[XmlArray]
		public ObservableCollection<Field> leftFields { get; set; } = new ObservableCollection<Field>();

		[XmlArray]
		public ObservableCollection<Field> middleFields { get; set; } = new ObservableCollection<Field>();
		
		[XmlArray]
		public ObservableCollection<Field> rightFields { get; set; } = new ObservableCollection<Field>();


		[XmlArray]
		public ObservableCollection<BaseComponent> leftComponents { get; set; } = new ObservableCollection<BaseComponent>();

		[XmlArray]
		public ObservableCollection<BaseComponent> middleComponents { get; set; } = new ObservableCollection<BaseComponent>();

		[XmlArray]
		public ObservableCollection<BaseComponent> rightComponents { get; set; } = new ObservableCollection<BaseComponent>();
	}
}
