using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using YoApp.Clients.Models;
using YoApp.Clients.Pages.Chats;
using YoApp.Clients.Persistence;
using Microsoft.AspNet.SignalR.Client;

namespace YoApp.Clients.Services
{
    //public class ChatService
    //{
    //    public ObservableCollection<ChatPage> Pages { get; set; }

    //    private readonly IRealmStore _store;
    //    private readonly Uri _baseAddress;
    //    private HubConnection _connection;
    //    private IHubProxy _mainProxy;

    //    public ChatService()
    //    {
    //        _store = App.StorageResolver.Resolve<IRealmStore>();
    //        Pages = new ObservableCollection<ChatPage>();
    //        _baseAddress = App.Settings.Identity.Url;
    //    }

    //    public async Task OpenChat(Friend friend)
    //    {
    //        if (App.Current.MainPage.Navigation.ModalStack.Any())
    //            await App.Current.MainPage.Navigation.PopModalAsync();

    //        if (GetPageByFriend(friend) == null)
    //        {
    //            var chatBook = _store.Find<ChatBook>(friend.Key);
    //            Pages.Add(new ChatPage(friend, chatBook));
    //        }

    //        await App.Current.MainPage.Navigation.PopToRootAsync(false);
    //        await App.Current.MainPage.Navigation.PushAsync(GetPageByFriend(friend));
    //    }

    //    public ChatPage GetPageByFriend(Friend friend)
    //    {
    //        return Pages.FirstOrDefault(p => p.Friend.Equals(friend));
    //    }

    //    public async Task Connect()
    //    {
    //        SetupSignalR();
    //        await _connection.Start();
    //    }

    //    private void SetupSignalR()
    //    {
    //        _connection = new HubConnection(_baseAddress.ToString());
    //        _connection.Headers.Add("Authorization", $"Bearer {AuthenticationService.AuthAccount.Properties["access_token"]}");

    //        _mainProxy = _connection.CreateHubProxy("MainHub");
    //        _mainProxy.On("OnWelcome", async (string s) =>
    //        {
    //            await App.Current.MainPage.DisplayAlert("SignalR Message", s, "Ok");
    //        });
    //    }
    //}
}
