using UnityEngine;

namespace DefaultNamespace.Responses
{

    #region GoodGirlFriendResponses

    public class CallName : GameManager.ResponseAction
    {
        protected override float CalcWeightInternal(GameManager.TurnInput input)
        {
            Debug.Log("Calc weight");
            return 5.0f;
        }

        public override void DoAction(GameManager.TurnInput input)
        {
            Debug.Log("Chosen!");
        }
    }
    
    

    #endregion
}