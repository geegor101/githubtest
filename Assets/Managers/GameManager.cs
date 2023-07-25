using Console;
using FishNet.Managing;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers
{
    [DisallowMultipleComponent]
    public class GameManager : MonoBehaviour
    {
        public static GameManager _instance { get; private set; }

        [SerializeField] private InputActionAsset _inputActionAsset;
        
        [SerializeField]
        public NetworkManager _manager;
        
        private void Start()
        {
            _instance = this;
            Application.logMessageReceived += UIManager.LOG;
            ConsoleLogger.Initialize();
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
            //if (InstanceFinder.IsClient)
                ClientInit();
        }


        private void ClientInit()
        {
            InputManager.Init(_inputActionAsset);
            
            //InputManager.SetFocus("GAME");
        }
    }
    
    
}