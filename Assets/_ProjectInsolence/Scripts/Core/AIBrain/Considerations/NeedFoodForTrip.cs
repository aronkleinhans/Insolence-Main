using Insolence.Core;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence.AIBrain.Considerations
{
    [CreateAssetMenu(fileName = "NeedFoodForTrip", menuName = "Insolence/AIBrain/Considerations/NeedFoodForTrip", order = 1)]
    public class NeedFoodForTrip : Consideration
    {
        public override float ScoreConsideration(NPCAIController npc)
        {
            Inventory inventory = npc.GetComponent<Inventory>();
            List<Item> food = new List<Item>();
            food.Clear();

            var foodList = from item in inventory.CreateItemList()
                           where item != null && item.consumableType == ItemEnums.ConsumableType.Food
                           select item;

            food = foodList.ToList();
            
            //has food for there and back
            return score = npc.neededFood > npc.ownedFood || npc.ownedFood == 0 ? 1 : 0f;
        }
    }
}
