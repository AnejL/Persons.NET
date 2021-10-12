using Microsoft.Extensions.Logging;
using Persons.NET.Helpers;
using Persons.NET.Model;
using Persons.NET.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Persons.NET.Stores;

namespace Persons.NET.ViewModels
{
    public class AddPersonViewModel : PersonViewModel
    {
        public AddPersonViewModel(PersonsService personsService, NavigationStore navigationStore)
            : base(personsService, navigationStore)
        {
        }

        /// <inheritdoc/>
        protected override async Task Save()
        {
            if (!this.ValidateInputs())
            {
                return;
            }

            var person = await this.personsService.AddPerson(this.FirstName, this.LastName, this.TaxNumber, this.Address);
            if (person == null)
            {
                this.Status = "Person was not inserted. Is your Tax Number unique?";
            }
            else
            {
                this.NavigateHome();
            }
        }
    }
}
