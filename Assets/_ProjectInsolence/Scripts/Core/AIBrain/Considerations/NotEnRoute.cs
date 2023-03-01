using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence.AIBrain.Considerations
{
    [CreateAssetMenu(fileName = "NotEnRoute", menuName = "Insolence/AIBrain/Considerations/NotEnRoute", order = 1)]
    public class NotEnRoute : Consideration
    {
        public override float ScoreConsideration(NPCAIController npc)
        {
            return score = !npc.enRoute ? 1 : 0;
        }
    }
}
