using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static GameManager;


public class TalkAction : ResponseAction
{
    protected override float CalcWeightInternal(TurnInput input)
    {
        // Calculate the weight based on the TalkInput and ActionInput
        // Return a negative value if the action is not valid for the given input
        float weight = 5;
        return weight;
    }

    public override void DoAction(TurnInput input)
    {
        
    }
}
