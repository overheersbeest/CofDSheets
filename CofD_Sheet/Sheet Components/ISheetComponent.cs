using CofD_Sheet.Modifications;
using System.Collections.Generic;
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
	[XmlInclude(typeof(ExperienceComponent))]
	[XmlInclude(typeof(HealthComponent))]
	[XmlInclude(typeof(ModificationSetComponent))]
	[XmlInclude(typeof(ResourceComponent))]
	[XmlInclude(typeof(TraitsComponent))]
	public abstract class ISheetComponent
	{
		[XmlIgnore]
		public const int rowHeight = 23;

		[XmlIgnore]
		public const int inputBoxHeight = 35;

		[XmlIgnore]
		public const int filledInputBoxHeight = 26;

		[XmlIgnore]
		public const int componentWidth = 303;

		[XmlIgnore]
		protected TableLayoutPanel uiElement = new TableLayoutPanel();

		[XmlIgnore]
		protected bool isCurrentlyModified = false;

		[XmlIgnore]
		protected bool wasPreviouslyModified = false;

		[XmlIgnore]
		protected bool isCurrentlyIncludedInModFormula = false;

		[XmlAttribute]
		public string name;

		[XmlAttribute]
		public ColumnId column = ColumnId.Undefined;

		public ISheetComponent(string componentName, ColumnId _column)
		{
			name = componentName;
			column = _column;
		}

		virtual public void Init()
		{
			//do nothing
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

			if (isCurrentlyIncludedInModFormula)
			{
				Form1.instance.sheet.RefreshModifications();
			}
		}

		virtual public int QueryInt(List<string> path)
		{
			throw new System.NotImplementedException();
		}

		virtual public void ApplyModification(Modification mod, Sheet sheet)
		{
			throw new System.NotImplementedException();
		}

		virtual public void ResetModifications()
		{
			wasPreviouslyModified = isCurrentlyModified;
			isCurrentlyModified = false;
			isCurrentlyIncludedInModFormula = false;
		}

		virtual public void OnModificationsComplete()
		{
			wasPreviouslyModified = false;
		}
	}
}
