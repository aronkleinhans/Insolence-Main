using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence.Core
{
    [CreateAssetMenu(fileName = "New Farmable Resource", menuName = "Insolence/Resource")]
    public class ResourceType : ScriptableObject
    {
        [SerializeField] public new string name;
        [SerializeField] public ResType type;
        [SerializeField] public int initialAmount;
        [SerializeField] public Item item;

        //add enum for type of resource
        
        public enum ResType
        {
            Wood,
            Stone,
            Iron,
            Copper
        }
    }
}


