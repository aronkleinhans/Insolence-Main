using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence.AIBrain.Considerations
{
    [CreateAssetMenu(fileName = "IsResourceWorker", menuName = "Insolence/AIBrain/Considerations/IsResourceWorker")]
    public class IsResourceWorker : Consideration
    {
        public override float ScoreConsideration(NPCAIController npc)
        {
            return score = npc.interest.workType != WorkType.Trading ? 1 : 0;
        }
    }
}
