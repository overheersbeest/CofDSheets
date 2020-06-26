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

		public AspirationsComponent() : base("AspirationsComponent", ColumnId.Undefined)
		{ }

		public AspirationsComponent(string componentName, int amountAllowed, ColumnId _column) : base(componentName, _column)
		{
			maxAspirations = amountAllowed;
			aspirations = new List<string>(new string[maxAspirations]);
		}

		override public Control GetUIElement()
		{
			uiElement.RowCount = maxAspirations;
			uiElement.ColumnCount = 1;
			uiElement.Dock = DockStyle.Fill;
			uiElement.Size = new Size(componentWidth, 23 * maxAspirations);
			uiElement.TabIndex = 0;
			textBoxes.Clear();

			uiElement.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			for (int r = 0; r < maxAspirations; r++)
			{
				uiElement.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / maxAspirations));

				TextBox textBox = new TextBox
				{
					Anchor = System.Windows.Forms.AnchorStyles.None,
					AutoSize = true,
					Size = new System.Drawing.Size(15, 14),
					Dock = DockStyle.Fill,
					TabIndex = 0,
					Text = aspirations[r]
				};
				textBox.TextChanged += OnValueChanged;
				textBoxes.Add(textBox);
				uiElement.Controls.Add(textBox, 0, r);
			}
			OnValueChanged();
			return uiElement;
		}

		void OnValueChanged(object sender = null, EventArgs e = null)
		{
			for (int i = 0; i < textBoxes.Count; i++)
			{
				aspirations[i] = textBoxes[i].Text;
			}

			OnComponentChanged();
		}
	}
}
