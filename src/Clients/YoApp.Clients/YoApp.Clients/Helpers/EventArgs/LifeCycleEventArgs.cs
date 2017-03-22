namespace YoApp.Clients.Helpers.EventArgs
{
    public enum Lifecycle
    {
        Start,
        Sleep,
        Resume
    }

    public class LifecycleEventArgs : System.EventArgs
    {
        public Lifecycle State { get; protected set; }

        public LifecycleEventArgs(Lifecycle state)
        {
            State = state;
        }
    }
}
