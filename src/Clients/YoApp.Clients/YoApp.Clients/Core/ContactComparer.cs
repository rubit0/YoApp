using System;
using System.Collections.Generic;
using YoApp.Clients.Models;

namespace YoApp.Clients.Core
{
    /// <summary>
    /// Class to compate LocalContact instances.
    /// </summary>
    public class ContactComparer : IEqualityComparer<LocalContact>, IComparer<LocalContact>
    {
        public enum CompareBy
        {
            LastName,
            DisplayName,
            PhoneNumber
        }

        public CompareBy CompareMode { get; set; } = CompareBy.LastName;

        public ContactComparer()
        {
            
        }

        public ContactComparer(CompareBy compareMode)
        {
            CompareMode = compareMode;
        }

        public bool Equals(LocalContact x, LocalContact y)
        {
            return StringComparer.Ordinal.Equals(x.NormalizedPhoneNumber, y.NormalizedPhoneNumber);
        }

        public int GetHashCode(LocalContact obj)
        {
            return StringComparer.Ordinal.GetHashCode(obj.NormalizedPhoneNumber);
        }

        public int Compare(LocalContact x, LocalContact y)
        {
            switch (CompareMode)
            {
                case CompareBy.LastName:
                    return x.GetSortFlag().CompareTo(y.GetSortFlag());
                case CompareBy.DisplayName:
                    return string.CompareOrdinal(x.DisplayName, y.DisplayName);
                case CompareBy.PhoneNumber:
                    return string.CompareOrdinal(x.NormalizedPhoneNumber, y.NormalizedPhoneNumber);
                default:
                    return x.GetSortFlag().CompareTo(y.GetSortFlag());
            }
        }
    }
}