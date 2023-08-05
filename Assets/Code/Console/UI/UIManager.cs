using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using FishNet.Utility.Performance;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

namespace Code.Console.UI
{
    public class UIManager : MonoBehaviour
    {
        // Start is called before the first frame update
        [AutofillBehavior] private UIDocument _UIDocument;
        [AutofillUIElement("ChannelSelector")] private static DropdownField _channelSelector;
        [AutofillUIElement("ConsoleInput")] private static TextField _consoleInput;
        [AutofillUIElement("ConsoleOutput")] private static ListView _consoleOutput;
        [AutofillUIElement("ConsoleWindow")] private static VisualElement _consoleWindow;

        private static ObjectPool<VisualElement> _labelPool =
            new ObjectPool<VisualElement>(MakeConsoleLabel, defaultCapacity: 25, maxSize: 60);

        private static UIManager _instance;

        private static readonly List<string> ConsoleOutputStrings = new List<string>() { };

        private delegate void ConsoleLoggedDelegate();

        private static event ConsoleLoggedDelegate ConsoleLoggedEvent;

        private void OnEnable()
        {
            this.AutofillAttributes();
            this.AutofillUIElements(_UIDocument);
            _instance = this;
            SetupConsoleOutput();
            HideConsole();
        }


        private static void ReloadConsole()
        {
            //_consoleOutput.MarkDirtyRepaint();
            _consoleOutput.RefreshItems();
        }

        //Maybe move this to the logger?
        public static void LOG(string condition, string stackTrace, LogType type)
        {
            string msg = "";
            switch (type)
            {
                case LogType.Warning:
                    msg += "<color=yellow>[Warn";
                    break;
                case LogType.Error:
                    msg += "<color=red>[Error";
                    break;
                case LogType.Log:
                    msg += "<color=blue>[Log";
                    break;
            }

            msg += $": {DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}] {condition} </color>";
            ReceiveConsoleMessage(msg, new ConsoleInfo(MessageSource.CONSOLE));
            //_instance.IfEnabled(manager => manager._consoleOutput.MarkDirtyRepaint());
        }

        public static void ReceiveConsoleMessage(string message, ConsoleInfo info)
        {
            if (info.Source == MessageSource.PLAYER)
            {
                message = Regex.Replace(message, @"[<][^>]*[>]", "");
            }

            ConsoleOutputStrings.Add(message);
            ConsoleLoggedEvent?.Invoke();
        }

        public record ConsoleInfo(MessageSource Source)
        {
        }

        public enum MessageSource
        {
            PLAYER,
            CONSOLE
        }

        private void SetupConsoleOutput()
        {
            //Add options for party? maybe add if someone sends a message and you want to reply?

            _consoleWindow.usageHints &= UsageHints.GroupTransform;
            _consoleWindow.usageHints &= UsageHints.DynamicTransform;
            foreach (VisualElement visualElement in _consoleWindow.Children())
            {
                visualElement.usageHints &= UsageHints.DynamicTransform;
            }

            _channelSelector.choices = new List<string>() { "Team", "Global" };
            _channelSelector.index = 0;

            _consoleOutput.horizontalScrollingEnabled = false;

            _consoleOutput.makeItem += GetConsoleLabel;
            _consoleOutput.bindItem += BindConsoleLabel;
            //_consoleOutput.unbindItem += ReleaseConsoleLabel;
            _consoleOutput.destroyItem += ReleaseConsoleLabel;

            _consoleOutput.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
            _consoleOutput.itemsSource = ConsoleOutputStrings;


            _consoleInput.RegisterCallback<KeyDownEvent>(KeyDownTextEvent);

            //Insert on vis element can change parentage
            //Maybe have multiple modes for console
        }

        public static bool ConsoleEnabled { get; private set; }

        public static void ToggleConsole()
        {
            if (ConsoleEnabled)
                HideConsole();
            else
                ShowConsole();
        }

        public static void HideConsole()
        {
            ConsoleLoggedEvent -= ReloadConsole;
            ConsoleEnabled = false;
            _consoleWindow.SetEnabled(false);
            _consoleWindow.visible = false;
            _consoleInput.visible = false;
        }

        public static void ShowConsole()
        {
            ConsoleLoggedEvent += ReloadConsole;
            ConsoleEnabled = true;
            _consoleWindow.SetEnabled(true);
            _consoleWindow.visible = true;
            _consoleInput.visible = true;
        }

        private void KeyDownTextEvent(KeyDownEvent @event)
        {
            if (@event.keyCode == KeyCode.KeypadEnter || @event.character == '\n')
            {
                SendTextFromConsole(_consoleInput.text);
                _consoleInput.value = "";
                @event.StopPropagation();
                @event.PreventDefault();
            }
        }

        private static void SendTextFromConsole(string text)
        {
            if (text.Length == 0)
                return;
            if (text[0] == '$')
                ConsoleLogger.SendCommandString(text);
        }

        private static VisualElement GetConsoleLabel()
        {
            return _labelPool.Get();
        }

        private static void ReleaseConsoleLabel(VisualElement element)
        {
            _labelPool.Release(element);
            (element as Label).text = "";
        }

        private static VisualElement MakeConsoleLabel()
        {
            Label label = new Label();
            //label.enableRichText = true;
            label.name = "ConsoleText";
            return label;
        }

        private static void BindConsoleLabel(VisualElement label, int i)
        {
            (label as Label).text = ConsoleOutputStrings[i];
        }
    }
}

namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit
    {
    }
}