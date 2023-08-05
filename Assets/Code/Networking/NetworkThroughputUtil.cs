using Code.Console;
using FishNet;
using FishNet.Managing.Statistic;

namespace Code.Networking
{
    public static class NetworkThroughputUtil
    {

        private static NetworkTrafficArgs _serverArgs;
        private static NetworkTrafficArgs _clientArgs;

        private static bool _initialized = false;
        
        public static void Initialize()
        {
            if (_initialized)
                return;
            InstanceFinder.StatisticsManager.NetworkTraffic.OnClientNetworkTraffic += args => _clientArgs = args;
            InstanceFinder.StatisticsManager.NetworkTraffic.OnServerNetworkTraffic += args => _serverArgs = args;
        }
        
        public static void GetTrafficArgs(out NetworkTrafficArgs serverArgs, out NetworkTrafficArgs clientArgs)
        {
            serverArgs = _serverArgs;
            clientArgs = _clientArgs;
        }

        
        public static void NetworkThroughputCommand(CommandCallInfo info)
        {
            
        }
        
    }
}