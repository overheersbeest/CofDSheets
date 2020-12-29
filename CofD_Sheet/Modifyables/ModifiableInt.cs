using CofD_Sheet.Modifications;
using System;
using System.Xml.Serialization;

namespace CofD_Sheet.Modifyables
{
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
		public int minValue = int.MinValue;

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
						defaultValue = Math.Max(defaultValue, minValue);
						defaultValue = Math.Min(defaultValue, maxValue);
						break;
					case IntModificationType.Absolute:
						//we'll modify the original value, but the modified value won't be updated as it is being overwritten by a modification
						defaultValue = value;
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
					modifier += mod.GetValue(sheet);
					break;
				case IntModificationType.Absolute:
					modifier = mod.GetValue(sheet);
					break;
			}
			if (modType != IntModificationType.Absolute)
			{
				modType = mod.modType;
			}
		}
	}
}
