using System;
using System.Threading.Tasks;
using YoApp.Authoring.Services;

namespace YoApp.Authoring.Commands
{
    public class RegisterUsersCommand
    {
        public static async Task RegisterUsers(string jsonPath = "Ressources.MOCK_DATA.json")
        {
            var accountService = new AccountService();
            var contacts = MockReader.ParseContacts(jsonPath);

            foreach (var contact in contacts)
            {
                Console.WriteLine("------");
                Console.WriteLine($"Trying to register an account for {contact.Phone}\n");
                try
                {
                    Console.WriteLine("Result:");
                    var result = await accountService.RegisterContact(contact);

                    if (result.IsSuccessStatusCode)
                        Console.WriteLine("Account created!");
                    else
                        Console.WriteLine($"Error\n{result.ReasonPhrase} - {result.Content.ReadAsStringAsync().Result}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Abborting command due exception. Probably no connection to server.");
                    break;
                }
            }
        }
    }
}
