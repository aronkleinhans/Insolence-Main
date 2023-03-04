using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence.AIBrain.Considerations
{
    [CreateAssetMenu(fileName = "TargetIs_WorkType", menuName = "Insolence/AIBrain/Considerations/TargetIsWorkType")]
    public class TargetIsWorkType : Consideration
    {
        [SerializeField] private NPCAIController.JobType jobType;
        public override float ScoreConsideration(NPCAIController npc)
        {
            if (npc.destination != null)
            {
                NPCAIController npcAI = npc.destination.GetComponent<NPCAIController>();
                if (npcAI != null)
                    return score = npcAI.job == jobType ? 1 : 0;
                else
                    return score = 0;
            }
            else
                return score = 0;

        }
    }
}
