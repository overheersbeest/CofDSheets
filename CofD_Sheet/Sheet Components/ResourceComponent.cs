﻿using System;
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

		[XmlAttribute]
		public int maxValue = 10;

		[XmlAttribute]
		public int currentValue = 0;

		[XmlIgnore]
		List<CheckBox> checkBoxes = new List<CheckBox>();

		public ResourceComponent() : base("SimpleComponent", ColumnId.Undefined)
		{
			Init();
		}

		public ResourceComponent(string componentName, ColumnId _column) : base(componentName, _column)
		{
			Init();
		}

		void Init()
		{
			uiElement.Dock = DockStyle.Fill;
			uiElement.TabIndex = 0;

			ContextMenuStrip contextMenu = new ContextMenuStrip();
			ToolStripItem changeMaxValueItem = contextMenu.Items.Add("Change maximum value");
			changeMaxValueItem.Click += new EventHandler(ChangeMaxValue);
			uiElement.ContextMenuStrip = contextMenu;
		}

		override public Control ConstructUIElement()
		{
			OnMaxValueChanged();

			return uiElement;
		}

		void ChangeMaxValue(object sender, EventArgs e)
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
			confirmation.Click += (sender2, e2) => { confirmed = true;  prompt.Close(); };
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
			}
			OnMaxValueChanged();
		}

		void ValueChanged(object sender, EventArgs e)
		{
			for (int i = 0; i < checkBoxes.Count; i++)
			{
				if (sender == checkBoxes[i])
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
			}
			OnValueChanged();
		}

		void OnMaxValueChanged()
		{
			int rowAmount = Convert.ToInt32(Math.Ceiling(maxValue / Convert.ToSingle(maxPerRow)));
			int checkBoxRows = Math.Min(maxValue, maxPerRow);
			int columnSeparatorCount = (checkBoxRows - 1) / 5;
			int columnAmount = checkBoxRows + columnSeparatorCount;
			uiElement.RowCount = rowAmount;
			uiElement.ColumnCount = columnAmount;
			uiElement.Size = new Size(componentWidth, rowHeight * rowAmount);
			ResizeParentColumn();
			uiElement.RowStyles.Clear();
			uiElement.ColumnStyles.Clear();

			float separatorWidth = uiElement.Size.Width / (checkBoxRows * separatorProportion + columnSeparatorCount);

			if (checkBoxes.Count > maxValue)
			{
				for (int i = maxValue; i < checkBoxes.Count; ++i)
				{
					checkBoxes[i].Dispose();
				}
				checkBoxes.RemoveRange(maxValue, checkBoxes.Count - maxValue);
			}

			int checkBoxIter = 0;
			for (int r = 0; r < rowAmount; r++)
			{
				uiElement.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / rowAmount));
				for (int c = 0; c < columnAmount; c++)
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
						if (checkBoxIter < maxValue)
						{
							if (checkBoxIter >= checkBoxes.Count)
							{
								CheckBox checkBox = new CheckBox
								{
									Anchor = System.Windows.Forms.AnchorStyles.None,
									AutoSize = true,
									Size = new System.Drawing.Size(15, 14),
									Dock = DockStyle.Fill,
									TabIndex = 0,
									UseVisualStyleBackColor = true,
									Checked = checkBoxIter < currentValue
								};
								checkBox.Click += new EventHandler(ValueChanged);
								checkBox.AutoCheck = false;
								checkBoxes.Add(checkBox);
							}
							uiElement.Controls.Add(checkBoxes[checkBoxIter], c, r);
							checkBoxIter++;
						}
					}
				}
			}

			currentValue = Math.Min(currentValue, maxValue);

			OnValueChanged();

			for (int r = 0; r < rowAmount; r++)
			{
				uiElement.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / rowAmount));
				for (int c = 0; c < columnAmount; c++)
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
						int checkBoxNr = checkBoxes.Count;
						if (r == 0)
						{
							uiElement.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, separatorWidth * separatorProportion));
						}
						if (checkBoxNr < maxValue)
						{

						}
					}
				}
			}
			OnValueChanged();
		}

		void OnValueChanged()
		{
			for (int i = checkBoxes.Count - 1; i >= 0; i--)
			{
				checkBoxes[i].Checked = i < currentValue;
			}

			OnComponentChanged();
		}
	}
}
