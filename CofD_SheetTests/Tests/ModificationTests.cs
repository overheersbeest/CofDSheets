using Microsoft.VisualStudio.TestTools.UnitTesting;
using CofD_Sheet;
using System.Collections.Generic;
using CofD_Sheet.Sheet_Components;
using CofD_Sheet.Modifyables;
using CofD_Sheet.Modifications;

namespace CofD_SheetTests.Tests
{
	[TestClass]
	public class ModificationTests
	{
		[TestMethod]
		public void ModifyInt_Simple()
		{
			Sheet sheet = new Sheet();

			//no changes
			int setValue1 = 10;
			ModifiableInt intValue = new ModifiableInt(setValue1);
			Assert.AreEqual(intValue.defaultValue, setValue1);
			Assert.AreEqual(intValue.CurrentValue, setValue1);

			//delta change
			int deltaValue1 = 2;
			intValue.ApplyModification(new IntModification(new List<string>(), deltaValue1, "", IntModificationType.Delta), sheet);
			Assert.AreEqual(intValue.CurrentValue, setValue1 + deltaValue1);

			//set value after delta change
			int setValue2 = 20;
			intValue.CurrentValue = setValue2;
			Assert.AreEqual(intValue.defaultValue, setValue2 - deltaValue1);
			Assert.AreEqual(intValue.CurrentValue, setValue2);

			//absolute change
			int absoluteValue = 15;
			intValue.ApplyModification(new IntModification(new List<string>(), absoluteValue, "", IntModificationType.Absolute), sheet);
			Assert.AreEqual(intValue.CurrentValue, absoluteValue);

			//set value after absolute change
			int setValue3 = 30;
			intValue.CurrentValue = setValue3;
			Assert.AreEqual(intValue.defaultValue, setValue3);
			Assert.AreEqual(intValue.CurrentValue, absoluteValue);

			//delta change on top of absolute change
			int deltaValue2 = -1;
			intValue.ApplyModification(new IntModification(new List<string>(), deltaValue2, "", IntModificationType.Delta), sheet);
			Assert.AreEqual(intValue.CurrentValue, absoluteValue + deltaValue2);

			//set value after absolute change
			int setValue4 = 40;
			intValue.CurrentValue = setValue4;
			Assert.AreEqual(intValue.defaultValue, setValue4);
			Assert.AreEqual(intValue.CurrentValue, absoluteValue + deltaValue2);

		}
	}
}
