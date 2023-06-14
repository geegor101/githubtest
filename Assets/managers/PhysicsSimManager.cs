using System.Collections.Generic;
using UnityEngine;

namespace managers
{
    public static class PhysicsSimManager
    {

        /*
         * //Physics.Simulate(Time);
            //Rigidbody rigidbody;
            //rigidbody.constraints = 
            ArticulationBody body;
         */

        public static List<Rigidbody> rigidBodies = new List<Rigidbody>();
    
    
        public static bool physEnabled { get; private set; } = true;
    
        public static void EnablePhysics()
        {
            if (!physEnabled)
                rigidBodies.ForEach(rigidbody1 => rigidbody1.constraints = RigidbodyConstraints.None);
            physEnabled = true;
        }

        public static void DisablePhysics()
        {
            if (physEnabled)
                rigidBodies.ForEach(rigidbody1 => rigidbody1.constraints = RigidbodyConstraints.FreezeAll);
            physEnabled = false;
        }

        public static void AddRigidBody(Rigidbody rigidbody)
        {
            rigidBodies.Add(rigidbody);
            rigidbody.constraints = physEnabled ? RigidbodyConstraints.FreezeAll : RigidbodyConstraints.None;
            
        }

    }
}
