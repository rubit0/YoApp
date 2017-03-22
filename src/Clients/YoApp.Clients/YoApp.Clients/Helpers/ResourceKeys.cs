namespace YoApp.Clients.Helpers
{
    public static class ResourceKeys
    {
        public static bool IsDebug
        {
            get { return (bool)App.Current.Resources[ResourceKeys.DebugKey]; }
            set { App.Current.Resources[ResourceKeys.DebugKey] = value; }
        }

        private const string DebugKey = "isDebug";

        static ResourceKeys()
        {
#if DEBUG
            IsDebug = true;
#endif
        }
    }
}
