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

        public PersonsService(IOptions<AppSettings> appSettings, FileService fileService)
        {
            this.appSettings = appSettings.Value;
            this.fileService = fileService;
        }

        public async Task<Person> AddPerson(string firstName, string lastName, long taxNumber, string address)
        {
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
            await this.fileService.WriteToFile(this.appSettings.StorageFile, persons);
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
            await this.fileService.WriteToFile(this.appSettings.StorageFile, persons);
            return true;
        }

        public async Task<Person> GetPerson(string id)
        {
            var persons = await this.GetPersons();

            return persons.Where(p => p.Id == id).FirstOrDefault();
        }

        public async Task<List<Person>> GetPersons()
        {
            var persons = await this.fileService.ReadFromFile<List<Person>>(this.appSettings.StorageFile);
            if (persons == null)
            {
                return new List<Person>();
            }

            return persons;
        }

        public async Task<Person> UpdatePerson(string id, string firstName, string lastName, long taxNumber, string address)
        {
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

            await this.fileService.WriteToFile(this.appSettings.StorageFile, persons);
            return person;
        }
    }
}
