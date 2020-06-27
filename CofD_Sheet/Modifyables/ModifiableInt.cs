﻿using System;
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
		[XmlAttribute]
		public int defaultValue = 1;

		[XmlIgnore]
		private int modifier = 0;

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

		public void ApplyModification(IntModificationType newType, int newModifier)
		{
			switch (newType)
			{
				default:
					throw new Exception("unhandled IntModificationType");
				case IntModificationType.Delta:
					if (modType == IntModificationType.Absolute)
					{
						throw new Exception("absolute modified int being modified again with delta value.");
					}
					modifier = newModifier;
					break;
				case IntModificationType.Absolute:
					if (modType != IntModificationType.Delta
						&& modifier != 0)
					{
						throw new Exception("modified int being modified again with absolute value.");
					}
					modifier = newModifier;
					break;
			}
			modType = newType;
		}
	}
}
