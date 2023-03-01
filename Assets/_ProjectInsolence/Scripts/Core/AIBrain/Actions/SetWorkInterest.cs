using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence.AIBrain.Actions
{
    [CreateAssetMenu(fileName = "SetWorkInterest", menuName = "Insolence/AIBrain/Actions/SetWorkInterest", order = 1)]
    public class SetWorkInterest : Action
    {
        public override void Execute(NPCAIController npc)
        {
            npc.SetWorkInterest();
        }
    }
}
