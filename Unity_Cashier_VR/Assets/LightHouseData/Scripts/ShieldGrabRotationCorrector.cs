using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ShieldGrabRotationCorrector : MonoBehaviour
{
    [SerializeField] private InputActionProperty leftHandGripProperty;
    [SerializeField] private InputActionProperty rightHandGripProperty;
    [SerializeField] private Transform attachPoint;

    private XRGrabInteractable grabInteractable;
    private bool isLastGrabbedByLeftHand;

    private void OnEnable()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(HandleGrab);
        grabInteractable.selectExited.AddListener(HandleLetGo);
    }

    private void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(HandleGrab);
        grabInteractable.selectExited.RemoveListener(HandleLetGo);
    }

    private void HandleGrab(SelectEnterEventArgs args)
    {
        var currentRot = attachPoint.localRotation;
        var newRot = Quaternion.identity;

        if (leftHandGripProperty.action.IsPressed() && !isLastGrabbedByLeftHand)
        {
            newRot = Quaternion.Euler(currentRot.x, 90f, currentRot.z);
            isLastGrabbedByLeftHand = true;
        }
        else if (rightHandGripProperty.action.IsPressed())
        {
            newRot = Quaternion.Euler(currentRot.x, 270f, currentRot.z);
            isLastGrabbedByLeftHand = false;
        }

        attachPoint.localRotation = newRot;
        Debug.Log("Grab: " + isLastGrabbedByLeftHand);
    }

    private void HandleLetGo(SelectExitEventArgs args)
    {
        if (!leftHandGripProperty.action.IsPressed() && !rightHandGripProperty.action.IsPressed())
            isLastGrabbedByLeftHand = false;
        Debug.Log("Let Go: " + isLastGrabbedByLeftHand);
    }
}
