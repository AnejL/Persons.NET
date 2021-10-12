using NUnit.Framework;
using Persons.NET.Model;
using Persons.NET.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persons.NET.Tests
{
    public class PersonsServiceTests
    {
        private PersonsService personsService;
        private FileService fileService;
        private string fileName;

        [SetUp]
        public void Setup()
        {
            this.fileService = new FileService();
            this.fileName = this.GenerateRandomFileName();
            this.personsService = new PersonsService(this.fileName, this.fileService);
        }

        private string GenerateRandomFileName()
        {
            return $"{Guid.NewGuid()}.json";
        }

        private async Task<Person> InsertSamplePerson(long taxNumber)
        {
            return await this.personsService.AddPerson("Sample", "Guy", taxNumber, "Address");
        }

        #region GetPersons()

        [Test]
        public async Task GetEmptyPersonList()
        {
            File.Delete(this.fileName);

            var result = await this.personsService.GetPersons();

            Assert.True(result.Count == 0);
        }

        #endregion

        #region GetPerson(string id)

        [Test]
        public async Task GetNonExistantPerson()
        {
            var result = await this.personsService.GetPerson("NonExistantId");
            Assert.True(result == null);
        }

        [Test]
        public async Task GetPerson()
        {
            File.Delete(this.fileName);

            var person1 = await this.InsertSamplePerson(11112222);

            var result = await this.personsService.GetPerson(person1.Id);
            Assert.True(result.FirstName == person1.FirstName);
        }

        #endregion

        #region DeletePerson(string id)

        [Test]
        public async Task DeleteExistingPerson()
        {
            File.Delete(this.fileName);

            var person1 = await this.InsertSamplePerson(11112222);

            Assert.True(await this.personsService.DeletePerson(person1.Id));
            Assert.True(await this.personsService.GetPerson(person1.Id) == null);
        }

        [Test]
        public async Task DeleteNonExistingPerson()
        {
            File.Delete(this.fileName);

            var person1 = await this.InsertSamplePerson(11112222);

            Assert.True(await this.personsService.DeletePerson(person1.Id));
            Assert.False(await this.personsService.DeletePerson(person1.Id));
        }

        #endregion

        #region AddPerson(string firstName, string lastName, long taxNumber, string address)

        [Test]
        public async Task AddTwoValidPersons()
        {
            File.Delete(this.fileName);
            
            var person1 = await this.InsertSamplePerson(11112222);
            var person2 = await this.InsertSamplePerson(11111111);

            var result = await this.personsService.GetPersons();

            Assert.True(result.Count == 2);
            Assert.True(result.Last().FirstName == person2.FirstName);
        }

        [Test]
        public async Task AddInvalidPerson()
        {
            File.Delete(this.fileName);

            var person = await this.personsService.AddPerson("Sample", "Guy", 12312312, "");

            Assert.True(person == null);
        }

        [Test]
        public async Task AddTwoSameTaxNumbers()
        {
            File.Delete(this.fileName);

            var person1 = await this.InsertSamplePerson(11112222);
            var person2 = await this.InsertSamplePerson(11112222);


            Assert.True(person1 != null);
            Assert.True(person2 == null);
        }

        [Test]
        public async Task AddShortTaxNumberNumber()
        {
            File.Delete(this.fileName);

            var person = await this.InsertSamplePerson(1);

            Assert.True(person == null);
        }
        
        [Test]
        public async Task AddNegativeTaxNumberNumber()
        {
            File.Delete(this.fileName);

            var person = await this.InsertSamplePerson(-12345678);

            Assert.True(person == null);
        }

        #endregion

        #region UpdatePerson(string id, string firstName, string lastName, long taxNumber, string address)

        [Test]
        public async Task UpdateNonExistingPersonWithInvalidData()
        {
            File.Delete(this.fileName);

            var person = await this.personsService.UpdatePerson("NonExistantId", "Sample", "Guy", 12312312, "");

            Assert.True(person == null);
        }

        [Test]
        public async Task UpdateNonExistingPerson()
        {
            File.Delete(this.fileName);

            var person = await this.personsService.UpdatePerson("NonExistantId", "Sample", "Guy", 12312312, "123");

            Assert.True(person == null);
        }

        [Test]
        public async Task UpdateExistingPersonWithInvalidData()
        {
            File.Delete(this.fileName);
            var person = await this.InsertSamplePerson(12345678);

            var updated = await this.personsService.UpdatePerson(person.Id, "Sample", "Guy", 12312312, "");

            Assert.True(updated == null);
        }
        
        [Test]
        public async Task UpdateExistingPerson()
        {
            File.Delete(this.fileName);
            var person = await this.InsertSamplePerson(12345678);

            var updated = await this.personsService.UpdatePerson(person.Id, "XXXXXXX", "XXXXXX", 44444444, "XXXXX");

            Assert.True(person.Id == updated.Id
                && updated.FirstName == "XXXXXXX");
        }

        #endregion

    }
}
