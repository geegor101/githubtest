using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using code;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    [AutofillBehavior]
    public UIDocument _UIDocument;

    [AutofillUIElement("ChannelSelector")]
    public DropdownField _channelSelector;
    [AutofillUIElement("ConsoleInput")]
    public TextField _consoleInput;
    [AutofillUIElement("ConsoleOutput")]
    public ListView _consoleOutput;

    private static UIManager _instance;

    private static readonly List<string> ConsoleOutputStrings = new List<string>(){""};

    private delegate void ConsoleLoggedDelegate();

    private static event ConsoleLoggedDelegate ConsoleLoggedEvent;


    void Start()
    {
        this.AutofillAttributes();
        this.AutofillUIElements(_UIDocument);
        _instance = this;
        SetupConsoleOutput();
        
        Debug.Log("Hello there!");
        Debug.LogWarning("Hello there!");
        Debug.LogError("Hello there!");

    }

    private void OnEnable()
    {
        ConsoleLoggedEvent += ReloadConsole;
    }

    private void OnDisable()
    {
        ConsoleLoggedEvent -= ReloadConsole;
    }

    private void ReloadConsole()
    {
        _consoleOutput.MarkDirtyRepaint();
    }

    //Maybe move this to the logger?
    internal static void LOG(string condition, string stackTrace, LogType type)
    {
        string msg = "";
        switch (type)
        {
            case LogType.Warning :
                msg += "<color=yellow>[Warn";
                break;
            case LogType.Error :
                msg += "<color=red>[Error";
                break;
            case LogType.Log :
                msg += "<color=blue>[Log";
                break;
        }

        msg +=  $": {DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}] {condition} </color>\n";
        RecieveConsoleMessage(msg, new ConsoleInfo(MessageSource.CONSOLE));
        ConsoleLoggedEvent?.Invoke();
    }

    public static void RecieveConsoleMessage(string message, ConsoleInfo info)
    {
        if (info.Source == MessageSource.PLAYER)
        {
            message = Regex.Replace(message, "[<][^>]*[>]", "");
        }

        ConsoleOutputStrings.Add(message);
    }

    public record ConsoleInfo(MessageSource Source){}
    
    public enum MessageSource
    {
        PLAYER,
        CONSOLE
    }

    private void SetupConsoleOutput()
    {
        //Add options for party? maybe add if someone sends a message and you want to reply?
        _channelSelector.choices = new List<string>() {"Team", "Global"}; 
        _channelSelector.index = 0;
        
        _consoleOutput.horizontalScrollingEnabled = false;

        _consoleOutput.makeItem += MakeConsoleLabel;
        _consoleOutput.bindItem += BindConsoleLabel;

        _consoleOutput.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
        
        _consoleOutput.itemsSource = ConsoleOutputStrings;
        _consoleOutput.MarkDirtyRepaint();
    }

    private VisualElement MakeConsoleLabel()
    {
        Label label = new Label();
        //label.enableRichText = true;
        label.name = "ConsoleText";
        return label;
    }

    private void BindConsoleLabel(VisualElement label, int i)
    {
        (label as Label).text = ConsoleOutputStrings[i];
    }
    

}
