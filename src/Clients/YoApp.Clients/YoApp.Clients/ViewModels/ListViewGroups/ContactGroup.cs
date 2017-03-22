using System.Collections.Generic;
using YoApp.Clients.Models;

namespace YoApp.Clients.ViewModels.ListViewGroups
{
    public class ContactGroup : List<LocalContact>
    {
        public string GroupSymbol { get; set; }

        public ContactGroup(string groupSymbol)
        {
            GroupSymbol = groupSymbol;
        }
    }
}