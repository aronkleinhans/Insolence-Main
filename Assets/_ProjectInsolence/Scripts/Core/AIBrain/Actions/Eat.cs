using UnityEngine;

namespace Insolence.AIBrain.Actions
{
    [CreateAssetMenu(fileName = "Eat", menuName = "Insolence/AIBrain/Actions/Eat", order = 3)]

    public class Eat : Action
    {
        public override void Execute(NPCAIController npc)
        {
            npc.DoEat(3);
        }

    }
}