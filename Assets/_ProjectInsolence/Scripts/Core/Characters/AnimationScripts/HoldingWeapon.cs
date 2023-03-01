using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence.Core
{
    public class HoldingWeapon : MonoBehaviour
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
            if (inv.holdingWeapon)
            {
                anim.SetBool("HoldingWeapon", true);
            }
            else
            {
                anim.SetBool("HoldingWeapon", false);
            }
        }
    }
}
