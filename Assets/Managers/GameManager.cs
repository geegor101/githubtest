using System.Diagnostics;
using System.Threading.Tasks;
using code;
using Console;
using FishNet;
using FishNet.Managing;
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
        
        private void Start()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            this.AutofillAttributes();
            _instance = this;
            Application.logMessageReceived += UIManager.LOG;
            ConsoleLogger.Initialize();
            RPCSender.initializePackets();
            InputManager.Init(_inputActionAsset);
            InitNetworking();
            stopwatch.Stop();
            Debug.Log($"Loading finished in : {stopwatch.Elapsed}");
        }

        private void InitNetworking()
        {
            switch (_startup)
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
    }
    
}