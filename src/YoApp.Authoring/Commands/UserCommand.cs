using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using YoApp.Authoring.Dtos;
using YoApp.Authoring.Helper;
using YoApp.Authoring.Services;

namespace YoApp.Authoring.Commands
{
    public class UserCommand
    {
        public static string RegisterMocks()
        {
            return RegisterUsersFromJson("Ressources.MOCK_DATA.json");
        }

        public static string IsMember(string phoneNumber)
        {
            Console.WriteLine($"Checking {phoneNumber} for membership of this service...");
            return "Service is not implemented.";
        }

        private static string RegisterUsersFromJson(string jsonPath)
        {
            var accountService = new AccountService();
            var contacts = ParseContacts(jsonPath);

            foreach (var contact in contacts)
            {
                Console.WriteLine("------");
                Console.WriteLine($"Trying to register an account for {contact.Phone}\n");
                try
                {
                    Console.WriteLine("Result:");
                    var result = accountService.RegisterContact(contact).Result;

                    if (result.IsSuccessStatusCode)
                        Console.WriteLine("Account created!");
                    else
                        Console.WriteLine($"Error\n{result.ReasonPhrase} - {result.Content.ReadAsStringAsync().Result}");
                }
                catch (WebException ex)
                {
                    ConsoleHelpers.WriteExeption("Connection error, backend maybe offline.");
                    ConsoleHelpers.WriteExeption(ex.Message);
                    break;
                }
                catch (Exception ex)
                {
                    ConsoleHelpers.WriteExeption(ex.Message);
                    break;
                }
            }

            return string.Empty;
        }

        private static IEnumerable<Contact> ParseContacts(string filePath)
        {
            Console.WriteLine("Reading MOCK_DATA.json ...");

            var stream = Program.Current
                .GetManifestResourceStream($"YoApp.Authoring.{filePath}");

            if (stream == null)
            {
                Console.WriteLine("Error: Could not find MOCK_DATA.json");
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
