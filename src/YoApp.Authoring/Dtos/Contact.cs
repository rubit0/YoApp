using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace YoApp.Authoring.Dtos
{
    public class Contact
    {
        [JsonProperty("mobile_phone")]
        public string Phone
        {
            get { return _phone; }
            set { _phone = Regex.Replace(value, @"\D", string.Empty); }
        }

        [JsonProperty("nickname")]
        public string Nickname { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        private string _phone;

        public FormUrlEncodedContent ToFormEncoded()
        {
            var values = new Dictionary<string, string>
            {
                {"phone", Phone },
                { "nickname", Nickname },
                { "status", Status },
                { "password", Password }
            };

            return new FormUrlEncodedContent(values);
        }
    }
}
