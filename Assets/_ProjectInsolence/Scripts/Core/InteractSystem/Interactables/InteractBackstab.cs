using Insolence.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence
{
    public class InteractBackstab : Interactable
    {
        private void Start()
        {
            interactionType = "Backstab";
            interactableName = GetComponentInParent<CharacterStatus>().name;
        }
        public override void Interaction(Transform tf)
        {
            CharacterStatus tfStatus = tf.GetComponentInParent<CharacterStatus>();
            IDamageable damageScript =  GetComponentInParent<IDamageable>();
            
            Debug.Log(tfStatus.name + " attempts to backstab " + interactableName);

            damageScript.Damage(tfStatus.weaponDamage * 2);
        }
    }
}
