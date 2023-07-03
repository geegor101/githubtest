using System;
using System.Collections.Generic;
using System.Reflection;
using FishNet;
using FishNet.Broadcast;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using TMPro;
using UnityEngine;

namespace Console
{
    
    public class ConsoleLogger : MonoBehaviour
    {

        [SerializeField]
        private TMP_Text _text;
    
        [SerializeField]
        private TMP_InputField _input;

        private static ConsoleLogger _instance;

        #region MONO-B

        private void Start()
        {
            
            //_manager = GetComponent<NetworkManager>();
            _instance = this;
            _input.onSubmit.AddListener(sendChatClient);
            addDefaultCommands();
            
            Debug.Log("Console Loaded!");
            
        }
        
        private void OnEnable()
        {
            Application.logMessageReceived += log;
        }

        private void OnDisable()
        {
            Application.logMessageReceived -= log;
        }
        
        private void log(string condition, string stackTrace, LogType type)
        {
            string msg = "";
            
            switch (type)
            {
                case LogType.Warning :
                    msg += "<color=yellow>[Warn";
                    break;
                case LogType.Error :
                    msg += "<color=red>[Error";
                    break;
                case LogType.Log :
                    msg += "<color=grey>[Log";
                    break;
            }

            msg +=  $": {DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}] {condition} </color>\n";
            
            _text.text += msg;
        }
        
        #endregion

        #region Call/Add CMDs
        
        private static Dictionary<string, ChatCommand> _commands = new Dictionary<string, ChatCommand>();

        private static void addDefaultCommands()
        {
            addCommand("test", (strings, info) => {Debug.Log(strings.ToString());}, true, false);
            addCommand("start", (strings, info) => {
                if (strings.Length != 2)
                {
                    InvalidArguments(strings, info);
                    return;
                }
                    switch (strings[1])
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
                        InvalidArguments(strings, info);
                        break;
                }
            }, false, true);
            addCommand("stophost", (strings, info) =>
            {
                Debug.LogWarning("Stopping host: ");
                InstanceFinder.ClientManager.StopConnection();
                InstanceFinder.ServerManager.StopConnection(true);
            }, true, true);
        }

        public static bool callCommand(string command, string[] args, CommandCallInfo info)
        {
            if (_commands.ContainsKey(command))
            {
                _commands[command].Invoke(args, info);
                return _commands[command].onServer;
            }
            MissingCommand(args, info);
            return false;
        }
        
        public static void callCommandS(string command, string[] args, CommandCallInfo info)
        {
            if (_commands.ContainsKey(command)) _commands[command].Invoke(args, info);
        }

        public static void addCommand(string name, Action<string[], CommandCallInfo> action, bool onServer = false, bool onClient = false)
        {
            if (_commands.ContainsKey(name))
                Debug.LogWarning($"Command : {name} : is registered and being overwritten!");
            _commands[name] = new ChatCommand(action);
        }
        
        private void sendChatClient(string message)
        {
            var info = new CommandCallInfo();
            _input.text = "";
            if (message.Length < 1 || message.Trim().Length == 0)
                return;
            
            if (message[0] == '$' && message.Length >= 2)
            {
                var spl = message.Split(' ');
                if (!callCommand(spl[0].Substring(1), spl, info))
                    return;
            }
            
            //TODO: impl (channels)
            InstanceFinder.ClientManager.Broadcast(new ChatMessage(message, "a"));
        }

        private static ChatCommand missingCommand = new ChatCommand(((strings, info) => 
            Debug.Log($"No command found with name: {strings[0].Substring(1)}")), false, true);

        private static Action<string[], CommandCallInfo> invalidArgs = (strings, info) => { Debug.Log("Invalid Arguments"); };
        
        private static void MissingCommand(string[] args, CommandCallInfo info)
        {
            Debug.LogWarning($"No command found with name: {args[0].Substring(1)}");
        }

        private static void InvalidArguments(string[] args, CommandCallInfo info)
        {
            string output = "";
            for (var i = 1; i < args.Length; i++)
            {
                output += args[i];
            }
            Debug.LogWarning($"Invalid command argument(s) : {output}");
        }
        
        #endregion
        
        //Should be server side only
        public static void handleChat(ChatMessage message, CommandCallInfo info)
        {
            if (ChatChannel._channels.ContainsKey(message.channelId) && ChatChannel._channels[message.channelId].canMessageInChannel(info.conn))
                InstanceFinder.ServerManager.Broadcast(ChatChannel._channels[message.channelId].Connections, message);
        }

        //S2C-C
        public static void sendChatMessage(ChatMessage chatMessage)
        {
            _instance._text.text += chatMessage.message + "\n";
        }

        //C2S-S
        public static void recieveChatMessage(NetworkConnection connection, ChatMessage chatMessage)
        {
            
            var info = new CommandCallInfo(conn:connection);
            
            if (chatMessage.message.Length == 0)
                return;

            if (chatMessage.message[0] != '$')
            {
                //needs to determine who sees the message and send a client RPC to them
                handleChat(chatMessage, info);
                return;
            }

            var spl = chatMessage.message.Split(' ');
            callCommandS(spl[0].Substring(1), spl, info);
        }
        
        public struct ChatMessage : IBroadcast
        {
            public string message;
            public string channelId;

            public ChatMessage(string message, string channelId)
            {
                this.message = message;
                this.channelId = channelId;
            }
        }
        public record CommandCallInfo(NetworkConnection conn = null);
        public struct ChatChannel
        {
            public readonly static Dictionary<string, ChatChannel> _channels = new Dictionary<string, ChatChannel>();

            //Check if the client has access
            public HashSet<NetworkConnection> Connections { get; private set; }

            public ChatChannel(string name)
            {
                Connections = new HashSet<NetworkConnection>();
                _channels[name] = this;
            }
            

            public bool canMessageInChannel(NetworkConnection id)
            {
                //TODO: FIX
                return true;// Connections.Contains(id);
            }
            
        }
        
        private class ChatCommand
        {
            public bool onServer { get; private set; }
            public bool onClient { get; private set; }
            
            private Action<string[], CommandCallInfo> cmd;

            public ChatCommand(Action<string[], CommandCallInfo> cmd, bool onServer = false, bool onClient = false)
            {
                this.cmd = cmd;
                this.onServer = onServer;
                this.onClient = onClient;
            }

            public void Invoke(string[] s, CommandCallInfo cci)
            {
                cmd.Invoke(s, cci);
            }
        }
    }
    
    
}
