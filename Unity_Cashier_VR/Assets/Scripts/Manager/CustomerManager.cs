using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomerManager
{

    [Header("Settings")]
    [SerializeField] bool spawnCustomers = true;
    [SerializeField] int maxCustomerCount = 10;
    [SerializeField] float spawnInterval = 5;

    [Header("References")]
    [SerializeField] GameObject customerPrefab;
    [SerializeField] GameObject realCustomerPrefab;
    [SerializeField] Transform customerParent;

    [Header("Paths")]
    [SerializeField] CustomerPathPoint customerSpawnPoint;
    [SerializeField] CustomerPathPoint customerExitPoint;
    [SerializeField] CustomerPathPoint customerEntryPoint;
    [SerializeField] CustomerPathPoint customerCashierPoint;
    [SerializeField] CustomerPathPoint[] customerPaths;

    //Spawning customers
    private float spawnTimer = 0;
    private List<Customer> customers = new List<Customer>();


    void Update()
    {
        SpawnCustomerProcedure();
    }

    private void SpawnCustomerProcedure()
    {
        if (spawnCustomers && customers.Count < maxCustomerCount)
        {
            spawnTimer += Time.deltaTime;

            if (spawnTimer >= spawnInterval)
            {
                spawnTimer = 0;
                SpawnCustomer();
            }
        }
    }

    private void SpawnCustomer()
    {
        GameObject prefab = customerPrefab;
        GameObject customerObject = GameObject.Instantiate(prefab, customerSpawnPoint.position, Quaternion.identity, customerParent);
        Customer customer = customerObject.GetComponent<Customer>();
        
        customer.PushPath(customerEntryPoint);

        int totalPathCount = Random.Range(1, customerPaths.Length);

        customer.ConcatPath(customerPaths.OrderBy(x => Random.value).Take(totalPathCount).ToList());

        customers.Add(customer);
    }
}
