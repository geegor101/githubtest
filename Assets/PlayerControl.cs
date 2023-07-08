using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{

    
    
    void Start()
    {
        
    }


    void Update()
    {
        
    }
    
    /*
    
    //Stuff for Camera following
    [SerializeField] private Camera _camera;
    private Vector3 _cameraOffset;// = new Vector3(0, 4.6f, -7.25f);
    [SerializeField] 
    [Range(0, float.MaxValue)]
    private float smoothingLinear = 2f;
    [SerializeField]
    [Range(0, float.MaxValue)]
    private float smoothingExpo = 2f;
            _cameraOffset = _camera.transform.position;

    
    //Camera follower for 3p camera
    Vector3 current = _camera.transform.position;
        Vector3 targetOffset = transform.position + _cameraOffset - current;
        Debug.DrawLine(current, targetOffset + current, Color.blue);
        _camera.transform.position = (targetOffset) * 
            (Time.deltaTime * smoothingLinear * Mathf.Pow(targetOffset.magnitude, smoothingExpo)) + current;
    
     */
}
