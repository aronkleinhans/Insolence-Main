using Insolence.AIBrain;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;

namespace Insolence.AIBrain
{
    public enum InterestType
    {
        None,
        Food,
        Drink,
        Sleep,
        Work,
        Play,
        Interact,
        Social,
        Trade,
        Wander
    }

    public enum WorkType
    {
        None,
        Farming,
        Mining,
        Fishing,
        Crafting,
        Cooking,
        Hunting,
        Woodcutting,
        Smithing,
        Tailoring,
        Carpentry,
        Masonry,
        Alchemy,
        Gardening,
        Trading
    }
    public class Interest : MonoBehaviour
    {
        public InterestType interestType;
        public WorkType workType;
        public Vector3 position;

        private void Update()
        {
            if(position != transform.position)
            {
                position = transform.position;
            }
        }
        [System.Serializable]
        public struct InterestStruct
        {
            public InterestType interestType;
            public WorkType workType;

            public void UpdateWorkType(NPCAIController npc)    
            {
                //if interestType is work set workType based on NPCAIController.Job
                if (interestType == InterestType.Work)
                {
                    workType = npc.job switch
                    {
                        NPCAIController.JobType.Woodcutter => WorkType.Woodcutting,
                        NPCAIController.JobType.Cook => WorkType.Cooking,
                        NPCAIController.JobType.Merchant => WorkType.Trading,

                        _ => WorkType.None
                    };
                }
                else
                    workType = WorkType.None;
            }
        }
    }
}
