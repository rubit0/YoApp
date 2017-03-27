﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using YoApp.Clients.Helpers;
using YoApp.Clients.Manager;
using YoApp.Clients.Models;

namespace YoApp.Clients.ViewModels.Chats
{
    public class FriendSelectionViewModel
    {
        public List<Friend> Friends => _friendsManager.Friends.ToList();

        public Command SelectCommand { get; private set; }
        public Command CloseCommand { get; private set; }

        private readonly IPageService _pageService;
        private readonly IFriendsManager _friendsManager;

        public FriendSelectionViewModel(IPageService pageService)
        {
            _pageService = pageService;
            _friendsManager = App.Resolver.Resolve<IFriendsManager>();

            CloseCommand = new Command(async () => await _pageService.Navigation.PopModalAsync());
            SelectCommand = new Command(async (o) => await SelectFriend(o));
        }

        private async Task SelectFriend(object item)
        {
            var friend = item as Friend;
            if (friend != null)
                await App.ChatService.OpenChat(friend);
        }
    }
}