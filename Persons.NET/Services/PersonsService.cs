using Microsoft.Extensions.Options;
using Persons.NET.Configuration;
using Persons.NET.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persons.NET.Services
{
    public class PersonsService
    {
        private readonly AppSettings appSettings;
        private readonly FileService fileService;

        private readonly string fileName;

        public PersonsService(string fileName, FileService fileService)
        {
            this.fileName = fileName;
            this.fileService = fileService;
        }

        public PersonsService(IOptions<AppSettings> appSettings, FileService fileService)
            : this(appSettings.Value.StorageFile, fileService)
        {
            this.appSettings = appSettings.Value;
        }

        public async Task<bool> ValidateTaxNumber(long taxNumber)
        {
            return taxNumber > 0 
                && taxNumber.ToString().Length == 8 
                && !(await this.GetPersons()).Select(p => p.TaxNumber).Contains(taxNumber);
        }

        private bool ValidateStrings(string firstName, string lastName, string address)
        {
            return !string.IsNullOrEmpty(firstName)
                && !string.IsNullOrEmpty(lastName)
                && !string.IsNullOrEmpty(address);
        }

        public async Task<Person> AddPerson(string firstName, string lastName, long taxNumber, string address)
        {
            if (!this.ValidateStrings(firstName, lastName, address)
               || !await this.ValidateTaxNumber(taxNumber))
            {
                return null;
            }

            var persons = await this.GetPersons();

            var person = new Person()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = firstName,
                LastName = lastName,
                TaxNumber = taxNumber,
                Address = address,
            };

            persons.Add(person);
            await this.fileService.WriteToFile(this.fileName, persons);
            return person;
        }

        public async Task<bool> DeletePerson(string id)
        {
            var persons = await this.GetPersons();

            var toDelete = persons.Where(p => p.Id == id).FirstOrDefault();
            if (toDelete == null)
            {
                return false;
            }

            persons.Remove(toDelete);
            await this.fileService.WriteToFile(this.fileName, persons);
            return true;
        }

        public async Task<Person> GetPerson(string id)
        {
            var persons = await this.GetPersons();

            return persons.Where(p => p.Id == id).FirstOrDefault();
        }

        public async Task<List<Person>> GetPersons()
        {
            var persons = await this.fileService.ReadFromFile<List<Person>>(this.fileName);
            if (persons == null)
            {
                return new List<Person>();
            }

            return persons;
        }

        public async Task<Person> UpdatePerson(string id, string firstName, string lastName, long taxNumber, string address)
        {
            if (!this.ValidateStrings(firstName, lastName, address)
               || !await this.ValidateTaxNumber(taxNumber))
            {
                return null;
            }

            var persons = await this.GetPersons();

            var person = persons.Where(p => p.Id == id).FirstOrDefault();
            if (person == null)
            {
                return null;
            }

            person.FirstName = firstName;
            person.LastName = lastName;
            person.TaxNumber = taxNumber;
            person.Address = address;

            await this.fileService.WriteToFile(this.fileName, persons);
            return person;
        }
    }
}
