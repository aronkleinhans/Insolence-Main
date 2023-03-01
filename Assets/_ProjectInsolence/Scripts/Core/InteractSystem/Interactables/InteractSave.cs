using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Insolence.SaveUtility;

namespace Insolence.Core
{
    public class InteractSave : Interactable
    {
        public override void Interaction(Transform tf)
        {
            if (tf.gameObject.tag == "Player")
            {
                SaveUtils.DoSave(tf.gameObject.GetComponent<CharacterStatus>().GetScene());
            }
            
        }
    }
}
