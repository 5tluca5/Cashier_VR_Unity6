using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] List<CustomerPathPoint> pathPoints = new List<CustomerPathPoint>();

    int currentPathIndex = 0;

    CustomerPathPoint customerPathPoint => pathPoints[currentPathIndex];

    public void Activate()
    {
        agent.enabled = true;

        if (pathPoints.Count > 0)
        {
            SetDestination(customerPathPoint.position);
        }
    }

    public void Deactivate()
    {
        agent.enabled = false;

        // Back to the pool?
    }

    void Update()
    {
        if (HasReachedDestination())
        {
            if (!customerPathPoint.IsOccupied())
            {
                float stayTime = customerPathPoint.CustomerArrived();
                
                StartCoroutine(GoToNextPathPoint(stayTime));
            }
            else
            {
                StartCoroutine(GoToNextPathPoint(1));
            }
        }
    }

    void SetDestination(Vector3 destination)
    {
         agent.SetDestination(destination);
    }

    IEnumerator GoToNextPathPoint(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        currentPathIndex++;
        if (currentPathIndex < pathPoints.Count)
        {
            SetDestination(customerPathPoint.position);
        }
        else
        {
            Deactivate();
        }
    }
    public void SetPath(List<CustomerPathPoint> path)
    {
        pathPoints = path;
    }

    public void PushPath(CustomerPathPoint pathPoint)
    {
        pathPoints.Add(pathPoint);
    }

    public void ConcatPath(List<CustomerPathPoint> path)
    {
        pathPoints.AddRange(path);
    }

    bool HasReachedDestination()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                return true; // Reached the point
            }
        }
        return false;
    }


}
