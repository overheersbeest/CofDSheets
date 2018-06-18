using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace CofD_Sheet.Sheet_Components
{
	[XmlInclude(typeof(AspirationsComponent))]
	[XmlInclude(typeof(AttributesComponent))]
	[XmlInclude(typeof(ExperienceComponent))]
	[XmlInclude(typeof(HealthComponent))]
	[XmlInclude(typeof(SimpleComponent))]
	[XmlInclude(typeof(SkillsComponent))]
	[XmlInclude(typeof(StatComponent))]
	public abstract class ISheetComponent
	{
		public enum Type
		{
			Simple,
			Stat,
			Health,
			Experience,
			Aspirations,
			Attributes,
			Skills
		}

		[XmlAttribute]
		public string name;

		[XmlAttribute]
		public ISheetComponent.Type type;

		public ISheetComponent(string componentName)
		{
			name = componentName;
		}

		public XmlElement getElement(XmlDocument doc)
		{
			XmlElement element = doc.CreateElement(name);
			element.SetAttribute("Type", type.ToString());
			fillElement(ref element, doc);
			return element;
		}

		abstract public Control getUIElement();

		abstract protected void fillElement(ref XmlElement node, XmlDocument doc);

		protected void onComponentChanged()
		{
			if (Form1.instance.autoSave)
			{
				if (Form1.instance.assosiatedFile.Length != 0)
				{
					Form1.instance.saveAgain();
				}
			}
			else
			{
				Form1.instance.sheet.changedSinceSave = true;
			}
		}
	}
}
