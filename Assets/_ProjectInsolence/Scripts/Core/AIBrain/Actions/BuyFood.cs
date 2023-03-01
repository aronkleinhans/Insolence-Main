using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence.AIBrain.Actions
{
    [CreateAssetMenu(fileName = "BuyFood", menuName = "Insolence/AIBrain/Actions/BuyFood", order = 1)]
    public class BuyFood : Action
    {
        public override void Execute(NPCAIController npc)
        {
            npc.DoBuyFood();
        }
    }
}
