using Projekt.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Projekt.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        private BaseViewModel _currentViewModel = new WeatherInfoModel();
        public BaseViewModel CurrentViewModel
        {
            get { return _currentViewModel; }
            set { 
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        public void OnChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        public ICommand UpdateViewCommand { get; set; }

        public MenuViewModel()
        {
            UpdateViewCommand = new UpdateViewCommand(this);
        }
    }
}
