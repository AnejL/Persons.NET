using Microsoft.Extensions.Logging;
using Persons.NET.Helpers;
using Persons.NET.Model;
using Persons.NET.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Persons.NET.ViewModels
{
    public class AddPersonViewModel : PersonViewModel
    {
        public AddPersonViewModel(ILogger<AddPersonViewModel> logger, PersonsService personsService, NavigationStore navigationStore)
            : base(logger, personsService, navigationStore)
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
                this.logger.LogError("Person was not logged!");
            }
            
            this.NavigateHome();
        }
    }
}
