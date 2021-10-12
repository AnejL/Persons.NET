using Persons.NET.Services;
using Persons.NET.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persons.NET.ViewModels
{
    public class EditPersonViewModel : PersonViewModel
    {
        public EditPersonViewModel(PersonsService personsService, NavigationStore navigationStore)
            : base(personsService, navigationStore)
        {
        }

        /// <summary>
        /// Fills the page bindings with data
        /// </summary>
        /// <param name="id">Person Id</param>
        public async Task Init(string id)
        {
            var person = await this.personsService.GetPerson(id);

            this.Id = person.Id;
            this.FirstName = person.FirstName;
            this.LastName = person.LastName;
            this.TaxNumber = person.TaxNumber;
            this.Address = person.Address;
        }

        /// <inheritdoc/>
        protected override async Task Save()
        {
            if (!this.ValidateInputs())
            {
                return;
            }

            var person = await this.personsService.UpdatePerson(this.Id, this.FirstName, this.LastName, this.TaxNumber, this.Address);

            if (person != null)
            {
                this.NavigateHome();
            }
            else
            {
                this.Status = "Person was not updated. Is Tax number unique?";
            }    
        }

        #region Bindings

        private string _id;
        public string Id
        {
            get
            {
                return this._id;
            }
            set
            {
                SetProperty(ref this._id, value);
            }
        }

        #endregion
    }
}
