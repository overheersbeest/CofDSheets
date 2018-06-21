using CofD_Sheet.Sheet_Components;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace CofD_Sheet
{
	public partial class Form1 : Form
	{
		public static Form1 instance = null;

		public Sheet sheet = new Sheet();

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
					string name  = assosiatedFile.Substring(slashIndex + 1, assosiatedFile.Length - slashIndex - 1);
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

		public Form1()
		{
			instance = this;
			InitializeComponent();

			foreach (SheetType type in Enum.GetValues(typeof(SheetType)))
			{
				System.Windows.Forms.ToolStripMenuItem newButton = new ToolStripMenuItem();
				newButton.Name = "New" + type.ToString() + "Button";
				newButton.Size = new System.Drawing.Size(152, 22);
				newButton.Text = type.ToString();
				newButton.Click += new System.EventHandler(this.NewSheetButtonClicked);
				this.newToolStripMenuItem.DropDownItems.Add(newButton);
			}
			
			watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
			watcher.Changed += new FileSystemEventHandler(loadAgain);
			autoSaveToolStripMenuItem.Checked = autoSave;
			autoLoadToolStripMenuItem.Checked = autoLoad;
		}

		private void loadToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog1 = new OpenFileDialog();
			openFileDialog1.Filter = "CofD Sheet files (*.cofds)|*.cofds|XML files (*.xml)|*.xml|All files (*.*)|*.*";
			openFileDialog1.FilterIndex = 0;
			
			autoSaveDisabled = true;
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				string path = openFileDialog1.FileName;
				watcher.Path = path.Substring(0, path.LastIndexOf('\\'));
				loadSheet(path);
				sheet.changedSinceSave = false;
			}

			autoSaveDisabled = false;
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveFileDialog saveFileDialog1 = new SaveFileDialog();
			saveFileDialog1.Filter = "CofD Sheet files (*.cofds)|*.cofds|XML files (*.xml)|*.xml|All files (*.*)|*.*";
			saveFileDialog1.FilterIndex = 0;
			watcher.EnableRaisingEvents = false;

			if (saveFileDialog1.ShowDialog() == DialogResult.OK)
			{
				string path = saveFileDialog1.FileName;
				watcher.Path = path.Substring(0, path.LastIndexOf('\\'));
				saveSheet(path);
				sheet.changedSinceSave = false;
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
				refreshSheet();
				return true;
			}
			catch (Exception e)
			{
				MessageBox.Show("Error: Could not load file from disk. " + e.Message);
				return false;
			}
		}
		
		public delegate void refreshSheetCallback();

		public void refreshSheet()
		{
			if (PlayerTextBox.InvokeRequired)
			{
				refreshSheetCallback d = new refreshSheetCallback(refreshSheet);
				this.Invoke(d, new object[] { });
				return;
			}

			autoSaveDisabled = true;

			NameTextBox.Text = sheet.name;
			PlayerTextBox.Text = sheet.player;
			ChronicleTextBox.Text = sheet.chronicle;

			LeftComponentTable.Controls.Clear();
			LeftComponentTable.RowStyles.Clear();
			MiddleComponentTable.Controls.Clear();
			MiddleComponentTable.RowStyles.Clear();
			RightComponentTable.Controls.Clear();
			RightComponentTable.RowStyles.Clear();

			int amountOfRows = Convert.ToInt32(Math.Ceiling(sheet.components.Count / 2.0F));

			for (int i = 0; i < sheet.components.Count; i++)
			{
				ISheetComponent component = sheet.components[i];

				TableLayoutPanel componentUIElement = new TableLayoutPanel();
				componentUIElement.ColumnCount = 1;
				componentUIElement.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
				componentUIElement.Dock = DockStyle.Fill;
				componentUIElement.Name = "tableLayout" + component.name;
				componentUIElement.RowCount = 2;
				componentUIElement.RowStyles.Add(new RowStyle(SizeType.AutoSize));
				componentUIElement.RowStyles.Add(new RowStyle(SizeType.AutoSize));
				componentUIElement.TabIndex = 0;
				componentUIElement.Size = new Size(292, 26);

				Label nameLabel = new Label();
				nameLabel.Anchor = AnchorStyles.None;
				nameLabel.AutoSize = true;
				nameLabel.Name = "nameLabel" + component.name;
				nameLabel.Size = new Size(35, 13);
				nameLabel.TabIndex = 0;
				nameLabel.Text = component.name;

				int componentRequiredHeight = 0;
				componentUIElement.Controls.Add(nameLabel, 0, 0);
				componentRequiredHeight += nameLabel.Size.Height;
				Control componentValueElement = component.getUIElement();
				componentUIElement.Controls.Add(componentValueElement, 0, 1);
				componentRequiredHeight += componentValueElement.Size.Height;
				componentRequiredHeight += 10;
				componentUIElement.Size = new Size(292, componentRequiredHeight);

				if (component.columnIndex == 0)
				{
					LeftComponentTable.RowStyles.Add(new RowStyle(SizeType.Absolute, componentRequiredHeight));
					LeftComponentTable.Controls.Add(componentUIElement);
				}
				else if (component.columnIndex == 1)
				{
					MiddleComponentTable.RowStyles.Add(new RowStyle(SizeType.Absolute, componentRequiredHeight));
					MiddleComponentTable.Controls.Add(componentUIElement);
				}
				else
				{
					RightComponentTable.RowStyles.Add(new RowStyle(SizeType.Absolute, componentRequiredHeight));
					RightComponentTable.Controls.Add(componentUIElement);
				}
			}

			LeftComponentTable.RowCount = amountOfRows;
			MiddleComponentTable.RowCount = amountOfRows;
			RightComponentTable.RowCount = amountOfRows;
			
			autoSaveDisabled = false;
		}

		private void NameChanged(object sender, EventArgs e)
		{
			sheet.name = NameTextBox.Text;
			RefreshFormTitle();
		}

		public void RefreshFormTitle()
		{
			if (sheet.name.Length == 0)
			{
				this.Text = "CofD Sheet";
			}
			else
			{
				this.Text = "CofD Sheet - " + sheet.name;
			}

			if (sheet.changedSinceSave)
			{
				this.Text += "*";
			}
		}

		private void PlayerChanged(object sender, EventArgs e)
		{
			sheet.player = PlayerTextBox.Text;
		}

		private void ChronicleChanged(object sender, EventArgs e)
		{
			sheet.chronicle = ChronicleTextBox.Text;
		}

		private void NewSheetButtonClicked(object sender, EventArgs e)
		{
			assosiatedFile = "";
			sheet = new Sheet((SheetType)Enum.Parse(typeof(SheetType), sender.ToString()));
			refreshSheet();
		}

		private void autoSaveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			autoSaveToolStripMenuItem.Checked = !autoSaveToolStripMenuItem.Checked;
			autoSave = autoSaveToolStripMenuItem.Checked;
		}

		private void autoLoadToolStripMenuItem_Click(object sender, EventArgs e)
		{
			autoLoadToolStripMenuItem.Checked = !autoLoadToolStripMenuItem.Checked;
			autoLoad = autoLoadToolStripMenuItem.Checked;
			if (watcher.Path.Length > 0)
			{
				watcher.EnableRaisingEvents = autoLoad;
			}
		}
	}
}
