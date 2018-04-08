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
	class AspirationsComponent : ISheetComponent
	{
		int maxAspirations = 3;
		List<string> aspirations = new List<string>();

		List<TextBox> textBoxes = new List<TextBox>();

		public AspirationsComponent(string componentName, int amountAllowed = 3) : base(componentName)
		{
			maxAspirations = amountAllowed;
			aspirations = new List<string>(new string[maxAspirations]);
			type = ISheetComponent.Type.Aspirations;
		}
		
		public AspirationsComponent(XmlNode node) : base(node.Name)
		{
			maxAspirations = Convert.ToInt32(node.Attributes["maxAspirations"].Value);
			type = ISheetComponent.Type.Aspirations;
		}

		override protected void fillElement(ref XmlElement node, XmlDocument doc)
		{
			node.SetAttribute("maxAspirations", maxAspirations.ToString());
		}

		override public Control getUIElement()
		{
			TableLayoutPanel uiElement = new TableLayoutPanel();
			uiElement.RowCount = maxAspirations;
			uiElement.ColumnCount = 1;
			uiElement.Dock = DockStyle.Fill;
			uiElement.Location = new Point(3, 3);
			uiElement.Name = "tableLayout" + name + "values";
			uiElement.Size = new Size(292, 60);
			uiElement.TabIndex = 0;
			textBoxes.Clear();

			uiElement.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			for (int r = 0; r < maxAspirations; r++)
			{
				uiElement.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / maxAspirations));

				TextBox textBox = new TextBox();
				textBox.Anchor = System.Windows.Forms.AnchorStyles.None;
				textBox.AutoSize = true;
				textBox.Location = new System.Drawing.Point(22, 14);
				textBox.Name = "slot" + name + "_" + r;
				textBox.Size = new System.Drawing.Size(15, 14);
				textBox.Dock = DockStyle.Fill;
				textBox.TabIndex = 0;
				textBox.TextChanged += onValueChanged;
				textBoxes.Add(textBox);
				uiElement.Controls.Add(textBox, 0, r);
			}
			onValueChanged();
			return uiElement;
		}

		void onValueChanged(object sender = null, EventArgs e = null)
		{
			for (int i = 0; i < textBoxes.Count; i++)
			{
				aspirations[i] = textBoxes[i].Text;
			}
		}
	}
}
