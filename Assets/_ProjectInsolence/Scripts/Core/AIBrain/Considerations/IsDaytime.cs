using System.Collections;
using System.Collections.Generic;
using Insolence.Core;
using UnityEngine;

namespace Insolence.AIBrain.Considerations
{
    [CreateAssetMenu(fileName = "IsDaytime", menuName = "Insolence/AIBrain/Considerations/IsDaytime", order = 1)]
    public class IsDaytime : Consideration
    {
        public override float ScoreConsideration(NPCAIController npc)
        {
            TimeManager tm = GameObject.Find("TimeManager").GetComponent<TimeManager>();
            score = (tm.GetTimeOfDay() >= 6 && tm.GetTimeOfDay() <= 18) ? 1 : 0f;
            return score;
        }
    }
}
