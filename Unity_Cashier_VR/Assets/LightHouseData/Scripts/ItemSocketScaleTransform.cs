using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(XRSocketInteractor))]
public class ItemSocketScaleTransform : MonoBehaviour
{
    [SerializeField] private Vector3 targetScale = Vector3.one;
    private Vector3 originalScale;

    private void Awake()
    {
        XRSocketInteractor socket = GetComponent<XRSocketInteractor>();
        socket.selectEntered.AddListener(OnSelectedEntered);
        socket.selectExited.AddListener(OnSelectedExited);
    }

    private void OnDestroy()
    {
        XRSocketInteractor socket = GetComponent<XRSocketInteractor>();
        socket.selectEntered.RemoveListener(OnSelectedEntered);
        socket.selectExited.RemoveListener(OnSelectedExited);
    }

    public void OnSelectedEntered(SelectEnterEventArgs eventArgs)
    {
        Transform interactableTransform = eventArgs.interactableObject.transform;
        Debug.Log("Selected entered: " + interactableTransform.name);
        originalScale = interactableTransform.localScale;
        interactableTransform.localScale = targetScale;
    }

    public void OnSelectedExited(SelectExitEventArgs eventArgs)
    {
        eventArgs.interactableObject.transform.localScale = originalScale;
    }
}
