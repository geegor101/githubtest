using System;
using System.Diagnostics;
using FishNet.Object;
using UnityEngine;

namespace code
{
    public class PrefabSpawner : NetworkBehaviour
    {
        [SerializeField] private int SecondsOfDelay;
        private Stopwatch _stopwatch;
        [SerializeField] private GameObject _gameObject;

        public override void OnStartServer()
        {
            base.OnStartServer();
            _stopwatch = Stopwatch.StartNew();
            TimeManager.OnFixedUpdate += TMFixedUpdate;
        }

        private void TMFixedUpdate()
        {
            if (_stopwatch.Elapsed.Seconds > SecondsOfDelay)
            {
                TimeManager.OnFixedUpdate -= TMFixedUpdate;
                GameObject go = Instantiate(_gameObject);
                ServerManager.Spawn(go);
            }
        }
    }
}