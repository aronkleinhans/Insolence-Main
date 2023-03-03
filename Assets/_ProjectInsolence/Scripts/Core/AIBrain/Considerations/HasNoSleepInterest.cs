using Insolence.AIBrain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace Insolence.AIBrain
{
    [CreateAssetMenu(fileName = "HasNoSleepInterest", menuName = "Insolence/AIBrain/Considerations/HasNoSleepInterest")]
    public class HasNoSleepInterest : Consideration
    {
        public override float ScoreConsideration(NPCAIController npc)
        {
            return score = npc.interest.interestType != InterestType.Sleep ? 1f : 0;
        }
    }
}
