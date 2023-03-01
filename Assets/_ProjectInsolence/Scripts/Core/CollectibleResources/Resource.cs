using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Insolence.SaveUtility;

namespace Insolence.Core
{
    [Serializable]
    public class Resource : MonoBehaviour
    {
        [SerializeField] ResourceType resourceType;
        [SerializeField] int _amount;

        public int amount
        {
            get
            {
                return _amount;
            }
            set
            {
                _amount = value;
                if (_amount < 0)
                {
                    _amount = 0;
                }
            }
        }
        public ResourceType GetResourceType()
        {
            return resourceType;
        }
        // Start is called before the first frame update
        void Start()
        {
            if (amount == 0)
            {
                amount = resourceType.initialAmount;
            }
            DynamicObject dynamicObject = gameObject.GetComponent<DynamicObject>();
            dynamicObject.prepareToSaveDelegates += PrepareToSaveObjectState;
            dynamicObject.loadObjectStateDelegates += LoadObjectState;
        }

        private void PrepareToSaveObjectState(ObjectState objectState)
        {
            objectState.genericValues[name + ".amount"] = amount;
        }
        private void LoadObjectState(ObjectState objectState)
        {
            amount = (int)objectState.genericValues[name + ".amount"];
        }  
    }
}