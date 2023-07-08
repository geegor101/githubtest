using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    

    /*
     * Use these to determine validity of taken actions
     * you can read/write to Gamestatus if you want to save data
     */
    
    /// Add any status effects here about past actions
    public static readonly Dictionary<string, string> gameStatus = new Dictionary<string, string>();
    /// Reference any previous inputs given by the player
    public static readonly List<TurnInput> pastActions = new List<TurnInput>();

    void Start()
    {
        LoadContextActions();
        //TakeTurn(TalkInput.TALKA, ActionInput.ACTIONA);
    }
    
    public void TakeTurn(TalkInput talkInput, ActionInput actionInput)
    {
        ResponseActionCallback callback = new ResponseActionCallback();
        TurnInput turnInput = new TurnInput(talkInput, actionInput);
        GatherResponsesEvent.Invoke(turnInput, callback);
        pastActions.Add(turnInput);
        IEnumerable<ResponseAction> responseActions = callback.GetResponseActions();//.Take(2);
        if (responseActions.Count() <= 0)
            return; //TODO Call default action here
        foreach (ResponseAction responseAction in responseActions)
        {
            responseAction.DoAction(turnInput);
        }
        
    }
    public enum TalkInput
    {
        TALKA,
        TALKB,
        TALKC,
        TALKD
    }
    
    public enum ActionInput
    {
        ACTIONA,
        ACTIONB,
        ACTIONC,
        ACTIOND
    }
    
    /**
     * Extend this class to make new responses
     */
    [ResponseAction]
    public abstract class ResponseAction
    {
        private float _weightCache;
        public void CalcWeight(TurnInput input, ResponseActionCallback callback)
        {
            _weightCache = CalcWeightInternal(input);
            if (_weightCache > 0)
                callback.AddViableResponse(this);
        }

        /**
         * Use the input to calculate the weighting of this action. return a negative value if it is not valid
         */
        protected abstract float CalcWeightInternal(TurnInput input);

        public float getWeightCache()
        {
            return _weightCache;
        }
        
        /**
         * What to call when this action is chosen as the desired action
         */
        public abstract void DoAction(TurnInput input);
    }

    #region Internal

    public struct TurnInput
    {
        public TalkInput TalkInput { get; private set; }
        public ActionInput ActionInput { get; private set; }

        public TurnInput(TalkInput talkInput, ActionInput actionInput)
        {
            TalkInput = talkInput;
            ActionInput = actionInput;
        }
    }

    private static void LoadContextActions()
    {
        foreach (Type type in Assembly.GetExecutingAssembly().GetTypes().Where(type => type.IsDefined(typeof(ResponseActionAttribute))))
        {
            if (type.IsAbstract)
                return;
            GatherResponsesEvent += ((ResponseAction)Activator.CreateInstance(type)).CalcWeight;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ResponseActionAttribute : Attribute
    {
        public ResponseActionAttribute()
        {
        }
    }

    private class ResponseComparer : IComparer<ResponseAction>
    {
        public static readonly ResponseComparer _Comparer = new ResponseComparer();
        
        private ResponseComparer(){}
        
        public int Compare(ResponseAction x, ResponseAction y)
        {
            if (x == null || y == null)
                return 0;
            return (int) (x.getWeightCache() - y.getWeightCache());
        }
    }

    public delegate void ResponseActionDelegate(TurnInput input, ResponseActionCallback callback);

    public static event ResponseActionDelegate GatherResponsesEvent;

    public class ResponseActionCallback
    {
        private readonly SortedSet<ResponseAction> viableResponses = new SortedSet<ResponseAction>(ResponseComparer._Comparer);

        public ResponseActionCallback()
        {
            
        }

        public void AddViableResponse(ResponseAction action)
        {
            viableResponses.Add(action);
        }

        public SortedSet<ResponseAction> GetResponseActions()
        {
            return viableResponses;
        }
    }

    #endregion
    
    
}
