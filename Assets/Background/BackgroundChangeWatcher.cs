using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

public class BackgroundChangeWatcher : MonoBehaviour
{
    
    [SerializeField]
    public string[] activeBackgrounds = new string[]{};

    private void OnEnable()
    {
        BackgroundManager.OnChangeBackground += OnBackgroundChange;
    }

    private void OnDisable()
    {
        BackgroundManager.OnChangeBackground -= OnBackgroundChange;
    }


    protected virtual void OnBackgroundChange(BackgroundManager.BackgroundChangeContext context, HWEventCallback callback)
    {
        
    }
}
