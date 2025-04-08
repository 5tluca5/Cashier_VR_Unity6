using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager Instance { private set; get; }

    [Header("Customer Spawn Settings")]
    [SerializeField] private NavMeshAgent customerNavMeshAgent;
    [SerializeField] private Transform customerSpawnPoint;
    [SerializeField] private float customerSpawnTimeMin = 5f;
    [SerializeField] private float customerSpawnTimeMax = 15f;
    [SerializeField] private float currrentSpawnTime = 0f;
    [SerializeField] private int customerCount = 0;

    [Header("Customer Stand Settings")]
    [Tooltip("The transform used as the point where the customer will stand and request for items.")]
    [SerializeField] private Transform[] customerStandPoints;
    
    private List<Transform> availableStandPointList;
    private List<Transform> customersList;
    private int MaxCustomerSpawnCount;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        availableStandPointList = new(customerStandPoints);
        customersList = new();
        MaxCustomerSpawnCount = Random.Range(3, customerStandPoints.Length);
    }

    private void Update()
    {
        if (customerCount >= MaxCustomerSpawnCount)
            return;

        currrentSpawnTime += Time.deltaTime;

        float spawnTime = Random.Range(customerSpawnTimeMin, customerSpawnTimeMax);
        if (currrentSpawnTime >= spawnTime)
        {
            currrentSpawnTime = 0f;
            customerCount++;

            SpawnCustomer();
        }
    }

    private void SpawnCustomer()
    {
        var standPoint = availableStandPointList[Random.Range(0, availableStandPointList.Count)];
        var customer = Instantiate(customerNavMeshAgent, customerSpawnPoint.position, customerSpawnPoint.rotation, standPoint);
        customer.SetDestination(standPoint.position);
        customer.name = "Customer_" + Random.Range(0, 100000);
        availableStandPointList.Remove(standPoint);
        customersList.Add(customer.transform);
    }

    public void RemoveCustomer(Transform customer)
    {
        if (customersList.Contains(customer))
        {
            NavMeshAgent agent = customer.GetComponent<NavMeshAgent>();
            agent.SetDestination(customerSpawnPoint.position);
            customersList.Remove(customer);
            availableStandPointList.Add(customer.parent);
            customerCount--;
            Destroy(customer.gameObject, 50f);
        }
    }
}
