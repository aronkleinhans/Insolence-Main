using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence.AIBrain.Considerations
{
    [CreateAssetMenu(fileName = "HasInterest", menuName = "Insolence/AIBrain/Considerations/HasInterest")]
    public class HasInterest : Consideration
    {
        public override float ScoreConsideration(NPCAIController npc)
        {
            return score = npc.interest.interestType != InterestType.None ? 1f : 0;
        }
    }
}
