using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace CofD_Sheet.Sheet_Components
{
	public class SkillsComponent : ISheetComponent
	{
		public class Skill
		{
			public Skill()
			{}
			public Skill(string _name)
			{
				this.name = _name;
			}
			public Skill(string _name, int _value)
			{
				this.name = _name;
				this.currentValue = _value;
			}
			
			[XmlAttribute]
			public string name = "Skill";

			[XmlAttribute]
			public int currentValue = 1;

			[XmlElement]
			public List<String> specialties = new List<String>();

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
		public List<Skill> skills = new List<Skill>();

		public SkillsComponent() : base("SkillsComponent")
		{
			type = ISheetComponent.Type.Skills;
		}

		public SkillsComponent(string componentName, List<string> skillNames) : base(componentName)
		{
			for (int i = 0; i < skillNames.Count; i++)
			{
				skills.Add(new Skill(skillNames[i]));
			}
			type = ISheetComponent.Type.Skills;
		}

		public SkillsComponent(XmlNode node) : base(node.Name)
		{
			maxValue = Convert.ToInt32(node.Attributes["MaxValue"].Value);
			for (int i = 0; i < node.ChildNodes.Count; i++)
			{
				skills.Add(new Skill(node.ChildNodes[i].Attributes.GetNamedItem("Name").Value, Convert.ToInt32(node.ChildNodes[i].Attributes.GetNamedItem("CurrentValue").Value)));
			}
			type = ISheetComponent.Type.Skills;
		}

		override protected void fillElement(ref XmlElement node, XmlDocument doc)
		{
			node.SetAttribute("MaxValue", maxValue.ToString());
			foreach (Skill attribute in skills)
			{
				XmlElement skillElement = doc.CreateElement("Skill");
				skillElement.SetAttribute("Name", attribute.name);
				skillElement.SetAttribute("CurrentValue", attribute.currentValue.ToString());
				node.AppendChild(skillElement);
			}
		}

		override public Control getUIElement()
		{
			int rowsPerSkill = Convert.ToInt32(Math.Ceiling(maxValue / Convert.ToSingle(maxDotsPerRow)));
			int rowAmount = skills.Count * rowsPerSkill;
			int columnAmount = 1 + Math.Min(maxValue, maxDotsPerRow);

			TableLayoutPanel uiElement = new TableLayoutPanel();
			uiElement.RowCount = rowAmount;
			uiElement.ColumnCount = columnAmount;
			uiElement.Dock = DockStyle.Fill;
			uiElement.Name = "tableLayout" + name + "values";
			uiElement.Size = new Size(292, 20 * rowAmount);
			uiElement.TabIndex = 0;

			//column styles
			uiElement.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, nameLabelWidth));
			for (int c = 1; c < columnAmount; c++)
			{
				uiElement.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / (columnAmount - 1)));
			}

			for (int a = 0; a < skills.Count; a++)
			{
				Skill skill = skills[a];
				for (int ar = 0; ar < rowsPerSkill; ++ar)
				{
					uiElement.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / rowAmount));
					if (ar == 0)
					{
						Label skillNameLabel = new Label();
						skillNameLabel.Anchor = AnchorStyles.None;
						skillNameLabel.AutoSize = true;
						skillNameLabel.Name = "skillNameLabel" + skill.name;
						skillNameLabel.Size = new Size(nameLabelWidth, 20);
						skillNameLabel.TabIndex = 0;
						skillNameLabel.Text = skill.name;
						uiElement.Controls.Add(skillNameLabel, 0, a * rowsPerSkill);
					}
				}

				//pips
				skill.pips.Clear();
				for (int p = 0; p < maxValue; p++)
				{
					RadioButton pip = new RadioButton();
					pip.Anchor = System.Windows.Forms.AnchorStyles.None;
					pip.AutoSize = true;
					pip.Name = "pip" + name + "_" + 0;
					pip.Size = new System.Drawing.Size(20, 20);
					pip.TabIndex = 0;
					pip.UseVisualStyleBackColor = true;
					pip.Checked = 0 < skill.currentValue;
					pip.Click += new EventHandler(valueChanged);
					pip.AutoCheck = false;
					skill.pips.Add(pip);

					//insert pip in table
					int rowIndexOfSkill = Convert.ToInt32(Math.Floor(p / Convert.ToSingle(maxDotsPerRow)));
					int column = (p - (rowIndexOfSkill * maxDotsPerRow)) + 1;
					int row = a * rowsPerSkill + rowIndexOfSkill;
					uiElement.Controls.Add(pip, column, row);
				}
			}
			onValueChanged();

			ContextMenuStrip contextMenu = new ContextMenuStrip();
			ToolStripItem addSpecialtyItem = contextMenu.Items.Add("Add");
			addSpecialtyItem.Click += new EventHandler(addSpecialty);
			ToolStripItem removeSpecialtyItem = contextMenu.Items.Add("Remove");
			removeSpecialtyItem.Click += new EventHandler(removeSpecialty);

			uiElement.ContextMenuStrip = contextMenu;

			return uiElement;
		}

		void addSpecialty(object sender, EventArgs e)
		{
			ToolStripItem addSpecialtyItem = sender as ToolStripItem;
			// your code here
		}

		void removeSpecialty(object sender, EventArgs e)
		{
			ToolStripItem removeSpecialtyItem = sender as ToolStripItem;
			// your code here
		}

		void valueChanged(object sender, EventArgs e)
		{
			foreach (Skill skill in skills)
			{
				for (int i = 0; i < skill.pips.Count; i++)
				{
					if (sender == skill.pips[i])
					{
						if (skill.currentValue == i + 1)
						{
							//when clicking the last pip, reduce value by 1
							skill.currentValue = i;
						}
						else
						{
							skill.currentValue = i + 1;
						}
					}
				}
			}
			onValueChanged();
		}

		void onValueChanged()
		{
			foreach (Skill skill in skills)
			{
				for (int i = 0; i < skill.pips.Count; i++)
				{
					skill.pips[i].Checked = i < skill.currentValue;
				}
			}

			onComponentChanged();
		}
	}
}
