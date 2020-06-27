using CofD_Sheet.Modifyables;
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
				this.Value.CurrentValue = _value;
			}

			[XmlAttribute]
			public string name = "Attribute";

			[XmlElement]
			public ModifiableInt Value = new ModifiableInt();

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

		public AttributesComponent() : base("AttributesComponent", ColumnId.Undefined)
		{ }

		public AttributesComponent(string componentName, List<string> attributeNames, ColumnId _column) : base(componentName, _column)
		{
			for (int i = 0; i < attributeNames.Count; i++)
			{
				attributes.Add(new Attribute(attributeNames[i]));
			}
		}
		
		override public Control ConstructUIElement()
		{
			int rowsPerAttribute = Convert.ToInt32(Math.Ceiling(maxValue / Convert.ToSingle(maxDotsPerRow)));
			int rowAmount = attributes.Count * rowsPerAttribute;
			int columnAmount = 1 + Math.Min(maxValue, maxDotsPerRow);
			
			uiElement.RowCount = rowAmount;
			uiElement.ColumnCount = columnAmount;
			uiElement.Dock = DockStyle.Fill;
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
						Label attributeNameLabel = new Label
						{
							Anchor = AnchorStyles.None,
							AutoSize = true,
							Name = "attributeNameLabel" + attribute.name,
							Size = new Size(nameLabelWidth, 20),
							TabIndex = 0,
							Text = attribute.name
						};
						uiElement.Controls.Add(attributeNameLabel, 0, a * rowsPerAttribute);
					}
				}

				//pips
				attribute.pips.Clear();
				for (int p = 0; p < maxValue; p++)
				{
					RadioButton pip = new RadioButton
					{
						Anchor = System.Windows.Forms.AnchorStyles.None,
						AutoSize = true,
						Size = new System.Drawing.Size(20, 20),
						TabIndex = 0,
						UseVisualStyleBackColor = true
					};
					pip.Click += new EventHandler(ValueChanged);
					pip.AutoCheck = false;
					attribute.pips.Add(pip);

					//insert pip in table
					int rowIndexOfAttribute = Convert.ToInt32(Math.Floor(p / Convert.ToSingle(maxDotsPerRow)));
					int column = (p - (rowIndexOfAttribute * maxDotsPerRow)) + 1;
					int row = a * rowsPerAttribute + rowIndexOfAttribute;
					uiElement.Controls.Add(pip, column, row);
				}
			}
			return uiElement;
		}

		void ValueChanged(object sender, EventArgs e)
		{
			foreach (Attribute attribute in attributes)
			{
				for (int i = 0; i < attribute.pips.Count; i++)
				{
					if (sender == attribute.pips[i])
					{
						if (attribute.Value.CurrentValue == i + 1)
						{
							//when clicking the last pip, reduce value by 1
							attribute.Value.CurrentValue = i;
						}
						else
						{
							attribute.Value.CurrentValue = i + 1;
						}
					}
				}
			}
			OnValueChanged();
		}

		void OnValueChanged()
		{
			foreach (Attribute attribute in attributes)
			{
				for (int i = 0; i < attribute.pips.Count; i++)
				{
					attribute.pips[i].Checked = i < attribute.Value.CurrentValue;
				}
			}

			OnComponentChanged();
		}

		override public void ApplyModification(ModificationSetComponent.Modification mod)
		{
			ModificationSetComponent.IntModification intMod = mod as ModificationSetComponent.IntModification;
			if (intMod != null)
			{
				if (mod.path.Count > 1)
				{
					string targetAttribute = mod.path[1];
					foreach (Attribute attribute in attributes)
					{
						if (attribute.name == targetAttribute)
						{
							attribute.Value.ApplyModification(intMod.modType, intMod.value);
							OnValueChanged();
							break;
						}
					}
				}
			}
		}

		override public void ResetModifications()
		{
			foreach (Attribute attribute in attributes)
			{
				attribute.Value.Reset();
			}
			OnValueChanged();
		}
	}
}
