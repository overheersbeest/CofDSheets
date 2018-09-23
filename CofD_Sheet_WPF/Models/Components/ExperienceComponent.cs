using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;
using System.Xml.Serialization;

namespace CofD_Sheet_WPF.Models
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
		
		public ExperienceComponent() : base("ExperienceComponent", -1)
		{ }

		public ExperienceComponent(string majorName, string minorName, int componentColumnIndex) : base(majorName, componentColumnIndex)
		{
			beatName = minorName;
		}
		
		//void onValueChanged(object sender = null, EventArgs e = null)
		//{
		//	try
		//	{
		//		experience = Convert.ToInt32(experienceCounter.Text);
		//	}
		//	catch (Exception)
		//	{}
		//
		//	beats = 0;
		//	for (int i = beatBoxes.Count - 1; i >= 0; i--)
		//	{
		//		if (beatBoxes[i].Checked)
		//		{
		//			beats++;
		//		}
		//	}
		//	if (beats >= maxBeats)
		//	{
		//		beats = 0;
		//		experience++;
		//	}
		//	for (int i = beatBoxes.Count - 1; i >= 0; i--)
		//	{
		//		beatBoxes[i].Checked = i < beats;
		//	}
		//	experienceCounter.TextChanged -= onValueChanged;
		//	experienceCounter.Text = experience.ToString();
		//	experienceCounter.TextChanged += onValueChanged;
		//
		//	onComponentChanged();
		//}
	}
}
