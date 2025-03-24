using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UniRx;

public enum CustomerBehaviour
{
    Idle,
    Arriving,
    LookingUp,
    LookingAround,
    Sitting,
    SelectingItems,
    CheckingOut,
    Leaving
}

public class Customer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] MeshRenderer model;
    [SerializeField] Animator animator;

    [Header("Animation keys")]
    [SerializeField] string idleKey = "Idle";
    [SerializeField] string lookUpKey = "LookUp";
    [SerializeField] string lookAroundKey = "LookAround";
    [SerializeField] string sitKey = "Sit";

    [Header("Debug")]
    [SerializeField] List<CustomerPathPoint> pathPoints = new List<CustomerPathPoint>();

    Subject<Customer> onArrived = new Subject<Customer>();
    Subject<Customer> onLeft = new Subject<Customer>();
    public Subject<Customer> OnArrived() => onArrived;

    public Subject<Customer> OnLeft() => onLeft;

    bool moving = false;
    int currentPathIndex = 0;

    CustomerPathPoint customerPathPoint => pathPoints[currentPathIndex];

    public void Reset()
    {
        currentPathIndex = 0;
        agent.enabled = false;
        pathPoints.Clear();

        onArrived.Dispose();
        onLeft.Dispose();
        onArrived = new Subject<Customer>();
        onLeft = new Subject<Customer>();
    }

    [ContextMenu("Activate")]
    public void Activate()
    {
        agent.enabled = true;

        if (pathPoints.Count > 0)
        {
            SetDestination(customerPathPoint).ToObservable().Subscribe();
        }
    }

    public void Deactivate()
    {
        agent.enabled = false;

        // Back to the pool?
    }

    void Update()
    {
        if (!agent.enabled) return;

        if (moving && HasReachedDestination())
        {
            ReachedDestination(customerPathPoint).ToObservable().Subscribe();

            // if (!customerPathPoint.IsOccupied())
            // {
            //     ReachedDestination(customerPathPoint).ToObservable().Subscribe();
            // }
            // else
            // {
            //     Debug.Log(name + "Path point is occupied");
            //     StartCoroutine(GoToNextPathPoint(0));
            // }
        }
        else
        {
            // SmoothLookAt(customerPathPoint.targetPosition);
        }
        // if (customerPathPoint != null)
        // {
        //     SmoothLookAt(customerPathPoint.targetPosition);
        // }
    }
    private void SmoothLookAt(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - model.transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        targetRotation.x = 0;
        float rotationSpeed = agent.angularSpeed * Time.deltaTime;
        model.transform.rotation = Quaternion.RotateTowards(model.transform.rotation, targetRotation, rotationSpeed);
    }
    IEnumerator SetDestination(CustomerPathPoint point)
    {
        point.CustomerReserved();
        //Debug.Log(name + "Set destination: " + point.name);

        if(point.GetRandomBehaviour() != CustomerBehaviour.Leaving)
        {
            yield return StartCoroutine(RotateTowards(Quaternion.LookRotation(point.position - transform.position)));

        bool lookAroundBeforeGoing = Random.value > 0.5f;

        if (lookAroundBeforeGoing)
        {
            animator.SetTrigger(lookAroundKey);

            yield return new WaitForSeconds(2f);

        }

        float waitTime = Random.value;

        yield return new WaitForSeconds(Random.value);
        
        }
        

        moving = true;
        // model.transform.rotation = transform.rotation;
        agent.SetDestination(point.position);

        yield return null;
    }

    protected IEnumerator GoToNextPathPoint(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        customerPathPoint.CustomerLeft();

        currentPathIndex++;

        while (customerPathPoint.IsOccupied())
        {
            currentPathIndex++;
        }

        if (currentPathIndex < pathPoints.Count)
        {
            SetDestination(customerPathPoint).ToObservable().Subscribe();
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

    IEnumerator ReachedDestination(CustomerPathPoint pathPoint)
    {
        moving = false;

        var behaviour = pathPoint.CustomerArrived();
        var stayTime = pathPoint.GetRandomStayTime();

        // rotate to the direction of the path point, then behave
        // model.transform.LookAt(pathPoint.targetPosition);
        //Debug.Log(name + "Reached destination: " + pathPoint.name);
        yield return StartCoroutine(RotateTowards(pathPoint.rotation));

        Behave(behaviour);

        if (behaviour == CustomerBehaviour.Leaving || behaviour == CustomerBehaviour.CheckingOut)
        {
            yield break;
        }

        yield return StartCoroutine(GoToNextPathPoint(stayTime));
    }

    float rotateTimer = 0f;
    IEnumerator RotateTowards(Quaternion rotation)
    {
        rotateTimer = 0f;
        float tolerance = 0.01f; // Adjust the tolerance value as needed
        while (Mathf.Abs(rotation.eulerAngles.y - transform.rotation.eulerAngles.y) > tolerance)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, agent.angularSpeed * 2 * Time.deltaTime);
            rotateTimer += Time.deltaTime;

            if (rotateTimer > 2f)
            {
                Debug.LogError(name + "Rotation took too long");
                break;
            }
            yield return null;
        }
    }
    void Behave(CustomerBehaviour behaviour)
    {
        //Debug.Log(name + "Behaving: " + behaviour);

        switch (behaviour)
        {
            case CustomerBehaviour.Idle:
                break;
            case CustomerBehaviour.LookingUp:
                break;
            case CustomerBehaviour.LookingAround:
                break;
            case CustomerBehaviour.Sitting:
                break;
            case CustomerBehaviour.Leaving:
                Leaving();
                break;
            case CustomerBehaviour.SelectingItems:
                // TODO: Select items on table
                break;
            case CustomerBehaviour.CheckingOut:
                Checkout();
                break;
        }
    }

    void Leaving()
    {
        onLeft.OnNext(this);
    }

    virtual protected void Checkout()
    {

    }

    public void LeaveStore()
    {
        //Debug.Log(name + "Leaving store");
        SetDestination(pathPoints[pathPoints.Count - 1]).ToObservable().Subscribe();
    }
}
