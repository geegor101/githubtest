using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FishNet;
using FishNet.Broadcast;
using FishNet.Connection;
using UnityEngine;

namespace Code.Console
{
    public static class ConsoleLogger
    {
        public delegate void CommandLoadedDelegate();

        public static event CommandLoadedDelegate OnCommandsLoaded;
        
        public static bool Initialized { get; private set; }

        public static void Initialize()
        {
            if (Initialized)
                return;
            CollectCommands();

            Initialized = true;
        }

        #region Call/Add CMDs

        private static readonly Dictionary<string, CommandInfo> Commands = new();

        private static void CollectCommands()
        {
            List<MethodInfo> holders =
                AppDomain.CurrentDomain.GetAssemblies().SelectMany(asm => asm.GetTypes())
                    .Where(type => CustomAttributeExtensions.IsDefined((MemberInfo)type, typeof(CommandHolderAttribute)))
                    .SelectMany(type =>
                        type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                    .Where(info => info.IsDefined(typeof(ConsoleAttribute)))
                    .ToList();
            
            holders.Where(info => info.IsDefined(typeof(ParserAttribute))).ForEach(Parsers.AddParser);
            holders.Where(info => info.IsDefined(typeof(CommandAttribute))).ForEach(AddCommand);

            /*
            foreach (MethodInfo methodInfo in holders.Where(info => info.IsDefined(typeof(ParserAttribute))))
            {
                Parsers.AddParser(methodInfo);
            }
            
            foreach (MethodInfo methodInfo in holders.Where(info => info.IsDefined(typeof(CommandAttribute))))
            {
                AddCommand(methodInfo);
            }
            */

            OnCommandsLoaded?.Invoke();
        }

        private static bool CallCommandC(string command, string[] args, CommandCallInfo info)
        {
            /*
            if (_commands.ContainsKey(command) && _commands[command].GetCustomAttribute<CommandAttribute>() is { } attribute)
            {
                //Debug.Log($"{attribute.isClient} C:S {attribute.isServer}, {attribute.permissionLevel}");
                if (attribute.isClient)
                    invokeCommand(command, args, info);
                return attribute.isServer;
            }
            MissingCommand(args, info);
            */
            return false;
        }

        private static void CallCommandS(string command, string[] args, CommandCallInfo info)
        {
            if (Commands.ContainsKey(command))
            {
                InvokeCommand(command, args, info);
            }
        }

        //TODO: Add metadata here

        private static void AddCommand(MethodInfo method)
        {
            CommandAttribute cmd = CommandAttribute.GetCommandAttribute(method);
            if (Commands.ContainsKey(cmd.name))
                Debug.LogWarning(
                    $"Command : {cmd.name} : is registered and being overwritten! Try adding the OverloadedCommand attribute!");
            //TODO

            if (method.IsDefined(typeof(CommandOverloadedAttribute)))
            {
            }
            else
            {
                Delegate actionType = Delegate.CreateDelegate(
                    Expression.GetActionType(method.GetParameters().Select(info => info.ParameterType).ToArray()),
                    method);
                Commands[cmd.name] = new SingleInvokerCommandInfo(actionType, cmd);
            }
        }

        /*
        private void SendChatClient(string message)
        {
            var info = new CommandCallInfo();
            //input.text = "";
            if (message.Length < 1 || message.Trim().Length == 0)
                return;
            
            if (message[0] == '$' && message.Length >= 2)
            {
                var spl = message.Split(' ');
                if (!CallCommandC(spl[0].Substring(1), spl, info))
                    return;
            }

            //TODO: impl (channels)
            InstanceFinder.ClientManager.Broadcast(new ChatMessage(message, "a"));
        }
        */

        public static void SendCommandString(string s)
        {
            string[] args = s.Split(' ');
            if (args[0].Length == 1 || args[0][0] != '$')
                return;
            InvokeCommand(args[0].Substring(1), args[1..], new CommandCallInfo());
        }


        private static void InvokeCommand(string command, string[] args, CommandCallInfo info)
        {
            if (Commands.ContainsKey(command))
            {
                Commands[command].Invoke(args, info);
            }

            /*
            if (info.conn != null && !PermissionHandler.PlayerHasPermission(_commands[command]
                    .GetCustomAttribute<CommandAttribute>().permissionLevel, info.conn))
            {
                Commands.NoPermissionError(args, info);
                return;
            }
            _commands[command].Invoke(null, new object[] { args, info });
            */
        }

        #endregion

        //Should be server side only
        private static void HandleChat(ChatMessage message, CommandCallInfo info)
        {
            if (ChatChannel.Channels.ContainsKey(message.ChannelId) &&
                ChatChannel.Channels[message.ChannelId].CanMessageInChannel(info.conn))
                InstanceFinder.ServerManager.Broadcast(ChatChannel.Channels[message.ChannelId].Connections, message);
        }

        //S2C-C
        public static void SendChatMessage(ChatMessage chatMessage)
        {
            //_instance.text.text += chatMessage.Message + "\n";
        }

        //C2S-S
        public static void ReceiveChatMessage(NetworkConnection connection, ChatMessage chatMessage)
        {
            var info = new CommandCallInfo(conn: connection);

            if (chatMessage.Message.Length == 0)
                return;

            if (chatMessage.Message[0] != '$')
            {
                //needs to determine who sees the message and send a client RPC to them
                HandleChat(chatMessage, info);
                return;
            }

            var spl = chatMessage.Message.Split(' ');
            CallCommandS(spl[0].Substring(1), spl, info);
        }

        public struct ChatMessage : IBroadcast
        {
            public string Message;
            public string ChannelId;

            public ChatMessage(string message, string channelId)
            {
                this.Message = message;
                this.ChannelId = channelId;
            }
        }

        public struct ChatChannel
        {
            public static readonly Dictionary<string, ChatChannel> Channels = new Dictionary<string, ChatChannel>();

            //Check if the client has access
            public HashSet<NetworkConnection> Connections { get; private set; }

            public ChatChannel(string name)
            {
                Connections = new HashSet<NetworkConnection>();
                Channels[name] = this;
            }


            public bool CanMessageInChannel(NetworkConnection id)
            {
                //TODO: fix
                return true; // Connections.Contains(id);
            }
        }

        public abstract class CommandInfo
        {
            public abstract void Invoke(string[] input, CommandCallInfo commandCallInfo);
        }

        private class SingleInvokerCommandInfo : CommandInfo
        {
            private Delegate _delegate;
            private Range _range;
            private int _permissionLevel;
            private bool _onServer;
            private bool _onClient;

            //Permission level
            //Sidedness

            public SingleInvokerCommandInfo(Delegate @delegate, CommandAttribute attribute)
            {
                _delegate = @delegate;
                _range = _delegate.GetMethodInfo().GetParameters().GetRange();
                _permissionLevel = attribute.permissionLevel;
                _onClient = attribute.isClient;
                _onServer = attribute.isServer;
            }

            public override void Invoke(string[] input, CommandCallInfo commandCallInfo)
            {
                //Check call info for sides and permissions
                //Fires on client only if exclusively client

                ParameterInfo[] parameters = _delegate.GetMethodInfo().GetParameters();
                object[] output = new object[parameters.Length];
                int paramNumber = 0;
                int len = 0;
                try
                {
                    if (_range.Start.Value > input.Length || _range.End.Value < input.Length)
                        throw new CommandParseException(
                            $"Number of args does not match for called command {input.Length} is not within {_range.Start.Value} and {_range.End.Value}");
                    ParameterInfo current;
                    for (int i = 0; i < input.Length; i += len)
                    {
                        current = parameters[paramNumber];

                        if (current.ParameterType.IsArray && current.IsDefined(typeof(CommandParameterLengthAttribute)))
                        {
                            //Handles array parameters

                            int paramLength = current.GetCustomAttribute<CommandParameterLengthAttribute>().range;
                            int length = Parsers.GetLength(current.ParameterType.GetElementType());
                            int numberOut = paramLength;

                            //Sets number of outputted values if the number is not capped
                            if (paramLength < 1)
                            {
                                int num = input.Length - i;
                                if (num % length != 0 || num / length < -paramLength)
                                    throw new CommandParseException(
                                        $"Incorrect number of trailing parameters, {num} is not divisible by {length}, " +
                                        $"or {num / length} was too few operations, {-paramLength} required.");
                                numberOut = num / length;
                            }

                            //Way more complex way to set up the value to be filled
                            Array arr = Array.CreateInstance(current.ParameterType.GetElementType(), numberOut);
                            for (int j = 0; j < numberOut; j++)
                            {
                                arr.SetValue(Parsers.getValue(current.ParameterType.GetElementType(),
                                    input[(i + j * length)..(i + j * length + length)],
                                    commandCallInfo), j);
                            }

                            //Filling the value into the output
                            output[paramNumber] = arr;

                            //We should break if this is the last parameter, as there *should* be no remaining strings
                            len = numberOut * length;
                            if (paramLength < 1)
                                break;
                        }
                        else
                        {
                            //Non-array parameters
                            len = Parsers.GetLength(current.ParameterType);
                            output[paramNumber] = Parsers.getValue(current.ParameterType, input[i..(i + len)],
                                commandCallInfo);
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