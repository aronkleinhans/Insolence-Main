using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence.AIBrain.Considerations
{
    [CreateAssetMenu(fileName = "HasNoTradeInterest", menuName = "Insolence/AIBrain/Considerations/HasNoTradeInterest")]
    public class HasNoTradeInterest : Consideration
    {
        public override float ScoreConsideration(NPCAIController npc)
        {
            return score = npc.interest.interestType != InterestType.Trade ? 1f : 0;
        }
    }
}
