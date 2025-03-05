using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class StoreItem : MonoBehaviour
{
    public StoreItemData storeItemData;
    private HashSet<GameObject> collidingObjects = new HashSet<GameObject>();

    Rigidbody rb;
    XRGrabInteractable xR;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnSelectEntered(SelectEnterEventArgs args)
    {
        //rb.isKinematic = false;
        GameController.Instance.SetGrabbingItem(this);
    }

    public void OnSelectExited(SelectExitEventArgs args)
    {
        //rb.isKinematic = false;
        GameController.Instance.ReleaseGrabbingItem();
    }


    //private void OnCollisionEnter(Collision collision)
    //{
    //    collidingObjects.Add(collision.gameObject);
    //    Debug.Log($"{name} Collided with: {collision.gameObject.name}");

    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    collidingObjects.Remove(collision.gameObject);
    //    Debug.Log($"{name} Stopped colliding with: {collision.gameObject.name}");
    //}

    public bool SetKinematic(bool isKinematic) => rb.isKinematic = isKinematic;

    public void AddForce(Vector3 force) => rb.AddForce(force);
}
