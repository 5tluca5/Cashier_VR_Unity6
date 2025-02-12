using UnityEngine;

public class CustomerPathPoint : MonoBehaviour
{
    [SerializeField] Transform direction;
    [SerializeField] float minStayTime = 3;
    [SerializeField] float maxStayTime = 20;
    [SerializeField] int capacity = 1;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float CustomerArrived()
    {
        capacity--;
        return GetRandomStayTime();
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

    public Vector3 position => transform.position;

    public Quaternion rotation => direction.rotation;
}
