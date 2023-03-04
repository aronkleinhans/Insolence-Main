using System.Collections;
using System.Collections.Generic;
using Insolence.Core;
using UnityEngine;

namespace Insolence.AIBrain.Considerations
{
    [CreateAssetMenu(fileName = "IsDayTime", menuName = "Insolence/AIBrain/Considerations/IsDayTime", order = 1)]
    public class IsDaytime : Consideration
    {
        [SerializeField] bool isNightTime;
        [SerializeField] int from = 6;
        [SerializeField] int to = 18;
        
        public override float ScoreConsideration(NPCAIController npc)
        {
            TimeManager tm = GameObject.Find("TimeManager").GetComponent<TimeManager>();

            if (isNightTime)
                score = (tm.GetTimeOfDay() >= from && tm.GetTimeOfDay() <= to) ? 0 : 1f;
            else
                score = (tm.GetTimeOfDay() >= from && tm.GetTimeOfDay() <= to) ? 1 : 0f;
            return score;
        }
    }
}
