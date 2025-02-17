using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CustomerPathPoint : MonoBehaviour
{
    [SerializeField] Transform direction;
    [SerializeField] Transform target;
    [SerializeField] float minStayTime = 3;
    [SerializeField] float maxStayTime = 20;
    [SerializeField] int capacity = 1;
    [SerializeField] List<CustomerBehaviour> posibleBehaviours = new List<CustomerBehaviour>() { CustomerBehaviour.Idle };
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public CustomerBehaviour CustomerReserved()
    {
        capacity--;
        return GetRandomBehaviour();
    }

    public CustomerBehaviour CustomerArrived()
    {
        return GetRandomBehaviour();
    }

    public void CustomerLeft()
    {
        capacity++;
    }

    public bool IsOccupied() => capacity <= 0;

    public float GetRandomStayTime()
    {
        return Random.Range(minStayTime, maxStayTime);
    }

    public CustomerBehaviour GetRandomBehaviour()
    {
        return posibleBehaviours[Random.Range(0, posibleBehaviours.Count)];
    }

    public Vector3 position => transform.position;

    public Quaternion rotation => direction.rotation;

    public Vector3 targetPosition => target.position;
}
