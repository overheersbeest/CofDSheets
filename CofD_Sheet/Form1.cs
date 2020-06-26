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
		public bool AutoSave
		{
			get { return _autoSave; }
			set
			{
				_autoSave = value;
				if (sheet.ChangedSinceSave)
				{
					SaveAgain();
				}
			}
		}
		public bool autoSaveDisabled = false;

		private bool _autoLoad = false;
		public bool AutoLoad
		{
			get { return _autoLoad; }
			set
			{
				_autoLoad = value;
				if (AssosiatedFile.Length != 0)
				{
					watcher.EnableRaisingEvents = value;
					int slashIndex = AssosiatedFile.LastIndexOf('\\');
					string dir = AssosiatedFile.Substring(0, slashIndex);
					string name  = AssosiatedFile.Substring(slashIndex + 1, AssosiatedFile.Length - slashIndex - 1);
					LoadAgain(this, new FileSystemEventArgs(WatcherChangeTypes.Changed, dir, name));
				}
			}
		}

		private string _assosiatedFile = "";
		public string AssosiatedFile
		{
			get { return _assosiatedFile; }
			set
			{
				_assosiatedFile = value;
				if (value.Length > 0)
				{
					watcher.EnableRaisingEvents = AutoLoad;
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
				System.Windows.Forms.ToolStripMenuItem newButton = new ToolStripMenuItem
				{
					Name = "New" + type.ToString() + "Button",
					Size = new System.Drawing.Size(152, 22),
					Text = type.ToString()
				};
				newButton.Click += new System.EventHandler(this.NewSheetButtonClicked);
				this.newToolStripMenuItem.DropDownItems.Add(newButton);
			}
			
			watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
			watcher.Changed += new FileSystemEventHandler(LoadAgain);
			autoSaveToolStripMenuItem.Checked = AutoSave;
			autoLoadToolStripMenuItem.Checked = AutoLoad;
		}

		private void LoadToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog1 = new OpenFileDialog
			{
				Filter = "CofD Sheet files (*.cofds)|*.cofds|XML files (*.xml)|*.xml|All files (*.*)|*.*",
				FilterIndex = 0
			};

			autoSaveDisabled = true;
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				string path = openFileDialog1.FileName;
				watcher.Path = path.Substring(0, path.LastIndexOf('\\'));
				LoadSheet(path);
				sheet.ChangedSinceSave = false;
			}

			autoSaveDisabled = false;
		}

		private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveFileDialog saveFileDialog1 = new SaveFileDialog
			{
				Filter = "CofD Sheet files (*.cofds)|*.cofds|XML files (*.xml)|*.xml|All files (*.*)|*.*",
				FilterIndex = 0
			};
			watcher.EnableRaisingEvents = false;

			if (saveFileDialog1.ShowDialog() == DialogResult.OK)
			{
				string path = saveFileDialog1.FileName;
				watcher.Path = path.Substring(0, path.LastIndexOf('\\'));
				SaveSheet(path);
				sheet.ChangedSinceSave = false;
			}
		}

		public void SaveAgain()
		{
			if (AssosiatedFile.Length > 0)
			{
				watcher.EnableRaisingEvents = false;
				SaveSheet(AssosiatedFile);
				sheet.ChangedSinceSave = false;
			}
		}

		public void LoadAgain(object sender, FileSystemEventArgs e)
		{
			if (e.FullPath == AssosiatedFile)
			{
				try
				{
					bool fileRead = false;
					while (!fileRead)
					{
						fileRead = LoadSheet(AssosiatedFile);
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: Could not reload file from disk. Original error: " + ex.Message);
				}
			}
		}

		public bool SaveSheet(string path)
		{
			try
			{
				XmlSerializer serializer = new XmlSerializer(typeof(Sheet));
				TextWriter writer = new StreamWriter(path);
				serializer.Serialize(writer, sheet);
				writer.Close();
				AssosiatedFile = path;
				return true;
			}
			catch (Exception e)
			{
				MessageBox.Show("Error: Could not save file to disk. " + e.Message);
				return false;
			}
		}

		public bool LoadSheet(string path)
		{
			try
			{
				XmlSerializer serializer = new XmlSerializer(typeof(Sheet));
				StreamReader reader = new StreamReader(path);
				sheet = (Sheet)serializer.Deserialize(reader);
				reader.Close();
				AssosiatedFile = path;
				RefreshSheet();
				return true;
			}
			catch (Exception e)
			{
				MessageBox.Show("Error: Could not load file from disk. " + e.Message);
				return false;
			}
		}
		
		public delegate void refreshSheetCallback();

		public void RefreshSheet()
		{
			if (PlayerTextBox.InvokeRequired)
			{
				refreshSheetCallback d = new refreshSheetCallback(RefreshSheet);
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

				TableLayoutPanel componentUIElement = new TableLayoutPanel
				{
					ColumnCount = 1
				};
				componentUIElement.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
				componentUIElement.Dock = DockStyle.Fill;
				componentUIElement.RowCount = 2;
				componentUIElement.RowStyles.Add(new RowStyle(SizeType.AutoSize));
				componentUIElement.RowStyles.Add(new RowStyle(SizeType.AutoSize));
				componentUIElement.TabIndex = 0;
				componentUIElement.Size = new Size(292, 26);

				Label nameLabel = new Label
				{
					Anchor = AnchorStyles.None,
					AutoSize = true,
					Size = new Size(35, 13),
					TabIndex = 0
				};
				nameLabel.Font = new Font(nameLabel.Font, FontStyle.Bold);
				nameLabel.Text = component.name.Replace('_', ' ');

				int componentRequiredHeight = 0;
				componentUIElement.Controls.Add(nameLabel, 0, 0);
				componentRequiredHeight += nameLabel.Size.Height;
				Control componentValueElement = component.GetUIElement();
				componentUIElement.Controls.Add(componentValueElement, 0, 1);
				componentRequiredHeight += componentValueElement.Size.Height;
				componentRequiredHeight += 10;
				componentUIElement.Size = new Size(292, componentRequiredHeight);

				if (component.column == ColumnId.Left)
				{
					LeftComponentTable.RowStyles.Add(new RowStyle(SizeType.AutoSize, componentRequiredHeight));
					LeftComponentTable.Controls.Add(componentUIElement);
				}
				else if (component.column == ColumnId.Middle)
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

			if (sheet.ChangedSinceSave)
			{
				this.Text += "*";
			}
		}

		public static void ResizeComponentColumn(Control component)
		{
			TableLayoutPanel cell = component.Parent as TableLayoutPanel;
			if (cell != null)
			{
				cell.Size = new Size(cell.Size.Width, component.Size.Height);
				Form1.ResizeTableHeight(ref cell);
				TableLayoutPanel column = cell.Parent as TableLayoutPanel;
				Form1.ResizeTableHeight(ref column);
			}
		}

		private static void ResizeTableHeight(ref TableLayoutPanel table)
		{
			int height = 0;
			table.RowStyles.Clear();
			for (int r = 0; r < table.RowCount; ++r)
			{
				int cellHeight = 30;
				for (int c = 0; c < table.ColumnCount; ++c)
				{
					Control cell = table.GetControlFromPosition(c, r);
					if (cell != null)
					{
						cellHeight = Math.Max(cell.Size.Height, cellHeight);
					}
				}
				height += cellHeight;
				table.RowStyles.Add(new RowStyle(SizeType.Absolute, cellHeight));
			}
			table.Size = new Size(table.Size.Width, height);
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
			AssosiatedFile = "";
			sheet = new Sheet((SheetType)Enum.Parse(typeof(SheetType), sender.ToString()));
			RefreshSheet();
		}

		private void AutoSaveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			autoSaveToolStripMenuItem.Checked = !autoSaveToolStripMenuItem.Checked;
			AutoSave = autoSaveToolStripMenuItem.Checked;
		}

		private void AutoLoadToolStripMenuItem_Click(object sender, EventArgs e)
		{
			autoLoadToolStripMenuItem.Checked = !autoLoadToolStripMenuItem.Checked;
			AutoLoad = autoLoadToolStripMenuItem.Checked;
			if (watcher.Path.Length > 0)
			{
				watcher.EnableRaisingEvents = AutoLoad;
			}
		}
    }
}
