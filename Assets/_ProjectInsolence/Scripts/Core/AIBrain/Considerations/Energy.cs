using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace Insolence.AIBrain.Considerations
{
    [CreateAssetMenu(fileName = "Energy", menuName = "Insolence/AIBrain/Considerations/Energy")]
    public class Energy : Consideration
    {
        public override float ScoreConsideration(NPCAIController npc)
        {
            //logic to score energy(stamina)
            score = 0.1f;
            return score;
        }
    }
}