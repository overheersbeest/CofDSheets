using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace CofD_Sheet.Sheet_Components
{
	[Serializable]
	public class StatComponent : ISheetComponent
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
		List<RadioButton> pips = new List<RadioButton>();

		public StatComponent() : base("StatComponent", ColumnId.Undefined)
		{
			Init();
		}

		public StatComponent(string componentName, ColumnId _column) : base(componentName, _column)
		{
			Init();
		}

		void Init()
		{
			uiElement.Dock = DockStyle.Fill;
			uiElement.TabIndex = 0;

			ContextMenuStrip contextMenu = new ContextMenuStrip();
			ToolStripItem ChangeMaxItem = contextMenu.Items.Add("Change maximum value");
			ChangeMaxItem.Click += new EventHandler(OpenChangeMaxValueDialog);
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
			}
			OnMaxValueChanged();
		}

		void ValueChanged(object sender, EventArgs e)
		{
			for (int i = pips.Count - 1; i >= 0; i--)
			{
				if (pips[i] == sender)
				{
					if (pips[i].Checked)
					{
						--currentValue;
					}
					else
					{
						++currentValue;
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

			if (pips.Count > maxValue)
			{
				for (int i = maxValue; i < pips.Count; ++i)
				{
					pips[i].Dispose();
				}
				pips.RemoveRange(maxValue, pips.Count - maxValue);
			}

			int pipIter = 0;
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
						if (pipIter < maxValue)
						{
							if (pipIter >= pips.Count)
							{
								RadioButton pip = new RadioButton
								{
									Anchor = System.Windows.Forms.AnchorStyles.None,
									AutoSize = true,
									Size = new System.Drawing.Size(15, 14),
									Dock = DockStyle.Fill,
									TabIndex = 0,
									UseVisualStyleBackColor = true,
									Checked = pipIter < currentValue
								};
								pip.Click += new EventHandler(ValueChanged);
								pip.AutoCheck = false;
								pips.Add(pip);
							}
							uiElement.Controls.Add(pips[pipIter], c, r);
							pipIter++;
						}
					}
				}
			}

			currentValue = Math.Min(currentValue, maxValue);

			OnValueChanged();
		}

		void OnValueChanged()
		{
			for (int i = 0; i < pips.Count; i++)
			{
				pips[i].Checked = i < currentValue;
			}

			OnComponentChanged();
		}
	}
}
