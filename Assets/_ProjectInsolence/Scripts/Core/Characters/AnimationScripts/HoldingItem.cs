using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence.Core
{
    public class HoldingItem : MonoBehaviour
    {
        Inventory inv;
        Animator anim;
        // Start is called before the first frame update
        void Start()
        {
            inv = GetComponent<Inventory>();
            anim = transform.Find("Root").GetComponentInChildren<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            if (inv.holdingItem)
            {
                anim.SetBool("HoldingItem", true);
            }
            else
            {
                anim.SetBool("HoldingItem", false);
            }
        }
    }
}
