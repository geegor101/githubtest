using System.Collections.Generic;
using UnityEngine;

namespace managers
{
    public class PhysicsSimManager
    {

        /*
         * //Physics.Simulate(Time);
            //Rigidbody rigidbody;
            //rigidbody.constraints = 
            ArticulationBody body;
         */

        public List<Rigidbody> rigidBodies = new List<Rigidbody>();
    
    
        public bool physEnabled { get; private set; } = true;
    
        public void EnablePhysics()
        {
            if (!physEnabled)
                rigidBodies.ForEach(rigidbody1 => rigidbody1.constraints = RigidbodyConstraints.None);
            physEnabled = true;
        }

        public void DisablePhysics()
        {
            if (physEnabled)
                rigidBodies.ForEach(rigidbody1 => rigidbody1.constraints = RigidbodyConstraints.FreezeAll);
            physEnabled = false;
        }

        public void AddRigidBody(Rigidbody rigidbody)
        {
            rigidBodies.Add(rigidbody);
            rigidbody.constraints = physEnabled ? RigidbodyConstraints.FreezeAll : RigidbodyConstraints.None;
            
        }

    }
}
