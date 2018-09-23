using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;
using System.Xml.Serialization;

namespace CofD_Sheet_WPF.Models
{
	[Serializable]
	public class AspirationsComponent : ISheetComponent
	{
		[XmlAttribute]
		public int maxAspirations = 3;

		[XmlArray]
		public List<string> aspirations = new List<string>();
		
		public AspirationsComponent() : base("AspirationsComponent", -1)
		{ }

		public AspirationsComponent(string componentName, int amountAllowed, int componentColumnIndex) : base(componentName, componentColumnIndex)
		{
			maxAspirations = amountAllowed;
			aspirations = new List<string>(new string[maxAspirations]);
		}
		
		//void onValueChanged(object sender = null, EventArgs e = null)
		//{
		//	for (int i = 0; i < textBoxes.Count; i++)
		//	{
		//		aspirations[i] = textBoxes[i].Text;
		//	}
		//
		//	onComponentChanged();
		//}
	}
}
