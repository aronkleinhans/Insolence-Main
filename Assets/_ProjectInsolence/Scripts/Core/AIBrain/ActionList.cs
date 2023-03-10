using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence.AIBrain
{
    [CreateAssetMenu(fileName = "ActionList", menuName = "Action List", order = 1)]
    public class ActionList : ScriptableObject
    {
        public List<Action> actions = new List<Action>();

        public void AddAction(Action action)
        {
            if (!actions.Contains(action))
            {
                actions.Add(action);
            }
        }
    }

}
