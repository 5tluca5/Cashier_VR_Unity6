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

    private void OnTriggerEnter(Collider other)
    {
        var agent = other.GetComponentInParent<NavMeshAgent>();
        if (agent)
        {
            animator.SetBool(DoorOpenHash, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var agent = other.GetComponentInParent<NavMeshAgent>();
        if (agent)
        {
            animator.SetBool(DoorOpenHash, false);
        }
    }
}
