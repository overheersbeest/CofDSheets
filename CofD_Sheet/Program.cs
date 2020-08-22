using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace CofD_Sheet
{
	static class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Form1 form = new Form1();
			if (args != null && args.Length > 0)
			{
				string fileName = args[0];
				if (File.Exists(fileName))
				{
					form.LoadSheet(fileName);
				}
			}

			Application.Run(form);
		}

		public static T DeepClone<T>(this T obj)
		{
			using (var ms = new MemoryStream())
			{
				var formatter = new BinaryFormatter();
				formatter.Serialize(ms, obj);
				ms.Position = 0;

				return (T)formatter.Deserialize(ms);
			}
		}
	}
}
