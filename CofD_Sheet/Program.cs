using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

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
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Form1 form = new Form1();
			if (args != null && args.Length > 0)
			{
				string fileName = args[0];
				//MessageBox.Show(fileName, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
				if (File.Exists(fileName))
				{
					XmlDocument doc = new XmlDocument();
					doc.Load(fileName);
					form.sheet = new Sheet(doc);
					form.refreshSheet();
				}
			}

			Application.Run(form);
		}
	}
}
