using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    
    public static string _background { get; private set; }

    [SerializeField] private string _startingBackground;
    
    private void Start()
    {
        ChangeBackground(_startingBackground);
    }

    public static void ChangeBackground(string destination)
    {
        if (destination.Equals(_background))
            return;
        OnChangeBackground.Invoke(new BackgroundChangeContext(_background, destination)
            , new HWEventCallback());
        _background = destination;
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
