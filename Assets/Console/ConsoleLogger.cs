using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FishNet;
using FishNet.Broadcast;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using Object = System.Object;

namespace Console
{
    [DisallowMultipleComponent]
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
        
        private static readonly Dictionary<string, MethodInfo> _commands = new Dictionary<string, MethodInfo>();
        

        private static void addDefaultCommands()
        {
            MethodInfo cache = null;
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes().Where(type => type.IsDefined(typeof(CommandHolderAttribute))))
            {
                MethodInfo[] methodInfos = type.GetMethods();
                foreach (MethodInfo methodInfo in methodInfos.Where(info => info.IsDefined(typeof(CommandAttribute))))
                {
                    //TODO: Make this use the new system
                    addCommand(methodInfo);
                    cache = methodInfo;

                    if (methodInfo.IsDefined(typeof(CommandOverloadedAttribute)))
                    {
                        
                    }
                    else
                    {
                        
                    }

                }
                
                foreach (MethodInfo methodInfo in methodInfos.Where(info => info.IsDefined(typeof(ParserAttribute))))
                {
                    Parsers.AddParser(methodInfo);
                }
            }
            var a = new SingleInvokerCommandInfo(Delegate.CreateDelegate(Expression.GetActionType(cache
                .GetParameters()
                .Select(info => info.ParameterType).ToArray()), cache));
            a.Invoke(new []{"3", "4", "5", "6", "2", "5", "4"});
            /*
            Debug.Log($"{Parsers.TryParse<string>(new []{"Generic Parse Worked!"})}");
            Debug.Log($"{Parsers.getValue(typeof(string), new []{"Non generic worked!"})}");
            */
        }

        private static bool callCommandC(string command, string[] args, CommandCallInfo info)
        {
            if (_commands.ContainsKey(command) && _commands[command].GetCustomAttribute<CommandAttribute>() is { } attribute)
            {
                //Debug.Log($"{attribute.isClient} C:S {attribute.isServer}, {attribute.permissionLevel}");
                if (attribute.isClient)
                    invokeCommand(command, args, info);
                return attribute.isServer;
            }
            MissingCommand(args, info);
            return false;
        }
        
        private static void callCommandS(string command, string[] args, CommandCallInfo info)
        {
            if (_commands.ContainsKey(command))
            {
                invokeCommand(command, args, info);
            }
        }
        
        //TODO: Add metadata here

        private static void addCommand(MethodInfo action)
        {
            var cmd = CommandAttribute.GetCommandAttribute(action);
            if (_commands.ContainsKey(cmd.name))
                Debug.LogWarning($"Command : {cmd.name} : is registered and being overwritten! Try adding the OverloadedCommand attribute!");
            _commands[cmd.name] = action;
            //action.GetParameters().Where(info => info.GetType())
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
                if (!callCommandC(spl[0].Substring(1), spl, info))
                    return;
            }

            //TODO: impl (channels)
            InstanceFinder.ClientManager.Broadcast(new ChatMessage(message, "a"));
        }

        private static void MissingCommand(string[] args, CommandCallInfo info)
        {
            Debug.LogWarning($"No command found with name: {args[0].Substring(1)}");
        }

        private static void invokeCommand(string command, string[] args, CommandCallInfo info)
        {
            if (info.conn != null && !PermissionHandler.PlayerHasPermission(_commands[command]
                    .GetCustomAttribute<CommandAttribute>().permissionLevel, info.conn))
            {
                Commands.NoPermissionError(args, info);
                return;
            }
            _commands[command].Invoke(null, new object[] { args, info });
        }
        

        #endregion
        
        //Should be server side only
        private static void handleChat(ChatMessage message, CommandCallInfo info)
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
        public record CommandCallInfo([CanBeNull] NetworkConnection conn = null);
        public struct ChatChannel
        {
            public static readonly Dictionary<string, ChatChannel> _channels = new Dictionary<string, ChatChannel>();

            //Check if the client has access
            public HashSet<NetworkConnection> Connections { get; private set; }

            public ChatChannel(string name)
            {
                Connections = new HashSet<NetworkConnection>();
                _channels[name] = this;
            }
            

            public bool canMessageInChannel(NetworkConnection id)
            {
                //TODO: fix
                return true;// Connections.Contains(id);
            }
            
        }

        public abstract class CommandInfo
        {
            public abstract void Invoke(string[] input);
            
            
        }
        
        public class SingleInvokerCommandInfo : CommandInfo
        {
            
            private Delegate _delegate;
            private Range _range;

            public SingleInvokerCommandInfo(Delegate @delegate)
            {
                _delegate = @delegate;
                _range = _delegate.GetMethodInfo().GetParameters().GetRange();
            }

            public override void Invoke(string[] input)
            {
                CommandCallInfo commandCallInfo = new CommandCallInfo();
                
                //MethodInfo methodInfo = _delegate.GetMethodInfo();
                ParameterInfo[] parameters = _delegate.GetMethodInfo().GetParameters();
                object[] output = new object[parameters.Length];
                int paramNumber = 0;
                int len = 0;
                try
                {
                    //Check length here
                    if (_range.Start.Value > input.Length || _range.End.Value < input.Length)
                        throw new CommandParseException($"Number of args does not match for called command {input.Length} is not within {_range.Start.Value} and {_range.End.Value}");
                    
                    ParameterInfo current;
                    for (int i = 0; i < input.Length; i+=len)
                    {
                        current = parameters[paramNumber];
                        
                        if (current.ParameterType.IsArray && current.IsDefined(typeof(CommandParameterLengthAttribute)))
                        {
                            int paramLength = current.GetCustomAttribute<CommandParameterLengthAttribute>().range;
                            int length = Parsers.GetLength(current.ParameterType.GetElementType());
                            int numberOut = paramLength;
                            if (paramLength < 1)
                            {
                                int num = input.Length - i;
                                if (num % length != 0 || num / length < - paramLength)
                                    throw new CommandParseException($"Incorrect number of trailing parameters, {num} is not divisible by {length}, " +
                                                                    $"or {num / length} was too few operations, {-paramLength} required.");
                                numberOut = num / length;
                            }
                            var arr = Array.CreateInstance(current.ParameterType.GetElementType(), numberOut);
                            for (int j = 0; j < numberOut; j++)
                            {
                                arr.SetValue(Parsers.getValue(current.ParameterType.GetElementType(), 
                                    input[(i + j * length)..(i + j * length + length)],
                                    commandCallInfo), j);
                            }
                            output[paramNumber] = arr;
                            if (paramLength < 1)
                                break;
                        }
                        else
                        {
                            len = Parsers.GetLength(current.ParameterType);
                            output[paramNumber] = Parsers.getValue(current.ParameterType, input[i..(i+len)], commandCallInfo);
                        }
                        paramNumber++;
                    }
                    output[^1] = commandCallInfo;
                    _delegate.DynamicInvoke(output);
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e);
                    // ignored
                }
            }
        }
    }
    
    
}
