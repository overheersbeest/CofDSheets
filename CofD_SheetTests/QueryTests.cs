using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CofD_Sheet;

namespace CofD_SheetTests
{
	[TestClass]
	public class QueryTests
	{
		[TestMethod]
		public void QueryHealthComponent()
		{
			CofD_Sheet.Sheet sheet = new Sheet();

			CofD_Sheet.Sheet_Components.HealthComponent component = new CofD_Sheet.Sheet_Components.HealthComponent("Health", CofD_Sheet.Sheet_Components.ColumnId.Undefined);
			int testValue = 10;
			component.MaxValue.CurrentValue = testValue;
			sheet.components.Add(component);

			Assert.AreEqual(testValue, sheet.QueryInt("Health.MaxValue"));
			Assert.AreEqual(testValue, sheet.QueryInt("health.maxvalue"));//incorrect case in query
		}
	}
}
