using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence.AIBrain.Considerations
{
    [CreateAssetMenu(fileName = "IsAtDestination", menuName = "Insolence/AIBrain/Considerations/IsAtDestination")]
    public class IsAtDestination : Consideration
    {
        public override float ScoreConsideration(NPCAIController npc)
        {
            return score = npc.hasArrived ? 1 : 0;
        }
    }
}
