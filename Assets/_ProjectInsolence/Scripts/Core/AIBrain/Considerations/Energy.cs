using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace Insolence.AIBrain.Considerations
{
    [CreateAssetMenu(fileName = "Energy", menuName = "Insolence/AIBrain/Considerations/Energy")]
    public class Energy : Consideration
    {
        [SerializeField] private AnimationCurve responseCurve;
        public override float ScoreConsideration(NPCAIController npc)
        {
            //logic to score energy(stamina)
            return score = responseCurve.Evaluate(Mathf.Clamp01(npc.status.currentStamina / npc.status.maxStamina));
        }
    }
}