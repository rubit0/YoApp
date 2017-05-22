using System;
using System.Collections.Generic;
using YoApp.Clients.Models;

namespace YoApp.Clients.Core
{
    /// <summary>
    /// Compare Friends instances.
    /// </summary>
    public class FriendsComparer : IEqualityComparer<Friend>
    {
        public bool Equals(Friend x, Friend y)
        {
            return string.CompareOrdinal(x.PhoneNumber, y.PhoneNumber) == 0;
        }

        public int GetHashCode(Friend obj)
        {
            return StringComparer.Ordinal.GetHashCode(obj.PhoneNumber);
        }
    }
}