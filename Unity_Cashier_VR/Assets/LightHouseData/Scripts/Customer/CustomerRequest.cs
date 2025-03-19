using System.Collections.Generic;
using UnityEngine;

public class CustomerRequest : MonoBehaviour
{
    [SerializeField] private CustomerRequestData customerRequestData;
    [SerializeField] private int Max_Requests = 8;

    public System.Action<CustomerRequestData> OnRequestedItemsListCompleted;
    public System.Action OnRequestedItemsInitilized;

    private void Start()
    {
        var randomNumber = Random.Range(1, Max_Requests + 1);

        customerRequestData.SetRequestID(CustomerRequestProvider.Instance.GetRequestID());

        for (int i = 0; i < randomNumber; i++)
        {
            var randomItem = CustomerRequestProvider.Instance.GetRandomRequest();
            customerRequestData.AddItem(randomItem);
        }

        customerRequestData.requestedItemsList.Sort((x, y) => x.GetItemID().CompareTo(y.GetItemID()));

        OnRequestedItemsListCompleted?.Invoke(customerRequestData);
        OnRequestedItemsInitilized?.Invoke();
    }

    public CustomerRequestData GetCustomerRequest()
    {
        return customerRequestData;
    }
}

[System.Serializable]
public class CustomerRequestData
{
    public int requestID;
    public List<ItemRequestSO> requestedItemsList;

    /// <summary>
    /// Set the unique request ID
    /// </summary>
    /// <param name="id">Provide unique id as some order may contain exactly same items</param>
    public void SetRequestID(int id) => requestID = id;

    /// <summary>
    /// Get the unique request ID
    /// </summary>
    /// <returns>Unique request id</returns>
    public int GetRequestID() => requestID;

    public void AddItem(ItemRequestSO item)
    {
        requestedItemsList.Add(item);
    }
}
