using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence.AIBrain.Considerations
{
    [CreateAssetMenu(fileName = "HasNoInterest", menuName = "Insolence/AIBrain/Considerations/HasNoInterest")]
    public class HasNoInterest : Consideration
    {
        public override float ScoreConsideration(NPCAIController npc)
        {
            return score = npc.interest.interestType == InterestType.None ? 1f : 0;
        }
    }
}
