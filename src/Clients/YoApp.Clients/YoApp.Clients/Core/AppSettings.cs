﻿using System;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YoApp.Clients.Persistence;

namespace YoApp.Clients.Core
{
    /// <summary>
    /// Holds all settings and preferences for this app.
    /// </summary>
    public class AppSettings : IKeyProvider, IStartable
    {
        public string Key => nameof(AppSettings);

        public bool SetupFinished { get; set; }
        public string ServiceId { get; set; }

        public ConventionsPreferences Conventions { get; set; }
        public BackendHost Identity { get; set; }
        public BackendHost Friends { get; set; }
        public BackendHost Chat { get; set; }

        private readonly IKeyValueStore _store;

        public AppSettings(IKeyValueStore store)
        {
            _store = store;

            Conventions = new ConventionsPreferences();
            Identity = new BackendHost();
            Friends = new BackendHost();
            Chat = new BackendHost();
        }

        /// <summary>
        /// Restore the Configuration from the embedded appsettings.json
        /// </summary>
        public void ClearConfiguration()
        {
            LoadFromRessource();

            SetupFinished = false;
        }

        public async void Start()
        {
            var settings = _store.GetObservable<AppSettings>(nameof(AppSettings)).Wait();
            if (settings != null)
            {
                SetupFinished = settings.SetupFinished;
                ServiceId = settings.ServiceId;
                Conventions = settings.Conventions;
                Identity = settings.Identity;
                Friends = settings.Friends;
                Chat = settings.Chat;
            }
            else
            {
                LoadFromRessource();
                await Persist();
            }

            ResourceKeys.NicknameMaxLength = Conventions.NicknameMaxLength;
            ResourceKeys.StatusMessageMaxLength = Conventions.StatusMessageMaxLength;
        }

        /// <summary>
        /// Load from an embedded json ressource.
        /// </summary>
        private void LoadFromRessource(string relativePath = "YoApp.Clients.Ressources")
        {
            var name = (!ResourceKeys.IsDebug) 
                ? $"{relativePath}.appsettings.json" 
                : $"{relativePath}.appsettings.Development.json";

            var assembly = typeof(AppSettings).GetTypeInfo().Assembly;

            var stream = assembly.GetManifestResourceStream(name);

            if (stream == null && ResourceKeys.IsDebug)
                stream = assembly.GetManifestResourceStream($"{relativePath}.appsettings.json");
            else if (stream == null)
                return;

            using (var reader = new System.IO.StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                var intermediate = JsonConvert.DeserializeObject<JObject>(json);

                //App
                var appToken = intermediate["App"];
                ServiceId = (string)appToken["ServiceId"];

                //Conventions
                var conventionsToken = intermediate["Conventions"];
                Conventions.NicknameMaxLength = (int)conventionsToken["NicknameMaxLength"];
                Conventions.StatusMessageMaxLength = (int)conventionsToken["StatusMessageMaxLength"];
                Conventions.PhoneNumberMaxLength = (int)conventionsToken["PhoneNumberMaxLength"];
                Conventions.DefaultStatusMessage = (string)conventionsToken["DefaultStatusMessage"];

                //Backend
                var backendTokenIdentity = intermediate["Backend"]["Identity"];
                Identity = new BackendHost
                {
                    Host = (string) backendTokenIdentity["Host"],
                    Port = (int) backendTokenIdentity["Port"],
                    Secure = (bool) backendTokenIdentity["Secure"],
                    TimeOut = (int) backendTokenIdentity["TimeOut"]
                };

                var backendTokenFriends = intermediate["Backend"]["Friends"];
                Friends = new BackendHost
                {
                    Host = (string)backendTokenFriends["Host"],
                    Port = (int)backendTokenFriends["Port"],
                    Secure = (bool)backendTokenFriends["Secure"],
                    TimeOut = (int)backendTokenFriends["TimeOut"]
                };

                var backendTokenChat = intermediate["Backend"]["Chat"];
                Chat = new BackendHost
                {
                    Host = (string)backendTokenChat["Host"],
                    Port = (int)backendTokenChat["Port"],
                    Secure = (bool)backendTokenChat["Secure"],
                    TimeOut = (int)backendTokenChat["TimeOut"]
                };
            }
        }

        public async Task Persist()
        {
            await _store.Insert(this);
            await _store.Persist();
        }

        #region POCOS
        public class ConventionsPreferences
        {
            public int NicknameMaxLength { get; set; }
            public int StatusMessageMaxLength { get; set; }
            public string DefaultStatusMessage { get; set; }
            public int PhoneNumberMaxLength { get; set; }
        }

        public class BackendHost
        {
            public int TimeOut { get; set; }
            public bool Secure { get; set; }
            public string Host { get; set; }
            public int Port { get; set; }
            public Uri Url => GetUrl();

            private Uri _url;

            private Uri GetUrl()
            {
                if (_url != null)
                    return _url;

                var scheme = (Secure) ? "https://" : "http://";
                return (_url = new UriBuilder(scheme, Host, Port).Uri);
            }
        } 
        #endregion
    }
}
