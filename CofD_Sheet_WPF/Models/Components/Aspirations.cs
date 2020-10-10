using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CofD_Sheet_WPF.Models.Components
{
	[Serializable]
	public class Aspirations : BaseComponent
	{
		[XmlAttribute]
		public int maxAspirations { get; set; } = 3;

		[XmlArray]
		public List<string> aspirations { get; set; } = new List<string>();
		
		public Aspirations() : base("AspirationsComponent")
		{ }

		public Aspirations(string componentName, int amountAllowed) : base(componentName)
		{
			maxAspirations = amountAllowed;
			aspirations = new List<string>(new string[maxAspirations]);
		}
	}
}
