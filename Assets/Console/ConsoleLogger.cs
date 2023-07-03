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
        
        

        [HideInInspector]
        private static Dictionary<string, ChatCommand> commands = 
            new Dictionary<string, ChatCommand>();

        private static ChatCommand missingCommand = new ChatCommand();
        
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

        private static void addDefaultCommands()
        {
            addCommand("test", (strings, info) => {Debug.Log(strings.ToString());});
        }

        public static void callCommand(string command, string[] args, CommandCallInfo info)
        {
            (commands.ContainsKey(command) ? commands[command] : missingCommand).Invoke(args, info);
        }

        public static void addCommand(string name, Action<string[], CommandCallInfo> action, bool onServer = false, bool onClient = false)
        {
            if (commands.ContainsKey(name))
                Debug.LogWarning($"Command : {name} : is registered and being overwritten!");
            commands[name] = new ChatCommand(action);
        }
        
        private void sendChatClient(string message)
        {
            var info = new CommandCallInfo();
            _input.text = "";
            //TODO: impl
            InstanceFinder.ClientManager.Broadcast(new ChatMessage(message, "a"));
        }
        
        #endregion
        
        //Should be server side only
        public static void handleChat(ChatMessage message, CommandCallInfo info)
        {

            //TODO: UNCOMMENT
            //if (ChatChannel._channels.ContainsKey(message.channelId) && ChatChannel._channels[message.channelId].canMessageInChannel(info.conn))
                
                InstanceFinder.ServerManager.Broadcast(ChatChannel._channels[message.channelId].Connections, message);
            
        }
        

        //TODO: Probably needs to send a player, and their permission level
        //Channel would also be good
        

        //S2C-R
        public static void sendChatMessage(ChatMessage chatMessage)
        {
            _instance._text.text += chatMessage.message + "\n";
        }
        
        
        //C2S-R
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
            callCommand(spl[0].Substring(1), spl, info);
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
                return Connections.Contains(id);
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
