using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace CofD_Sheet.Sheet_Components
{
	[Serializable]
	public class AspirationsComponent : ISheetComponent
	{
		[XmlAttribute]
		public int maxAspirations = 3;

		[XmlArray]
		public List<string> aspirations = new List<string>();

		[XmlIgnore]
		List<TextBox> textBoxes = new List<TextBox>();

		public AspirationsComponent() : base("AspirationsComponent", -1)
		{ }

		public AspirationsComponent(string componentName, int amountAllowed, int componentColumnIndex) : base(componentName, componentColumnIndex)
		{
			maxAspirations = amountAllowed;
			aspirations = new List<string>(new string[maxAspirations]);
		}

		override public Control getUIElement()
		{
			TableLayoutPanel uiElement = new TableLayoutPanel();
			uiElement.RowCount = maxAspirations;
			uiElement.ColumnCount = 1;
			uiElement.Dock = DockStyle.Fill;
			uiElement.Name = "tableLayout" + name + "values";
			uiElement.Size = new Size(componentWidth, 23 * maxAspirations);
			uiElement.TabIndex = 0;
			textBoxes.Clear();

			uiElement.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			for (int r = 0; r < maxAspirations; r++)
			{
				uiElement.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / maxAspirations));

				TextBox textBox = new TextBox();
				textBox.Anchor = System.Windows.Forms.AnchorStyles.None;
				textBox.AutoSize = true;
				textBox.Name = "slot" + name + "_" + r;
				textBox.Size = new System.Drawing.Size(15, 14);
				textBox.Dock = DockStyle.Fill;
				textBox.TabIndex = 0;
				textBox.Text = aspirations[r];
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

			onComponentChanged();
		}
	}
}
