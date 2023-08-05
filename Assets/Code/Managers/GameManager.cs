using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Code;
using Code.Console;
using Code.Console.UI;
using Code.Networking;
using FishNet;
using FishNet.Managing;
using FishNet.Managing.Statistic;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;

namespace Managers
{
    [DisallowMultipleComponent]
    public class GameManager : MonoBehaviour
    {
        public static GameManager _instance { get; private set; }

        [SerializeField] private InputActionAsset _inputActionAsset;
        [AutofillBehavior] private NetworkManager _manager;
        private enum Startup { SERVER, CLIENT, HOST, NONE }
        [SerializeField] private Startup _startup = Startup.NONE;

        

        private void OnEnable()
        {
            this.AutofillAttributes();
            _instance = this;
            Application.logMessageReceived += UIManager.LOG;
            Debug.Log("Loading started");
            Stopwatch stopwatch = Stopwatch.StartNew();
            //UnityEngine.Device.Application.logMessageReceived += UIManager.LOG;
            
            ConsoleLogger.Initialize();
            RPCSender.initializePackets();
            InputManager.Init(_inputActionAsset);
            InitNetworking();
            NetworkThroughputUtil.Initialize();
            InitActions();
            
            stopwatch.Stop();
            Debug.Log($"Loading finished in : {stopwatch.Elapsed}");
        }

        private static void InitNetworking()
        {
            switch (_instance._startup)
            {
                case Startup.HOST :
                    InstanceFinder.ClientManager.StartConnection();
                    InstanceFinder.ServerManager.StartConnection();
                    break;
                case Startup.CLIENT : 
                    InstanceFinder.ClientManager.StartConnection();
                    break;
                case Startup.SERVER :
                    InstanceFinder.ServerManager.StartConnection(); 
                    break;
                case Startup.NONE :
                    break;
            }
        }

        

        private void InitActions()
        {
            InputManager.QuickAddInput("DEBUG/ToggleConsole", ConsoleToggle);
        }
        
        private void ConsoleToggle(InputAction.CallbackContext context)
        {
            
            UIManager.ToggleConsole();
        }
    }
    
}