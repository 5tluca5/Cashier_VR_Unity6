using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class StoreItem : MonoBehaviour
{
    public StoreItemData storeItemData;
    private HashSet<GameObject> collidingObjects = new HashSet<GameObject>();

    Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnSelectEntered(SelectEnterEventArgs args)
    {
        
    }

    public void OnSelectExited(SelectExitEventArgs args)
    {
        if (collidingObjects.Count > 0)
        {
            foreach (GameObject obj in collidingObjects)
            {
                Debug.Log($"Released while touching: {obj.name}");

                if (obj.TryGetComponent(out RealCustomer realCustomer))
                {
                    realCustomer.CatchItem(this);
                }
            }
        }
        else
        {
            Debug.Log("Released in mid-air!");
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        collidingObjects.Add(collision.gameObject);
        Debug.Log($"{name} Collided with: {collision.gameObject.name}");

    }

    private void OnCollisionExit(Collision collision)
    {
        collidingObjects.Remove(collision.gameObject);
        Debug.Log($"{name} Stopped colliding with: {collision.gameObject.name}");
    }

    public bool SetKinematic(bool isKinematic) => rb.isKinematic = isKinematic;

    public void AddForce(Vector3 force) => rb.AddForce(force);
}
