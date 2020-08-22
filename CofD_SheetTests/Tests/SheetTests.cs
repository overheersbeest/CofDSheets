using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Serialization;
using CofD_Sheet;
using System.Collections.Generic;
using CofD_Sheet.Sheet_Components;
using System.IO;

namespace CofD_SheetTests.Tests
{
	[TestClass]
	public class SheetTests
	{
		private void SaveLoadSheet(Sheet sheetIn, out Sheet sheetOut)
		{
			string path = "test.cofds";
			XmlSerializer serializer = new XmlSerializer(typeof(Sheet));

			TextWriter writer = new StreamWriter(path);
			serializer.Serialize(writer, sheetIn);
			writer.Close();

			StreamReader reader = new StreamReader(path);
			sheetOut = (Sheet)serializer.Deserialize(reader);
			foreach (ISheetComponent component in sheetOut.components)
			{
				component.Init(sheetOut);
			}
			reader.Close();
		}

		[TestMethod]
		public void ConstructSaveLoad()
		{
			Sheet sheet = new Sheet();
			Sheet loadedSheet;
			SaveLoadSheet(sheet, out loadedSheet);

			foreach (SheetType sheetType in SheetType.GetValues(typeof(SheetType)))
			{
				sheet = new Sheet(sheetType);
				SaveLoadSheet(sheet, out loadedSheet);
			}
		}
	}
}
