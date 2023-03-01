using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence.AIBrain.Considerations
{
    [CreateAssetMenu(fileName = "MustInteractWithCubes", menuName = "Insolence/AIBrain/Considerations/MustInteractWithCubes")]
    public class MustInteractWithCubes : Consideration
    {
        public override float ScoreConsideration(NPCAIController npc)
        {
            return score = 1f;
        }
    }
}