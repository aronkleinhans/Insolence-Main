using Insolence.AIBrain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence
{
    public class NPCAIManager : MonoBehaviour
    {
        public static NPCAIManager Instance { get; private set; }

        private Dictionary<int, NPCAIController> npcIndexMap;

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

            npcIndexMap = new Dictionary<int, NPCAIController>();
        }

        public void AddNPC(int index, NPCAIController npc)
        {
            npcIndexMap[index] = npc;
        }

        public NPCAIController GetNPCByIndex(int index)
        {
            if (npcIndexMap.TryGetValue(index, out NPCAIController npc))
            {
                return npc;
            }
            else
            {
                return null;
            }
        }

        public int GetNPCIndex(NPCAIController npc)
        {
            foreach (KeyValuePair<int, NPCAIController> pair in npcIndexMap)
            {
                if (pair.Value == npc)
                {
                    return pair.Key;
                }
            }

            return -1;
        }

        public void RemoveNPC(int index)
        {
            npcIndexMap.Remove(index);
        }
    }

}
