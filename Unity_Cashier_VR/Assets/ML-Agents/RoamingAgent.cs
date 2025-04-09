using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class RoamingAgent : Agent
{

    // mlagents-learn config/ppo/RoamingAgent.yaml --run-id=RoamingAgent

    Rigidbody rb;
    public Transform target;
    public int pathIndex = 0;
    public float moveSpeed = 5f;
    public Transform[] path;
    public Vector3 spawnPos = Vector3.zero;
    public Transform modelGFX;

    public float simulationTime = 0f;

    public float closestDistance = 100f;
    public float lastDistance = 0f;

    protected override void Awake()
    {
        rb = GetComponent<Rigidbody>();
        spawnPos = transform.localPosition;
    }

    public override void OnEpisodeBegin()
    {
        if (this.transform.localPosition.y < 0)
        {
            this.rb.angularVelocity = Vector3.zero;
            this.rb.linearVelocity = Vector3.zero;
        }
        simulationTime = 0f;
        closestDistance = 100f;
        pathIndex = 0;
        transform.localPosition = spawnPos;
        target.localPosition = path[0].localPosition;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(target.localPosition); // 3 floats
        sensor.AddObservation(this.transform.localPosition); // 3 floats
        sensor.AddObservation(closestDistance); // 1 float
        sensor.AddObservation(rb.linearVelocity.x); // 1 float
        sensor.AddObservation(rb.linearVelocity.z); // 1 float

        // Total floats: 9 floats
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actions.ContinuousActions[0];
        controlSignal.z = actions.ContinuousActions[1];
        transform.Translate(moveSpeed * Time.deltaTime * controlSignal, Space.World); // move the agent

        simulationTime += Time.deltaTime;

        float distanceToTarget = Vector3.Distance(this.transform.localPosition, target.localPosition);

        if (distanceToTarget < closestDistance) // If this is the new closest disstance
        {
            AddReward(10f); // reward huge for getting closer
        }

        if (distanceToTarget >= lastDistance) // If the agent is getting further away or staying the same distance
        {
            AddReward(-0.1f); // penalize for getting further away
        }
        else if (distanceToTarget < lastDistance) // If the agent is getting closer
        {
            AddReward(0.1f); // reward for getting closer
        }

        closestDistance = Mathf.Min(closestDistance, distanceToTarget); // update closest distance

        if (distanceToTarget < 1.2f)
        {
            pathIndex += 1;
            if (pathIndex >= path.Length)
            {
                AddReward(50f);
                EndEpisode();
            }
            else
            {
                AddReward(1.0f);
                closestDistance = 100f; // reset closest distance
                target.localPosition = path[pathIndex].localPosition;
            }
        }

        if (this.transform.localPosition.y < 0 || simulationTime >= 60f)
        {
            SetReward(-10.0f);
            EndEpisode();
        }

        var dir = target.localPosition - transform.localPosition;
        dir.y = modelGFX.forward.y;
        modelGFX.forward = Vector3.Slerp(modelGFX.forward, dir, Time.deltaTime * 2f);
        lastDistance = distanceToTarget;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var actions = actionsOut.ContinuousActions;
        actions[0] = Input.GetAxis("Horizontal");
        actions[1] = Input.GetAxis("Vertical");
    }
}
