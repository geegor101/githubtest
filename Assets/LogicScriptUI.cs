using System;
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

        var botBtns = uiDocument.rootVisualElement.Q<VisualElement>("botBtns");
        var topBtns = uiDocument.rootVisualElement.Q<VisualElement>("topBtns");
        var mainButtonContainer = uiDocument.rootVisualElement.Q<VisualElement>("mainButtonContainer");
        
        var attackButton = uiDocument.rootVisualElement.Q<Button>("attackButton");
        var talkButton = uiDocument.rootVisualElement.Q<Button>("talkButton");
        var actionButton = uiDocument.rootVisualElement.Q<Button>("actionButton");
        
        var mainAttackButtons = new List<Button>();
        mainAttackButtons.Add(attackButton);
        mainAttackButtons.Add(talkButton);
        mainAttackButtons.Add(actionButton);
        

        //Buttons for the main menu

        //Buttons for the talk menu

        //callBacks for the buttons
        
        
        var itsNotYouItsMe = makeButton("It's not you, it's me");
        var iNeedSomeTime = makeButton("I need some time");
        var iLoveYou = makeButton("I love you");
        var doNothing = makeButton("Nothing");
        
        var talkButtons = new List<Button>();
        talkButtons.Add(itsNotYouItsMe);
        talkButtons.Add(iNeedSomeTime);
        talkButtons.Add(iLoveYou);
        talkButtons.Add(doNothing);
        
        itsNotYouItsMe.clicked += () =>
        {
            Debug.Log("itsNotYouItsMe Clicked");
            loveBar.value += 0.1f;

            swapFromTalkOptionsToMainOptions(talkButtons, mainAttackButtons, botBtns);
        };
        
        iNeedSomeTime.clicked += () =>
        {
            Debug.Log("iNeedSomeTime Clicked");
            loveBar.value += 0.1f;
            
            swapFromTalkOptionsToMainOptions(talkButtons, mainAttackButtons, botBtns);
        };
        
        iLoveYou.clicked += () =>
        {
            Debug.Log("iLoveYou Clicked");
            swapFromTalkOptionsToMainOptions(talkButtons, mainAttackButtons, botBtns);
        };
        
        doNothing.clicked += () =>
        {
            Debug.Log("doNothing Clicked");
            loveBar.value += 0.1f;
            
            swapFromTalkOptionsToMainOptions(talkButtons, mainAttackButtons, botBtns);
        };
        
        //Buttons for the action menu
        var makeCookiesForSister = makeButton("Make cookies for sister");
        var makeCookiesForMe = makeButton("Make cookies for me");
        var spendTimeTogether = makeButton("Spend time together");
        var doNothingAction = makeButton("Nothing");
        
        var actionButtons = new List<Button>();
        actionButtons.Add(makeCookiesForSister);
        actionButtons.Add(makeCookiesForMe);
        actionButtons.Add(spendTimeTogether);
        actionButtons.Add(doNothingAction);
        
        makeCookiesForSister.clicked += () =>
        {
            Debug.Log("makeCookiesForSister Clicked");
            loveBar.value += 0.1f;
            
            swapFromTalkOptionsToMainOptions(actionButtons, mainAttackButtons, botBtns);
        };
        
        makeCookiesForMe.clicked += () =>
        {
            Debug.Log("makeCookiesForMe Clicked");
            loveBar.value += 0.1f;
            
            swapFromTalkOptionsToMainOptions(actionButtons, mainAttackButtons, botBtns);
        };
        
        spendTimeTogether.clicked += () =>
        {
            Debug.Log("spendTimeTogether Clicked");
            loveBar.value += 0.1f;
            
            swapFromTalkOptionsToMainOptions(actionButtons, mainAttackButtons, botBtns);
        };
        
        doNothingAction.clicked += () =>
        {
            Debug.Log("doNothingAction Clicked");
            loveBar.value += 0.1f;
            
            swapFromTalkOptionsToMainOptions(actionButtons, mainAttackButtons, botBtns);
        };
        
        
        loveBar.style.backgroundColor = Color.red;
        loveBar.style.unityBackgroundImageTintColor = Color.red;
        loveBar.style.color = Color.red;
        
        //attackButton.SetEnabled(false);
        loveBar.value = 5f;

        uiDocument.rootVisualElement.Q<Button>("attackButton").clicked += () =>
        {
            Debug.Log("attackButton Clicked");
            loveBar.value += 0.1f;
            //topBtns.RemoveFromHierarchy();
            mainButtonContainer.Add(botBtns);
        };
        
        uiDocument.rootVisualElement.Q<Button>("talkButton").clicked += () =>
        {
            Debug.Log("talkButton Clicked");
            attackButton.RemoveFromHierarchy();
            talkButton.RemoveFromHierarchy();
            actionButton.RemoveFromHierarchy();
            
            topBtns.Add(itsNotYouItsMe);
            topBtns.Add(iNeedSomeTime);
            botBtns.Add(iLoveYou);
            botBtns.Add(doNothing);
            
        };
        
        uiDocument.rootVisualElement.Q<Button>("actionButton").clicked += () =>
        {
            Debug.Log("actionButton Clicked");
            
            attackButton.RemoveFromHierarchy();
            talkButton.RemoveFromHierarchy();
            actionButton.RemoveFromHierarchy();
            
            topBtns.Add(makeCookiesForSister);
            topBtns.Add(makeCookiesForMe);
            botBtns.Add(spendTimeTogether);
            botBtns.Add(doNothingAction);
            

        };
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    
    void swapFromTalkOptionsToMainOptions(List<Button> talkButtons, List<Button> mainAttackButtons, VisualElement botBtns)
    {
        foreach (var button in talkButtons)
        {
            button.RemoveFromHierarchy();
        }
            
        foreach (var button in mainAttackButtons)
        {
            botBtns.Add(button);
        }   
    }
    
    Button makeButton(string text, Action onClick = null)
    {
        Button button = new Button();
        button.AddToClassList("button");

        if (onClick != null)
        {
            button.clicked += onClick;
        }
   
        button.text = text;
        return button;
    }
}
