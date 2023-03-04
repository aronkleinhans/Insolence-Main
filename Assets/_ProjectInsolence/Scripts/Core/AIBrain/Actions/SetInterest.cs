using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence.AIBrain.Actions
{
    [CreateAssetMenu(fileName = "SetInterest_Type", menuName = "Insolence/AIBrain/Actions/SetInterest", order = 1)]
    public class SetInterest : Action
    {
        [SerializeField] private InterestType interestType;
        public override void Execute(NPCAIController npc)
        {
            npc.SetInterest(interestType);
        }
    }
}
