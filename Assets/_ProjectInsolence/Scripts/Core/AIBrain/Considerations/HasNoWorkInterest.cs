using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence.AIBrain.Considerations
{
    [CreateAssetMenu(fileName = "HasNoWorkInterest", menuName = "Insolence/AIBrain/Considerations/HasNoWorkInterest")]
    public class HasNoWorkInterest : Consideration
    {
        public override float ScoreConsideration(NPCAIController npc)
        {
            return score = npc.interest.interestType != InterestType.Work ? 1f : 0;
        }
    }
}
