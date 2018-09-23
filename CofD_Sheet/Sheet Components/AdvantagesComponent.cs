using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace CofD_Sheet.Sheet_Components
{
	[Serializable]
	public class AdvantagesComponent : ISheetComponent
	{
		public class Advantage
		{
			public Advantage()
			{ }
			public Advantage(string _name)
			{
				this.name = _name;
			}
			public Advantage(string _name, string _value)
			{
				this.name = _name;
				this.value = _value;
			}

			[XmlAttribute]
			public string name = "Advantage";

			[XmlAttribute]
			public string value = "";
		}
		
		[XmlArray]
		public List<Advantage> advantages = new List<Advantage>();

		[XmlIgnore]
		List<TextBox> textBoxes = new List<TextBox>();

		public AdvantagesComponent() : base("AdvantagesComponent", ColumnId.Undefined)
		{ }

		public AdvantagesComponent(string componentName, List<string> advantageNames, ColumnId _column) : base(componentName, _column)
		{
			foreach (string advantageName in advantageNames)
			{
				advantages.Add(new Advantage(advantageName));
			}
		}

		override public Control getUIElement()
		{
			uiElement.RowCount = advantages.Count;
			uiElement.ColumnCount = 1;
			uiElement.Dock = DockStyle.Fill;
			uiElement.Name = "tableLayout" + name + "values";
			uiElement.Size = new Size(componentWidth, 30 * advantages.Count);
			uiElement.TabIndex = 0;
			textBoxes.Clear();

			uiElement.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			for (int a = 0; a < advantages.Count; a++)
			{
				Advantage advantage = advantages[a];
				uiElement.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / advantages.Count));

				TableLayoutPanel advantageElement = new TableLayoutPanel();
				advantageElement.Name = "advantageTable" + advantage.name;
				advantageElement.Size = new Size(componentWidth, 23);

				Label label = new Label();
				label.Anchor = AnchorStyles.Left;
				label.Text = advantage.name + ": ";
				advantageElement.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize, label.Width));
				advantageElement.Controls.Add(label, 0, 0);

				TextBox textBox = new TextBox();
				textBox.Anchor = AnchorStyles.None;
				textBox.AutoSize = true;
				textBox.Name = "slot" + name + "_" + a;
				textBox.Size = new Size(15, 14);
				textBox.Dock = DockStyle.Fill;
				textBox.TabIndex = 0;
				textBox.Text = advantage.value;
				textBox.TextChanged += onValueChanged;
				textBoxes.Add(textBox);
				advantageElement.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
				advantageElement.Controls.Add(textBox, 1, 0);
				
				uiElement.Controls.Add(advantageElement, 0, a);
			}
			onValueChanged();
			return uiElement;
		}

		void onValueChanged(object sender = null, EventArgs e = null)
		{
			for (int i = 0; i < textBoxes.Count; i++)
			{
				advantages[i].value = textBoxes[i].Text;
			}

			onComponentChanged();
		}
	}
}
