﻿using System.Windows.Forms;
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
		[XmlAttribute]
		public string name;
		
		public ISheetComponent(string componentName)
		{
			name = componentName;
		}

		abstract public Control getUIElement();
		
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
