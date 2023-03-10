using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Insolence.AIBrain
{
    public class ActionManager : MonoBehaviour
    {
        public static ActionManager Instance { get; private set; }
        public ActionList actionList;

        private Dictionary<int, Action> actionIdMap;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            actionIdMap = new Dictionary<int, Action>();
        }
        private void Update()
        {
            //if dict is empty add items from actionlist

            if (actionIdMap.Count == 0)
            {
                for (int i = 0; i < actionList.actions.Count; i++)
                {
                    AddAction(i, actionList.actions[i]);
                }
            }
        }
        public void AddAction(int id, Action action)
        {
            
            //log added action and its id
            Debug.Log("Added action " + action.name + " with id " + id);
            if (!actionIdMap.ContainsKey(id))
            {
                actionIdMap.Add(id, action);
            }
            actionIdMap[id] = action;
        }

        public Action GetActionById(int id)
        {
            if (actionIdMap.TryGetValue(id, out Action action))
            {
                return action;
            }
            else
            {
                return null;
            }
        }
        public int GetActionIndex(Action action)
        {
            foreach (KeyValuePair<int, Action> pair in actionIdMap)
            {
                if (pair.Value == action)
                {
                    return pair.Key;
                }
            }

            return -1;
        }

        public void RemoveAction(int id)
        {
            actionIdMap.Remove(id);
        }
    }

}
