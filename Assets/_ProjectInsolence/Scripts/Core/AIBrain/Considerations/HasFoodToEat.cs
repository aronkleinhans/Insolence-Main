using Insolence.Core;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Insolence.AIBrain.Considerations
{
    [CreateAssetMenu(fileName = "HasFoodToEat", menuName = "Insolence/AIBrain/Considerations/HasFoodToEat", order = 1)]
    public class HasFoodToEat : Consideration
    {
        public override float ScoreConsideration(NPCAIController npc)
        {
            Inventory inventory = npc.GetComponent<Inventory>();

            var foodList = from item in inventory.CreateItemList()
                           where item != null && item.consumableType == ItemEnums.ConsumableType.Food
                           select item;

            List<Item> food = foodList.ToList();

            score = food.Count > 0 ? 1 : 0f;
            return score;
        }
    }
}
