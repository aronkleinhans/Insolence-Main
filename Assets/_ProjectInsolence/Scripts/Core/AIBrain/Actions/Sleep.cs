using UnityEngine;

namespace Insolence.AIBrain.Actions
{
    [CreateAssetMenu(fileName = "Sleep", menuName = "Insolence/AIBrain/Actions/Sleep", order = 2)]
    public class Sleep : Action
    {
        public override void Execute(NPCAIController npc)
        {
            npc.DoSleep(3);
        }
    }
}