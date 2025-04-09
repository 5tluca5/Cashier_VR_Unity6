using Unity.MLAgents;
using UnityEngine;
using UnityEngine.AI;

public class StoreDoorAnim : MonoBehaviour
{
    private readonly int DoorOpenHash = Animator.StringToHash("Open");

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerStay(Collider other)
    {
        var customer = other.GetComponentInParent<NavMeshAgent>();
        var agent = other.GetComponent<RoamingAgent>();
        if (customer || agent)
        {
            animator.SetBool(DoorOpenHash, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var customer = other.GetComponentInParent<NavMeshAgent>();
        var agent = other.GetComponent<RoamingAgent>();
        if (customer || agent)
        {
            animator.SetBool(DoorOpenHash, false);
        }
    }
}
