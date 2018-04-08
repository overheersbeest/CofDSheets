﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace CofD_Sheet.Sheet_Components
{
	class SimpleComponent : ISheetComponent
	{
		const int maxPerRow = 15;
		const float separatorProportion = 2F;

		int maxValue = 10;
		int currentValue = 0;
		List<CheckBox> checkBoxes = new List<CheckBox>();

		public SimpleComponent(string componentName) : base(componentName)
		{
			type = ISheetComponent.Type.Simple;
		}

		public SimpleComponent(XmlNode node) : base(node.Name)
		{
			maxValue = Convert.ToInt32(node.Attributes["MaxValue"].Value);
			currentValue = Convert.ToInt32(node.Attributes["CurrentValue"].Value);
			type = ISheetComponent.Type.Simple;
		}

		override protected void fillElement(ref XmlElement node, XmlDocument doc)
		{
			node.SetAttribute("MaxValue", maxValue.ToString());
			node.SetAttribute("CurrentValue", currentValue.ToString());
		}

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
			uiElement.Location = new Point(3, 3);
			uiElement.Name = "tableLayout" + name + "values";
			uiElement.Size = new Size(292, 60);
			uiElement.TabIndex = 0;
			checkBoxes.Clear();

			float separatorWidth = 100F / (checkBoxRows * separatorProportion + columnSeparatorCount);

			for (int r = 0; r < rowAmount; r++)
			{
				uiElement.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / rowAmount));
				for (int c = 0; c < columnAmount; c++)
				{
					if ((c + 1) % 6 == 0)
					{
						//break, to separate groups of 5
						uiElement.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, separatorWidth));
					}
					else
					{
						int checkBoxNr = checkBoxes.Count;
						uiElement.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, separatorWidth * separatorProportion));
						CheckBox checkBox = new CheckBox();
						checkBox.Anchor = System.Windows.Forms.AnchorStyles.None;
						checkBox.AutoSize = true;
						checkBox.Location = new System.Drawing.Point(22, 14);
						checkBox.Name = "checkbox" + name + "_" + checkBoxNr;
						checkBox.Size = new System.Drawing.Size(15, 14);
						checkBox.TabIndex = 0;
						checkBox.UseVisualStyleBackColor = true;
						checkBox.Checked = checkBoxNr < currentValue;
						checkBox.Click += new EventHandler(valueChanged);
						checkBoxes.Add(checkBox);
						uiElement.Controls.Add(checkBox, c, r);
					}
				}
			}
			onValueChanged();
			return uiElement;
		}

		void valueChanged(object sender, EventArgs e)
		{
			onValueChanged();
		}

		void onValueChanged()
		{
			int counter = 0;
			for (int i = checkBoxes.Count - 1; i >= 0; i--)
			{
				if (checkBoxes[i].Checked)
				{
					counter++;
				}
			}
			currentValue = counter;
			for (int i = checkBoxes.Count - 1; i >= 0; i--)
			{
				checkBoxes[i].Checked = i < counter;
			}
		}
	}
}