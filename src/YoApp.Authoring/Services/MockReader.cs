using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using YoApp.Authoring.Dtos;

namespace YoApp.Authoring.Services
{
    public class MockReader
    {
        public static IEnumerable<Contact> ParseContacts(string filePath)
        {
            Console.WriteLine("Reading MOCK_DATA.json ...");
            
            var stream = typeof(MockReader).GetTypeInfo().Assembly.GetManifestResourceStream($"YoApp.Authoring.{filePath}");
            if (stream == null)
            {
                Console.WriteLine("Error: Could not find MOCK_DATA.json");
                Console.WriteLine("App termiated.");
                Console.ReadLine();
            }

            using (var file = new StreamReader(stream))
            {
                var json = file.ReadToEnd();
                Console.WriteLine("Parsing json to Contacts ...");
                var result = JsonConvert.DeserializeObject<IEnumerable<Contact>>(json);
                if (result == null)
                {
                    Console.WriteLine("Error: No contacts found on this json.");
                    return null;
                }

                Console.WriteLine($"Parsed and found {result.Count()} Contacts.\nParsing done.");

                return result;
            }
        }
    }
}
