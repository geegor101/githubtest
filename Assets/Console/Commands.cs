using System;
using System.Reflection;
using FishNet;
using UnityEngine;

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
                case "server" :
                    Debug.LogWarning("Starting server: ");
                    InstanceFinder.ServerManager.StartConnection();
                    break;
                case "client" :
                    Debug.LogWarning("Starting client: ");
                    InstanceFinder.ClientManager.StartConnection();
                    break;
                case "host" :
                    Debug.LogWarning("Starting host: ");
                    InstanceFinder.ServerManager.StartConnection();
                    InstanceFinder.ClientManager.StartConnection();
                        
                    //TODO: REMOVE
                    (new ConsoleLogger.ChatChannel("a")).Connections.Add(InstanceFinder.ClientManager.Connection);

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

        #region Attributes

        public enum DebugEnabled
        {
            TRUE,
            FALSE
        }
        
        [AttributeUsage(AttributeTargets.Method, Inherited = false)]
        public sealed class CommandAttribute : Attribute
        {
            public readonly bool isServer;
            public readonly bool isClient;
            /*
             * 0 - Clientside
             * 1 - spectator
             * 2 - player
             * 3 - mod
             * 4 - admin
             * 5 - dev
             */
            public readonly int permissionLevel;
            public readonly string name;
            
            public CommandAttribute(string name, bool isServer, bool isClient, int permissionLevel = 0)
            {
                this.name = name;
                this.isServer = isServer;
                this.isClient = isClient;
                this.permissionLevel = permissionLevel;
            }

            public static CommandAttribute GetCommandAttribute(MethodInfo mi)
            {
                return mi.GetCustomAttribute<CommandAttribute>();
            }
            
        }
        
        [AttributeUsage(AttributeTargets.Class, Inherited = false)]
        public sealed class CommandHolderAttribute : Attribute
        {
            // See the attribute guidelines at 
            //  http://go.microsoft.com/fwlink/?LinkId=85236
        }
        
        #endregion
    }
}