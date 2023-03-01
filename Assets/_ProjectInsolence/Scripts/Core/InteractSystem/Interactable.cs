using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("Object Settings")]
    [SerializeField] public string interactionType = "Interact with";
    [SerializeField] public string interactableName = "Object";
    public virtual void Interaction(Transform actorTransform)
    {
        Debug.Log(actorTransform.gameObject.name + " just interacted with me");
    }
}
