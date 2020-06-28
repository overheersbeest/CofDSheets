using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace CofD_Sheet.Sheet_Components
{
	[Serializable]
	public class ExperienceComponent : ISheetComponent
	{
		[XmlIgnore]
		const int maxPerRow = 10;

		[XmlIgnore]
		const float separatorProportion = 2F;

		[XmlAttribute]
		public string beatName = "Beats";

		[XmlAttribute]
		public int maxBeats = 5;

		[XmlAttribute]
		public int beats = 0;

		[XmlAttribute]
		public int experience = 0;

		[XmlIgnore]
		List<CheckBox> beatBoxes = new List<CheckBox>();

		[XmlIgnore]
		TextBox experienceCounter = null;

		public ExperienceComponent() : base("ExperienceComponent", ColumnId.Undefined)
		{ }

		public ExperienceComponent(string majorName, string minorName, ColumnId _column) : base(majorName, _column)
		{
			beatName = minorName;
		}

		override public Control ConstructUIElement()
		{
			int rowAmount = Convert.ToInt32(Math.Ceiling(maxBeats / Convert.ToSingle(maxPerRow)));
			int checkBoxRows = Math.Min(maxBeats, maxPerRow);
			int columnSeparatorCount = (checkBoxRows - 1) / 5;
			int columnAmount = checkBoxRows + columnSeparatorCount;

			uiElement.RowCount = 1;
			uiElement.ColumnCount = 2;
			uiElement.Dock = DockStyle.Fill;
			uiElement.Size = new Size(292, inputBoxHeight);
			uiElement.TabIndex = 0;

			//beats element
			uiElement.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
			TableLayoutPanel beatElement = new TableLayoutPanel
			{
				RowCount = rowAmount,
				ColumnCount = columnAmount,
				Dock = DockStyle.Fill,
				Size = new Size(componentWidth, 20 * rowAmount),
				TabIndex = 0
			};
			uiElement.Controls.Add(beatElement, 0, 0);
			beatBoxes.Clear();
			float separatorWidth = 100F / (checkBoxRows * separatorProportion + columnSeparatorCount);

			for (int r = 0; r < rowAmount; r++)
			{
				beatElement.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / rowAmount));
				for (int c = 0; c < columnAmount; c++)
				{
					if ((c + 1) % 6 == 0)
					{
						//break, to separate groups of 5
						beatElement.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, separatorWidth));
					}
					else
					{
						int checkBoxNr = beatBoxes.Count;
						beatElement.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, separatorWidth * separatorProportion));
						CheckBox checkBox = new CheckBox
						{
							Anchor = System.Windows.Forms.AnchorStyles.None,
							AutoSize = true,
							Size = new System.Drawing.Size(15, 14),
							TabIndex = 0,
							UseVisualStyleBackColor = true,
							Checked = checkBoxNr < beats
						};
						checkBox.Click += OnValueChanged;
						beatBoxes.Add(checkBox);
						beatElement.Controls.Add(checkBox, c, r);
					}
				}
			}

			//experience element
			uiElement.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
			experienceCounter = new TextBox
			{
				Anchor = System.Windows.Forms.AnchorStyles.None,
				AutoSize = true,
				Size = new System.Drawing.Size(100, 14),
				TabIndex = 0,
				Text = experience.ToString()
			};
			experienceCounter.TextChanged += OnValueChanged;
			uiElement.Controls.Add(experienceCounter, 1, 0);

			OnValueChanged();
			return uiElement;
		}

		void OnValueChanged(object sender = null, EventArgs e = null)
		{
			bool emptyText = experienceCounter.Text.Length == 0;
			if (emptyText)
			{
				experience = 0;
			}
			else
			{
				try
				{
					experience = Convert.ToInt32(experienceCounter.Text);
				}
				catch (Exception)
				{ }
			}

			beats = 0;
			for (int i = beatBoxes.Count - 1; i >= 0; i--)
			{
				if (beatBoxes[i].Checked)
				{
					beats++;
				}
			}

			int xpEarned = beats / maxBeats;
			experience += xpEarned;
			beats %= maxBeats;

			for (int i = beatBoxes.Count - 1; i >= 0; i--)
			{
				beatBoxes[i].Checked = i < beats;
			}
			if (!emptyText)
			{
				experienceCounter.TextChanged -= OnValueChanged;
				experienceCounter.Text = experience.ToString();
				experienceCounter.TextChanged += OnValueChanged;
			}

			OnComponentChanged();
		}
	}
}
