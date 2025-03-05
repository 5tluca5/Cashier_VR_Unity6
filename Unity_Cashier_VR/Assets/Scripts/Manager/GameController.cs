using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Instance of the GameController
    public static GameController Instance { private set;  get; }


    [SerializeField] List<StoreItemData> customerDesiredItems = new List<StoreItemData>();
    [SerializeField] CustomerManager customerManager;
    [SerializeField] float spawnDelay = 5f;

    Queue<StoreItemData> itemQueue = new Queue<StoreItemData>();
    StoreItem grabbingItem = null;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        itemQueue = new Queue<StoreItemData>(customerDesiredItems);

        StartCoroutine(SpawnRealCustomer());
    }

    IEnumerator SpawnRealCustomer()
    {
        yield return new WaitUntil(() => customerManager.HasCurrentRealCustomerLeft());

        yield return new WaitForSeconds(spawnDelay);

        customerManager.SpawnRealCustomer(itemQueue.Dequeue());

        if(itemQueue.Count <= 0)
        {
            // End game
            Debug.Log("Game Over");
            yield break;
        }

        StartCoroutine(SpawnRealCustomer());
    }

    public void SetGrabbingItem(StoreItem storeItem)
    {
        grabbingItem = storeItem;
    }

    public void ReleaseGrabbingItem()
    {
        Debug.Log($"ReleaseGrabbingItem {grabbingItem.name}");

        var realCustomer = customerManager.GetRealCustomer();

        if (realCustomer != null)
        {
            realCustomer.CatchItem(grabbingItem);
        }
        grabbingItem = null;
    }
}
