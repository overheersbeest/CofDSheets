using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace CofD_Sheet.Sheet_Components
{
	class ExperienceComponent : ISheetComponent
	{
		const int maxPerRow = 10;
		const float separatorProportion = 2F;

		string beatName;
		int maxBeats = 5;
		int beats = 0;
		int experience = 0;

		List<CheckBox> beatBoxes = new List<CheckBox>();
		TextBox experienceCounter = null;

		public ExperienceComponent(string majorName, string minorName) : base(majorName)
		{
			beatName = minorName;
			type = ISheetComponent.Type.Experience;
		}

		public ExperienceComponent(XmlNode node) : base(node.Name)
		{
			maxBeats = Convert.ToInt32(node.Attributes["MaxBeats"].Value);
			beats = Convert.ToInt32(node.Attributes["Beats"].Value);
			experience = Convert.ToInt32(node.Attributes["Experience"].Value);
			type = ISheetComponent.Type.Experience;
		}

		override protected void fillElement(ref XmlElement node, XmlDocument doc)
		{
			node.SetAttribute("MaxBeats", maxBeats.ToString());
			node.SetAttribute("Beats", beats.ToString());
			node.SetAttribute("Experience", experience.ToString());
		}

		override public Control getUIElement()
		{
			int rowAmount = Convert.ToInt32(Math.Ceiling(maxBeats / Convert.ToSingle(maxPerRow)));
			int checkBoxRows = Math.Min(maxBeats, maxPerRow);
			int columnSeparatorCount = (checkBoxRows - 1) / 5;
			int columnAmount = checkBoxRows + columnSeparatorCount;

			TableLayoutPanel uiElement = new TableLayoutPanel();
			uiElement.RowCount = 1;
			uiElement.ColumnCount = 2;
			uiElement.Dock = DockStyle.Fill;
			uiElement.Location = new Point(3, 3);
			uiElement.Name = "tableLayout" + name + "values";
			uiElement.Size = new Size(292, 60);
			uiElement.TabIndex = 0;

			//beats element
			uiElement.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
			TableLayoutPanel beatElement = new TableLayoutPanel();
			beatElement.RowCount = rowAmount;
			beatElement.ColumnCount = columnAmount;
			beatElement.Dock = DockStyle.Fill;
			beatElement.Location = new Point(3, 3);
			beatElement.Name = "tableLayout" + name + "beats";
			beatElement.Size = new Size(292, 60);
			beatElement.TabIndex = 0;
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
						CheckBox checkBox = new CheckBox();
						checkBox.Anchor = System.Windows.Forms.AnchorStyles.None;
						checkBox.AutoSize = true;
						checkBox.Location = new System.Drawing.Point(22, 14);
						checkBox.Name = "beatbox" + name + "_" + checkBoxNr;
						checkBox.Size = new System.Drawing.Size(15, 14);
						checkBox.TabIndex = 0;
						checkBox.UseVisualStyleBackColor = true;
						checkBox.Checked = checkBoxNr < beats;
						checkBox.Click += onValueChanged;
						beatBoxes.Add(checkBox);
						beatElement.Controls.Add(checkBox, c, r);
					}
				}
			}

			//experience element
			uiElement.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
			experienceCounter = new TextBox();
			experienceCounter.Anchor = System.Windows.Forms.AnchorStyles.None;
			experienceCounter.AutoSize = true;
			experienceCounter.Location = new System.Drawing.Point(22, 14);
			experienceCounter.Name = "slot" + name + "_exp";
			experienceCounter.Size = new System.Drawing.Size(100, 14);
			experienceCounter.TabIndex = 0;
			experienceCounter.Text = experience.ToString();
			experienceCounter.TextChanged += onValueChanged;
			uiElement.Controls.Add(experienceCounter, 1, 0);
			
			onValueChanged();
			return uiElement;
		}
		
		void onValueChanged(object sender = null, EventArgs e = null)
		{
			try
			{
				experience = Convert.ToInt32(experienceCounter.Text);
			}
			catch (Exception)
			{}

			beats = 0;
			for (int i = beatBoxes.Count - 1; i >= 0; i--)
			{
				if (beatBoxes[i].Checked)
				{
					beats++;
				}
			}
			if (beats >= maxBeats)
			{
				beats = 0;
				experience++;
			}
			for (int i = beatBoxes.Count - 1; i >= 0; i--)
			{
				beatBoxes[i].Checked = i < beats;
			}
			experienceCounter.TextChanged -= onValueChanged;
			experienceCounter.Text = experience.ToString();
			experienceCounter.TextChanged += onValueChanged;

			onComponentChanged();
		}
	}
}
