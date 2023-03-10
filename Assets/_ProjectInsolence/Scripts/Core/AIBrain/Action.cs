using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Insolence.Core;
using Unity.Collections;
using UnityEditor;
using Insolence.AIBrain.Actions;

namespace Insolence.AIBrain
{
    public abstract class Action : ScriptableObject
    {
        public new string name;
        private float _score;

        public float score
        {
            get { return _score; }
            set 
            {
                _score = Mathf.Clamp01(value);
            }
        }

        public Consideration[] considerations;

        public ActionList actionList;
        private void OnEnable()
        {
            if (actionList != null)
            {
                actionList.AddAction(this);
            }
        }
        
        public void Awake()
        {
            score = 0;

        }

        public void ScheduleJob(NPCAIController npc)
        {
            Debug.Log("Scheduling job for action " + name);
            int npcIndex = NPCAIManager.Instance.GetNPCIndex(npc);
            int actionId = ActionManager.Instance.GetActionIndex(this);
            
            ActionJob job = new ActionJob();

            job.npcIndex = npcIndex;
            job.actionId = actionId;
            //log index and id
            Debug.Log("npcIndex: " + npcIndex + " actionId: " + actionId);

            JobHandle handle = job.Schedule();
            handle.Complete();

        }
        public abstract void Execute(NPCAIController npc);
        public struct ActionJob : IJob
        {
            public int npcIndex;
            public int actionId;

            public void Execute()
            {
                NPCAIController npc = NPCAIManager.Instance.GetNPCByIndex(npcIndex);
                Action action = ActionManager.Instance.GetActionById(actionId);

                if (npc != null && action != null)
                {
                    action.Execute(npc);
                }
            }
        }
    }


}