using System.Collections;
using System.Collections.Generic;
using UnityEditor.TestTools.CodeCoverage;
using UnityEngine;
using UnityEngine.UIElements;

public class LogicScriptUI : MonoBehaviour
{
    // Start is called before the first frame update
    private UIDocument uiDocument;
    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        
        ProgressBar loveBar = uiDocument.rootVisualElement.Q<ProgressBar>("loveBar");
        ProgressBar hateBar = uiDocument.rootVisualElement.Q<ProgressBar>("hateBar");
        
        var attackOptions = uiDocument.rootVisualElement.Q<VisualElement>("attackOptions");
        var battleOptions = uiDocument.rootVisualElement.Q<VisualElement>("battleOptions");
        
        var botBtns = uiDocument.rootVisualElement.Q<VisualElement>("botBtns");
        var topBtns = uiDocument.rootVisualElement.Q<VisualElement>("topBtns");
        var mainButtonContainer = uiDocument.rootVisualElement.Q<VisualElement>("mainButtonContainer");


        loveBar.style.backgroundColor = Color.red;
        loveBar.style.unityBackgroundImageTintColor = Color.red;
        loveBar.style.color = Color.red;

        Button attackButton = uiDocument.rootVisualElement.Q<Button>("attackButton");
        //attackButton.SetEnabled(false);
        attackOptions.visible = false;
        
        loveBar.value = 5f;

        uiDocument.rootVisualElement.Q<Button>("attackButton").clicked += () =>
        {
            Debug.Log("attackButton Clicked");
            loveBar.value += 0.1f;
            topBtns.RemoveFromHierarchy();
            mainButtonContainer.Add(botBtns);
        };
        
        uiDocument.rootVisualElement.Q<Button>("talkButton").clicked += () =>
        {
            Debug.Log("talkButton Clicked");
            attackOptions.visible = !attackOptions.visible;
            battleOptions.visible = !battleOptions.visible;
        };
        
        uiDocument.rootVisualElement.Q<Button>("actionButton").clicked += () =>
        {
            Debug.Log("actionButton Clicked");
            Button newButton = new Button();
            newButton.text = "New Button";
            botBtns.RemoveFromHierarchy();

        };
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
