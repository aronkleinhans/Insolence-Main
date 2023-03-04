using Insolence.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence.AIBrain
{
    public class NPCPointOfInterest : MonoBehaviour
    {
        public GameObject owner;
        public bool isPublic;
        public List<Interest> interests = new List<Interest>();

        private void Start()
        {
            foreach(Interest i in GetComponentsInChildren<Interest>())
            {
                interests.Add(i);
            }
        }
        /// <summary>
        /// Takes an InterestStruct and returns True if the POI has a matching interestType
        /// </summary>
        /// <param name="interest"></param>
        /// <returns></returns>
        public bool HasNeededInterest(Interest.InterestStruct interest)
        {
            foreach(Interest i in interests)
            {
                if(i.interestType == interest.interestType)
                {
                    return true;
                }
            }
            return false;
        }
        public bool HasNeededWorkType(Interest.InterestStruct interest)
        {
            foreach (Interest i in interests)
            {
                if (i.workType == interest.workType)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
