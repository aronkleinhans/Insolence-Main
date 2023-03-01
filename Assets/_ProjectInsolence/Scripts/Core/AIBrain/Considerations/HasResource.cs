using Insolence.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence.AIBrain.Considerations
{
    [CreateAssetMenu(fileName = "HasResource", menuName = "Insolence/AIBrain/Considerations/HasResource")]
    public class HasResource : Consideration
    {
        public override float ScoreConsideration(NPCAIController npc)
        {
            if (npc.GetComponent<Inventory>().equippedInRightHandSlot != null)
                return score = npc.GetComponent<Inventory>().equippedInRightHandSlot.type == ItemEnums.ItemType.Misc ? 1f : 0f;
            else
                return score = 0f;
        }
    }
}
