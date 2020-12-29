using Microsoft.VisualStudio.TestTools.UnitTesting;
using CofD_Sheet;
using System.Collections.Generic;
using CofD_Sheet.Sheet_Components;

namespace CofD_SheetTests
{
	[TestClass]
	public class QueryTests
	{
		//health component
		[TestMethod]
		public void QueryHealthComponent_Int()
		{
			Sheet sheet = new Sheet();

			HealthComponent component = new HealthComponent("Health", ColumnId.Undefined, sheet);
			int testValue = 10;
			component.MaxValue.CurrentValue = testValue;
			sheet.components.Add(component);

			Assert.AreEqual(testValue, sheet.QueryInt("Health.MaxValue"));
			Assert.AreEqual(testValue, sheet.QueryInt("health.maxvalue"));//incorrect case in query
		}

		//advantages component
		[TestMethod]
		public void QueryAdvantagesComponent_Int()
		{
			Sheet sheet = new Sheet();

			int testValue1 = 1;
			int testValue2 = 2;
			int testValue3 = 3;
			AdvantagesComponent component = new AdvantagesComponent("Advantages", new List<AdvantagesComponent.Advantage>() { new AdvantagesComponent.NumericAdvantage("Numeric", testValue1), new AdvantagesComponent.ArmorAdvantage("Armor", testValue2, testValue3) }, ColumnId.Undefined, sheet);
			sheet.components.Add(component);

			Assert.AreEqual(testValue1, sheet.QueryInt("Advantages.Numeric"));
			Assert.AreEqual(testValue1, sheet.QueryInt("Advantages.NUMERIC"));//incorrect case in query

			Assert.AreEqual(testValue2, sheet.QueryInt("Advantages.Armor.General"));
			Assert.AreEqual(testValue2, sheet.QueryInt("Advantages.Armor.GENERAL"));//incorrect case in query

			Assert.AreEqual(testValue3, sheet.QueryInt("Advantages.Armor.Ballistic"));
			Assert.AreEqual(testValue3, sheet.QueryInt("Advantages.Armor.BALLISTIC"));//incorrect case in query
		}

		[TestMethod]
		public void QueryAdvantagesComponent_String()
		{
			Sheet sheet = new Sheet();

			string testValue = "testValue";
			AdvantagesComponent component = new AdvantagesComponent("Advantages", new List<AdvantagesComponent.Advantage>() { new AdvantagesComponent.StringAdvantage("String", testValue) }, ColumnId.Undefined, sheet);
			sheet.components.Add(component);

			Assert.AreEqual(testValue, sheet.QueryString("Advantages.String"));
			Assert.AreEqual(testValue, sheet.QueryString("Advantages.STRING"));//incorrect case in query
		}
	}
}
