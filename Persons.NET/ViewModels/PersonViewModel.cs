using Microsoft.Extensions.Logging;
using Persons.NET.Helpers;
using Persons.NET.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Persons.NET.Stores;

namespace Persons.NET.ViewModels
{
    public class PersonViewModel : BaseViewModel
    {
        protected readonly PersonsService personsService;
        protected readonly NavigationStore navigationStore;
        protected readonly ILogger logger;

        public PersonViewModel(ILogger<PersonViewModel> logger, PersonsService personsService, NavigationStore navigationStore)
        {
            this.personsService = personsService;
            this.navigationStore = navigationStore;
            this.logger = logger;

            this.SaveCommand = new DelegateCommand(async () => await this.Save());
            this.BackCommand = new DelegateCommand(NavigateHome);

            this.Status = string.Empty;
        }

        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand BackCommand { get; set; }

        /// <summary>
        /// Virtual method for Adding and Editing persons
        /// </summary>
        /// <returns></returns>
        protected virtual async Task Save()
        {
        }

        protected void NavigateHome()
        {
            this.navigationStore.CurrentViewModel = App.ServiceProvider.GetService<DashboardViewModel>();
        }

        /// <summary>
        /// Validates if text box info looks correct
        /// </summary>
        /// <returns></returns>
        protected bool ValidateInputs()
        {
            if (string.IsNullOrEmpty(this.FirstName))
            {
                this.Status = "First name is invalid!";
                return false;
            }

            if (string.IsNullOrEmpty(this.LastName))
            {
                this.Status = "Last name is invalid!";
                return false;
            }

            if (this.TaxNumber <= 0 || this.TaxNumber.ToString().Length != 8)
            {
                this.Status = "Tax number is invalid!";
                return false;
            }

            if (string.IsNullOrEmpty(this.Address))
            {
                this.Status = "Address is invalid!";
                return false;
            }

            return true;
        }

        #region Bindings

        private string _status;
        public string Status
        {
            get
            {
                return this._status;
            }
            set
            {
                SetProperty(ref this._status, value);
            }
        }

        private string _firstName;
        public string FirstName
        {
            get
            {
                return this._firstName;
            }
            set
            {
                SetProperty(ref this._firstName, value);
            }
        }


        private string _lastName;
        public string LastName
        {
            get
            {
                return this._lastName;
            }
            set
            {
                SetProperty(ref this._lastName, value);
            }
        }


        private long _taxNumber;
        public long TaxNumber
        {
            get
            {
                return this._taxNumber;
            }
            set
            {
                SetProperty(ref this._taxNumber, value);
            }
        }


        private string _address;
        public string Address
        {
            get
            {
                return this._address;
            }
            set
            {
                SetProperty(ref this._address, value);
            }
        }

        #endregion
    }
}
