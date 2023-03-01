using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence.Core
{
    public class PlayerInventoryActions : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            Inventory inv = gameObject.GetComponent<Inventory>();

            if (Input.GetKeyDown(KeyCode.Q))
            {
               
            }
            if (Input.GetButtonDown("Drop Item"))
            {
                inv.DropItem(inv.equippedInRightHandSlot);
            }
            if (Input.GetButtonDown("Swap Weapon"))
            {
                inv.CycleRightHandWeapons();
            }
            if (Input.GetButtonDown("Dual Wield"))
            {
                inv.DualWieldWeapons();
            }
        }
    }
}
