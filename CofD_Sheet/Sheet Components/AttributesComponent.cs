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
			public ModifiableInt Value = new ModifiableInt(1);

			[XmlIgnore]
			public List<RadioButton> pips = new List<RadioButton>();
		}

		[XmlIgnore]
		const int maxDotsPerRow = 10;

		[XmlIgnore]
		const int nameLabelWidth = 100;

		[XmlAttribute]
		public int maxValue = 5;

		[XmlIgnore]
		public int maxValueVisible = 0;

		[XmlArray]
		public List<Attribute> attributes = new List<Attribute>();

		public AttributesComponent() : base("AttributesComponent", ColumnId.Undefined)
		{
			Init();
		}

		public AttributesComponent(string componentName, List<string> attributeNames, ColumnId _column) : base(componentName, _column)
		{
			Init();

			for (int i = 0; i < attributeNames.Count; i++)
			{
				attributes.Add(new Attribute(attributeNames[i]));
			}
		}

		void Init()
		{
			uiElement.Dock = DockStyle.Fill;
			uiElement.TabIndex = 0;

			ContextMenuStrip contextMenu = new ContextMenuStrip();
			ToolStripItem changeMaxValueItem = contextMenu.Items.Add("Change maximum value");
			changeMaxValueItem.Click += new EventHandler(OpenChangeMaxValueDialog);
			uiElement.ContextMenuStrip = contextMenu;
		}

		override public Control ConstructUIElement()
		{
			OnMaxValuePossiblyChanged();

			return uiElement;
		}

		void OpenChangeMaxValueDialog(object sender, EventArgs e)
		{
			Form prompt = new Form
			{
				StartPosition = FormStartPosition.CenterParent,
				Width = 325,
				Height = 100,
				Text = "Change maximum value"
			};

			bool confirmed = false;

			NumericUpDown inputBox = new NumericUpDown() { Left = 5, Top = 5, Width = 300 };
			inputBox.Value = maxValue;
			inputBox.TabIndex = 0;
			inputBox.KeyDown += (sender2, e2) => { if (e2.KeyCode == Keys.Return) { confirmed = true; prompt.Close(); } };
			Button confirmation = new Button() { Text = "Confirm", Left = 205, Width = 100, Top = 30 };
			confirmation.TabIndex = 1;
			confirmation.Click += (sender2, e2) => { confirmed = true; prompt.Close(); };
			Button cancel = new Button() { Text = "Cancel", Left = 100, Width = 100, Top = 30 };
			cancel.TabIndex = 2;
			cancel.Click += (sender2, e2) => { prompt.Close(); };
			prompt.Controls.Add(inputBox);
			prompt.Controls.Add(confirmation);
			prompt.Controls.Add(cancel);
			prompt.ShowDialog();

			if (confirmed)
			{
				maxValue = (int)inputBox.Value;

				OnMaxValuePossiblyChanged();
			}
		}

		void RecomputeValues(object sender, EventArgs e)
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

		void OnMaxValuePossiblyChanged()
		{
			int newVisibleMaxValue = maxValue;
			foreach (Attribute attribute in attributes)
			{
				attribute.Value.maxValue = maxValue;
				newVisibleMaxValue = Math.Max(newVisibleMaxValue, attribute.Value.CurrentValue);
			}

			if (newVisibleMaxValue != maxValueVisible)
			{
				int rowsPerAttribute = Convert.ToInt32(Math.Ceiling(newVisibleMaxValue / Convert.ToSingle(maxDotsPerRow)));
				int rowAmount = attributes.Count * rowsPerAttribute;
				int columnAmount = 1 + Math.Min(newVisibleMaxValue, maxDotsPerRow);

				uiElement.RowCount = rowAmount;
				uiElement.ColumnCount = columnAmount;
				uiElement.Size = new Size(componentWidth, rowHeight * rowAmount);
				ResizeParentColumn();
				uiElement.RowStyles.Clear();
				uiElement.ColumnStyles.Clear();
				uiElement.Controls.Clear();

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
								Text = attribute.name
							};
							uiElement.Controls.Add(attributeNameLabel, 0, a * rowsPerAttribute);
						}
					}

					//pips
					attribute.pips.Clear();
					for (int p = 0; p < newVisibleMaxValue; p++)
					{
						RadioButton pip = new RadioButton
						{
							Anchor = System.Windows.Forms.AnchorStyles.None,
							AutoSize = true,
							Size = new System.Drawing.Size(20, 20),
							TabIndex = 0,
							UseVisualStyleBackColor = true
						};
						pip.Click += new EventHandler(RecomputeValues);
						pip.AutoCheck = false;
						attribute.pips.Add(pip);

						//insert pip in table
						int rowIndexOfAttribute = Convert.ToInt32(Math.Floor(p / Convert.ToSingle(maxDotsPerRow)));
						int column = (p - (rowIndexOfAttribute * maxDotsPerRow)) + 1;
						int row = a * rowsPerAttribute + rowIndexOfAttribute;
						uiElement.Controls.Add(pip, column, row);
					}
				}

				maxValueVisible = newVisibleMaxValue;
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
			if (mod is ModificationSetComponent.IntModification intMod)
			{
				if (mod.path.Count > 1)
				{
					string targetAttribute = mod.path[1];
					foreach (Attribute attribute in attributes)
					{
						if (attribute.name == targetAttribute)
						{
							attribute.Value.ApplyModification(intMod.modType, intMod.value);
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
		}

		override public void OnModificationsComplete()
		{
			OnMaxValuePossiblyChanged();
		}
	}
}
