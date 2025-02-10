using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ItemThrowHandler : MonoBehaviour
{
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnSelectEntered(SelectEnterEventArgs args)
    {
        rb.constraints = RigidbodyConstraints.None;
    }

    public void OnSelectExited(SelectExitEventArgs args)
    {
        rb.constraints = RigidbodyConstraints.None;
    }
}
