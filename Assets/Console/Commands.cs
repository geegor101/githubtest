using System.Numerics;
using FishNet;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Console
{
    [CommandHolder]
    public static class Commands
    {
        public static DebugEnabled _debugStatus = DebugEnabled.FALSE;
        

        private static void InvalidArguments(string[] args, ConsoleLogger.CommandCallInfo info)
        {
            string output = "";
            for (var i = 1; i < args.Length; i++)
            {
                output += args[i];
            }

            Debug.LogWarning($"Invalid command argument(s) : {output}");
        }

        public static void NoPermissionError(string[] args, ConsoleLogger.CommandCallInfo info)
        {
            Debug.LogWarning("You lack the permission to perform that command");
        }

        /*
        [Command("start", false, true)]
        public static void StartCommand(string[] args, ConsoleLogger.CommandCallInfo info)
        {
            if (args.Length != 2)
            {
                InvalidArguments(args, info);
                return;
            }

            switch (args[1])
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
                    InvalidArguments(args, info);
                    break;
            }
        }

        [Command("stophost", true, true)]
        public static void StopCommand(string[] args, ConsoleLogger.CommandCallInfo info)
        {
            Debug.LogWarning("Stopping host: ");
            InstanceFinder.ClientManager.StopConnection();
            InstanceFinder.ServerManager.StopConnection(true);
        }

        [Command("test", true, true)]
        public static void TestCommand(string[] args, ConsoleLogger.CommandCallInfo info)
        {
        }

        [Command("whoami", false, true)]
        public static void WhoAmICommand(string[] args, ConsoleLogger.CommandCallInfo info)
        {
        }

        [Command("debugstate", true, false, 3)]
        public static void DebugStateCommand(string[] args, ConsoleLogger.CommandCallInfo info)
        {
            if (info.conn != null)
                return;
            if (DebugEnabled.TryParse(args[1], out DebugEnabled state) && state != _debugStatus)
                _debugStatus = state; //Any update requiring stuff can be called here
        }
        */

        
        [Command("tester", true, true)]
        public static void TestCommand(string s, int[] ints, ConsoleLogger.CommandCallInfo info)
        {
            Debug.Log($"Command sent: {s}, {ints.Length}");
        }

        [Command("vector3test", true, true)]
        public static void Vector3Test(Vector3 vector3, [CommandParameterLength(2)] int[] ints, ConsoleLogger.CommandCallInfo info)
        {
            Debug.Log($"({vector3.x}, {vector3.y}, {vector3.z}) {ints.Length}");
        }
        
        [Parser]
        public static string StringParser(string[] strings, ConsoleLogger.CommandCallInfo info)
        {
            if (strings.Length == 0)
                return "";
            return strings[0];
        }

        [Parser(3)]
        public static Vector3 Vector3Parser(string[] strings, ConsoleLogger.CommandCallInfo info)
        {
            return new Vector3(float.Parse(strings[0]), float.Parse(strings[1]), float.Parse(strings[2]));
        }

        [Parser]
        public static int Int32Parser(string[] strings, ConsoleLogger.CommandCallInfo info)
        {
            return int.Parse(strings[0]);
        }
        
        [Parser]
        public static float FloatParser(string[] strings, ConsoleLogger.CommandCallInfo info)
        {
            return float.Parse(strings[0]);
        }

        public enum DebugEnabled
        {
            TRUE,
            FALSE
        }

        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236

        
    }
}