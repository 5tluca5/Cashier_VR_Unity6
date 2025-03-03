using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class StoreItem : MonoBehaviour
{
    public StoreItemData storeItemData;
    private HashSet<GameObject> collidingObjects = new HashSet<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnReleasedFromGrab(SelectExitEventArgs args)
    {
        if (collidingObjects.Count > 0)
        {
            foreach (GameObject obj in collidingObjects)
            {
                Debug.Log($"Released while touching: {obj.name}");

                if (obj.TryGetComponent(out RealCustomer realCustomer))
                {

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
    }

    private void OnCollisionExit(Collision collision)
    {
        collidingObjects.Remove(collision.gameObject);
    }
}
