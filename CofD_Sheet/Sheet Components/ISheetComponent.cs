using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace CofD_Sheet.Sheet_Components
{
	public enum ColumnId
	{
		[XmlEnum(Name = "Left")]
		Left,
		[XmlEnum(Name = "Middle")]
		Middle,
		[XmlEnum(Name = "Right")]
		Right,
		[XmlEnum(Name = "Undefined")]
		Undefined
	}
	[XmlInclude(typeof(AdvantagesComponent))]
	[XmlInclude(typeof(AspirationsComponent))]
	[XmlInclude(typeof(AttributesComponent))]
	[XmlInclude(typeof(ExperienceComponent))]
	[XmlInclude(typeof(HealthComponent))]
	[XmlInclude(typeof(MeritsComponent))]
	[XmlInclude(typeof(ResourceComponent))]
	[XmlInclude(typeof(SkillsComponent))]
	[XmlInclude(typeof(StatComponent))]
	public abstract class ISheetComponent
	{
		[XmlIgnore]
		public const int componentWidth = 315;

		[XmlIgnore]
		protected TableLayoutPanel uiElement = new TableLayoutPanel();

		[XmlAttribute]
		public string name;

		[XmlAttribute]
		public ColumnId column = ColumnId.Undefined;

		public ISheetComponent(string componentName, ColumnId _column)
		{
			name = componentName;
			column = _column;
		}

		abstract public Control GetUIElement();
		
		protected void ResizeParentColumn()
		{
			Form1.resizeComponentColumn(uiElement);
		}

		protected void OnComponentChanged()
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
