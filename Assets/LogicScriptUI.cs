using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Content;
using DefaultNamespace;
using UnityEditor.TestTools.CodeCoverage;
using UnityEngine;
using UnityEngine.UIElements;

using static GameManager;

public class LogicScriptUI : BackgroundChangeWatcher
{
    // Start is called before the first frame update
    private UIDocument uiDocument;
    public TalkInput currentTalkInput = TalkInput.NONE;
    public ActionInput currentActionInput = ActionInput.NONE;
    public List<(TalkInput, ActionInput)> pastActions = new List<(TalkInput, ActionInput)>();
    public Label turnLabel;
    private static LogicScriptUI _instance;
    
    private Label dialog;
    public static void SendDialog(string text, bool you)
    {
        _instance.dialog.text += $"{(you ? "You" : "Alice" )}: {text}";
    }
    
    private ProgressBar loveBar;
    private ProgressBar hateBar;
    private float _loveIntenal = 0;
    private float _hateInternal = 0;

    /// Their value, goal is to send to 0
    public static float Love
    {
        get => _instance._loveIntenal;
        set
        {
            OnLoveChanged?.Invoke( _instance._loveIntenal, value);
            _instance._loveIntenal = value;
            _instance.loveBar.value = _instance._loveIntenal;
        } 
    }
    
    /// Our value, goal is to stay above 0
    public static float Hate
    {
        get => _instance._hateInternal;
        set
        {
            OnHateChanged?.Invoke(_instance._hateInternal, value);
            _instance._hateInternal = value;
            _instance.hateBar.value = _instance._hateInternal;
        } 
    }
    
    public delegate void LoveChangedDelegate(float oldValue, float newValue);
    public delegate void HateChangedDelegate(float oldValue, float newValue);

    public static event LoveChangedDelegate OnLoveChanged;
    public static event HateChangedDelegate OnHateChanged;

    public static void ReduceLove(float value)
    {
        if (_instance != null)
            Love -= value;
    }

    public static void ReduceHate(float value)
    {
        if (_instance != null)
            Hate -= value;
    }
    

    void Start()
    {
        _instance = this;
        uiDocument = GetComponent<UIDocument>();
        
        this.setDiaglogueHeader("haha");
        this.setDialogueText("peepee");
        
        currentTalkInput = TalkInput.NONE;
        currentActionInput = ActionInput.NONE;
        
        
        loveBar = uiDocument.rootVisualElement.Q<ProgressBar>("loveBar");
        hateBar = uiDocument.rootVisualElement.Q<ProgressBar>("hateBar");
        //TODO: get dialog here
        
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
        
        turnLabel = uiDocument.rootVisualElement.Q<Label>("turnLabel");
        //Default state
        attackButton.SetEnabled(false);

        //Buttons for the main menu

        //Buttons for the talk menu

        //callBacks for the buttons
        
        
        var itsNotYouItsMe = makeButton("It's not you, it's me");
        var iNeedSomeTime = makeButton("I need some time");
        var iLoveYou = makeButton("I love you");
        var doNothing = makeButton("Ignore");
        
        var talkButtons = new List<Button>();
        talkButtons.Add(itsNotYouItsMe);
        talkButtons.Add(iNeedSomeTime);
        talkButtons.Add(iLoveYou);
        talkButtons.Add(doNothing);
        
        itsNotYouItsMe.clicked += () =>
        {
            Debug.Log("itsNotYouItsMe Clicked");
            currentTalkInput = TalkInput.TALKA;

            swapFromTalkOptionsToMainOptions(talkButtons, mainAttackButtons, botBtns);
        };
        
        iNeedSomeTime.clicked += () =>
        {
            Debug.Log("iNeedSomeTime Clicked");
            currentTalkInput = TalkInput.TALKB;
            
            swapFromTalkOptionsToMainOptions(talkButtons, mainAttackButtons, botBtns);
        };
        
        iLoveYou.clicked += () =>
        {
            Debug.Log("iLoveYou Clicked");
            currentTalkInput = TalkInput.TALKC;
            
            swapFromTalkOptionsToMainOptions(talkButtons, mainAttackButtons, botBtns);
        };
        
        doNothing.clicked += () =>
        {
            Debug.Log("doNothing Clicked");
            currentTalkInput = TalkInput.TALKD;
            
            swapFromTalkOptionsToMainOptions(talkButtons, mainAttackButtons, botBtns);
        };
        
        //Buttons for the action menu
        var makeCookiesForSister = makeButton("Make cookies for sister");
        var makeCookiesForMe = makeButton("Give Gift");
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
            currentActionInput = ActionInput.ACTIONA;
            
            swapFromTalkOptionsToMainOptions(actionButtons, mainAttackButtons, botBtns);
        };
        
        makeCookiesForMe.clicked += () =>
        {
            Debug.Log("makeCookiesForMe Clicked");
            currentActionInput = ActionInput.ACTIONB;
            
            swapFromTalkOptionsToMainOptions(actionButtons, mainAttackButtons, botBtns);
        };
        
        spendTimeTogether.clicked += () =>
        {
            Debug.Log("spendTimeTogether Clicked");
            currentActionInput = ActionInput.ACTIONC;
            
            swapFromTalkOptionsToMainOptions(actionButtons, mainAttackButtons, botBtns);
        };
        
        doNothingAction.clicked += () =>
        {
            Debug.Log("doNothingAction Clicked");
            currentActionInput = ActionInput.ACTIOND;
            
            swapFromTalkOptionsToMainOptions(actionButtons, mainAttackButtons, botBtns);
        };
        
        

        uiDocument.rootVisualElement.Q<Button>("attackButton").clicked += () =>
        {
            Debug.Log("attackButton Clicked");
            GameManager.TakeTurn(currentTalkInput, currentActionInput);
            currentTalkInput = TalkInput.NONE;
            currentActionInput = ActionInput.NONE;
            pastActions.Add((currentTalkInput, currentActionInput));
            var currentTurn = pastActions.Count + 1;

            attackButton.SetEnabled(false);
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
        if (currentTalkInput != TalkInput.NONE && currentActionInput != ActionInput.NONE)
        {
            Button attackButton = uiDocument.rootVisualElement.Q<Button>("attackButton");
            attackButton?.SetEnabled(true);
        }
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
    
    public void setDialogueText(string text)
    {
        var dialogueText = uiDocument.rootVisualElement.Q<Label>("dialogueText");
        dialogueText.text = text;
    }
    
    public void setDiaglogueHeader(string text)
    {
        var dialogueHeader = uiDocument.rootVisualElement.Q<Label>("dialogueHeader");
        dialogueHeader.text = text;
    }
    
    protected override void OnBackgroundChange(BackgroundManager.BackgroundChangeContext context, HWEventCallback callback)
    {
        //uiDocument.rootVisualElement.SetEnabled(context.dest == "Game");
        //foreach (VisualElement visualElement in uiDocument.rootVisualElement.Children()) { visualElement.visible = context.dest == "Game"; }
        //uiDocument.enabled = context.dest == "Game";
    }
    

}
