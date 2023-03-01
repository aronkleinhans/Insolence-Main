using Insolence.Core;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Insolence.AIBrain.Considerations
{
    [CreateAssetMenu(fileName = "HasFoodForTrip", menuName = "Insolence/AIBrain/Considerations/HasFoodForTrip", order = 1)]
    public class HasFoodForTrip : Consideration
    {
        public override float ScoreConsideration(NPCAIController npc)
        {
            CharacterStatus status = npc.GetComponent<CharacterStatus>();
            score = (npc.neededFood <= (npc.ownedFood + (status.maxHunger - status.hunger) / 2)) ? 1 : 0f;
            return score;
        }
    }
}
