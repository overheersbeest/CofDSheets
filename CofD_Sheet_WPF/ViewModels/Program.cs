using CofD_Sheet_WPF.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Xml.Serialization;

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

		public Program()
		{
			foreach (SheetType type in Enum.GetValues(typeof(SheetType)))
			{
				newSheetButtons.Add(type);
			}

			watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
			watcher.Changed += new FileSystemEventHandler(reload);
		}
		
		public string windowTitle
		{
			get
			{
				string title = "CofD Sheet";
				List<Field> allFields = new List<Field>();
				allFields.AddRange(sheet.leftFields);
				allFields.AddRange(sheet.middleFields);
				allFields.AddRange(sheet.rightFields);

				foreach (Field field in allFields)
				{
					if (field.label == "Name"
						&& field.value.Length > 0)
					{
						title += " - " + field.value;
						break;
					}
				}

				//if (sheet.changedSinceSave)
				//{
				//	title += "*";
				//}

				return title;
			}
		}

		#region IO
		FileSystemWatcher watcher = new FileSystemWatcher();

		private string assosiatedFile = "";

		public RelayCommand onSaveSheetButtonPressed => new RelayCommand(saveSheetClicked);

		public RelayCommand onLoadSheetButtonPressed => new RelayCommand(loadSheetClicked);

		private void saveSheetClicked()
		{
			SaveFileDialog saveFileDialog1 = new SaveFileDialog
			{
				Filter = "CofD Sheet files (*.cofds)|*.cofds|XML files (*.xml)|*.xml|All files (*.*)|*.*",
				FilterIndex = 0
			};
			watcher.EnableRaisingEvents = false;

			bool? dialogResult = saveFileDialog1.ShowDialog();
			if (dialogResult.HasValue
				&& dialogResult.Value == true)
			{
				string path = saveFileDialog1.FileName;
				watcher.Path = path.Substring(0, path.LastIndexOf('\\'));
				saveSheet(path);
				//sheet.changedSinceSave = false;
			}
		}

		private void loadSheetClicked()
		{
			OpenFileDialog openFileDialog1 = new OpenFileDialog
			{
				Filter = "CofD Sheet files (*.cofds)|*.cofds|XML files (*.xml)|*.xml|All files (*.*)|*.*",
				FilterIndex = 0
			};

			//autoSaveDisabled = true;
			bool? dialogResult = openFileDialog1.ShowDialog();
			if (dialogResult.HasValue
				&& dialogResult.Value == true)
			{
				string path = openFileDialog1.FileName;
				watcher.Path = path.Substring(0, path.LastIndexOf('\\'));
				loadSheet(path);
				//sheet.changedSinceSave = false;
			}

			//autoSaveDisabled = false;
		}

		public void saveSheet(string path)
		{
			try
			{
				XmlSerializer serializer = new XmlSerializer(typeof(Sheet));
				TextWriter writer = new StreamWriter(path);
				serializer.Serialize(writer, sheet);
				writer.Close();
				assosiatedFile = path;
			}
			catch (Exception e)
			{
				string ExceptionTrace = "";
				Exception Inner = e.InnerException;
				while (Inner != null)
				{
					ExceptionTrace += "\r\n" + Inner.Message;
					Inner = Inner.InnerException;
				}
				MessageBox.Show("Error: Could not save file to disk. " + e.Message + ExceptionTrace);
			}
		}

		public bool loadSheet(string path)
		{
			try
			{
				XmlSerializer serializer = new XmlSerializer(typeof(Sheet));
				StreamReader reader = new StreamReader(path);
				sheet = (Sheet)serializer.Deserialize(reader);
				reader.Close();
				assosiatedFile = path;
				return true;
			}
			catch (Exception e)
			{
				string ExceptionTrace = "";
				Exception Inner = e.InnerException;
				while (Inner != null)
				{
					ExceptionTrace += "\r\n" + Inner.Message;
					Inner = Inner.InnerException;
				}
				MessageBox.Show("Error: Could not load file from disk. " + e.Message + ExceptionTrace);
				return false;
			}
		}

		public void resave()
		{
			if (assosiatedFile.Length > 0)
			{
				watcher.EnableRaisingEvents = false;
				saveSheet(assosiatedFile);
				//sheet.changedSinceSave = false;
			}
		}

		public void reload(object sender, FileSystemEventArgs e)
		{
			if (e.FullPath == assosiatedFile)
			{
				try
				{
					bool fileRead = false;
					while (!fileRead)
					{
						fileRead = loadSheet(assosiatedFile);
					}
				}
				catch (Exception ex)
				{
					string ExceptionTrace = "";
					Exception Inner = ex.InnerException;
					while (Inner != null)
					{
						ExceptionTrace += "\r\n" + Inner.Message;
						Inner = Inner.InnerException;
					}
					MessageBox.Show("Error: Could not reload file from disk. Original error: " + ex.Message + ExceptionTrace);
				}
			}
		}
		#endregion

		#region NewSheetButtons
		public ObservableCollection<SheetType> newSheetButtons { get; set; } = new ObservableCollection<SheetType>();

		public RelayCommand<SheetType> onNewSheetButtonPressed => new RelayCommand<SheetType>(createNewSheet);

		private void createNewSheet(SheetType type)
		{
			sheet = new Sheet(type);
		}
		#endregion
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
