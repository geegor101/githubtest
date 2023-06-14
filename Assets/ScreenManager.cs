using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class ScreenManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreDisplay;

    [SerializeField] private TMP_Text gameOverText;

    [SerializeField] private Button gameOverButton;

    [SerializeField] private Button quitButton;

    [SerializeField] private Button ToggleA;
    [SerializeField] private Button ToggleB;
    [SerializeField] private Button AddScore;
    

    public static ScreenManager Instance;
    
    private void Start()
    {
        //TODO CALL HERE
        gameOverButton.onClick.AddListener(OnGameOverButton);
        //Add any callbacks for restarting the game here
        
        
        quitButton.onClick.AddListener(Application.Quit);
        
        
        ToggleA.onClick.AddListener(EnableButtons);
        ToggleB.onClick.AddListener(EndGame);
        AddScore.onClick.AddListener(() => addToScore(1));
        
        
        EnableButtons();
        //DisableButtons();
        Instance = this;
    }
    
    //Here is what a gameover callback can look like
    private void OnGameOverButton()
    {
        EnableButtons();
    }

    //TODO CALL HERE
    public void EndGame()
    {
        gameOverText.text = "Game over! Score: " + score;
        score = 0;
        DisableButtons();
        UpdateScoreDisp();
    }
    
    //TODO CALL HERE
    public int score { get; private set; }

    public void SetScore(int i)
    {
        score = i;
        UpdateScoreDisp();
    }

    public void addToScore(int i)
    {
        score += i;
        UpdateScoreDisp();
    }
    
    private void EnableButtons()
    {
        scoreDisplay.enabled = true;
        gameOverText.enabled = false;

        gameOverButton.image.enabled = false;
        gameOverButton.enabled = false;
        gameOverButton.GetComponentInChildren<TMP_Text>().enabled = false;
        
        quitButton.image.enabled = false;
        quitButton.enabled = false;
        quitButton.GetComponentInChildren<TMP_Text>().enabled = false;
        
    }

    private void DisableButtons()
    {
        scoreDisplay.enabled = false;
        gameOverText.enabled = true;
        
        gameOverButton.enabled = true;
        gameOverButton.image.enabled = true;
        gameOverButton.GetComponentInChildren<TMP_Text>().enabled = true;
        
        quitButton.enabled = true;
        quitButton.image.enabled = true;
        quitButton.GetComponentInChildren<TMP_Text>().enabled = true;
    }
    
    private void UpdateScoreDisp()
    {
        scoreDisplay.text = "Score: " + score;
    }

    


}
