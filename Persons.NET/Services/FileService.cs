using Microsoft.Extensions.Logging;
using Persons.NET.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persons.NET.Services
{
    public class FileService
    {
        public FileService()
        {
        }

        public async Task<T> ReadFromFile<T>(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return default(T);
            }

            StringBuilder sb = new StringBuilder();

            foreach (var line in this.GetFileLines(fileName))
            {
                sb.Append(line);
            }

            return JsonSerializer.Deserialize<T>(sb.ToString());
        }

        public async Task WriteToFile(string fileName, object value)
        {
            try
            {
                FileStream stream = new FileStream(fileName, FileMode.Truncate);
                using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
                {
                    writer.Write(JsonSerializer.Serialize(value));
                }
            }
            catch (Exception ex)
            {
            }
        }

        private IEnumerable<string> GetFileLines(string fileName)
        {
            using (StreamReader sr = new StreamReader(fileName))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }
    }
}
