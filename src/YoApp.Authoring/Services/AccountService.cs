using System;
using System.Net.Http;
using System.Threading.Tasks;
using YoApp.Authoring.Dtos;

namespace YoApp.Authoring.Services
{
    public class AccountService
    {
        private readonly Uri _accountDebugEndPoint = new Uri("http://localhost:5000/api/debug");

        public async Task<HttpResponseMessage> RegisterContact(Contact contact)
        {
            using (var client = new HttpClient())
            {
                var content = contact.ToFormEncoded();
                return await client.PostAsync(_accountDebugEndPoint, content);
            }
        }
    }
}
