using CofD_Sheet.Sheet_Components;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace CofD_Sheet
{
	public partial class Form1 : Form
	{
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
					string name = AssosiatedFile.Substring(slashIndex + 1, AssosiatedFile.Length - slashIndex - 1);
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

		readonly FileSystemWatcher watcher = new FileSystemWatcher();

		public DrawingHelper drawingHelper;

		static public Dictionary<SheetType, List<SheetType>> SheetTypeParentage = new Dictionary<SheetType, List<SheetType>>()
		{
			{ SheetType.None, new List<SheetType>() { SheetType.Mortal, SheetType.Mage, SheetType.Vampire, SheetType.Werewolf, SheetType.Ephemeral_Entity, SheetType.Other } },
			{ SheetType.Mage, new List<SheetType>() { SheetType.Proximi } },
			{ SheetType.Vampire, new List<SheetType>() { SheetType.Ghoul} },
			{ SheetType.Werewolf, new List<SheetType>() { SheetType.Wolf_Blooded} },
			{ SheetType.Ephemeral_Entity, new List<SheetType>() { SheetType.Angel, SheetType.Ghost, SheetType.Spirit, SheetType.Goetia } },
			{ SheetType.Other, new List<SheetType>() { SheetType.Abyssal_Entity, SheetType.Supernal_Entity } },
			{ SheetType.Abyssal_Entity, new List<SheetType>() { SheetType.Acamoth, SheetType.Gulmoth} }
		};

		public Form1()
		{
			InitializeComponent();

			sheet.form = this;

			if (SheetTypeParentage.TryGetValue(SheetType.None, out List<SheetType> rootTypes))
			{
#if DEBUG
				rootTypes.Insert(0, SheetType.Test);
#endif
				foreach (SheetType rootType in rootTypes)
				{
					AddToolStripMenuItemsForSheetType(rootType, ref newToolStripMenuItem);
				}
			}

			watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
			watcher.Changed += new FileSystemEventHandler(LoadAgain);
			autoSaveToolStripMenuItem.Checked = AutoSave;
			autoLoadToolStripMenuItem.Checked = AutoLoad;

			drawingHelper = new DrawingHelper(this);
		}

		private void AddToolStripMenuItemsForSheetType(SheetType SheetTypeToAdd, ref ToolStripMenuItem ParentMenuItem)
		{
			ToolStripMenuItem NewButton = new ToolStripMenuItem
			{
				Name = "New" + SheetTypeToAdd.ToString() + "Button",
				Size = new Size(152, 22),
				Text = SheetTypeToAdd.ToString().Replace("_", " ")
			};
			if (SheetTypeToAdd < SheetType.None)
			{
				NewButton.Click += new System.EventHandler(this.NewSheetButtonClicked);
			}
			ParentMenuItem.DropDownItems.Add(NewButton);
			if (SheetTypeParentage.TryGetValue(SheetTypeToAdd, out List<SheetType> ChildTypes))
			{
				foreach (SheetType ChildType in ChildTypes)
				{
					AddToolStripMenuItemsForSheetType(ChildType, ref NewButton);
				}
			}
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
				string ExceptionTrace = "";
				Exception Inner = e.InnerException;
				while (Inner != null)
				{
					ExceptionTrace += "\r\n" + Inner.Message;
					Inner = Inner.InnerException;
				}
				MessageBox.Show("Error: Could not save file to disk. " + e.Message + ExceptionTrace);
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
				foreach (ISheetComponent component in sheet.components)
				{
					component.Init(sheet);
				}
				reader.Close();
				AssosiatedFile = path;
				RefreshSheet();
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

		public delegate void refreshSheetCallback();

		Dictionary<Control, List<Control>> contextMenuDuplicators = new Dictionary<Control, List<Control>>();

		public void RefreshSheet()
		{
			//in case we're being called from another thread
			if (PlayerTextBox.InvokeRequired)
			{
				refreshSheetCallback d = new refreshSheetCallback(RefreshSheet);
				this.Invoke(d, new object[] { });
				return;
			}

			autoSaveDisabled = true;
			drawingHelper.SuspendDrawing();

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

			sheet.allowRefreshingMods = false;

			contextMenuDuplicators.Clear();

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

				componentUIElement.Controls.Add(nameLabel, 0, 0);
				Control componentValueElement = component.ConstructUIElement();
				componentUIElement.Controls.Add(componentValueElement, 0, 1);
				//pass on context menu strip to other components as well
				contextMenuDuplicators.Add(componentValueElement, new List<Control> { componentUIElement, nameLabel });

				if (component.column == ColumnId.Left)
				{
					LeftComponentTable.Controls.Add(componentUIElement);
					++LeftComponentTable.RowCount;
				}
				else if (component.column == ColumnId.Middle)
				{
					MiddleComponentTable.Controls.Add(componentUIElement);
					++MiddleComponentTable.RowCount;
				}
				else
				{
					RightComponentTable.Controls.Add(componentUIElement);
					++RightComponentTable.RowCount;
				}
			}

			sheet.allowRefreshingMods = true;

			//all components refreshed, now apply modification sets
			sheet.RefreshModifications(false);

			//add empty tables to be used for padding out each column
			LeftComponentTable.Controls.Add(new TableLayoutPanel());
			++LeftComponentTable.RowCount;
			MiddleComponentTable.Controls.Add(new TableLayoutPanel());
			++MiddleComponentTable.RowCount;
			RightComponentTable.Controls.Add(new TableLayoutPanel());
			++RightComponentTable.RowCount;

			ResizeColumn(LeftComponentTable);
			ResizeColumn(MiddleComponentTable);
			ResizeColumn(RightComponentTable);

			drawingHelper.ResumeDrawing(true);
			autoSaveDisabled = false;
		}

		public static void TransferContextMenuForControl(ISheetComponent source)
		{
			if (source.sheet != null
				&& source.sheet.form != null
				&& source.sheet.form.contextMenuDuplicators.TryGetValue(source.uiElement, out List<Control> destinations))
			{
				foreach (Control dest in destinations)
				{
					dest.ContextMenuStrip = source.uiElement.ContextMenuStrip;
				}
			}
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
			if (component.Parent is TableLayoutPanel cell)
			{
				cell.Size = new Size(cell.Size.Width, component.Size.Height);
				ResizeTableHeight(ref cell);
				TableLayoutPanel column = cell.Parent as TableLayoutPanel;
				ResizeTableHeight(ref column);
			}
		}

		public static void ResizeColumn(TableLayoutPanel column)
		{
			foreach (Control child in column.Controls)
			{
				TableLayoutPanel component = child as TableLayoutPanel;
				foreach (Control componentChild in column.Controls)
				{
					TableLayoutPanel componentValue = componentChild as TableLayoutPanel;
					ResizeTableHeight(ref componentValue);
				}
				ResizeTableHeight(ref component);
			}
			ResizeTableHeight(ref column);
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
			sheet = new Sheet((SheetType)Enum.Parse(typeof(SheetType), sender.ToString().Replace(" ", "_")));
			sheet.form = this;
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
