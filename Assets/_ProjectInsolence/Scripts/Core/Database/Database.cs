using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Insolence.Core
{
    /// <summary>
    /// This class is used to store Items by ID.
    /// </summary>
    public class Database : ScriptableObject
    {
        [SerializeField] public string itemType;

        [SerializeField] public List<Item> contents = new List<Item>();

        [SerializeField] public List<string> keys = new List<string>();

        [SerializeField] public List<Item> values = new List<Item>();

        /// <summary>
        /// FindItem returns the item corresponding to the given ID.
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public Item FindItem(string itemID)
        {
            //returns Item of ID
            int index = keys.IndexOf(itemID);
            return values[index];
        }
    }

}
