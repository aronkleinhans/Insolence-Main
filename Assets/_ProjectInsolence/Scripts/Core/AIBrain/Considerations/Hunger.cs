using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence.AIBrain.Considerations
{
    [CreateAssetMenu(fileName = "Hunger", menuName = "Insolence/AIBrain/Considerations/Hunger")]
    public class Hunger : Consideration
    {
        public override float ScoreConsideration(NPCAIController npc)
        {
            //logic to score hunger
            return score = npc.status.hunger > 50 ? 1 : 0f;
        }
    }
}