using System.Collections.Generic;
using System.Xml.Serialization;

namespace CofD_Sheet.Modifications
{
	public class StringModification : Modification
	{
		public StringModification() : base()
		{ }
		public StringModification(List<string> _path, string _value) : base(_path)
		{
			this.value = _value;
		}

		[XmlAttribute]
		public string value = "";
	}
}
