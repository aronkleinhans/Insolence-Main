using Insolence.Core;
using UnityEngine;

namespace Insolence.AIBrain.Considerations
{
    [CreateAssetMenu(fileName = "HasNoResource", menuName = "Insolence/AIBrain/Considerations/HasNoResource")]
    public class HasNoResource : Consideration
    {
        public override float ScoreConsideration(NPCAIController npc)
        {
            {
                if (npc.GetComponent<Inventory>().equippedInRightHandSlot != null)
                    return score = npc.GetComponent<Inventory>().equippedInRightHandSlot.type != ItemEnums.ItemType.Misc ? 1f : 0;
                else
                    return score = 1;
            }
        }
    }
}
