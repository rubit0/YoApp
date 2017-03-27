using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading.Tasks;
using YoApp.Clients.Persistence;

namespace YoApp.Clients.Helpers
{
    /// <summary>
    /// Holds all settings and preferences for this app.
    /// </summary>
    public class AppSettings : IKeyProvider
    {
        public string Key => nameof(AppSettings);

        public bool SetupFinished { get; set; }
        public string ServiceId { get; set; }

        public ConventionsPreferences Conventions { get; private set; }
        public BackendHost Identity { get; private set; }
        public BackendHost Friends { get; private set; }
        public BackendHost Chat { get; private set; }

        public AppSettings()
        {
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
            var fresh = LoadAppSettingsFromRessource();

            SetupFinished = fresh.SetupFinished;
            ServiceId = fresh.ServiceId;
            Conventions = fresh.Conventions;
            Identity = fresh.Identity;
        }

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

            public Uri GetUrl()
            {
                if (_url != null)
                    return _url;

                var scheme = (Secure) ? "https://" : "http://";
                return (_url = new UriBuilder(scheme, Host, Port).Uri);
            }
        }

        /// <summary>
        /// Init AppSettings.
        /// </summary>
        /// <returns>AppSettings from either the store or embedded json ressource.</returns>
        public static AppSettings InitAppSettings()
        {
            var store = App.StorageResolver.Resolve<IKeyValueStore>();
            var settings = store.GetObservable<AppSettings>(nameof(AppSettings)).Wait();
            if (settings != null)
                return settings;

            return LoadAppSettingsFromRessource();
        }

        /// <summary>
        /// Load from an embedded json ressource.
        /// </summary>
        /// <returns></returns>
        public static AppSettings LoadAppSettingsFromRessource(string relativePath = "YoApp.Clients.Ressources")
        {
            var name = (!ResourceKeys.IsDebug) 
                ? $"{relativePath}.appsettings.json" 
                : $"{relativePath}.appsettings.Development.json";

            var assembly = typeof(AppSettings).GetTypeInfo().Assembly;

            var stream = assembly.GetManifestResourceStream(name);

            if (stream == null && ResourceKeys.IsDebug)
                stream = assembly.GetManifestResourceStream($"{relativePath}.appsettings.json");
            else if (stream == null)
                return null;

            var appPreferences = new AppSettings();

            using (var reader = new System.IO.StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                var intermediate = JsonConvert.DeserializeObject<JObject>(json);

                //App
                var appToken = intermediate["App"];
                appPreferences.ServiceId = (string)appToken["ServiceId"];

                //Conventions
                var conventionsToken = intermediate["Conventions"];
                appPreferences.Conventions.NicknameMaxLength = (int)conventionsToken["NicknameMaxLength"];
                appPreferences.Conventions.StatusMessageMaxLength = (int)conventionsToken["StatusMessageMaxLength"];
                appPreferences.Conventions.PhoneNumberMaxLength = (int)conventionsToken["PhoneNumberMaxLength"];
                appPreferences.Conventions.DefaultStatusMessage = (string)conventionsToken["DefaultStatusMessage"];

                //Backend
                var backendTokenIdentity = intermediate["Backend"]["Identity"];
                appPreferences.Identity.TimeOut = (int)backendTokenIdentity["TimeOut"];
                appPreferences.Identity.Secure = (bool)backendTokenIdentity["Secure"];
                appPreferences.Identity.Host = (string)backendTokenIdentity["Host"];
                appPreferences.Identity.Port = (int)backendTokenIdentity["Port"];

                var backendTokenFriends = intermediate["Backend"]["Friends"];
                appPreferences.Friends.TimeOut = (int)backendTokenFriends["TimeOut"];
                appPreferences.Friends.Secure = (bool)backendTokenFriends["Secure"];
                appPreferences.Friends.Host = (string)backendTokenFriends["Host"];
                appPreferences.Friends.Port = (int)backendTokenFriends["Port"];

                var backendTokenChat = intermediate["Backend"]["Chat"];
                appPreferences.Chat.TimeOut = (int)backendTokenChat["TimeOut"];
                appPreferences.Chat.Secure = (bool)backendTokenChat["Secure"];
                appPreferences.Chat.Host = (string)backendTokenChat["Host"];
                appPreferences.Chat.Port = (int)backendTokenChat["Port"];
            }

            return appPreferences;
        }

        public async Task Persist()
        {
            var store = App.StorageResolver.Resolve<IKeyValueStore>();

            var instance = await store.Get<AppSettings>(Key);
            if(instance == null)
                await store.Insert(this);

            await store.Persist();
        }
    }
}
