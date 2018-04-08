using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace CofD_Sheet.Sheet_Components
{
	class HealthComponent : ISheetComponent
	{
		const int maxPerRow = 15;
		const float separatorProportion = 2F;

		int maxValue = 10;
		int aggrivated = 0;
		int lethal = 0;
		int bashing = 0;

		List<TextBox> slots = new List<TextBox>();

		public HealthComponent(string componentName) : base(componentName)
		{
			type = ISheetComponent.Type.Health;
		}

		public HealthComponent(XmlNode node) : base(node.Name)
		{
			maxValue = Convert.ToInt32(node.Attributes["MaxValue"].Value);
			aggrivated = Convert.ToInt32(node.Attributes["Aggrivated"].Value);
			lethal = Convert.ToInt32(node.Attributes["Lethal"].Value);
			bashing = Convert.ToInt32(node.Attributes["Bashing"].Value);
			type = ISheetComponent.Type.Health;
		}

		override protected void fillElement(ref XmlElement node, XmlDocument doc)
		{
			node.SetAttribute("MaxValue", maxValue.ToString());
			node.SetAttribute("Aggrivated", aggrivated.ToString());
			node.SetAttribute("Lethal", lethal.ToString());
			node.SetAttribute("Bashing", bashing.ToString());
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
			slots.Clear();

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
						int slotNr = slots.Count;
						uiElement.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, separatorWidth * separatorProportion));
						TextBox slot = new TextBox();
						slot.Anchor = System.Windows.Forms.AnchorStyles.None;
						slot.KeyDown += new KeyEventHandler(backSpacePressed);
						slot.AutoSize = true;
						slot.Location = new System.Drawing.Point(22, 14);
						slot.Name = "slot" + name + "_" + slotNr;
						slot.Size = new System.Drawing.Size(15, 14);
						slot.TabIndex = 0;
						slot.TextChanged += valueChanged;
						slots.Add(slot);
						uiElement.Controls.Add(slot, c, r);
					}
				}
			}
			onValueChanged();
			return uiElement;
		}

		void valueChanged(object sender, EventArgs e)
		{
			aggrivated = 0;
			lethal = 0;
			bashing = 0;
			for (int i = 0; i < slots.Count; i++)
			{
				string text = slots[i].Text;
				aggrivated += text.Count(f => f == '*');
				lethal += text.Count(f => f == 'x' || f == 'X');
				bashing += text.Count(f => f == '/' || f == '\\');
				int overDamage = Math.Max(0, aggrivated + lethal + bashing - maxValue);
				while (aggrivated < maxValue
					   && overDamage > 0)
				{
					if (bashing > 0)
					{
						//use bashing to upgrade least severe damage
						bashing--;
						if (bashing >= 1)
						{
							//2 bashing -> 1 lethal
							bashing--;
							lethal++;
						}
						else
						{
							//bashing + lethal -> aggrivated
							lethal--;
							aggrivated++;
						}
					}
					else
					{
						//no bashing damage, but still too much damage
						//2 lethal -> 1 aggrivated
						lethal -= 2;
						aggrivated++;
					}
					overDamage--;
				}

				//in case we still have damage left after having a health track filled with aggrivated damage, remove all other damages
				if (aggrivated >= maxValue)
				{
					aggrivated = maxValue;
					lethal = 0;
					bashing = 0;
				}
			}
			onValueChanged();
		}

		void onValueChanged()
		{
			for (int i = 0; i < slots.Count; i++)
			{
				slots[i].TextChanged -= valueChanged;
				if (i < aggrivated)
				{
					slots[i].Text = "*";
				}
				else if (i < aggrivated + lethal)
				{
					slots[i].Text = "x";
				}
				else if (i < aggrivated + lethal + bashing)
				{
					slots[i].Text = "/";
				}
				else
				{
					slots[i].Text = "";
				}
				slots[i].TextChanged += valueChanged;
			}
		}

		void backSpacePressed(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Back)
			{
				((TextBox)sender).Text = "";
				valueChanged(sender, null);
			}
			else if (e.KeyData == Keys.Left
					 || e.KeyData == Keys.Right)
			{
				int currentFocusIndex = 0;
				for (int i = 0; i < slots.Count; i++)
				{
					if (sender == slots[i])
					{
						currentFocusIndex = i;
						break;
					}
				}
				if (e.KeyData == Keys.Left
					&& currentFocusIndex > 0)
				{
					slots[currentFocusIndex - 1].Focus();
				}
				else if (e.KeyData == Keys.Right
						 && currentFocusIndex < slots.Count - 1)
				{
					slots[currentFocusIndex + 1].Focus();
				}
			}
		}
	}
}
