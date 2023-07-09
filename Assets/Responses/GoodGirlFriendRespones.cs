using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using static GameManager;
using static LogicScriptUI;

// Hate good love bad

#region BaseResponses

public class smallHate : ResponseAction
{
    protected override float CalcWeightInternal(TurnInput input)
    {
        return input.isApplicable(0b0000_0000_0000_0000) ? 5 : 0;
    }

    public override void DoAction(TurnInput input)
    {
        ReduceHate(-10);
        SendDialog("That wasn't very nice of you", false);
    }
}

public class smallLove : ResponseAction
{
    protected override float CalcWeightInternal(TurnInput input)
    {
        return input.isApplicable(0b0000_0000_0100_0010) ? 6 : 0;
    }

    public override void DoAction(TurnInput input)
    {
        ReduceLove(-10);
    }
}

public class mediumHate : ResponseAction
{
    protected override float CalcWeightInternal(TurnInput input)
    {
        return input.isApplicable(0b1010_0011_1001_0011) ? 9 : 0;
    }

    public override void DoAction(TurnInput input)
    {
        ReduceHate(-20 * CoefIfPast(0.5f, input));
    }
}

public class mediumLove : ResponseAction
{
    protected override float CalcWeightInternal(TurnInput input)
    {
        return input.isApplicable(0b1010_0011_1001_0011) ? 10 : 0;
    }

    public override void DoAction(TurnInput input)
    {
        ReduceLove(-20 * CoefIfPast(1.25f, input));
    }
}

public class bigHate : ResponseAction
{
    protected override float CalcWeightInternal(TurnInput input)
    {
        return input.isApplicable(0b1010_0011_1001_0011) ? 20 : 0;
    }

    public override void DoAction(TurnInput input)
    {
        ReduceHate(-30 * CoefIfPast(0.5f, input));
        SendDialog("Why don't you love me", false);
    }
}

public class bigLove : ResponseAction
{
    protected override float CalcWeightInternal(TurnInput input)
    {
        return input.isApplicable(0b1010_0011_1001_0011) ? 21 : 0;
    }

    public override void DoAction(TurnInput input)
    {
        ReduceLove(-30 * CoefIfPast(1.25f, input));
    }
}

#endregion


public class SisterLove : ResponseAction
{
    protected override float CalcWeightInternal(TurnInput input)
    {
        return pastActions.Count(turnInput => turnInput.isApplicable(0b0000_0000_0000_1111)) >= 2 
               && !gameStatus.ContainsKey("sister") ? 40f : 0f;
    }

    public override void DoAction(TurnInput input)
    {
        ReduceHate(-40);
        ReduceLove(-15);
        SendDialog("Shes just your sister! Isn't that flattering?", true);
        SendDialog("NO!", false);
        gameStatus["sister"] = "done";
    }
}

public class Overspending : ResponseAction
{
    protected override float CalcWeightInternal(TurnInput input)
    {
        return pastActions.Count(turnInput => turnInput.isApplicable(0b0000_0000_1111_0000)) >= 1 
               && !gameStatus.ContainsKey("spending") ? 40f : 0f;
    }

    public override void DoAction(TurnInput input)
    {
        ReduceHate(-15);
        ReduceLove(-20);
        SendDialog("How can you afford this?", false);
        SendDialog("I just cant help seeing you happy!", true);
        gameStatus["spending"] = "done";
    }
}

public class Giftcookies : ResponseAction
{
    protected override float CalcWeightInternal(TurnInput input)
    {
        return !gameStatus.ContainsKey("cookies") && input.isApplicable(0b0000_0000_0010_0000) && 
               pastActions.Count(turnInput => turnInput.isApplicable(0b0000_0000_0000_0101)) >= 1 ? 40 : 0;
    }

    public override void DoAction(TurnInput input)
    {
        ReduceHate(-20);
        ReduceLove(-5);
        SendDialog("Oatmeal raisin? What's wrong with you?", false);
        SendDialog("Your sister said they were your favorite!", true);
        SendDialog("She said what?", false);
        gameStatus["cookies"] = "done";
    }
}