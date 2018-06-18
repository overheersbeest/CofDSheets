using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace CofD_Sheet.Sheet_Components
{
	public class StatComponent : ISheetComponent
	{
		[XmlIgnore]
		const int maxPerRow = 15;

		[XmlIgnore]
		const float separatorProportion = 2F;

		[XmlAttribute]
		int maxValue = 10;

		[XmlAttribute]
		int currentValue = 0;

		[XmlIgnore]
		List<RadioButton> pips = new List<RadioButton>();

		public StatComponent() : base("StatComponent") { }

		public StatComponent(string componentName) : base(componentName)
		{
			type = ISheetComponent.Type.Stat;
		}

		public StatComponent(XmlNode node) : base(node.Name)
		{
			maxValue = Convert.ToInt32(node.Attributes["MaxValue"].Value);
			currentValue = Convert.ToInt32(node.Attributes["CurrentValue"].Value);
			type = ISheetComponent.Type.Stat;
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
			uiElement.Name = "tableLayout" + name + "values";
			uiElement.Size = new Size(292, 60);
			uiElement.TabIndex = 0;
			pips.Clear();

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
						int pipNr = pips.Count;
						uiElement.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, separatorWidth * separatorProportion));
						RadioButton pip = new RadioButton();
						pip.Anchor = System.Windows.Forms.AnchorStyles.None;
						pip.AutoSize = true;
						pip.Name = "pip" + name + "_" + pipNr;
						pip.Size = new System.Drawing.Size(15, 14);
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
			onValueChanged();
			return uiElement;
		}

		void valueChanged(object sender, EventArgs e)
		{
			for (int i = 0; i < pips.Count; i++)
			{
				if (sender == pips[i])
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
			for (int i = 0; i < pips.Count; i++)
			{
				pips[i].Checked = i < currentValue;
			}

			onComponentChanged();
		}
	}
}
