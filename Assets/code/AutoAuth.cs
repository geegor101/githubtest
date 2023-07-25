using System;
using FishNet.Authenticating;
using FishNet.Connection;

public class AutoAuth : HostAuthenticator
{
    public override event Action<NetworkConnection, bool> OnAuthenticationResult;
    protected override void OnHostAuthenticationResult(NetworkConnection conn, bool authenticated)
    {
        
    }
}
