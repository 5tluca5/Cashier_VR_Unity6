using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] List<StoreItemData> customerDesiredItems = new List<StoreItemData>();
    [SerializeField] CustomerManager customerManager;
    [SerializeField] float spawnDelay = 5f;

    Queue<StoreItemData> itemQueue = new Queue<StoreItemData>();

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
}
