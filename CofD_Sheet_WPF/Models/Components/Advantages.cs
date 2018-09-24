using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CofD_Sheet_WPF.Models.Components
{
	[Serializable]
	public class Advantages : IComponent
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
			public string name = "Advantage";

			[XmlAttribute]
			public string value = "";
		}

		[XmlArray]
		public List<Advantage> advantages = new List<Advantage>();
		
		public Advantages() : base("AdvantagesComponent", ColumnId.Undefined)
		{ }

		public Advantages(string componentName, List<string> advantageNames, ColumnId _column) : base(componentName, _column)
		{
			foreach (string advantageName in advantageNames)
			{
				advantages.Add(new Advantage(advantageName));
			}
		}
	}
}
