using CofD_Sheet_WPF.Models.Components;
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
					middleFields.Add(new Field("Vice", ""));
					middleFields.Add(new Field("Virtue", ""));
					rightFields.Add(new Field("Faction", ""));
					rightFields.Add(new Field("Group Name", ""));
					rightFields.Add(new Field("Age", ""));
					break;
				case SheetType.Mage:
					middleFields.Add(new Field("Vice", ""));
					middleFields.Add(new Field("Virtue", ""));
					rightFields.Add(new Field("Path", ""));
					rightFields.Add(new Field("Order", ""));
					rightFields.Add(new Field("Legacy", ""));
					break;
				case SheetType.Werewolf:
					middleFields.Add(new Field("Vice", ""));
					middleFields.Add(new Field("Virtue", ""));
					rightFields.Add(new Field("Age", ""));
					rightFields.Add(new Field("Faction", ""));
					rightFields.Add(new Field("Group Name", ""));
					break;
				case SheetType.WolfBlooded:
					middleFields.Add(new Field("Vice", ""));
					middleFields.Add(new Field("Virtue", ""));
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
