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

		override public Control ConstructUIElement()
		{
			uiElement.RowCount = advantages.Count;
			uiElement.ColumnCount = 1;
			uiElement.Dock = DockStyle.Fill;
			uiElement.Size = new Size(componentWidth, 30 * advantages.Count);
			uiElement.TabIndex = 0;
			textBoxes.Clear();

			uiElement.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			for (int a = 0; a < advantages.Count; a++)
			{
				Advantage advantage = advantages[a];
				uiElement.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / advantages.Count));

				TableLayoutPanel advantageElement = new TableLayoutPanel
				{
					Name = "advantageTable" + advantage.name,
					Size = new Size(componentWidth, 23)
				};

				Label label = new Label
				{
					Anchor = AnchorStyles.Left,
					Text = advantage.name + ": "
				};
				advantageElement.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize, label.Width));
				advantageElement.Controls.Add(label, 0, 0);

				TextBox textBox = new TextBox
				{
					Anchor = AnchorStyles.None,
					AutoSize = true,
					Size = new Size(15, 14),
					Dock = DockStyle.Fill,
					TabIndex = 0,
					Text = advantage.value
				};
				textBox.TextChanged += OnValueChanged;
				textBoxes.Add(textBox);
				advantageElement.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
				advantageElement.Controls.Add(textBox, 1, 0);

				uiElement.Controls.Add(advantageElement, 0, a);
			}
			OnValueChanged();
			return uiElement;
		}

		void OnValueChanged(object sender = null, EventArgs e = null)
		{
			for (int i = 0; i < textBoxes.Count; i++)
			{
				advantages[i].value = textBoxes[i].Text;
			}

			OnComponentChanged();
		}
	}
}
