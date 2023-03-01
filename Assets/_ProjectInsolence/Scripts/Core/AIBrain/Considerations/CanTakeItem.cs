using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Insolence.Core;

namespace Insolence.AIBrain.Considerations
{
    [CreateAssetMenu(fileName = "CanTakeItem", menuName = "Insolence/AIBrain/Considerations/CanTakeItem", order = 1)]
    public class CanTakeItem : Consideration
    {
        public override float ScoreConsideration(NPCAIController npc)
        {
            if (npc.GetComponent<Inventory>().IsFull() == false)
            {
                return score = 1;
            }
            else
            {
                return score = 0;
            }
        }
    }
}
