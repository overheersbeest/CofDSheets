using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace CofD_Sheet.Sheet_Components
{
	[Serializable]
	public class SimpleComponent : ISheetComponent
	{
		[XmlIgnore]
		const int maxPerRow = 15;

		[XmlIgnore]
		const float separatorProportion = 2F;

		[XmlAttribute]
		public int maxValue = 10;

		[XmlAttribute]
		public int currentValue = 0;

		[XmlIgnore]
		List<CheckBox> checkBoxes = new List<CheckBox>();

		public SimpleComponent() : base("SimpleComponent", -1)
		{
			init();
		}

		public SimpleComponent(string componentName, int componentColumnIndex) : base(componentName, componentColumnIndex)
		{
			init();
		}

		void init()
		{
			uiElement.Dock = DockStyle.Fill;
			uiElement.Name = "tableLayout" + name + "values";
			uiElement.TabIndex = 0;

			ContextMenuStrip contextMenu = new ContextMenuStrip();
			ToolStripItem addMeritItem = contextMenu.Items.Add("Change maximum value");
			addMeritItem.Click += new EventHandler(changeMaxValue);
			uiElement.ContextMenuStrip = contextMenu;
		}

		override public Control getUIElement()
		{
			onMaxValueChanged();

			return uiElement;
		}

		void changeMaxValue(object sender, EventArgs e)
		{
			ContextMenuStrip owner = (sender as ToolStripItem).Owner as ContextMenuStrip;
			TableLayoutPanel uiElement = owner.SourceControl as TableLayoutPanel;

			Form prompt = new Form();
			prompt.Width = 325;
			prompt.Height = 100;
			prompt.Text = "Change maximum value";
			NumericUpDown inputBox = new NumericUpDown() { Left = 5, Top = 5, Width = 300 };
			inputBox.Value = maxValue;
			Button confirmation = new Button() { Text = "Confirm", Left = 205, Width = 100, Top = 30 };
			confirmation.Click += (sender2, e2) => { prompt.Close(); };
			Button cancel = new Button() { Text = "Cancel", Left = 100, Width = 100, Top = 30 };
			cancel.Click += (sender2, e2) => { inputBox.Value = maxValue; prompt.Close(); };
			prompt.Controls.Add(inputBox);
			prompt.Controls.Add(confirmation);
			prompt.Controls.Add(cancel);
			prompt.ShowDialog();

			maxValue = (int)inputBox.Value;
			onMaxValueChanged();
		}

		void valueChanged(object sender, EventArgs e)
		{
			for (int i = 0; i < checkBoxes.Count; i++)
			{
				if (sender == checkBoxes[i])
				{
					if (currentValue == i + 1)
					{
						//when clicking the last pip, reduce value by 1
						currentValue = i;
					}
					else
					{
						currentValue = i + 1;
					}
				}
			}
			onValueChanged();
		}

		void onMaxValueChanged()
		{
			int rowAmount = Convert.ToInt32(Math.Ceiling(maxValue / Convert.ToSingle(maxPerRow)));
			int checkBoxRows = Math.Min(maxValue, maxPerRow);
			int columnSeparatorCount = (checkBoxRows - 1) / 5;
			int columnAmount = checkBoxRows + columnSeparatorCount;
			uiElement.RowCount = rowAmount;
			uiElement.ColumnCount = columnAmount;
			uiElement.Size = new Size(componentWidth, 20 * rowAmount);
			uiElement.RowStyles.Clear();
			uiElement.ColumnStyles.Clear();

			float separatorWidth = uiElement.Size.Width / (checkBoxRows * separatorProportion + columnSeparatorCount);

			if (checkBoxes.Count > maxValue)
			{
				checkBoxes.RemoveRange(maxValue, checkBoxes.Count - maxValue);
			}

			int checkBoxIter = 0;
			for (int r = 0; r < rowAmount; r++)
			{
				uiElement.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / rowAmount));
				for (int c = 0; c < columnAmount; c++)
				{
					if ((c + 1) % 6 == 0)
					{
						//break, to separate groups of 5
						if (r == 0)
						{
							uiElement.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, separatorWidth));
						}
					}
					else
					{
						if (r == 0)
						{
							uiElement.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, separatorWidth * separatorProportion));
						}
						if (checkBoxIter < maxValue)
						{
							if (checkBoxIter >= checkBoxes.Count)
							{
								CheckBox checkBox = new CheckBox();
								checkBox.Anchor = System.Windows.Forms.AnchorStyles.None;
								checkBox.AutoSize = true;
								checkBox.Name = "pip" + name + "_" + checkBoxIter;
								checkBox.Size = new System.Drawing.Size(15, 14);
								checkBox.Dock = DockStyle.Fill;
								checkBox.TabIndex = 0;
								checkBox.UseVisualStyleBackColor = true;
								checkBox.Checked = checkBoxIter < currentValue;
								checkBox.Click += new EventHandler(valueChanged);
								checkBox.AutoCheck = false;
								checkBoxes.Add(checkBox);
							}
							uiElement.Controls.Add(checkBoxes[checkBoxIter], c, r);
							checkBoxIter++;
						}
					}
				}
			}

			currentValue = Math.Min(currentValue, maxValue);

			onValueChanged();
			
			for (int r = 0; r < rowAmount; r++)
			{
				uiElement.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / rowAmount));
				for (int c = 0; c < columnAmount; c++)
				{
					if ((c + 1) % 6 == 0)
					{
						//break, to separate groups of 5
						if (r == 0)
						{
							uiElement.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, separatorWidth));
						}
					}
					else
					{
						int checkBoxNr = checkBoxes.Count;
						if (r == 0)
						{
							uiElement.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, separatorWidth * separatorProportion));
						}
						if (checkBoxNr < maxValue)
						{
							
						}
					}
				}
			}
			onValueChanged();
		}

		void onValueChanged()
		{
			for (int i = checkBoxes.Count - 1; i >= 0; i--)
			{
				checkBoxes[i].Checked = i < currentValue;
			}

			onComponentChanged();
		}
	}
}
