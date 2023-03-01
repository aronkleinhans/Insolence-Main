using Insolence.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence
{
    [Serializable]
    public class ShowEquipment : MonoBehaviour
    {
        GameObject newObject;
        Item currentItem;
        private void Update()
        {
            Item item = GetComponent<ItemSOHolder>().item;
            if (item && newObject == null)
            {
                currentItem = item;
                newObject = Instantiate(item.itemEquipPrefab, transform.position, transform.rotation);
                newObject.transform.parent = gameObject.transform;
            }
            if (item != currentItem && newObject != null)
            {
                Destroy(newObject);
            }

        }
    }
}
