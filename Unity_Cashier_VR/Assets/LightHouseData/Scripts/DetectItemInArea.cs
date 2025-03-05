using UnityEngine;

public class DetectItemInArea : MonoBehaviour
{
    public event System.Action<ItemCostData> OnItemEnter;
    public event System.Action<ItemCostData> OnItemExit;

    private void OnTriggerEnter(Collider other)
    {
        ItemCostData costData = other.transform.GetComponentInParent<ItemCostData>();
        if (costData)
        {
            Debug.Log("Item entered: " + costData.transform);
            OnItemEnter?.Invoke(costData);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ItemCostData costData = other.transform.GetComponentInParent<ItemCostData>();
        if (costData)
        {
            Debug.Log("Item exited: " + costData.transform);
            OnItemExit?.Invoke(costData);
        }
    }
}
