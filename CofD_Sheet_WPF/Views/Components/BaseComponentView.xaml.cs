using CofD_Sheet_WPF.Models.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CofD_Sheet_WPF.Views.Components
{
	/// <summary>
	/// Interaction logic for BaseComponentView.xaml
	/// </summary>
	public partial class BaseComponentView : UserControl
	{
		public BaseComponentView()
		{
			InitializeComponent();
		}

		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(BaseComponent), typeof(BaseComponentView), new PropertyMetadata(new BaseComponent()));

		public BaseComponent Value
		{
			get { return (BaseComponent)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}
	}
}
