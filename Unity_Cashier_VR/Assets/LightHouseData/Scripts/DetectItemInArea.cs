using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class DetectItemInArea : MonoBehaviour
{
    public event System.Action OnItemEnter;
    public event System.Action OnItemExit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponentInParent<XRGrabInteractable>())
        {
            Debug.Log("Item entered: " + other.transform);
            OnItemEnter?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.GetComponentInParent<XRGrabInteractable>())
        {
            Debug.Log("Item exited: " + other.transform);
            OnItemExit?.Invoke();
        }
    }
}
