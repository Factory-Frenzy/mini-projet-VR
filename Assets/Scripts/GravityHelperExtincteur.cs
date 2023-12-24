using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GravityHelperExtincteur : MonoBehaviour
{

    private Rigidbody rb;
    private void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
    }
    public void OnExtincteurGrab()
    {
        rb.useGravity = false;
    }
    public void OnExtincteurGrabExted()
    {
        rb.useGravity = true;
    }
}
