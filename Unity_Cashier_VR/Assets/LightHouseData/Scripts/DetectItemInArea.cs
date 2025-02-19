using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class DetectItemInArea : MonoBehaviour
{
    public event System.Action<Transform> OnItemEnter;
    public event System.Action<Transform> OnItemExit;

    private void OnTriggerEnter(Collider other)
    {
        ItemCostData costData = other.transform.GetComponentInParent<ItemCostData>();
        if (costData)
        {
            Debug.Log("Item entered: " + costData.transform);
            OnItemEnter?.Invoke(costData.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ItemCostData costData = other.transform.GetComponentInParent<ItemCostData>();
        if (costData)
        {
            Debug.Log("Item exited: " + costData.transform);
            OnItemExit?.Invoke(costData.transform);
        }
    }
}
