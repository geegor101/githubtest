using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class WomanWatcher : MonoBehaviour
{
    
    [SerializeField]
    private string type;

    [SerializeField] private bool running;

    private MeshRenderer _renderer;
    
    void Start()
    {
        WomanChanger.OnWomanChanged += WomanChanged;
        _renderer = GetComponent<MeshRenderer>();
        type = GetComponentInParent<WomanType>().type;
    }

    public void WomanChanged(string state, string type)
    {
        _renderer.enabled = type == this.type && state == this.name;
        running = _renderer.enabled;
    }
}


