using CofD_Sheet.Modifications;
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
	public class ResourceComponent : ISheetComponent
	{
		[XmlIgnore]
		const int maxPerRow = 15;

		[XmlIgnore]
		const float separatorProportion = 2F;

		[XmlElement]
		public ModifiableInt MaxValue = new ModifiableInt(0);

		[XmlAttribute]
		public int currentValue = 0;

		[XmlAttribute]
		public bool canMultistep = false;

		[XmlAttribute]
		public bool canModifyMaxValue = true;

		[XmlIgnore]
		private readonly List<Control> controls = new List<Control>();

		public ResourceComponent() : base("SimpleComponent", ColumnId.Undefined)
		{ }

		public ResourceComponent(string componentName, bool _canMultistep, bool _canModifyMaxValue, int _startValue, int _maxValue, ColumnId _column) : base(componentName, _column)
		{
			canMultistep = _canMultistep;
			canModifyMaxValue = _canModifyMaxValue;
			MaxValue.CurrentValue = _maxValue;
			currentValue = _startValue;

			Init();
		}

		override public void Init()
		{
			uiElement.Dock = DockStyle.Fill;
			uiElement.TabIndex = 0;

			ContextMenuStrip contextMenu = new ContextMenuStrip();
			if (canModifyMaxValue)
			{
				ToolStripItem changeMaxValueItem = contextMenu.Items.Add("Change maximum value");
				changeMaxValueItem.Click += new EventHandler(OpenChangeMaxValueDialog);
			}
			uiElement.ContextMenuStrip = contextMenu;
		}

		override public Control ConstructUIElement()
		{
			OnMaxValueChanged();

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
			inputBox.Minimum = Math.Min(0, MaxValue.CurrentValue);
			inputBox.Value = MaxValue.CurrentValue;
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
				MaxValue.CurrentValue = (int)inputBox.Value;

				OnMaxValueChanged();
			}
		}

		void RecomputeValue(object sender, EventArgs e)
		{
			for (int i = 0; i < controls.Count; ++i)
			{
				if (sender == controls[i])
				{
					if (canMultistep)
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
					else
					{
						bool isChecked = currentValue > i;
						if (isChecked)
						{
							--currentValue;
						}
						else
						{
							++currentValue;
						}
					}
				}
			}
			OnValueChanged();
		}

		void OnMaxValueChanged()
		{
			int rowAmount = Convert.ToInt32(Math.Ceiling(MaxValue.CurrentValue / Convert.ToSingle(maxPerRow)));
			int checkBoxRows = Math.Min(MaxValue.CurrentValue, maxPerRow);
			int columnSeparatorCount = (checkBoxRows - 1) / 5;
			int columnAmount = checkBoxRows + columnSeparatorCount;
			uiElement.RowCount = rowAmount;
			uiElement.ColumnCount = columnAmount;
			uiElement.Size = new Size(componentWidth, rowHeight * rowAmount);
			ResizeParentColumn();
			uiElement.RowStyles.Clear();
			uiElement.ColumnStyles.Clear();

			float separatorWidth = uiElement.Size.Width / (checkBoxRows * separatorProportion + columnSeparatorCount);

			if (controls.Count > MaxValue.CurrentValue)
			{
				for (int i = MaxValue.CurrentValue; i < controls.Count; ++i)
				{
					controls[i].Dispose();
				}
				controls.RemoveRange(MaxValue.CurrentValue, controls.Count - MaxValue.CurrentValue);
			}

			int controlIter = 0;
			for (int r = 0; r < rowAmount; ++r)
			{
				uiElement.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / rowAmount));
				for (int c = 0; c < columnAmount; ++c)
				{
					if ((c + 1) % 6 == 0)
					{
						//break, to separate groups of 5
						if (r == 0)
						{
							uiElement.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, separatorWidth));
						}
					}
					else
					{
						if (r == 0)
						{
							uiElement.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, separatorWidth * separatorProportion));
						}
						if (controlIter < MaxValue.CurrentValue)
						{
							if (controlIter >= controls.Count)
							{
								Control newControl;
								if (canMultistep)
								{
									newControl = new CheckBox
									{
										Anchor = System.Windows.Forms.AnchorStyles.None,
										AutoSize = true,
										Size = new System.Drawing.Size(15, 14),
										Dock = DockStyle.None,
										TabIndex = 0,
										UseVisualStyleBackColor = true,
										Checked = controlIter < currentValue,
										AutoCheck = false
									};
								}
								else
								{
									newControl = new RadioButton
									{
										Anchor = System.Windows.Forms.AnchorStyles.None,
										AutoSize = true,
										Size = new System.Drawing.Size(15, 14),
										Dock = DockStyle.None,
										TabIndex = 0,
										UseVisualStyleBackColor = true,
										Checked = controlIter < currentValue,
										AutoCheck = false
									};
								}
								newControl.Click += new EventHandler(RecomputeValue);

								controls.Add(newControl);
							}
							uiElement.Controls.Add(controls[controlIter], c, r);
							++controlIter;
						}
					}
				}
			}

			currentValue = Math.Min(currentValue, MaxValue.CurrentValue);

			OnValueChanged();
		}

		void OnValueChanged()
		{
			for (int i = controls.Count - 1; i >= 0; i--)
			{
				if (controls[i] is CheckBox checkBox)
				{
					checkBox.Checked = i < currentValue;
				}
				else if (controls[i] is RadioButton pip)
				{
					pip.Checked = i < currentValue;
				}
				else
				{
					throw new Exception("unrecognized control in ResourceComponent");
				}
			}

			OnComponentChanged();
		}

		public override int QueryInt(List<string> path)
		{
			if (path.Count > 1)
			{
				if (path[1] == "MaxValue")
				{
					isCurrentlyIncludedInModFormula = true;
					return MaxValue.CurrentValue;
				}
				if (path[1] == "Value")
				{
					isCurrentlyIncludedInModFormula = true;
					return currentValue;
				}
			}

			throw new Exception("Component could not complete Query: " + path.ToString());
		}

		override public void ApplyModification(Modification mod, Sheet sheet)
		{
			if (mod is IntModification intMod)
			{
				if (mod.path.Count > 1)
				{
					if (mod.path[1] == "MaxValue")
					{
						MaxValue.ApplyModification(intMod, sheet);
						isCurrentlyModified = true;
					}
				}
			}
		}

		override public void ResetModifications()
		{
			base.ResetModifications();
			MaxValue.Reset();
		}

		override public void OnModificationsComplete()
		{
			if (isCurrentlyModified)
			{
				OnMaxValueChanged();
			}
		}
	}
}
