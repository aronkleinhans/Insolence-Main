using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence.AIBrain.Considerations
{
    [CreateAssetMenu(fileName = "HasWorkInterest", menuName = "Insolence/AIBrain/Considerations/HasWorkInterest")]
    public class HasWorkInterest : Consideration
    {
        public override float ScoreConsideration(NPCAIController npc)
        {
            return score = npc.interest.interestType == InterestType.Work ? 1f : 0;
        }
    }
}
