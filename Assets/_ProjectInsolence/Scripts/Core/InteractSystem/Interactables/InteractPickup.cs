using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence.Core
{
    public class InteractPickup : Interactable
    {
        public override void Interaction(Transform tf)
        {
            ItemSOHolder item = GetComponent<ItemSOHolder>();
            Debug.Log(tf.gameObject.name + " attempts to pick up " + item.name);
            if (tf.GetComponent<Inventory>().AddItem(item.item))
            {
                Debug.Log("Destroying world item");
                Destroy(gameObject);
            }
        }
    }
}