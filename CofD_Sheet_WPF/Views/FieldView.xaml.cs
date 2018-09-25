using CofD_Sheet_WPF.Models;
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

namespace CofD_Sheet_WPF.Views
{
	/// <summary>
	/// Interaction logic for FieldView.xaml
	/// </summary>
	public partial class FieldView : UserControl
	{
		public FieldView()
		{
			InitializeComponent();
		}

		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(Field), typeof(FieldView), new PropertyMetadata(new Field()));

		public Field Value
		{
			get { return (Field)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}
	}
}
