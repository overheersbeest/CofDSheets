using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CofD_Sheet_WPF
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private void Application_Startup(object sender, StartupEventArgs e)
		{
			MainWindow wnd = new MainWindow();
			if (e.Args.Length > 0)
			{
				string fileName = e.Args[0];
				if (File.Exists(fileName))
				{
					//wnd.DataContext.loadSheet(fileName);
				}
			}
			wnd.Show();
		}
	}
}
