using CofD_Sheet_WPF.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;

namespace CofD_Sheet_WPF.Models
{
	[Serializable]
	public class Field : INotifyPropertyChanged
	{
		public Field()
		{
			valueChangedEvent = new FieldValueChangedCommand(this);
		}

		public Field(string inLabel, string inValue)
		{
			this.label = inLabel;
			this.value = inValue;
			valueChangedEvent = new FieldValueChangedCommand(this);
		}

		public bool CanChangeValue
		{
			get;
			set;
		}

		[XmlAttribute]
		private string label = "<>: ";

		[XmlAttribute]
		private string value = "";

		[XmlIgnore]
		public String Label
		{
			get
			{
				return label;
			}
			set
			{
				label = value;
				OnPropertyChanged("label");
			}
		}

		[XmlIgnore]
		public String Value
		{
			get
			{
				return value;
			}
			set
			{
				this.value = value;
				OnPropertyChanged("value");
			}
		}

		public ICommand valueChangedEvent
		{
			get;
			private set;
		}

		public void SaveChanges()
		{
			Debug.Assert(false, "field is in");
		}

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler handler = PropertyChanged;

			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		#endregion
	}
}
