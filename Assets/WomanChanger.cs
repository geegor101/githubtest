using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering;
using UnityEngine.ResourceManagement.ResourceLocations;


[RequireComponent(typeof(MeshRenderer))]
public class WomanChanger : MonoBehaviour
{

    private MeshRenderer _renderer;
    
    
    [SerializeField]
    public Dictionary<string, Texture> WomanVariants = new Dictionary<string, Texture>();

    public static WomanChanger _womanChanger;

    public static string WomanType;
    //[SerializeField] private Texture _texture;
    
    void Start()
    {
        _womanChanger = this;
        _renderer = GetComponent<MeshRenderer>();
        
        /*
        RenderTexture renderTexture = new RenderTexture(512, 512, 24, RenderTextureFormat.Default);
        Resources.Load<Texture>("Assets/Materials/image.png");
        Addressables.LoadAsset<Texture>("");
        //Addressables.LoadAssetsAsync<Texture>("")
        
        List<string> keys = new List<string>();
        keys.Add("Woman");
            //
        Addressables.LoadAssetsAsync<Texture>(keys, addr =>
        {
            if (addr == null)
                return;
            
            RenderTexture renderTexture = new RenderTexture(addr.width, addr.height, 24);
            Graphics.Blit();
            renderTexture.Create();
            
            WomanVariants[addr.name] = addr;
            //_texture = addr;
        }, Addressables.MergeMode.Union, false).Completed += h => ChangerIBarelyKnowHer("image");
        
        */
        //renderTexture.Create();
        //WomanVariants.Add("", renderTexture);
    }

    public static void SelectWoman(string woman)
    {
        WomanType = woman;
        ChangerIBarelyKnowHer("default");
    }

    /*
     * loss
     * victory
     * damaged
     */
    
    public static void ChangerIBarelyKnowHer(string s)
    {
        //List<Material> mats = new List<Material>();
        //mats.Add(_womanChanger.WomanVariants[s]);
        //_womanChanger._renderer.material.SetTexture("_MainTex", _womanChanger.WomanVariants[s]);
        //_womanChanger._renderer.material.mainTexture = _womanChanger.WomanVariants[s];
        //Graphics.Blit(_womanChanger.WomanVariants[s], _womanChanger._renderer.material);
        //_womanChanger._renderer.material.SetTexture("_MainTex", _womanChanger.WomanVariants[s]);
        OnWomanChanged?.Invoke(s, WomanType);
    }

    public delegate void WomanChangedDelegate(string target, string womanType);

    public static event WomanChangedDelegate OnWomanChanged;

}
