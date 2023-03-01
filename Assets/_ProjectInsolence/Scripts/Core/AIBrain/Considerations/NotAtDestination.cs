using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence.AIBrain.Considerations
{
    [CreateAssetMenu(fileName = "NotAtDestination", menuName = "Insolence/AIBrain/Considerations/NotAtDestination")]
    public class NotAtDestination : Consideration
    {
        public override float ScoreConsideration(NPCAIController npc)
        {
            return score = !npc.hasArrived ? 1 : 0;
        }
    }
}
