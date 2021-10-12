using NUnit.Framework;
using Persons.NET.Model;
using Persons.NET.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Persons.NET.Tests
{
    public class FileServiceTests
    {
        private FileService fileService;

        [SetUp]
        public void Setup()
        {
            this.fileService = new FileService();
        }

        private string GenerateRandomFileName()
        {
            return $"{Guid.NewGuid()}.json";
        }

        [Test]
        public async Task ReadPersonsFromNonExistantFile()
        {
            var result = await this.fileService.ReadFromFile<List<Person>>("DoesntExist.json");
            Assert.True(result == default(List<Person>));
        }
        
        [Test]
        public async Task WriteToNonExistantFile()
        {
            var filename = this.GenerateRandomFileName();
            var personsService = new PersonsService(filename, this.fileService);
            var person1 = await personsService.AddPerson("a", "b", 11112222, "ad");
            var result = await this.fileService.ReadFromFile<List<Person>>(filename);
            Assert.True(result.First().Id == person1.Id);
        }

        [Test]
        public async Task ReadMultiplePersonsFromFile()
        {
            var filename = this.GenerateRandomFileName();

            var personsService = new PersonsService(filename, this.fileService);
            var person1 = await personsService.AddPerson("a", "b", 11112222, "ad");
            var person2 = await personsService.AddPerson("aa", "ba", 11112223, "ada");

            var result = await this.fileService.ReadFromFile<List<Person>>(filename);

            Assert.True(result.Count == 2);
            Assert.True(result.Last().FirstName == person2.FirstName);

            File.Delete(filename);
        }
    }
}