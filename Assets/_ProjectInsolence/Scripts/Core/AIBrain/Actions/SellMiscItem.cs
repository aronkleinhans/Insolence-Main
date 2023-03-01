using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence.AIBrain.Actions
{
    [CreateAssetMenu(fileName = "SellMiscItem", menuName = "Insolence/AIBrain/Actions/SellMiscItem", order = 1)]
    public class SellMiscItem : Action
    {
        public override void Execute(NPCAIController npc)
        {
            npc.DoSellMisc();
        }
    }
}
