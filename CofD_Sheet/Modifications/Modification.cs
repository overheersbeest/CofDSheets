using System.Collections.Generic;
using System.Xml.Serialization;

namespace CofD_Sheet.Modifications
{
	[XmlInclude(typeof(IntModification))]
	[XmlInclude(typeof(StringModification))]
	public class Modification
	{
		public Modification()
		{ }
		public Modification(List<string> _path)
		{
			path = _path;
		}

		[XmlArray]
		public List<string> path = new List<string>();
	}
}
