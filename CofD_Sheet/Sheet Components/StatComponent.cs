using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace CofD_Sheet.Sheet_Components
{
	[Serializable]
	public class StatComponent : ISheetComponent
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
		List<RadioButton> pips = new List<RadioButton>();

		public StatComponent() : base("StatComponent", -1) { }

		public StatComponent(string componentName, int componentColumnIndex) : base(componentName, componentColumnIndex)
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
			pips.Clear();

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
						int pipNr = pips.Count;
						if (r == 0)
						{
							uiElement.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, separatorWidth * separatorProportion));
						}
						if (pipNr < maxValue)
						{
							RadioButton pip = new RadioButton();
							pip.Anchor = System.Windows.Forms.AnchorStyles.None;
							pip.AutoSize = true;
							pip.Name = "pip" + name + "_" + pipNr;
							pip.Size = new System.Drawing.Size(15, 14);
							pip.Dock = DockStyle.Fill;
							pip.TabIndex = 0;
							pip.UseVisualStyleBackColor = true;
							pip.Checked = pipNr < currentValue;
							pip.Click += new EventHandler(valueChanged);
							pip.AutoCheck = false;
							pips.Add(pip);
							uiElement.Controls.Add(pip, c, r);
						}
					}
				}
			}
			onValueChanged();
			return uiElement;
		}

		void valueChanged(object sender, EventArgs e)
		{
			for (int i = pips.Count - 1; i >= 0; i--)
			{
				if (pips[i] == sender)
				{
					if (pips[i].Checked)
					{
						--currentValue;
					}
					else
					{
						++currentValue;
					}
				}
			}
			onValueChanged();
		}

		void onValueChanged()
		{
			for (int i = 0; i < pips.Count; i++)
			{
				pips[i].Checked = i < currentValue;
			}

			onComponentChanged();
		}
	}
}
