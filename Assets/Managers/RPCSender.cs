using System;
using System.Collections;
using System.Collections.Generic;
using Console;
using FishNet;
using FishNet.Broadcast;
using FishNet.Connection;
using FishNet.Object;
using Managers;
using UnityEngine;

public static class RPCSender
{

    public static void initializePackets()
    {
        RegisterBroadcastC2S<ConsoleLogger.ChatMessage>(ConsoleLogger.sendChatMessage);
        RegisterBroadcastS2C<ConsoleLogger.ChatMessage>(ConsoleLogger.recieveChatMessage, false);

        (new ConsoleLogger.ChatChannel("a")).Connections.Add(InstanceFinder.ClientManager.Connection);

    }
    
    
    public static void RegisterBroadcastC2S<T>(Action<T> action) where T : struct, IBroadcast
    {
        InstanceFinder.ClientManager.RegisterBroadcast(action);
    }
    
    public static void RegisterBroadcastS2C<T>(Action<NetworkConnection, T> action, bool reqAuth = true) where T : struct, IBroadcast
    {
        InstanceFinder.ServerManager.RegisterBroadcast(action, reqAuth);
    }
    
}
