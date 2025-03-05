using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;

public class WindowShopperManager : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] bool spawnCustomers = true;
    [SerializeField] int maxCustomerCount = 20;
    [SerializeField] float spawnInterval = 5;

    [Header("References")]
    [SerializeField] ObjectPool customerPool;
    

    [Header("Paths")]
    [SerializeField] CustomerPathPoint customerSpawnPoint;
    [SerializeField] CustomerPathPoint customerExitPoint;
    [SerializeField] CustomerPathPoint customerEntryPoint;
    [SerializeField] CustomerPathPoint customerCashierPoint;
    [SerializeField] CustomerPathPoint[] customerPaths;

    //Spawning customers
    private float spawnTimer = 0;

    private void Start()
    {
        customerPool.setPoolSize(maxCustomerCount);

        customerPool.Initialize();
    }

    void Update()
    {
        SpawnCustomerProcedure();
    }

    private void SpawnCustomerProcedure()
    {
        if (spawnCustomers && customerPool.GetActiveObjectCount() < maxCustomerCount)
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
        GameObject customerObject = customerPool.GetObject();
        Customer customer = customerObject.GetComponent<Customer>();
        customer.Reset();

        customerObject.transform.position = customerSpawnPoint.position;
        customerObject.transform.rotation = customerSpawnPoint.rotation;
        
        customer.PushPath(customerEntryPoint);

        int totalPathCount = Random.Range(1, customerPaths.Length);

        customer.ConcatPath(customerPaths.OrderBy(x => Random.value).Take(totalPathCount).ToList());

        customer.PushPath(customerExitPoint);
        customer.OnLeft().Subscribe(OnCustomerLeft);

        customer.Activate();
    }

    private void OnCustomerLeft(Customer customer)
    {
        customerPool.ReturnObject(customer.gameObject);
    }
}
