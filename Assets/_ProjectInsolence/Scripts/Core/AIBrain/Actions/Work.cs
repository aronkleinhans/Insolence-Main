using UnityEngine;

namespace Insolence.AIBrain.Actions
{
    [CreateAssetMenu(fileName = "Work", menuName = "Insolence/AIBrain/Actions/Work", order = 1)]
    public class Work : Action
    {
        public override void Execute(NPCAIController npc)
        {
            npc.DoWork(3);
        }
    }
}