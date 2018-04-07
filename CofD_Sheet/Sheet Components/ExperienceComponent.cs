using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace CofD_Sheet.Sheet_Components
{
	class ExperienceComponent : ISheetComponent
	{
		string beatName;
		int maxBeats = 5;
		int beats = 0;
		int experience = 0;

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
		}

		override protected void fillElement(ref XmlElement node, XmlDocument doc)
		{
			node.SetAttribute("MaxBeats", maxBeats.ToString());
			node.SetAttribute("Beats", beats.ToString());
			node.SetAttribute("Experience", experience.ToString());
		}

		override public Control getUIElement()
		{
			return new TableLayoutPanel();
		}
	}
}
