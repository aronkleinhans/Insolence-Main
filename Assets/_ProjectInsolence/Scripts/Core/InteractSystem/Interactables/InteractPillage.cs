using Insolence.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Insolence
{
    public class InteractPillage : Interactable 
    {
        Inventory inv;
        public List<Item> itemList;
        // Start is called before the first frame update
        void Start()
        {
            interactionType = "Pillage";
            interactableName = "Corpse";
            inv = GetComponent<Inventory>();
            itemList = inv.CreateItemList();
        }

        // Update is called once per frame
        void Update()
        {
            itemList.RemoveAll(item => item == null);

            if (itemList.Count == 0)
            {
                Destroy(this);
                this.tag = "Untagged";
            }
        }
        public override void Interaction(Transform actorTransform)
        {
            //drop all items from inventory

            foreach (Item i in itemList.ToList())
            {
                if (i != null)
                {
                    inv.DropItem(i);
                    inv.RemoveItem(i);
                    itemList.Remove(i);
                }
            }   

        }

    }
}
