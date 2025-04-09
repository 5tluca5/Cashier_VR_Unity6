using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System.Linq;
public class MLAgentCustomer : Agent
{
    Rigidbody rBody;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    [SerializeField] Transform customerSpawnPoint;
    [SerializeField] Transform customerExitPoint;
    [SerializeField] Transform customerEntryPoint;
    public Transform[] TargetPositions;
    public Transform Target;
    private float episodeTimer;
    private List<Transform> paths = new List<Transform>();
    private int pathIndex = 0;
    public override void OnEpisodeBegin()
    {

        // Reset the agent's position and velocity
        transform.position = customerSpawnPoint.position;
        this.rBody.angularVelocity = Vector3.zero;
        this.rBody.linearVelocity = Vector3.zero;
        this.transform.localRotation = Quaternion.Euler(0, 0, 0);
        // transform.localRotation = SpawnPoint.localRotation;
        // Move the target to a new spot
        pathIndex = 0;
        episodeTimer = 0;
        lastDistance = 0;
        paths.Clear(); // Clear previous paths before generating new ones
        int totalPathCount = Mathf.Min(Random.Range(1, TargetPositions.Length), TargetPositions.Length - 1);

        paths.Add(customerEntryPoint);
        paths.AddRange(TargetPositions.OrderBy(x => Random.value).Take(totalPathCount).ToList());
        paths.Add(customerExitPoint);

        Target.position = paths[pathIndex].position;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        sensor.AddObservation(Target.localPosition);    //3 floats
        sensor.AddObservation(this.transform.localPosition);    //3 floats
        Vector3 relativePos = transform.InverseTransformPoint(Target.position);
        sensor.AddObservation(relativePos); // 3 floats
        sensor.AddObservation(transform.InverseTransformDirection(rBody.linearVelocity)); // 3 floats

    }

    public float moveSpeed = 5f;
    public float turnSpeed = 150f;
    float lastDistance = 0;
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Small negative reward per step to encourage faster reaching
        AddReward(-0.001f);

        // Samll reward for facing the target
        // Vector3 targetDirection = (Target.position - transform.position).normalized;
        // float angle = Vector3.Angle(transform.forward, targetDirection);
        // if (angle < 10f)
        // {
        //     AddReward(0.01f);
        // }
        // else if (angle < 30f)
        // {
        //     AddReward(0.005f);
        // }
        
        MoveAgent(actionBuffers.DiscreteActions);

        float distanceToTarget = Vector3.Distance(transform.position, Target.position);

        // small reward for getting closer to the target
        // if(lastDistance != 0)
        //     AddReward(0.01f * (lastDistance - distanceToTarget));

        if (distanceToTarget < 1f)
        {
            AddReward(0.2f);
            pathIndex++;
            if (pathIndex < paths.Count)
            {
                Target.position = paths[pathIndex].position;
            }
            else
            {
                SetReward(1f); // Successful full path!
                EndEpisode();
            }
        }

        episodeTimer += Time.deltaTime;

        if (episodeTimer > 30f) // e.g. 30 seconds max
        {
            AddReward(-0.5f);
            EndEpisode();
        }

        lastDistance = distanceToTarget;
    }
    public void MoveAgent(ActionSegment<int> act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        var action = act[0];
        switch (action)
        {
            case 1:
                dirToGo = transform.forward * 1f;
                break;
            case 2:
                dirToGo = transform.forward * -1f;
                break;
            case 3:
                rotateDir = transform.up * 1f;
                break;
            case 4:
                rotateDir = transform.up * -1f;
                break;
        }

        transform.Rotate(rotateDir, Time.deltaTime * turnSpeed);
        rBody.AddForce(dirToGo * moveSpeed, ForceMode.VelocityChange);
        // transform.position += dirToGo * moveSpeed * Time.deltaTime;

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[0] = 3;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            discreteActionsOut[0] = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            discreteActionsOut[0] = 4;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            discreteActionsOut[0] = 2;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Level"))
        {
            AddReward(-0.1f); // Smaller penalty for minor collisions
            if (collision.relativeVelocity.magnitude > 8f) // Large collision
            {
                SetReward(-1f); // End episode after major impact
                EndEpisode();
            }
            Debug.Log("Hit wall");
        }
    }
}
