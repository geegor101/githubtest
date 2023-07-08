using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{


    public static void ChangeBackground()
    {
        
    }
    
    
    
        /*
        OnChangeBackground += (context, callback) => { };
        OnChangeBackground.Invoke(null, null);
        */
        
        
    public delegate void BackgroundChangeDelegate(BackgroundChangeContext context, HWEventCallback callback);
    public static event BackgroundChangeDelegate OnChangeBackground;

    public record BackgroundChangeContext(string current, string dest)
    {

        public static implicit operator string(BackgroundChangeContext ctx)
        {
            return ctx.dest;
        }
        
    }
}
