using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static GameManager;
using static LogicScriptUI;


#region GoodGirlFriendResponses
public class smallHate : ResponseAction
{
    protected override float CalcWeightInternal(TurnInput input)
    {
        // Calculate the weight value on the TalkInput and ActionInput
        // Return a negative value if the action is not valid for the given input
        float weight = -5;
        return weight;
    }

    public override void DoAction(TurnInput input)
    {
        
    }
}

public class smallLove : ResponseAction
{
    protected override float CalcWeightInternal(TurnInput input)
    {
        Debug.Log("help");
         // Calculate the weight based on the TalkInput and ActionInput
         // Return a negative value if the action is not valid for the given input
         
         // float weight = -5;
         // var talkInput = input.TalkInput;
         // var actionInput = input.ActionInput;
         //
         // if( talkInput == TalkInput.TALKC && actionInput == ActionInput.ACTIONA)
         // {
         //     weight *= -1;
         // }
         // else if (talkInput == TalkInput.TALKB && actionInput == ActionInput.ACTIONB)
         // {
         //     weight *= -1;
         // }
         // else if (talkInput == TalkInput.TALKB && actionInput == ActionInput.ACTIONC)
         // {
         //     weight *= -1;
         // }
         // else if (talkInput == TalkInput.TALKD && actionInput == ActionInput.ACTIONB)
         // {
         //     weight *= -1;
         // }
         // else if (talkInput == TalkInput.TALKD && actionInput == ActionInput.ACTIOND)
         // {
         //     weight *= -1;
         // }
         //
         // Debug.Log("Small Love");
         //
         //
         //
         // return weight;
         //
         return input.isApplicable(0b1111_1111_1111_1111) ? 5 : 0;
    }

    public override void DoAction(TurnInput input)
    {
        Debug.Log("reducing love");
        ReduceLove(-10);
    }
}

public class mediumHate : ResponseAction
{
    protected override float CalcWeightInternal(TurnInput input)
    {
        // Calculate the weight based on the TalkInput and ActionInput
        // Return a negative value if the action is not valid for the given input
        float weight = -10;

        return weight;
    }

    public override void DoAction(TurnInput input)
    {
        ReduceHate(-10);
    }
}

public class mediumLove : ResponseAction
{
    protected override float CalcWeightInternal(TurnInput input)
    {
        // Calculate the weight based on the TalkInput and ActionInput
        // Return a negative value if the action is not valid for the given input
        float weight = -10;
        return weight;
    }

    public override void DoAction(TurnInput input)
    {
        ReduceLove(-10);
    }
}

public class bigHate : ResponseAction
{
    protected override float CalcWeightInternal(TurnInput input)
    {
        // Calculate the weight based on the TalkInput and ActionInput
        // Return a negative value if the action is not valid for the given input
        float weight = -20;
        return weight;
    }

    public override void DoAction(TurnInput input)
    {
        ReduceHate(-20);
    }
}

public class bigLove : ResponseAction
{
    protected override float CalcWeightInternal(TurnInput input)
    {
        // Calculate the weight based on the TalkInput and ActionInput
        // Return a negative value if the action is not valid for the given input
        float weight = -20;
        return weight;
    }

    public override void DoAction(TurnInput input)
    {
        ReduceLove(-20);
    }
}



#endregion
