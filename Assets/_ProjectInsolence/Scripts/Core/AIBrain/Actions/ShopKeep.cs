using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence.AIBrain.Actions
{
    [CreateAssetMenu(fileName = "ShopKeep", menuName = "Insolence/AIBrain/Actions/ShopKeep", order = 3)]
    public class ShopKeep : Action
    {
        public override void Execute(NPCAIController npc)
        {
            npc.DoShopKeep(10);
        }
    }
}
