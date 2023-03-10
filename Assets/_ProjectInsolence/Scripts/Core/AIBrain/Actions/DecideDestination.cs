using UnityEngine;

namespace Insolence.AIBrain.Actions
{
    [CreateAssetMenu(fileName = "DecideDestination", menuName = "Insolence/AIBrain/Actions/DecideDestination", order = 1)]
    public class DecideDestination : Action
    {
        public override void Execute(NPCAIController npc)
        {
            npc.DecideDestination();
        }       
    }
}
