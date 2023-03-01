using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence.AIBrain.Actions
{
    [CreateAssetMenu(fileName = "MoveToDestination", menuName = "Insolence/AIBrain/Actions/MoveToDestination", order = 1)]
    public class MoveToDestination : Action
    {
        public override void Execute(NPCAIController npc)
        {
            npc.MoveToDestination();
        }
    }
}
