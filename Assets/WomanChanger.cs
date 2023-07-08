using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshRenderer))]
public class WomanChanger : MonoBehaviour
{

    private MeshRenderer _renderer;
    [SerializeField]
    private Dictionary<string, Material> WomanVariants = new Dictionary<string, Material>();

    private static WomanChanger _womanChanger;

    void Start()
    {
        _womanChanger = this;
        _renderer = GetComponent<MeshRenderer>();
        
    }

    public static void ChangerIBarelyKnowHer(string s)
    {
        List<Material> mats = new List<Material>();
        mats.Add(_womanChanger.WomanVariants[s]);
        _womanChanger._renderer.SetMaterials(mats);
    }
    
}
