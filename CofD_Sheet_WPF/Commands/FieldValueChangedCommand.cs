using CofD_Sheet_WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CofD_Sheet_WPF.Commands
{
	class FieldValueChangedCommand : ICommand
	{
		public FieldValueChangedCommand(Field model)
		{
			_model = model;
		}

		private Field _model;

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public bool CanExecute(object parameter)
		{
			return _model.CanChangeValue;
		}

		public void Execute(object parameter)
		{
			_model.SaveChanges();
		}
	}
}
