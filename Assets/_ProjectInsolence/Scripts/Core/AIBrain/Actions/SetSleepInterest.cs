using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence.AIBrain.Actions
{
    [CreateAssetMenu(fileName = "SetSleepInterest", menuName = "Insolence/AIBrain/Actions/SetSleepInterest", order = 1)]
    public class SetSleepInterest : Action
    {
        public override void Execute(NPCAIController npc)
        {
            npc.SetSleepInterest();
        }
    }
}
