using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DetectItemInArea : MonoBehaviour
{
    public event System.Action<ItemCostData> OnItemEnter;
    public event System.Action<ItemCostData> OnItemExit;

    public event System.Action<CustomerRequest> OnCustomerEnter;
    public event System.Action<CustomerRequest> OnCustomerExit;

    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        ItemCostData costData = other.transform.GetComponentInParent<ItemCostData>();
        if (costData)
        {
            Debug.Log("Item entered: " + costData.transform);
            OnItemEnter?.Invoke(costData);
        }

        var requestData = other.GetComponentInParent<CustomerRequest>();
        if (requestData)
        {
            Debug.Log("Customer entered: " + requestData.transform);
            OnCustomerEnter?.Invoke(requestData);
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

        var requestData = other.GetComponentInParent<CustomerRequest>();
        if (requestData)
        {
            Debug.Log("Customer exited: " + requestData.transform);
            OnCustomerExit?.Invoke(requestData);
        }
    }

}
