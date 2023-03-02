using System.Collections;
using System.Collections.Generic;
using Insolence.Core;
using UnityEngine;

namespace Insolence.AIBrain.Considerations
{
    [CreateAssetMenu(fileName = "IsNightTime", menuName = "Insolence/AIBrain/Considerations/IsNightTime", order = 1)]
    public class IsNightTime : Consideration
    {
        public override float ScoreConsideration(NPCAIController npc)
        {
            TimeManager tm = GameObject.Find("TimeManager").GetComponent<TimeManager>();
            score = (tm.GetTimeOfDay() >= 6 && tm.GetTimeOfDay() <= 18) ? 0 : 1f;
            return score;
        }
    }
}
