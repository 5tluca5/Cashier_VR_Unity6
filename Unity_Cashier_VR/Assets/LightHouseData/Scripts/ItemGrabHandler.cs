using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ItemGrabHandler : MonoBehaviour
{
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnSelectEntered(SelectEnterEventArgs selectArgs)
    {
        rb.constraints = RigidbodyConstraints.None;
    }

    public void OnSelectExited(SelectExitEventArgs exitArgs)
    {
        rb.constraints = RigidbodyConstraints.None;
    }
}
