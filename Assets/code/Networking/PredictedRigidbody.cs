using FishNet.Object;
using FishNet.Object.Prediction;
using FishNet.Transporting;
using UnityEngine;

namespace code.Networking
{
    public class PredictedRigidbody : NetworkBehaviour
    {
        /*
        [SerializeField] private Vector3 offset = Vector3.zero;


        public struct MoveData : IReplicateData
        {
            private uint _tick;

            public void Dispose()
            {
            }

            public uint GetTick() => _tick;
            public void SetTick(uint value) => _tick = value;
        }

        public struct ReconcileData : IReconcileData
        {
            public Vector3 Position;
            public Quaternion Rotation;
            public Vector3 Velocity;
            public Vector3 AngularVelocity;

            public ReconcileData(Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angularVelocity)
            {
                Position = position;
                Rotation = rotation;
                Velocity = velocity;
                AngularVelocity = angularVelocity;
                _tick = 0;
            }

            private uint _tick;

            public void Dispose()
            {
            }

            public uint GetTick() => _tick;
            public void SetTick(uint value) => _tick = value;
        }


        private Rigidbody _rigidbody;

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();
            _rigidbody = GetComponent<Rigidbody>();
            base.TimeManager.OnPostTick += TimeManager_OnPostTick;
            base.TimeManager.OnTick += TM_PreTick;
        }

        public override void OnStopNetwork()
        {
            base.OnStopNetwork();
            base.TimeManager.OnPostTick -= TimeManager_OnPostTick;
            base.TimeManager.OnTick -= TM_PreTick;
        }

        private void TM_PreTick()
        {
            //_rigidbody.AddForce(offset);

            //Physics.Simulate(0.17f);

            //_rigidbody.position = new Vector3(0, 10, 3);
        }


        private void TimeManager_OnPostTick()
        {
            //Does not move using input, this is only a reactive object so pass in default to move.
            MoveData moveData = default(MoveData);
            Move(moveData);
            //Send reconcile per usual.
            if (IsServer)
            {
                ReconcileData rd = new ReconcileData(transform.position, transform.rotation, _rigidbody.velocity,
                    _rigidbody.angularVelocity);
                Reconciliation(rd);
            }
        }


        [ReplicateV2]
        private void Move(MoveData md, ReplicateState state = ReplicateState.Invalid,
            Channel channel = Channel.Unreliable)
        {
        }


        [ReconcileV2]
        private void Reconciliation(ReconcileData rd, Channel channel = Channel.Unreliable)
        {
            transform.position = rd.Position;
            transform.rotation = rd.Rotation;
            _rigidbody.velocity = rd.Velocity;
            _rigidbody.angularVelocity = rd.AngularVelocity;
        }
        */
        
        [AutofillBehavior] private Rigidbody _rigidbody;

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();
            this.AutofillAttributes();
            TimeManager.OnTick += TM_OnTick;
            TimeManager.OnPostTick += TM_OnPostTick;
            
        }

        public override void OnStopNetwork()
        {
            base.OnStopNetwork();
            TimeManager.OnTick -= TM_OnTick;
            TimeManager.OnPostTick -= TM_OnPostTick;
        }


        private void TM_OnTick()
        {
            //TimeManager.PhysicsMode
        }
        
        private void TM_OnPostTick()
        {
            Move(default);
            if (IsServer)
            {
                var data = new RigidbodyReconcileData();
                data.Position = transform.position;
                data.Rotation = transform.rotation;
                data.AngularVelocity = _rigidbody.angularVelocity;
                data.Velocity = _rigidbody.velocity;
                Reconcile(data);
            }
        }

        [ReplicateV2]
        private void Move(EmptyMoveData emptyMoveData, ReplicateState state = ReplicateState.Invalid,
            Channel channel = Channel.Unreliable) {}

        [ReconcileV2]
        private void Reconcile(RigidbodyReconcileData rigidbodyReconcileData, Channel channel = Channel.Unreliable)
        {
            _rigidbody.position = rigidbodyReconcileData.Position;
            _rigidbody.rotation = rigidbodyReconcileData.Rotation;
            _rigidbody.angularVelocity = rigidbodyReconcileData.AngularVelocity;
            _rigidbody.velocity = rigidbodyReconcileData.Velocity;
        }
        
        
        //Copy paste me :)
        public struct EmptyMoveData : IReplicateData
        {
            private uint _tick;
            public uint GetTick() => _tick;
            public void SetTick(uint value) => _tick = value;
            public void Dispose() {}
        }
        
        public struct EmptyReconcileData : IReconcileData
        {
            private uint _tick;
            public uint GetTick() => _tick;
            public void SetTick(uint value) => _tick = value;
            public void Dispose() {}
        }
        
        public struct RigidbodyReconcileData : IReconcileData
        {
            public Vector3 Position;
            public Quaternion Rotation;
            public Vector3 Velocity;
            public Vector3 AngularVelocity;
            
            private uint _tick;
            public uint GetTick() => _tick;
            public void SetTick(uint value) => _tick = value;
            public void Dispose() {}
        }
        
    }
}