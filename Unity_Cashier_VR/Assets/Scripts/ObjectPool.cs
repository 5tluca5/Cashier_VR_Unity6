using System;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] bool selfInitialize = false;
    [SerializeField] GameObject prefab; // Object to pool
    [SerializeField] int poolSize = 10;

    private Queue<GameObject> pool = new Queue<GameObject>();

    void Start()
    {
        if(!selfInitialize)
            return;

        Initialize();
    }

    public void setPoolSize(int size)
    {
        poolSize = size;
    }

    public void Initialize()
    {
        if(prefab == null)
        {
            Debug.LogError("Prefab is not set in ObjectPool");
            return;
        }

        if(pool.Count > 0)
        {
            Debug.LogWarning("Pool is already initialized");
            return;
        }

        // Pre-instantiate objects and deactivate them
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject GetObject()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            // Optional: Expand the pool if needed
            GameObject obj = Instantiate(prefab, transform);
            return obj;
        }
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }

    public int GetActiveObjectCount()
    {
        return poolSize - pool.Count;
    }
}
