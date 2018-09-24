using System.Xml.Serialization;

namespace CofD_Sheet_WPF.Models.Components
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
	[XmlInclude(typeof(Advantages))]
	public class IComponent
	{
		[XmlIgnore]
		public const int componentWidth = 315;

		[XmlAttribute]
		public string name;

		[XmlAttribute]
		public ColumnId column = ColumnId.Undefined;

		public IComponent(string componentName, ColumnId _column)
		{
			name = componentName;
			column = _column;
		}

		protected void onComponentChanged()
		{
			if (Program.instance.autoSave)
			{
				if (Program.instance.assosiatedFile.Length != 0)
				{
					Program.instance.saveAgain();
				}
			}
			else
			{
				Program.instance.sheet.changedSinceSave = true;
			}
		}
	}
}
