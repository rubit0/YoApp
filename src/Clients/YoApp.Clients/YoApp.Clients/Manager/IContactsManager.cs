using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using YoApp.Clients.Models;
using YoApp.Clients.ViewModels.Contacts;
using YoApp.Clients.ViewModels.ListViewGroups;

namespace YoApp.Clients.Manager
{
    public interface IContactsManager : INotifyPropertyChanged
    {
        List<LocalContact> Contacts { get; }
        Task<bool> LoadContactsAsync();
        List<ContactGroup> BuildContactGroup();
        List<ContactGroup> BuildContactGroup(IEnumerable<LocalContact> contacts);
    }
}