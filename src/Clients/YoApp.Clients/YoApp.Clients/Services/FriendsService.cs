using Newtonsoft.Json;
using Rubito.SimpleFormsAuth;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using YoApp.Clients.Models;
using YoApp.DataObjects.Users;

namespace YoApp.Clients.Services
{
    /// <summary>
    /// Service to handle friends on the backend.
    /// </summary>
    public static class FriendsService
    {
        private static readonly Uri _usersAddress;
        private static readonly Uri _isMemberAddress;

        static FriendsService()
        {
            _usersAddress = new Uri(App.Settings.Friends.Url + "friends/");
            _isMemberAddress = new Uri(_usersAddress, "check");
        }

        /// <summary>
        /// Check if the there is a member registered under the given phonenumber.
        /// </summary>
        /// <param name="phoneNumber">Check by this.</param>
        /// <returns>Is this a member.</returns>
        public static async Task<bool> IsMember(string phoneNumber)
        {
            await AuthenticationService.RequestToken();

            var request = new OAuth2BearerRequest("GET",
                new Uri(_isMemberAddress, phoneNumber),
                null,
                AuthenticationService.AuthAccount);

            var response = await request.GetResponseAsync();

            return (response.StatusCode == HttpStatusCode.OK);
        }

        public static async Task<Dictionary<string, bool>> AreMember(Dictionary<string, bool> phoneNumber)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fetch an Friend from the backend by the given phone number.
        /// Also stores it after fetching to the local db and Friends property.
        /// Make sure to check before if it is a Member.
        /// </summary>
        /// <param name="phoneNumber">A validated phoneNumber.</param>
        /// <returns>Friend from backend.</returns>
        public static async Task<Friend> FetchFriend(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentNullException(nameof(phoneNumber));

            await AuthenticationService.RequestToken();

            var request = new OAuth2BearerRequest("GET",
                new Uri(_usersAddress, phoneNumber),
                null,
                AuthenticationService.AuthAccount);

            var response = await request.GetResponseAsync();

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            var body = await response.GetResponseTextAsync();

            var dto = JsonConvert.DeserializeObject<UserDto>(body);
            return Friend.CreateFromDto(dto);
        }

        public static async Task<IEnumerable<Friend>> FetchFriends(IEnumerable<string> phoneNumbers)
        {
            throw new NotImplementedException();
        }
    }
}