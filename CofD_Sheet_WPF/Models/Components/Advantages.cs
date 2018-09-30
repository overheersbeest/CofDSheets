using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CofD_Sheet_WPF.Models.Components
{
	[Serializable]
	public class Advantages : BaseComponent
	{
		public class Advantage
		{
			public Advantage()
			{ }
			public Advantage(string _name)
			{
				this.name = _name;
			}
			public Advantage(string _name, string _value)
			{
				this.name = _name;
				this.value = _value;
			}

			[XmlAttribute]
			public string name { get; set; } = "Advantage";

			[XmlAttribute]
			public string value { get; set; } = "";
		}

		[XmlArray]
		public List<Advantage> advantages { get; set; } = new List<Advantage>();
		
		public Advantages() : base("AdvantagesComponent")
		{ }

		public Advantages(string componentName, List<string> advantageNames) : base(componentName)
		{
			foreach (string advantageName in advantageNames)
			{
				advantages.Add(new Advantage(advantageName));
			}
		}
	}
}
