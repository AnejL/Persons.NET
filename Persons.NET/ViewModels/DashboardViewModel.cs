using Microsoft.Extensions.Logging;
using Persons.NET.Helpers;
using Persons.NET.Model;
using Persons.NET.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Persons.NET.Stores;

namespace Persons.NET.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private readonly PersonsService personsService;
        private readonly NavigationStore navigationService;

        public DashboardViewModel(PersonsService personsService, NavigationStore navigationService)
        {
            this.personsService = personsService;
            this.navigationService = navigationService;

            this.AddCommand = new DelegateCommand(AddPerson);

            this.UpdateCommand = new DelegateCommand<Person>(async (Person p) => await this.UpdatePerson(p));
            this.DeleteCommand = new DelegateCommand<Person>(async (Person p) => await this.DeletePerson(p));
            
            this.Status = "Ready!";
        }

        public DelegateCommand AddCommand { get; set; }

        public DelegateCommand<Person> UpdateCommand { get; set; }

        public DelegateCommand<Person> DeleteCommand { get; set; }

        /// <summary>
        /// Initializes data on ViewModel
        /// </summary>
        public async Task Init()
        {
            this.Persons = (await this.personsService.GetPersons()).ToList();
        }

        /// <summary>
        /// Loads AddPersonViewModel as Main Window ViewModel
        /// </summary>
        private void AddPerson()
        {
            this.navigationService.CurrentViewModel = App.ServiceProvider.GetService<AddPersonViewModel>();
        }

        /// <summary>
        /// Loads EditPersonViewModel as Main Window ViewModel and passes in the Person's identifier
        /// </summary>
        private async Task UpdatePerson(Person person)
        {
            this.navigationService.CurrentViewModel = App.ServiceProvider.GetService<EditPersonViewModel>();
            var vm = this.navigationService.CurrentViewModel as EditPersonViewModel;
            await vm.Init(person.Id);
        }

        private async Task DeletePerson(Person person)
        {
            if (await this.personsService.DeletePerson(person.Id))
            {
                this.Status = $"Person {person.FirstName} {person.LastName} was deleted!";
            }
            else
            {
                this.Status = $"Person {person.FirstName} {person.LastName} could not be deleted!";
            }

            await this.Init();
        }

        #region Bindings

        private string status;
        public string Status
        {
            get
            {
                return this.status;
            }
            set
            {
                SetProperty(ref this.status, value);
            }
        }

        private List<Person> persons;
        public List<Person> Persons
        {
            get
            {
                return this.persons;
            }
            set
            {
                SetProperty(ref this.persons, value);
            }
        }

        #endregion
    }
}
