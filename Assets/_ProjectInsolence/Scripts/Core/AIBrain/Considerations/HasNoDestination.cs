using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//same as hasInterest but for destination


namespace Insolence.AIBrain.Considerations
{
    [CreateAssetMenu(fileName = "HasNoDestination", menuName = "Insolence/AIBrain/Considerations/HasNoDestination")]
    public class HasNoDestination : Consideration
    {
        public override float ScoreConsideration(NPCAIController npc)
        {
            return score = npc.destination == null ? 1 : 0;
        }
    }
}
