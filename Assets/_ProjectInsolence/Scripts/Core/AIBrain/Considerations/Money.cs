using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence.AIBrain.Considerations
{
    [CreateAssetMenu(fileName = "Money", menuName = "Insolence/AIBrain/Considerations/Money")]
    public class Money : Consideration
    {
        [SerializeField] AnimationCurve responseCurve;
        public override float ScoreConsideration(NPCAIController npc)
        {
            //logic to score money
            return score = responseCurve.Evaluate(Mathf.Clamp01(npc.status.gold / npc.status.goldNeededPerDay));
        }
    }
}
