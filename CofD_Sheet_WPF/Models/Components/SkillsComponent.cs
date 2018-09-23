using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;
using System.Xml.Serialization;

namespace CofD_Sheet_WPF.Models
{
	[Serializable]
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
			public int currentValue = 0;

			[XmlElement]
			public List<String> specialties = new List<String>();
		}

		[XmlIgnore]
		const int maxDotsPerRow = 10;

		[XmlIgnore]
		const int nameLabelWidth = 180;

		[XmlAttribute]
		public int maxValue = 5;

		[XmlArray]
		public List<Skill> skills = new List<Skill>();

		public SkillsComponent() : base("SkillsComponent", -1)
		{ }

		public SkillsComponent(string componentName, List<string> skillNames, int componentColumnIndex) : base(componentName, componentColumnIndex)
		{
			for (int i = 0; i < skillNames.Count; i++)
			{
				skills.Add(new Skill(skillNames[i]));
			}
		}
		
		//void addSpecialty(object sender, EventArgs e)
		//{
		//	ContextMenuStrip owner = (sender as ToolStripItem).Owner as ContextMenuStrip;
		//	Label skillLabel = owner.SourceControl as Label;
		//	Skill skill = skills.Find(x => skillLabel.Text.StartsWith(x.name));
		//	if (skill == null)
		//	{
		//		return;
		//	}
		//	Form prompt = new Form();
		//	prompt.Width = 325;
		//	prompt.Height = 100;
		//	prompt.Text = "Add Specialty";
		//	TextBox inputBox = new TextBox() { Left = 5, Top = 5, Width = 300 };
		//	Button confirmation = new Button() { Text = "Add", Left = 205, Width = 100, Top = 30 };
		//	confirmation.Click += (sender2, e2) => { prompt.Close(); };
		//	prompt.Controls.Add(confirmation);
		//	prompt.Controls.Add(inputBox);
		//	prompt.ShowDialog();
		//
		//	string newSpecialty = inputBox.Text;
		//	if (!skill.specialties.Contains(newSpecialty))
		//	{
		//		skill.specialties.Add(newSpecialty);
		//	}
		//
		//	onSpecialtiesChanged(skillLabel, skill);
		//}
		//
		//void removeSpecialty(object sender, EventArgs e)
		//{
		//	ContextMenuStrip owner = (sender as ToolStripItem).Owner as ContextMenuStrip;
		//	Label skillLabel = owner.SourceControl as Label;
		//	Skill skill = skills.Find(x => skillLabel.Text.StartsWith(x.name));
		//	if (skill == null)
		//	{
		//		return;
		//	}
		//	Form prompt = new Form();
		//	prompt.Width = 325;
		//	prompt.Height = 100;
		//	prompt.Text = "Remove Specialty";
		//	ComboBox inputBox = new ComboBox() { Left = 5, Top = 5, Width = 300 };
		//	foreach (string specialty in skill.specialties)
		//	{
		//		inputBox.Items.Add(specialty);
		//	}
		//	Button confirmation = new Button() { Text = "Remove", Left = 205, Width = 100, Top = 30 };
		//	confirmation.Click += (sender2, e2) => { prompt.Close(); };
		//	prompt.Controls.Add(confirmation);
		//	prompt.Controls.Add(inputBox);
		//	prompt.ShowDialog();
		//
		//	string specialtytoRemove = inputBox.SelectedItem as string;
		//	if (skill.specialties.Contains(specialtytoRemove))
		//	{
		//		skill.specialties.Remove(specialtytoRemove);
		//	}
		//
		//	onSpecialtiesChanged(skillLabel, skill);
		//}
		//
		//void onSpecialtiesChanged(Label label, Skill skill)
		//{
		//	if (skill.specialties.Count > 0)
		//	{
		//		label.Text = skill.name + " (" + string.Join(", ", skill.specialties) + ")";
		//	}
		//	else
		//	{
		//		label.Text = skill.name;
		//	}
		//
		//	ContextMenuStrip contextMenu = new ContextMenuStrip();
		//	ToolStripItem addSpecialtyItem = contextMenu.Items.Add("Add Specialty");
		//	addSpecialtyItem.Click += new EventHandler(addSpecialty);
		//	if (skill.specialties.Count > 0)
		//	{
		//		ToolStripItem removeSpecialtyItem = contextMenu.Items.Add("Remove Specialty");
		//		removeSpecialtyItem.Click += new EventHandler(removeSpecialty);
		//	}
		//
		//	label.ContextMenuStrip = contextMenu;
		//}
		//
		//void valueChanged(object sender, EventArgs e)
		//{
		//	foreach (Skill skill in skills)
		//	{
		//		for (int i = 0; i < skill.pips.Count; i++)
		//		{
		//			if (sender == skill.pips[i])
		//			{
		//				if (skill.currentValue == i + 1)
		//				{
		//					//when clicking the last pip, reduce value by 1
		//					skill.currentValue = i;
		//				}
		//				else
		//				{
		//					skill.currentValue = i + 1;
		//				}
		//			}
		//		}
		//	}
		//	onValueChanged();
		//}
		//
		//void onValueChanged()
		//{
		//	foreach (Skill skill in skills)
		//	{
		//		for (int i = 0; i < skill.pips.Count; i++)
		//		{
		//			skill.pips[i].Checked = i < skill.currentValue;
		//		}
		//	}
		//
		//	onComponentChanged();
		//}
	}
}
