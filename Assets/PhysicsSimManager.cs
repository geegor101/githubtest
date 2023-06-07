using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsSimManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Physics.Simulate(Time);
        //Rigidbody rigidbody;
        //rigidbody.constraints = 
        ArticulationBody body;
        
    }

    // Update is called once per frame

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

}
