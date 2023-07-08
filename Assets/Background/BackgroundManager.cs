using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BackgroundManager : MonoBehaviour
{
    
    public static string _background { get; private set; }

    //[SerializeField] private string _startingBackground;
    
    /*
     * Viable Backgrounds
     * Title
     * End
     * Game
     */

    public static void StartGame()
    {
        ChangeBackground("Game");
    }

    public static void EndGame()
    {
        ChangeBackground("End");
    }

    public static void TitleScreen()
    {
        ChangeBackground("Title");
    }
    
    public static void ChangeBackground(string destination)
    {
        if (destination.Equals(_background))
            return;
        OnChangeBackground.Invoke(new BackgroundChangeContext(_background, destination)
            , new HWEventCallback());
        _background = destination;
    }

    #region Internal

    private void Start()
    {
        //TitleScreen();
        StartGame();
    }

    public delegate void BackgroundChangeDelegate(BackgroundChangeContext context, HWEventCallback callback);
    public static event BackgroundChangeDelegate OnChangeBackground;

    public record BackgroundChangeContext(string current, string dest)
    {
        public static implicit operator string(BackgroundChangeContext ctx)
        {
            return ctx.dest;
        }
    }

    #endregion
    
}
