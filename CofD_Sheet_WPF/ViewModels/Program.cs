using CofD_Sheet_WPF.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Markup;

namespace CofD_Sheet_WPF.ViewModels
{
	class Program : ViewModelBase
	{
		private Sheet _sheet = new Sheet();

		public Sheet sheet
		{
			get
			{
				return _sheet;
			}
			set
			{
				_sheet = value;
				RaisePropertyChanged("sheet");
			}
		}

		public ObservableCollection<SheetType> newSheetButtons { get; set; } = new ObservableCollection<SheetType>();

		public Program()
		{
			foreach (SheetType type in Enum.GetValues(typeof(SheetType)))
			{
				newSheetButtons.Add(type);
			}
		}
		
		public string windowTitle
		{
			get
			{
				string title = "CofD Sheet";
				bool foundName = false;
				foreach (Field field in sheet.leftFields)
				{
					if (field.label == "Name")
					{
						title += " - " + field.value;
						foundName = true;
						break;
					}
				}
				if (!foundName)
				{
					foreach (Field field in sheet.middleFields)
					{
						if (field.label == "Name")
						{
							title += " - " + field.value;
							foundName = true;
							break;
						}
					}
				}
				if (!foundName)
				{
					foreach (Field field in sheet.rightFields)
					{
						if (field.label == "Name")
						{
							title += " - " + field.value;
							foundName = true;
							break;
						}
					}
				}

				//if (sheet.changedSinceSave)
				//{
				//	title += "*";
				//}

				return title;
			}
		}
		
		private void createNewSheet(SheetType type)
		{
			sheet = new Sheet(type);
		}

		public RelayCommand<SheetType> onNewSheetButtonPressed => new RelayCommand<SheetType>(createNewSheet);
	}



	[ValueConversion(typeof(SheetType), typeof(string))]
	public class SheetTypeStringConverter : MarkupExtension, IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return value.ToString();
		}
		
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			foreach (SheetType type in Enum.GetValues(typeof(SheetType)))
			{
				if (type.ToString().Equals(value))
				{
					return type;
				}
			}
			return null;
		}

		private static SheetTypeStringConverter _converter;
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			if (_converter == null) _converter = new SheetTypeStringConverter();
			return _converter;
		}
	}
}
