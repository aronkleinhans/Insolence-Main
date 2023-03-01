using Insolence.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence.Core
{
    public class InteractLootCorpse : Interactable
    {
        int gold;
        public InteractPillage p;
        private void Start()
        {
            interactionType = "Loot";
            interactableName = "Corpse";
            
            gold = GetComponent<RagdollInfo>().gold;
            p = GetComponent<InteractPillage>();
        }
        private void Update()
        {
            if (gold <= 0 && enabled == true)
            {
                p.enabled = true;
                Destroy(this);
            }
        }
        public override void Interaction(Transform actorTransform)
        {
            actorTransform.GetComponent<CharacterStatus>().gold += gold;
            GetComponent<RagdollInfo>().gold = 0;
            gold = 0;
        }

    }
}
