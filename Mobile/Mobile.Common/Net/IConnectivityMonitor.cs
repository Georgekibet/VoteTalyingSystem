namespace Mobile.Common.Net
{
    public interface IConnectivityMonitor
    {
        bool IsNetworkAvailable();
        void WaitForNetwork();
    }
}