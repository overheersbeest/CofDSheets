using GalaSoft.MvvmLight;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Xml.Serialization;

namespace CofD_Sheet_WPF.Models
{
	class Program : ViewModelBase
	{
		public static Program instance = null;

		public Sheet sheet = new Sheet();

		public ObservableCollection<SheetType> newSheetButtons { get; set; } = new ObservableCollection<SheetType>();

		public Program()
		{
			instance = this;

			foreach (SheetType type in Enum.GetValues(typeof(SheetType)))
			{
				newSheetButtons.Add(type);
			}
			
			watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
			watcher.Changed += new FileSystemEventHandler(loadAgain);
			//autoSaveToolStripMenuItem.Checked = autoSave;
			//autoLoadToolStripMenuItem.Checked = autoLoad;
		}

		private void loadToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog1 = new OpenFileDialog();
			openFileDialog1.Filter = "CofD Sheet files (*.cofds)|*.cofds|XML files (*.xml)|*.xml|All files (*.*)|*.*";
			openFileDialog1.FilterIndex = 0;

			autoSaveDisabled = true;
			//if (openFileDialog1.ShowDialog() == DialogResult.OK)
			//{
			//	string path = openFileDialog1.FileName;
			//	watcher.Path = path.Substring(0, path.LastIndexOf('\\'));
			//	loadSheet(path);
			//	sheet.changedSinceSave = false;
			//}

			autoSaveDisabled = false;
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveFileDialog saveFileDialog1 = new SaveFileDialog();
			saveFileDialog1.Filter = "CofD Sheet files (*.cofds)|*.cofds|XML files (*.xml)|*.xml|All files (*.*)|*.*";
			saveFileDialog1.FilterIndex = 0;
			watcher.EnableRaisingEvents = false;

			//if (saveFileDialog1.ShowDialog() == DialogResult.OK)
			//{
			//	string path = saveFileDialog1.FileName;
			//	watcher.Path = path.Substring(0, path.LastIndexOf('\\'));
			//	saveSheet(path);
			//	sheet.changedSinceSave = false;
			//}
		}
		
		public string windowTitle
		{
			get
			{
				string title = "CofD Sheet";
				foreach (Field field in sheet.Fields)
				{
					if (field.Label == "Name")
					{
						title += " - " + field.Value;
					}
				}
				
				if (sheet.changedSinceSave)
				{
					title += "*";
				}

				return title;
			}
		}

		#region io
		public bool saveSheet(string path)
		{
			try
			{
				XmlSerializer serializer = new XmlSerializer(typeof(Sheet));
				TextWriter writer = new StreamWriter(path);
				serializer.Serialize(writer, sheet);
				writer.Close();
				assosiatedFile = path;
				return true;
			}
			catch (Exception e)
			{
				MessageBox.Show("Error: Could not save file to disk. " + e.Message);
				return false;
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
				MessageBox.Show("Error: Could not load file from disk. " + e.Message);
				return false;
			}
		}

		public void saveAgain()
		{
			if (assosiatedFile.Length > 0)
			{
				watcher.EnableRaisingEvents = false;
				saveSheet(assosiatedFile);
				sheet.changedSinceSave = false;
			}
		}

		public void loadAgain(object sender, FileSystemEventArgs e)
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
					MessageBox.Show("Error: Could not reload file from disk. Original error: " + ex.Message);
				}
			}
		}

		private bool _autoSave = true;
		public bool autoSave
		{
			get { return _autoSave; }
			set
			{
				_autoSave = value;
				if (sheet.changedSinceSave)
				{
					saveAgain();
				}
			}
		}
		public bool autoSaveDisabled = false;

		private bool _autoLoad = false;
		public bool autoLoad
		{
			get { return _autoLoad; }
			set
			{
				_autoLoad = value;
				if (assosiatedFile.Length != 0)
				{
					watcher.EnableRaisingEvents = value;
					int slashIndex = assosiatedFile.LastIndexOf('\\');
					string dir = assosiatedFile.Substring(0, slashIndex);
					string name = assosiatedFile.Substring(slashIndex + 1, assosiatedFile.Length - slashIndex - 1);
					loadAgain(this, new FileSystemEventArgs(WatcherChangeTypes.Changed, dir, name));
				}
			}
		}

		private string _assosiatedFile = "";
		public string assosiatedFile
		{
			get { return _assosiatedFile; }
			set
			{
				_assosiatedFile = value;
				if (value.Length > 0)
				{
					watcher.EnableRaisingEvents = autoLoad;
				}
			}
		}

		FileSystemWatcher watcher = new FileSystemWatcher();
		#endregion

		private void NewSheetButtonClicked(object sender, EventArgs e)
		{
			assosiatedFile = "";
			sheet = new Sheet((SheetType)Enum.Parse(typeof(SheetType), sender.ToString()));
		}

		//private void autoSaveToolStripMenuItem_Click(object sender, EventArgs e)
		//{
		//	autoSaveToolStripMenuItem.Checked = !autoSaveToolStripMenuItem.Checked;
		//	autoSave = autoSaveToolStripMenuItem.Checked;
		//}
		//
		//private void autoLoadToolStripMenuItem_Click(object sender, EventArgs e)
		//{
		//	autoLoadToolStripMenuItem.Checked = !autoLoadToolStripMenuItem.Checked;
		//	autoLoad = autoLoadToolStripMenuItem.Checked;
		//	if (watcher.Path.Length > 0)
		//	{
		//		watcher.EnableRaisingEvents = autoLoad;
		//	}
		//}
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
