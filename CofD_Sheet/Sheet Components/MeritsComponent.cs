using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace CofD_Sheet.Sheet_Components
{
	[Serializable]
	public class MeritsComponent : ISheetComponent
	{
		public class Merit
		{
			public Merit()
			{ }
			public Merit(string _name)
			{
				this.name = _name;
			}
			public Merit(string _name, int _value)
			{
				this.name = _name;
				this.currentValue = _value;
			}

			[XmlAttribute]
			public string name = "Merit";

			[XmlAttribute]
			public int currentValue = 0;
			
			[XmlIgnore]
			public List<RadioButton> pips = new List<RadioButton>();
		}

		[XmlIgnore]
		const int maxDotsPerRow = 10;

		[XmlIgnore]
		const int nameLabelWidth = 180;

		[XmlAttribute]
		public int maxValue = 5;
		
		[XmlAttribute]
		public string singularName = "merit";

		[XmlAttribute]
		public bool mutable = true;

		[XmlArray]
		public List<Merit> merits = new List<Merit>();

		public MeritsComponent() : base("SkillsComponent", ColumnId.Undefined)
		{ }

		public MeritsComponent(string componentName, string _singularName, bool _mutable, List<string> meritNames, int _maxValue, ColumnId _column) : base(componentName, _column)
		{
			this.singularName = _singularName;
			this.mutable = _mutable;
			this.maxValue = _maxValue;
			foreach (string meritName in meritNames)
			{
				merits.Add(new Merit(meritName));
			}
		}

		override public Control GetUIElement()
		{
			int columnAmount = 1 + Math.Min(maxValue, maxDotsPerRow);
			
			uiElement.ColumnCount = columnAmount;
			uiElement.Dock = DockStyle.Fill;
			uiElement.TabIndex = 0;

			//column styles
			uiElement.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, nameLabelWidth));
			for (int c = 1; c < columnAmount; c++)
			{
				uiElement.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / (columnAmount - 1)));
			}

			if (mutable)
			{
				ContextMenuStrip contextMenu = new ContextMenuStrip();
				ToolStripItem addMeritItem = contextMenu.Items.Add("Add " + singularName);
				addMeritItem.Click += new EventHandler(AddMerit);
				uiElement.ContextMenuStrip = contextMenu;
			}

			OnMeritsChanged();

			return uiElement;
		}

		void AddMerit(object sender, EventArgs e)
		{
			ContextMenuStrip owner = (sender as ToolStripItem).Owner as ContextMenuStrip;
			TableLayoutPanel uiElement = owner.SourceControl as TableLayoutPanel;

			Form prompt = new Form
			{
				StartPosition = FormStartPosition.CenterParent,
				Width = 325,
				Height = 100,
				Text = "Add " + singularName
			};
			TextBox inputBox = new TextBox() { Left = 5, Top = 5, Width = 300 };
			inputBox.TabIndex = 0;
			inputBox.KeyDown += (sender2, e2) => { if (e2.KeyCode == Keys.Return) { prompt.Close(); } };
			Button confirmation = new Button() { Text = "Add", Left = 205, Width = 100, Top = 30 };
			confirmation.TabIndex = 1;
            confirmation.Click += (sender2, e2) => { prompt.Close(); };
			prompt.Controls.Add(confirmation);
			prompt.Controls.Add(inputBox);
			prompt.ShowDialog();
			
			string newMerit = inputBox.Text;
			merits.Add(new Merit(newMerit));
			
			OnMeritsChanged();
		}

		void RemoveMerit(object sender, EventArgs e)
		{
			ContextMenuStrip owner = (sender as ToolStripItem).Owner as ContextMenuStrip;
			Label skillLabel = owner.SourceControl as Label;
			Merit merit = merits.Find(x => skillLabel.Text.StartsWith(x.name));
			if (merit == null)
			{
				return;
			}
			Form prompt = new Form
            {
                StartPosition = FormStartPosition.CenterParent,
                Width = 325,
				Height = 100,
				Text = "Remove " + singularName
			};
			Label question = new Label() { Left = 5, Top = 5, Width = 300 };
			question.Text = "Are you sure you want to remove the \"" + merit.name +"\" " + singularName + "?";
			Button confirmation = new Button() { Text = "Yes", Left = 205, Width = 100, Top = 30 };
			confirmation.Click += (sender2, e2) => { merits.Remove(merit); prompt.Close(); };
			Button cancel = new Button() { Text = "No", Left = 100, Width = 100, Top = 30 };
			cancel.Click += (sender2, e2) => { prompt.Close(); };
			prompt.Controls.Add(question);
			prompt.Controls.Add(confirmation);
			prompt.Controls.Add(cancel);
			prompt.ShowDialog();

			OnMeritsChanged();
		}

		void OnMeritsChanged()
		{
			int rowsPerMerit = Math.Max(1, Convert.ToInt32(Math.Ceiling(maxValue / Convert.ToSingle(maxDotsPerRow))));
			int rowAmount = merits.Count * rowsPerMerit;

			uiElement.RowStyles.Clear();
			uiElement.Controls.Clear();
			uiElement.RowStyles.Clear();
			uiElement.RowCount = rowAmount;

			for (int m = 0; m < merits.Count; m++)
			{
				Merit merit = merits[m];
				for (int mr = 0; mr < rowsPerMerit; ++mr)
				{
					uiElement.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / rowAmount));
					if (mr == 0)
					{
						Label meritNameLabel = new Label
						{
							Anchor = AnchorStyles.None,
							AutoSize = true,
							Name = "meritNameLabel" + merit.name,
							Size = new Size(nameLabelWidth, 20),
							TabIndex = 0,
							Text = merit.name
						};

						if (mutable)
						{
							ContextMenuStrip contextMenu = new ContextMenuStrip();
							ToolStripItem addMeritItem = contextMenu.Items.Add("Remove " + singularName);
							addMeritItem.Click += new EventHandler(RemoveMerit);
							meritNameLabel.ContextMenuStrip = contextMenu;
						}

						uiElement.Controls.Add(meritNameLabel, 0, m * rowsPerMerit);
					}
				}

				//pips
				merit.pips.Clear();
				for (int p = 0; p < maxValue; p++)
				{
					RadioButton pip = new RadioButton
					{
						Anchor = AnchorStyles.None,
						AutoSize = true,
						Size = new Size(22, 22),
						Dock = DockStyle.Fill,
						TabIndex = 0,
						UseVisualStyleBackColor = true,
						Checked = p < merit.currentValue
					};
					pip.Click += new EventHandler(PipClicked);
					pip.AutoCheck = false;
					merit.pips.Add(pip);

					//insert pip in table
					int rowIndexOfSkill = Convert.ToInt32(Math.Floor(p / Convert.ToSingle(maxDotsPerRow)));
					int column = (p - (rowIndexOfSkill * maxDotsPerRow)) + 1;
					int row = m * rowsPerMerit + rowIndexOfSkill;
					uiElement.Controls.Add(pip, column, row);
				}
			}

			uiElement.Size = new Size(componentWidth, (23 * Math.Max(3, rowAmount)));
			ResizeParentColumn();

			OnComponentChanged();
		}

		void PipClicked(object sender, EventArgs e)
		{
			foreach (Merit merit in merits)
			{
				for (int i = 0; i < merit.pips.Count; i++)
				{
					if (sender == merit.pips[i])
					{
						if (merit.currentValue == i + 1)
						{
							//when clicking the last pip, reduce value by 1
							merit.currentValue = i;
						}
						else
						{
							merit.currentValue = i + 1;
						}
					}
				}
			}
			OnMeritValuesChanged();
		}

		void OnMeritValuesChanged()
		{
			foreach (Merit merit in merits)
			{
				for (int i = 0; i < merit.pips.Count; i++)
				{
					merit.pips[i].Checked = i < merit.currentValue;
				}
			}

			OnComponentChanged();
		}
	}
}
