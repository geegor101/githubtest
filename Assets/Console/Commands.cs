using FishNet;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Console
{
    [CommandHolder]
    public static class Commands
    {
        public static DebugEnabled _debugStatus = DebugEnabled.FALSE;
        

        /*
        private static void InvalidArguments(string[] args, CommandCallInfo info)
        {
            string output = "";
            for (var i = 1; i < args.Length; i++)
            {
                output += args[i];
            }

            Debug.LogWarning($"Invalid command argument(s) : {output}");
        }
        */

        public static void NoPermissionError(CommandCallInfo info)
        {
            Debug.LogWarning("You lack the permission to perform that command");
        }

        
        [Command("start", false, true)]
        public static void StartCommand(string location, CommandCallInfo info)
        {
            switch (location)
            {
                case "server":
                    Debug.LogWarning("Starting server: ");
                    InstanceFinder.ServerManager.StartConnection();
                    break;
                case "client":
                    Debug.LogWarning("Starting client: ");
                    InstanceFinder.ClientManager.StartConnection();
                    break;
                case "host":
                    Debug.LogWarning("Starting host: ");
                    InstanceFinder.ServerManager.StartConnection();
                    InstanceFinder.ClientManager.StartConnection();

                    //TODO: REMOVE
                    //(new ConsoleLogger.ChatChannel("a")).Connections.Add(InstanceFinder.ClientManager.Connection);

                    break;
                default:
                    Debug.LogWarning("Invalid start location");
                    break;
            }
        }

        [Command("connect", false, true)]
        public static void ConnectCommand(string destination, CommandCallInfo info)
        {
            if (!InstanceFinder.IsClient)
                InstanceFinder.ClientManager.StartConnection();
            InstanceFinder.ClientManager.StartConnection(destination);
        }

        [Command("stophost", true, true)]
        public static void StopCommand(CommandCallInfo info)
        {
            Debug.LogWarning("Stopping host: ");
            InstanceFinder.ClientManager.StopConnection();
            InstanceFinder.ServerManager.StopConnection(true);
        }

        [Command("test", true, true)]
        public static void TestCommand(CommandCallInfo info)
        {
        }

        [Command("whoami", false, true)]
        public static void WhoAmICommand(CommandCallInfo info)
        {
            if (info.conn != null)
                Debug.Log($"ID: {info.conn.ClientId}");
        }

        [Command("debugstate", true, false, 3)]
        public static void DebugStateCommand(string state, CommandCallInfo info)
        {
            if (info.conn != null)
                return;
            if (DebugEnabled.TryParse(state, out DebugEnabled st) && st != _debugStatus)
                _debugStatus = st; //Any update requiring stuff can be called here
        }
        

        
        [Command("tester", true, true)]
        public static void TestCommand(string s, [CommandParameterLength(0)] int[] ints, CommandCallInfo info)
        {
            Debug.Log($"Command sent: {s}, {ints.Length}");
        }

        [Command("vector3test", true, true)]
        public static void Vector3Test(Vector3 vector3, [CommandParameterLength(-2)] int[] ints, CommandCallInfo info)
        {
            Debug.Log($"({vector3.x}, {vector3.y}, {vector3.z}) {ints.Length}");
        }

        [Command("benchmark", false, true)]
        public static void BenchmarkCommand(int ints, [CommandParameterLength(2)] string[] strs, CommandCallInfo info)
        {
            
        }
        

        public enum DebugEnabled
        {
            TRUE,
            FALSE
        }

    }
}