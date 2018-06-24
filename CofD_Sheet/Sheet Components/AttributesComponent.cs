using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace CofD_Sheet.Sheet_Components
{
	[Serializable]
	public class AttributesComponent : ISheetComponent
	{
		public class Attribute
		{
			public Attribute()
			{ }
			public Attribute(string _name)
			{
				this.name = _name;
			}
			public Attribute(string _name, int _value)
			{
				this.name = _name;
				this.currentValue = _value;
			}

			[XmlAttribute]
			public string name = "Attribute";

			[XmlAttribute]
			public int currentValue = 1;

			[XmlIgnore]
			public List<RadioButton> pips = new List<RadioButton>();
		}

		[XmlIgnore]
		const int maxDotsPerRow = 10;

		[XmlIgnore]
		const int nameLabelWidth = 100;

		[XmlAttribute]
		public int maxValue = 5;

		[XmlArray]
		public List<Attribute> attributes = new List<Attribute>();

		public AttributesComponent() : base("AttributesComponent", -1)
		{ }

		public AttributesComponent(string componentName, List<string> attributeNames, int componentColumnIndex) : base(componentName, componentColumnIndex)
		{
			for (int i = 0; i < attributeNames.Count; i++)
			{
				attributes.Add(new Attribute(attributeNames[i]));
			}
		}
		
		override public Control getUIElement()
		{
			int rowsPerAttribute = Convert.ToInt32(Math.Ceiling(maxValue / Convert.ToSingle(maxDotsPerRow)));
			int rowAmount = attributes.Count * rowsPerAttribute;
			int columnAmount = 1 + Math.Min(maxValue, maxDotsPerRow);

			TableLayoutPanel uiElement = new TableLayoutPanel();
			uiElement.RowCount = rowAmount;
			uiElement.ColumnCount = columnAmount;
			uiElement.Dock = DockStyle.Fill;
			uiElement.Name = "tableLayout" + name + "values";
			uiElement.Size = new Size(componentWidth, 21 * rowAmount);
			uiElement.TabIndex = 0;

			//column styles
			uiElement.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, nameLabelWidth));
			for (int c = 1; c < columnAmount; c++)
			{
				uiElement.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / (columnAmount - 1)));
			}

			for (int a = 0; a < attributes.Count; a++)
			{
				Attribute attribute = attributes[a];
				for (int ar = 0; ar < rowsPerAttribute; ++ar)
				{
					uiElement.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / rowAmount));
					if (ar == 0)
					{
						Label attributeNameLabel = new Label();
						attributeNameLabel.Anchor = AnchorStyles.None;
						attributeNameLabel.AutoSize = true;
						attributeNameLabel.Name = "attributeNameLabel" + attribute.name;
						attributeNameLabel.Size = new Size(nameLabelWidth, 20);
						attributeNameLabel.TabIndex = 0;
						attributeNameLabel.Text = attribute.name;
						uiElement.Controls.Add(attributeNameLabel, 0, a * rowsPerAttribute);
					}
				}

				//pips
				attribute.pips.Clear();
				for (int p = 0; p < maxValue; p++)
				{
					RadioButton pip = new RadioButton();
					pip.Anchor = System.Windows.Forms.AnchorStyles.None;
					pip.AutoSize = true;
					pip.Name = "pip" + name + "_" + 0;
					pip.Size = new System.Drawing.Size(20, 20);
					pip.TabIndex = 0;
					pip.UseVisualStyleBackColor = true;
					pip.Checked = 0 < attribute.currentValue;
					pip.Click += new EventHandler(valueChanged);
					pip.AutoCheck = false;
					attribute.pips.Add(pip);

					//insert pip in table
					int rowIndexOfAttribute = Convert.ToInt32(Math.Floor(p / Convert.ToSingle(maxDotsPerRow)));
					int column = (p - (rowIndexOfAttribute * maxDotsPerRow)) + 1;
					int row = a * rowsPerAttribute + rowIndexOfAttribute;
					uiElement.Controls.Add(pip, column, row);
				}
			}
			onValueChanged();
			return uiElement;
		}

		void valueChanged(object sender, EventArgs e)
		{
			foreach (Attribute attribute in attributes)
			{
				for (int i = 0; i < attribute.pips.Count; i++)
				{
					if (sender == attribute.pips[i])
					{
						if (attribute.currentValue == i + 1)
						{
							//when clicking the last pip, reduce value by 1
							attribute.currentValue = i;
						}
						else
						{
							attribute.currentValue = i + 1;
						}
					}
				}
			}
			onValueChanged();
		}

		void onValueChanged()
		{
			foreach (Attribute attribute in attributes)
			{
				for (int i = 0; i < attribute.pips.Count; i++)
				{
					attribute.pips[i].Checked = i < attribute.currentValue;
				}
			}

			onComponentChanged();
		}
	}
}
