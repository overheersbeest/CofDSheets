using CofD_Sheet.Modifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CofD_Sheet.Modifyables
{
	[Serializable]
	public enum IntModificationType
	{
		[XmlEnum(Name = "Delta")]
		Delta,
		[XmlEnum(Name = "Absolute")]
		Absolute
	}

	[Serializable]
	public class ModifiableInt
	{
		public ModifiableInt()
		{ }
		public ModifiableInt(int _defaultValue)
		{
			defaultValue = _defaultValue;
		}

		[XmlAttribute]
		public int defaultValue = 1;

		[XmlIgnore]
		private int modifier = 0;

		[XmlIgnore]
		public int maxValue = int.MaxValue;

		[XmlIgnore]
		private IntModificationType modType = IntModificationType.Delta;

		[XmlIgnore]
		public int CurrentValue
		{
			get
			{
				switch (modType)
				{
					default:
						throw new Exception("unhandled IntModificationType");
					case IntModificationType.Delta:
						return defaultValue + modifier;
					case IntModificationType.Absolute:
						return modifier;
				}
			}
			set
			{
				switch (modType)
				{
					default:
						throw new Exception("unhandled IntModificationType");
					case IntModificationType.Delta:
						//we can modify the original value with the difference to the current value
						int current = defaultValue + modifier;
						int setDelta = value - current;
						defaultValue += setDelta;
						defaultValue = Math.Max(defaultValue, 0);
						defaultValue = Math.Min(defaultValue, maxValue);
						break;
					case IntModificationType.Absolute:
						//we can't modify the original value, so we'll set a new absolute value instead
						modifier = value;
						break;
				}
			}
		}

		public void Reset()
		{
			modifier = 0;
			modType = IntModificationType.Delta;
		}

		public void ApplyModification(IntModification mod, Sheet sheet)
		{
			switch (mod.modType)
			{
				default:
					throw new Exception("unhandled IntModificationType");
				case IntModificationType.Delta:
					if (modType == IntModificationType.Absolute)
					{
						throw new Exception("absolute modified int being modified again with delta value.");
					}
					modifier += mod.GetValue(sheet);
					break;
				case IntModificationType.Absolute:
					if (modType != IntModificationType.Delta
						&& modifier != 0)
					{
						throw new Exception("modified int being modified again with absolute value.");
					}
					modifier = mod.GetValue(sheet);
					break;
			}
			modType = mod.modType;
		}
	}
}
