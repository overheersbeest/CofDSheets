using GalaSoft.MvvmLight;
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
	public class BaseComponent : ObservableObject
	{
		[XmlAttribute]
		public string _name = "BaseComponent";

		[XmlIgnore]
		public string name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
				RaisePropertyChanged("name");
			}
		}

		[XmlAttribute]
		public ColumnId _column = ColumnId.Undefined;

		[XmlIgnore]
		public ColumnId column
		{
			get
			{
				return _column;
			}
			set
			{
				_column = value;
				RaisePropertyChanged("column");
			}
		}

		public BaseComponent(string componentName, ColumnId _column)
		{
			name = componentName;
			column = _column;
		}
	}
}
