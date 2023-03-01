using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence.AIBrain.Considerations
{
    [CreateAssetMenu(fileName = "IsMerchant", menuName = "Insolence/AIBrain/Considerations/IsMerchant")]
    public class IsMerchant : Consideration
    {
        public override float ScoreConsideration(NPCAIController npc)
        {
            return score = npc.job == NPCAIController.JobType.Merchant ? 1 : 0;
        }
    }
}
