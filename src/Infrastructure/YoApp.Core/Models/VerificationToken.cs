﻿using System;

namespace YoApp.Core.Models
{
    public class VerificationToken
    {
        public int Id { get; set; }
        public string User { get; set; }
        public string Code { get; set; }
        public DateTime Created { get; set; }
        public DateTime Expires { get; set; }

        public VerificationToken()
        {
        }
        
        public VerificationToken(string user, TimeSpan duration, string code)
        {
            this.User = user;
            this.Created = DateTime.Now;
            this.Expires = Created.Add(duration);
            this.Code = code;
        }

        public bool IsExpired()
        {
            return DateTime.Compare(Expires, DateTime.Now) < 0;
        }
    }
}
