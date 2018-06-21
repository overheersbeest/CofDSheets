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
		{ }

		public SimpleComponent(string componentName, int componentColumnIndex) : base(componentName, componentColumnIndex)
		{ }
		
		override public Control getUIElement()
		{
			int rowAmount = Convert.ToInt32(Math.Ceiling(maxValue / Convert.ToSingle(maxPerRow)));
			int checkBoxRows = Math.Min(maxValue, maxPerRow);
			int columnSeparatorCount = (checkBoxRows - 1) / 5;
			int columnAmount = checkBoxRows + columnSeparatorCount;

			TableLayoutPanel uiElement = new TableLayoutPanel();
			uiElement.RowCount = rowAmount;
			uiElement.ColumnCount = columnAmount;
			uiElement.Dock = DockStyle.Fill;
			uiElement.Name = "tableLayout" + name + "values";
			uiElement.Size = new Size(componentWidth, 20 * rowAmount);
			uiElement.TabIndex = 0;
			checkBoxes.Clear();
			
			float separatorWidth = uiElement.Size.Width / (checkBoxRows * separatorProportion + columnSeparatorCount);

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
							CheckBox checkBox = new CheckBox();
							checkBox.Anchor = System.Windows.Forms.AnchorStyles.None;
							checkBox.AutoSize = true;
							checkBox.Name = "checkbox" + name + "_" + checkBoxNr;
							checkBox.Size = new System.Drawing.Size(13, 13);
							checkBox.Dock = DockStyle.Fill;
							checkBox.TabIndex = 0;
							checkBox.UseVisualStyleBackColor = true;
							checkBox.Checked = checkBoxNr < currentValue;
							checkBox.Click += new EventHandler(valueChanged);
							checkBoxes.Add(checkBox);
							uiElement.Controls.Add(checkBox, c, r);
						}
					}
				}
			}
			onValueChanged();
			return uiElement;
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
