using Microsoft.VisualStudio.TestTools.UnitTesting;
using CofD_Sheet;
using System.Collections.Generic;
using CofD_Sheet.Sheet_Components;
using CofD_Sheet.Modifications;

namespace CofD_SheetTests
{
	[TestClass]
	public class HealthComponentTests
	{
		[TestMethod]
		public void GetValues()
		{
			Sheet sheet = new Sheet();

			HealthComponent component = new HealthComponent("Health", ColumnId.Undefined);
			sheet.components.Add(component);

			int aggTestValue = 1;
			int lethalTestValue = 2;
			int bashTestValue = 3;

			component.MaxValue.CurrentValue = 10;
			component.aggrivated = aggTestValue;
			component.lethal = lethalTestValue;
			component.bashing = bashTestValue;
			component.OnModificationsComplete();

			Assert.AreEqual(aggTestValue, component.aggrivated);
			Assert.AreEqual(lethalTestValue, component.lethal);
			Assert.AreEqual(bashTestValue, component.bashing);
		}

		[TestMethod]
		public void GetValues_Rollover()
		{
			Sheet sheet = new Sheet();

			HealthComponent component = new HealthComponent("Health", ColumnId.Undefined);
			sheet.components.Add(component);

			int aggTestValue = 1;
			int lethalTestValue = 2;
			int bashTestValue = 3;

			component.MaxValue.CurrentValue = 10;
			component.aggrivated = aggTestValue;
			component.lethal = lethalTestValue;
			component.bashing = bashTestValue;
			component.ApplyModification(new IntModification(new List<string>() { "Health", "MaxValue" }, 5, "", IntModificationType.Absolute), sheet);
			component.OnModificationsComplete();

			Assert.AreEqual(component.aggrivated, 1);
			Assert.AreEqual(component.lethal, 3);
			Assert.AreEqual(component.bashing, 1);

			component.ApplyModification(new IntModification(new List<string>() { "Health", "MaxValue" }, -1, "", IntModificationType.Delta), sheet);
			component.OnModificationsComplete();

			Assert.AreEqual(component.aggrivated, 2);
			Assert.AreEqual(component.lethal, 2);
			Assert.AreEqual(component.bashing, 0);
		}
	}
}
