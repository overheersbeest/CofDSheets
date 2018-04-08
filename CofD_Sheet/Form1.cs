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
		static string initialSearchFolder = "c:\\";

		Sheet sheet = new Sheet();


		public Form1()
		{
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
		}

		private void loadToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Stream myStream = null;
			OpenFileDialog openFileDialog1 = new OpenFileDialog();

			//openFileDialog1.InitialDirectory = initialSearchFolder;
			openFileDialog1.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
			openFileDialog1.FilterIndex = 1;

			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				try
				{
					if ((myStream = openFileDialog1.OpenFile()) != null)
					{
						using (myStream)
						{
							XmlDocument doc = new XmlDocument();
							doc.Load(myStream);
							sheet = new Sheet(doc);
							string xmlcontents = doc.InnerXml;
						}
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
				}
			}

			refreshSheet();
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Stream myStream;
			SaveFileDialog saveFileDialog1 = new SaveFileDialog();

			saveFileDialog1.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
			saveFileDialog1.FilterIndex = 1;

			if (saveFileDialog1.ShowDialog() == DialogResult.OK)
			{
				if ((myStream = saveFileDialog1.OpenFile()) != null)
				{
					sheet.getXMLDoc().Save(myStream);
					myStream.Close();
				}
			}
		}
		
		private void refreshSheet()
		{
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
		}

		private void NameChanged(object sender, EventArgs e)
		{
			sheet.name = NameTextBox.Text;
			if (sheet.name.Length == 0)
			{
				this.Text = "CofD Sheet";
			}
			else
			{
				this.Text = "CofD Sheet - " + sheet.name;
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
			sheet = new Sheet((SheetType)Enum.Parse(typeof(SheetType), sender.ToString()));
			refreshSheet();
		}
	}
}
