namespace YoApp.Clients.Core
{
    public static class ResourceKeys
    {
        public static bool IsDebug
        {
            get { return (bool)App.Current.Resources[ResourceKeys.DebugKey]; }
            set { App.Current.Resources[ResourceKeys.DebugKey] = value; }
        }

        public static int NicknameMaxLength
        {
            get { return (int)App.Current.Resources[ResourceKeys.NicknameMaxLengthKey]; }
            set { App.Current.Resources[ResourceKeys.NicknameMaxLengthKey] = value; }
        }

        public static int StatusMessageMaxLength
        {
            get { return (int)App.Current.Resources[ResourceKeys.StatusMessageMaxLengthKey]; }
            set { App.Current.Resources[ResourceKeys.StatusMessageMaxLengthKey] = value; }
        }

        private const string DebugKey = "isDebug";
        private const string NicknameMaxLengthKey = "nicknameMaxLength";
        private const string StatusMessageMaxLengthKey = "statusMessageMaxLength";

        static ResourceKeys()
        {
#if DEBUG
            IsDebug = true;
#endif
        }
    }
}
