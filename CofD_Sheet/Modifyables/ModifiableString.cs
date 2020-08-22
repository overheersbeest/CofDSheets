using CofD_Sheet.Modifications;
using System;
using System.Xml.Serialization;

namespace CofD_Sheet.Modifyables
{
	[Serializable]
	public class ModifiableString
	{
		public ModifiableString()
		{ }
		public ModifiableString(string _defaultValue)
		{
			defaultValue = _defaultValue;
		}

		[XmlAttribute]
		public string defaultValue = "";

		[XmlIgnore]
		private string modifiedValue = "";

		[XmlIgnore]
		private int defaultStringStartIndex = 0;

		[XmlIgnore]
		private StringModificationState modType = StringModificationState.None;

		[XmlIgnore]
		public string CurrentValue
		{
			get
			{
				return modifiedValue;
			}
			set
			{
				switch (modType)
				{
					default:
						throw new Exception("unhandled StringModificationType");
					case StringModificationState.None:
						defaultValue = value;
						modifiedValue = value;
						break;
					case StringModificationState.Additive:
						//find the original string value and replace it
						int originalLength = defaultValue.Length;
						defaultValue = value;
						modifiedValue = modifiedValue.Substring(0, defaultStringStartIndex) + value + modifiedValue.Substring(defaultStringStartIndex + originalLength);
						break;
					case StringModificationState.Replace:
						//we'll modify the original value, but the modified value won't be updated as it is being overwritten by a modification
						defaultValue = value;
						break;
				}
			}
		}

		public void Reset()
		{
			modifiedValue = defaultValue;
			modType = StringModificationState.None;
		}

		public void ApplyModification(StringModification mod, Sheet sheet)
		{
			switch (mod.modType)
			{
				default:
					throw new Exception("unhandled StringModificationType");
				case StringModificationType.AddElement:
					throw new Exception("AddElement StringModificationType applied to string");
				case StringModificationType.Prefix:
					string prefix = mod.GetValue(sheet);
					defaultStringStartIndex += prefix.Length;
					modifiedValue = prefix + modifiedValue;

					if (modType != StringModificationState.Replace)
					{
						modType = StringModificationState.Additive;
					}
					break;
				case StringModificationType.Suffix:
					modifiedValue += mod.GetValue(sheet);

					if (modType != StringModificationState.Replace)
					{
						modType = StringModificationState.Additive;
					}
					break;
				case StringModificationType.Replace:
					modifiedValue = mod.GetValue(sheet);
					modType = StringModificationState.Replace;
					break;
			}
		}
	}
}
