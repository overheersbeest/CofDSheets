using GalaSoft.MvvmLight;
using System.Xml.Serialization;

namespace CofD_Sheet_WPF.Models.Components
{
	[XmlInclude(typeof(Advantages))]
	public class BaseComponent : ObservableObject
	{
		[XmlIgnore]
		private string _name = "BaseComponentName";

		[XmlAttribute]
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
		
		public BaseComponent()
		{ }

		public BaseComponent(string componentName)
		{
			name = componentName;
		}
	}
}
