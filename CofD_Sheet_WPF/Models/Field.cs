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
			this._label = inLabel;
			this._value = inValue;
		}

		[XmlAttribute]
		private string _label = "<>: ";

		[XmlIgnore]
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

		[XmlAttribute]
		private string _value = "";

		[XmlIgnore]
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
