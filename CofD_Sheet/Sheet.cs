using CofD_Sheet.Sheet_Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CofD_Sheet
{
	enum SheetType
	{
		Mortal,
		Mage,
		Werewolf,
		Spirit
	}
	class Sheet
	{
		public string name = "";
		public string player = "";
		public string chronicle = "";
		public List<ISheetComponent> components = new List<ISheetComponent>();

		public Sheet()
		{ }

		public Sheet(SheetType type)
		{
			switch (type)
			{
				case SheetType.Mortal:
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

		public Sheet(XmlDocument doc)
		{
			XmlNode sheet = doc.FirstChild;
			name = sheet.Attributes["Name"].Value;
			player = sheet.Attributes["Player"].Value;
			chronicle = sheet.Attributes["Chronicle"].Value;

			XmlNode componentRoot = sheet.SelectSingleNode("components");
			foreach(XmlNode componentNode in componentRoot.ChildNodes)
			{
				components.Add(getSheetComponent(componentNode));
			}
		}

		private ISheetComponent getSheetComponent(XmlNode node)
		{
			switch ((ISheetComponent.Type)Enum.Parse(typeof(ISheetComponent.Type), node.Attributes["Type"].Value))
			{
				case ISheetComponent.Type.Health:
					return new HealthComponent(node);
				case ISheetComponent.Type.Simple:
					return new SimpleComponent(node);
				case ISheetComponent.Type.Stat:
					return new StatComponent(node);
				case ISheetComponent.Type.Experience:
					return new ExperienceComponent(node);
				case ISheetComponent.Type.Aspirations:
					return new AspirationsComponent(node);
				default:
					return new SimpleComponent("unknownType:" + node.Attributes["Type"].Value);
			}
		}

		public XmlDocument getXMLDoc()
		{
			XmlDocument doc = new XmlDocument();
			XmlElement sheet = (XmlElement)doc.AppendChild(doc.CreateElement("Sheet"));
			sheet.SetAttribute("Name", name);
			sheet.SetAttribute("Player", player);
			sheet.SetAttribute("Chronicle", chronicle);

			XmlElement componentRoot = doc.CreateElement("components");
			foreach ( ISheetComponent component in components)
			{
				componentRoot.AppendChild(component.getElement(doc));
			}
			sheet.AppendChild(componentRoot);
			return doc;
		}
	}
}
