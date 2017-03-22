using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Reactive.Linq;
using System.Reflection;
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

        public AppUser User { get; private set; }
        public ConventionsPreferences Conventions { get; private set; }
        public BackendPreferences Backend { get; private set; }

        public AppSettings()
        {
            User = new AppUser();
            Conventions = new ConventionsPreferences();
            Backend = new BackendPreferences();
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
            Backend = fresh.Backend;
        }

        public class AppUser
        {
            public string PhoneNumber { get; set; }
            public string Nickname { get; set; }
            public string Status { get; set; }
        }

        public class ConventionsPreferences
        {
            public int NicknameMaxLength { get; set; }
            public int StatusMessageMaxLength { get; set; }
            public string DefaultStatusMessage { get; set; }
            public int PhoneNumberMaxLength { get; set; }
        }

        public class BackendPreferences
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

            return settings ?? LoadAppSettingsFromRessource();
        }

        /// <summary>
        /// Load from an embedded json ressource.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static AppSettings LoadAppSettingsFromRessource(string name = "YoApp.Clients.Ressources.appsettings.json")
        {
            var assembly = typeof(AppSettings).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream(name);
            if (stream == null)
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
                var backendToken = intermediate["Backend"];
                appPreferences.Backend.TimeOut = (int)backendToken["TimeOut"];
                appPreferences.Backend.Secure = (bool)backendToken["Secure"];
                appPreferences.Backend.Host = (string)backendToken["Host"];
                appPreferences.Backend.Port = (int)backendToken["Port"];
            }

            return appPreferences;
        }
    }
}
