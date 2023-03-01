using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractPush : Interactable
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float pushForce = 20f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public override void Interaction(Transform tf)
    {
        Debug.Log(tf.gameObject.name + " pushed " + transform.gameObject.name);

        rb.AddForce(tf.forward * pushForce, ForceMode.VelocityChange);
    }
}
