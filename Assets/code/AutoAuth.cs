using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Authenticating;
using FishNet.Connection;
using UnityEngine;

public class AutoAuth : HostAuthenticator
{
    public override event Action<NetworkConnection, bool> OnAuthenticationResult;
    protected override void OnHostAuthenticationResult(NetworkConnection conn, bool authenticated)
    {
        
    }
}
