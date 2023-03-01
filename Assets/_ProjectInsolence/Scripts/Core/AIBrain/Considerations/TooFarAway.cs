using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence.AIBrain.Considerations
{
    [CreateAssetMenu(fileName = "TooFarAway", menuName = "Insolence/AIBrain/Considerations/TooFarAway", order = 1)]
    public class TooFarAway : Consideration
    {
        public override float ScoreConsideration(NPCAIController npc)
        {
            //has food for there and back
            return score = npc.hungerGainOnArrival > (npc.status.maxHunger - npc.status.hunger) ? 0 : 1f;
        }
    }
}
