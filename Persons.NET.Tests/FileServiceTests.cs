using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Persons.NET.Services;

namespace Persons.NET.Tests
{
    public class FileServiceTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ReadFromEmptyFile()
        {
            var fileService = new FileService();

            Assert.Pass();
        }
    }
}