using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence.AIBrain.Actions
{
    [CreateAssetMenu(fileName = "SetTradeInterest", menuName = "Insolence/AIBrain/Actions/SetTradeInterest", order = 1)]
    public class SetTradeInterest : Action
    {
        public override void Execute(NPCAIController npc)
        {
            npc.SetTradeInterest();
        }
    }
}
