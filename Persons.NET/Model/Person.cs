using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Persons.NET.Model
{
    public class Person
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        [JsonPropertyName("taxNumber")]
        public long TaxNumber { get; set; }

        [JsonPropertyName("address")]
        public string Address{ get; set; }
    }
}
