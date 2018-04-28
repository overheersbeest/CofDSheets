using CofD_Sheet.Sheet_Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

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
				try
				{
					Stream myStream = openFileDialog1.OpenFile();
					if (myStream != null)
					{
						using (myStream)
						{
							string path = openFileDialog1.FileName;
							watcher.Path = path.Substring(0, path.LastIndexOf('\\'));
							assosiatedFile = path;
							XmlDocument doc = new XmlDocument();
							doc.Load(myStream);
							sheet = new Sheet(doc);
							sheet.changedSinceSave = false;
						}
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
				}
			}

			autoSaveDisabled = false;
			refreshSheet();
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveFileDialog saveFileDialog1 = new SaveFileDialog();
			saveFileDialog1.Filter = "CofD Sheet files (*.cofds)|*.cofds|XML files (*.xml)|*.xml|All files (*.*)|*.*";
			saveFileDialog1.FilterIndex = 0;
			watcher.EnableRaisingEvents = false;

			if (saveFileDialog1.ShowDialog() == DialogResult.OK)
			{
				try
				{
					Stream myStream = saveFileDialog1.OpenFile();
					if (myStream != null)
					{
						using (myStream)
						{
							string path = saveFileDialog1.FileName;
							watcher.Path = path.Substring(0, path.LastIndexOf('\\'));
							assosiatedFile = path;
							sheet.getXMLDoc().Save(myStream);
							sheet.changedSinceSave = false;
						}
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: Could not write file to disk. Original error: " + ex.Message);
				}
			}
		}

		public void saveAgain()
		{
			if (assosiatedFile.Length > 0)
			{
				watcher.EnableRaisingEvents = false;
				try
				{
					StreamWriter myStream = new StreamWriter(assosiatedFile);
					if (myStream != null)
					{
						using (myStream)
						{
							sheet.getXMLDoc().Save(myStream);
							sheet.changedSinceSave = false;
						}
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: Could not autosave file to disk. Original error: " + ex.Message);
				}
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
						fileRead = true;
						try
						{
							StreamReader myStream = new StreamReader(assosiatedFile);
							if (myStream != null)
							{
								using (myStream)
								{
									XmlDocument doc = new XmlDocument();
									doc.Load(myStream);
									sheet = new Sheet(doc);
								}
							}
						}
						catch (Exception fe)
						{
							fileRead = false;
						}
					}
					refreshSheet();
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: Could not reload file from disk. Original error: " + ex.Message);
				}

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
				componentUIElement.Location = new Point(3, 3);
				componentUIElement.Name = "tableLayout" + component.name;
				componentUIElement.RowCount = 2;
				componentUIElement.RowStyles.Add(new RowStyle(SizeType.AutoSize));
				componentUIElement.RowStyles.Add(new RowStyle(SizeType.AutoSize));
				componentUIElement.Size = new Size(292, 60);
				componentUIElement.TabIndex = 0;

				Label nameLabel = new Label();
				nameLabel.Anchor = AnchorStyles.None;
				nameLabel.AutoSize = true;
				nameLabel.Location = new Point(128, 17);
				nameLabel.Name = "nameLabel" + component.name;
				nameLabel.Size = new Size(35, 13);
				nameLabel.TabIndex = 0;
				nameLabel.Text = component.name;
				componentUIElement.Controls.Add(nameLabel, 0, 0);

				componentUIElement.Controls.Add(component.getUIElement(), 0, 1);

				if (i % 2 == 0)
				{
					LeftComponentTable.RowStyles.Add(new RowStyle(SizeType.Percent, Convert.ToSingle(100F / amountOfRows)));
					LeftComponentTable.Controls.Add(componentUIElement);
				}
				else
				{
					RightComponentTable.RowStyles.Add(new RowStyle(SizeType.Percent, Convert.ToSingle(100F / amountOfRows)));
					RightComponentTable.Controls.Add(componentUIElement);
				}
			}

			LeftComponentTable.RowCount = amountOfRows;
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
