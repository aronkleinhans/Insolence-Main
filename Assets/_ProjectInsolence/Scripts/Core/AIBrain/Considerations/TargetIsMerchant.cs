using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence.AIBrain.Considerations
{
    [CreateAssetMenu(fileName = "TargetIsMerchant", menuName = "Insolence/AIBrain/Considerations/TargetIsMerchant")]
    public class TargetIsMerchant : Consideration
    {
        public override float ScoreConsideration(NPCAIController npc)
        {
            if (npc.destination != null)
            {
                NPCAIController npcAI = npc.destination.GetComponent<NPCAIController>();
                if (npcAI != null)
                    return score = npcAI.job == NPCAIController.JobType.Merchant ? 1 : 0;
                else
                    return score = 0;
            }
            else
                return score = 0;

        }
    }
}
