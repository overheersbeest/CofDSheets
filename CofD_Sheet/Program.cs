using CofD_Sheet.Sheet_Components;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace CofD_Sheet
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			//Sheet sheet = new Sheet(SheetType.Mortal);
			//Serialize<Sheet>(sheet);

			Sheet sheet2 = Deserialize<Sheet>();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Form1 form = new Form1();
			if (args != null && args.Length > 0)
			{
				string fileName = args[0];
				if (File.Exists(fileName))
				{
					XmlDocument doc = new XmlDocument();
					doc.Load(fileName);
					form.sheet = new Sheet(doc);
					form.assosiatedFile = fileName;
					form.refreshSheet();
				}
			}

			Application.Run(form);
		}

		static public void Serialize<T>(Sheet details)
		{
			try
			{
				XmlSerializer serializer = new XmlSerializer(typeof(T));
				using (TextWriter writer = new StreamWriter(@"C:\Users\Overheersbeest\Google Drive\WoD\Chronicles\Apocalypse\Player Stats\Xml.xml"))
				{
					serializer.Serialize(writer, details);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}

		static public T Deserialize<T>()
		{
			try
			{
				
				XmlSerializer serializer = new XmlSerializer(typeof(T));
				StreamReader reader = new StreamReader(@"C:\Users\Overheersbeest\Google Drive\WoD\Chronicles\Apocalypse\Player Stats\TestMage.cofds");
				T retVal = (T)serializer.Deserialize(reader);
				reader.Close();
				return retVal;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				XmlSerializer serializer = new XmlSerializer(typeof(T));
				StreamReader reader = new StreamReader(@"C:\Users\Overheersbeest\Google Drive\WoD\Chronicles\Apocalypse\Player Stats\Xml.xml");
				reader.ReadToEnd();
				T retVal = (T)serializer.Deserialize(reader);
				reader.Close();
				return retVal;
			}
		}
	}
}
