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
	[XmlInclude(typeof(ModificationSetComponent))]
	[XmlInclude(typeof(ResourceComponent))]
	[XmlInclude(typeof(SkillsComponent))]
	[XmlInclude(typeof(StatComponent))]
	public abstract class ISheetComponent
	{
		[XmlIgnore]
		public const int rowHeight = 23;

		[XmlIgnore]
		public const int inputBoxHeight = 35;

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

		abstract public Control ConstructUIElement();

		public void ResizeParentColumn()
		{
			Form1.ResizeComponentColumn(uiElement);
		}

		protected void OnComponentChanged()
		{
			if (Form1.instance.AutoSave)
			{
				if (Form1.instance.AssosiatedFile.Length != 0)
				{
					Form1.instance.SaveAgain();
				}
			}
			else
			{
				Form1.instance.sheet.ChangedSinceSave = true;
			}
		}

		virtual public void ApplyModification(ModificationSetComponent.Modification mod)
		{
			throw new System.NotImplementedException();
		}

		virtual public void ResetModifications()
		{
			//do nothing
		}

		virtual public void OnModificationsComplete()
		{
			//do nothing
		}
	}
}
