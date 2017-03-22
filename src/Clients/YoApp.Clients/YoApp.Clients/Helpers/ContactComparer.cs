using System.Collections.Generic;
using YoApp.Clients.Models;

namespace YoApp.Clients.Helpers
{
    public class ContactComparer : IEqualityComparer<LocalContact>, IComparer<LocalContact>
    {
        public enum CompareBy
        {
            LastName,
            DisplayName
        }

        public CompareBy CompareMode { get; set; } = CompareBy.LastName;

        public bool Equals(LocalContact x, LocalContact y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(LocalContact obj)
        {
            return obj.Id.GetHashCode();
        }

        public int Compare(LocalContact x, LocalContact y)
        {
            switch (CompareMode)
            {
                case CompareBy.LastName:
                    return x.GetSortFlag().CompareTo(y.GetSortFlag());
                case CompareBy.DisplayName:
                    return string.Compare(x.DisplayName, y.DisplayName);
                default:
                    return x.GetSortFlag().CompareTo(y.GetSortFlag());
            }
        }
    }
}