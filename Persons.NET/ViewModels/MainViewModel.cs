using Persons.NET.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persons.NET.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly NavigationStore navigationService;

        public MainViewModel(NavigationStore navigationService, DashboardViewModel vm)
        {
            this.navigationService = navigationService;
            this.navigationService.CurrentViewModel = vm;

            this.navigationService.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        /// <summary>
        /// ViewModel that is currently bound to MainWindow.
        /// Decides the displayed View.
        /// </summary>
        public BaseViewModel CurrentViewModel => this.navigationService.CurrentViewModel;

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(this.CurrentViewModel));
        }
    }
}
