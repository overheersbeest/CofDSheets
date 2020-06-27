using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CofD_Sheet
{
	[Serializable]
	class ModdableInt
	{
		[XmlIgnore]
		private int _currentValue = 1;

		[XmlAttribute]
		public int CurrentValue
		{
			get { return _currentValue; }
			set
			{
				_currentValue = value;
				modifiedValue = value;

				//re-apply modifications
				bool applied;

				foreach (ModificationSetComponent.IntModification appliedModification in appliedModifications)
				{
					switch (appliedModification.modType)
					{
						case ModificationSetComponent.IntModificationType.Delta:
							modifiedValue += appliedModification.value;
							break;
						case ModificationSetComponent.IntModificationType.Absolute:
							break;
					}
				}
			}
		}

		[XmlIgnore]
		public int modifiedValue = 1;

		[XmlIgnore]
		private List<ModificationSetComponent.IntModification> appliedModifications = new List<ModificationSetComponent.IntModification>();

	}
}
