using Projekt.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Projekt.Command
{
    public class UpdateViewCommand : ICommand
    {
        private MenuViewModel viewModel;

        public UpdateViewCommand(MenuViewModel menuViewModel)
        {
            this.viewModel = menuViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if (parameter.ToString() == "Home")
                viewModel.CurrentViewModel = new MenuViewModel();
            else if (parameter.ToString() == "WeatherInfo")
                viewModel.CurrentViewModel = new WeatherInfoModel();
            else if (parameter.ToString() == "Settings")
                viewModel.CurrentViewModel = new SettingsViewModel();
        }
    }
}
