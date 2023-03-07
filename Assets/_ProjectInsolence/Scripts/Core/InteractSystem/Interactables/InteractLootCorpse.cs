using Insolence.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence.Core
{
    public class InteractLootCorpse : Interactable
    {
        public int gold;
        public RagdollInfo rdi;
        public InteractPillage pillage;
        private void Start()
        {
            interactionType = "Loot";
            interactableName = "Corpse";

            //check if null and get rdi, gold & pillage
            if (rdi == null)
            {
                rdi = transform.root.gameObject.GetComponent<RagdollInfo>();
            }
            if (gold == 0 && rdi != null)
            {
                gold = rdi.gold;
            }
            if (pillage == null)
            {
                pillage = GetComponent<InteractPillage>();
            }
        }
        private void Update()
        {

            if (gold <= 0 && enabled == true)
            {
                pillage.enabled = true;
                Destroy(this);
            }
        }
        public override void Interaction(Transform actorTransform)
        {
            actorTransform.GetComponent<CharacterStatus>().gold += gold;
            rdi.gold = 0;
            gold = 0;
        }

    }
}
