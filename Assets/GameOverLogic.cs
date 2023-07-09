using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameOverLogic : MonoBehaviour
{
    public Button replayButton = null;
    public UIDocument battleUI;
    
    // Start is called before the first frame update
    void Start()
    {
        replayButton = battleUI.rootVisualElement.Q<Button>("replayButton");
        
        replayButton.clicked += () => { Debug.Log("replayButton clicked"); };
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void displayMainMenu()
    {
        Debug.Log("displayMainMenu");
        

    }
}
