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
		const float separatorProportion = 0.5F;

		[XmlAttribute]
		public string beatName = "Beats";

		[XmlAttribute]
		public int maxBeats = 5;

		[XmlAttribute]
		public int beats = 0;

		[XmlAttribute]
		public int experience = 0;

		[XmlIgnore]
		private readonly List<CheckBox> beatBoxes = new List<CheckBox>();

		[XmlIgnore]
		TableLayoutPanel beatElement = null;

		[XmlIgnore]
		TextBox experienceCounter = null;

		public ExperienceComponent() : base("ExperienceComponent", ColumnId.Undefined)
		{ }

		public ExperienceComponent(string majorName, string minorName, ColumnId _column, Sheet parentSheet) : base(majorName, _column)
		{
			beatName = minorName;
			Init(parentSheet);
		}

		override public void Init(Sheet parentSheet)
		{
			base.Init(parentSheet);
			uiElement.RowCount = 1;
			uiElement.ColumnCount = 2;
			uiElement.Dock = DockStyle.Fill;
			uiElement.TabIndex = 0;

			ContextMenuStrip contextMenu = new ContextMenuStrip();
			ToolStripItem changeMaxValueItem = contextMenu.Items.Add("Change maximum beats");
			changeMaxValueItem.Click += new EventHandler(OpenChangeMaxBeatsDialog);
			uiElement.ContextMenuStrip = contextMenu;
			Form1.TransferContextMenuForControl(this);

			beatElement = new TableLayoutPanel
			{
				Dock = DockStyle.Fill,
				TabIndex = 0
			};
			uiElement.Controls.Add(beatElement, 0, 0);

			//experience element
			experienceCounter = new TextBox
			{
				Anchor = AnchorStyles.None,
				AutoSize = true,
				Dock = DockStyle.Fill,
				Size = new Size(50, inputBoxHeight),
				TabIndex = 0,
				Text = experience.ToString()
			};
			experienceCounter.TextChanged += OnValueChanged;
			uiElement.Controls.Add(experienceCounter, 1, 0);
		}

		override public Control ConstructUIElement()
		{
			OnMaxBeatsChanged();

			return uiElement;
		}

		void OpenChangeMaxBeatsDialog(object sender, EventArgs e)
		{
			Form prompt = new Form
			{
				StartPosition = FormStartPosition.CenterParent,
				Width = 325,
				Height = 100,
				Text = "Change maximum beats"
			};

			bool confirmed = false;

			NumericUpDown inputBox = new NumericUpDown() { Left = 5, Top = 5, Width = 300 };
			inputBox.Minimum = Math.Min(0, maxBeats);
			inputBox.Value = maxBeats;
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
				maxBeats = (int)inputBox.Value;

				OnMaxBeatsChanged();
			}
		}

		void OnMaxBeatsChanged()
		{
			int rowAmount = Convert.ToInt32(Math.Ceiling(maxBeats / Convert.ToSingle(maxPerRow)));
			int checkBoxRows = Math.Min(maxBeats, maxPerRow);
			int columnSeparatorCount = (checkBoxRows - 1) / 5;
			int columnAmount = checkBoxRows + columnSeparatorCount;

			int beatElementHeight = 28 * rowAmount;
			uiElement.ColumnStyles.Clear();
			uiElement.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, Math.Max(150, checkBoxRows * 25)));
			uiElement.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
			uiElement.Size = new Size(292, Math.Max(inputBoxHeight, beatElementHeight));

			//beats element
			beatElement.RowCount = rowAmount;
			beatElement.ColumnCount = columnAmount;
			beatElement.Size = new Size(componentWidth, beatElementHeight);
			beatBoxes.Clear();
			beatElement.Controls.Clear();
			beatElement.RowStyles.Clear();
			beatElement.ColumnStyles.Clear();
			float columnWidth = 100F / (checkBoxRows + (columnSeparatorCount * separatorProportion));
			float separatorWidth = columnWidth * separatorProportion;

			for (int r = 0; r < rowAmount; ++r)
			{
				beatElement.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / rowAmount));
				for (int c = 0; c < columnAmount; ++c)
				{
					int checkBoxNr = beatBoxes.Count;
					if (checkBoxNr >= maxBeats)
					{
						break;
					}
					if ((c + 1) % 6 == 0)
					{
						//break, to separate groups of 5
						beatElement.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, separatorWidth));
					}
					else
					{
						beatElement.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, columnWidth));
						CheckBox checkBox = new CheckBox
						{
							Anchor = AnchorStyles.Top,
							AutoSize = true,
							Size = new Size(15, 14),
							TabIndex = 0,
							UseVisualStyleBackColor = true
						};
						checkBox.Click += OnValueChanged;
						beatBoxes.Add(checkBox);
						beatElement.Controls.Add(checkBox, c, r);
					}
				}
			}

			ResizeParentColumn();
			OnValueChanged();
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
					++beats;
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

		public override int QueryInt(List<string> path)
		{
			if (path.Count > 1)
			{
				if (String.Equals(path[1], "Experience", StringComparison.OrdinalIgnoreCase))
				{
					isCurrentlyIncludedInModFormula = true;
					return experience;
				}
				if (String.Equals(path[1], "Beats", StringComparison.OrdinalIgnoreCase))
				{
					isCurrentlyIncludedInModFormula = true;
					return beats;
				}
			}

			throw new Exception("Component could not complete Query: " + path.ToString());
		}
	}
}
