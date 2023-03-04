using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence.AIBrain.Considerations
{
    [CreateAssetMenu(fileName = "HasInterest_Type", menuName = "Insolence/AIBrain/Considerations/HasInterest")]
    public class HasInterest : Consideration
    {
        //add a tooltip
        [Tooltip("If true, checks if the NPC has a different interest from the specified type. If false, checks if the NPC has the specified Interest.")]
        [SerializeField] bool hasNoInterest = false;
        [SerializeField] InterestType interestType;
        public override float ScoreConsideration(NPCAIController npc)
        {
            if (hasNoInterest)
                return score = npc.interest.interestType != interestType ? 1f : 0;
            else
                return score = npc.interest.interestType == interestType ? 1f : 0;
        }
    }
}
