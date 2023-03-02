using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Insolence.Core;

namespace Insolence.AIBrain.Considerations
{
    [CreateAssetMenu(fileName = "Hunger", menuName = "Insolence/AIBrain/Considerations/Hunger")]
    public class Hunger : Consideration
    {
        [SerializeField] private AnimationCurve responseCurve;
        public override float ScoreConsideration(NPCAIController npc)
        {
            //logic to score hunger
            return score = responseCurve.Evaluate(Mathf.Clamp01(npc.status.hunger / npc.status.maxHunger));
        }
    }
}