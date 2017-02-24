using System;
using System.Net.Http;
using System.Threading.Tasks;
using YoApp.Authoring.Dtos;

namespace YoApp.Authoring.Services
{
    public class AccountService
    {
        public async Task<HttpResponseMessage> RegisterContact(Contact contact)
        {
            using (var client = new HttpClient())
            {
                var content = contact.ToFormEncoded();
                return await client.PostAsync(new Uri(Program.DefaultEndpoint, "api/debug"), content);
            }
        }
    }
}
