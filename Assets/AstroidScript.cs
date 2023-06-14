using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroidScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D myRigidBody;
    public GameObject centerTarget;
    void Start()
    {
        myRigidBody.velocity = (centerTarget.transform.position - this.transform.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
