using System;
using FishNet.Managing;
using UnityEngine;

namespace Managers
{
    [DisallowMultipleComponent]
    public class GameManager : MonoBehaviour
    {
        public static GameManager _instance { get; private set; }
        
        
        [SerializeField]
        public NetworkManager _manager;
        
        private void Start()
        {
            _instance = this;
            
            RPCSender.initializePackets();
            
            
            if (_manager == null)
            {
                Debug.LogError("Network Manager is null");
            }
            else
            {
                //_manager.ServerManager.StartConnection();
                //_manager.ClientManager.StartConnection();
            }
        }
    }
}