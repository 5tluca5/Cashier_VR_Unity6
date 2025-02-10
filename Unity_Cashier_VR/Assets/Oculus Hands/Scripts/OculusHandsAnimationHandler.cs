using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class OculusHandsAnimationHandler : MonoBehaviour
{
    [SerializeField] private InputActionProperty triggerPropeerty;
    [SerializeField] private InputActionProperty gripPropeerty;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float triggerValue = triggerPropeerty.action.ReadValue<float>();
        float gripValue = gripPropeerty.action.ReadValue<float>();

        animator.SetFloat("Trigger", triggerValue);
        animator.SetFloat("Grip", gripValue);
    }
}
