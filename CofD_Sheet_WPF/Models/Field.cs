using GalaSoft.MvvmLight;
using System;
using System.Xml.Serialization;

namespace CofD_Sheet_WPF.Models
{
	[Serializable]
	public class Field : ObservableObject
	{
		public Field()
		{}

		public Field(string inLabel, string inValue)
		{
			this.label = inLabel;
			this.value = inValue;
		}

		[XmlIgnore]
		private string _label = "<>: ";

		[XmlAttribute]
		public String label
		{
			get
			{
				return _label;
			}
			set
			{
				_label = value;
				RaisePropertyChanged("label");
			}
		}

		[XmlIgnore]
		private string _value = "";

		[XmlAttribute]
		public String value
		{
			get
			{
				return _value;
			}
			set
			{
				this._value = value;
				RaisePropertyChanged("value");
			}
		}
	}
}
