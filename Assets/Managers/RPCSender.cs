using System;
using Console;
using FishNet;
using FishNet.Broadcast;
using FishNet.Connection;


public static class RPCSender
{

    public static void initializePackets()
    {
        RegisterBroadcastC2S<ConsoleLogger.ChatMessage>(ConsoleLogger.SendChatMessage);
        RegisterBroadcastS2C<ConsoleLogger.ChatMessage>(ConsoleLogger.ReceiveChatMessage, false);
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
