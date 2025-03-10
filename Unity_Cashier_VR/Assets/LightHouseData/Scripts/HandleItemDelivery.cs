using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class HandleItemDelivery : MonoBehaviour
{
    [Header("Cost Data")]
    [SerializeField] private float totalCost;

    [Header("UI settings")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI priceText;
    private CanvasGroup canvasGroup;

    [Header("Detect Area")]
    [SerializeField] private DetectItemInArea detectItem;

    [Header("Items Data")]
    [SerializeField] private List<ItemCostData> itemsOnTableList;

    [Header("Customer Request List")]
    [SerializeField] private List<CustomerRequestData> customerRequestsList;

    public System.Action OnDeliveryComplete;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        titleText.text = "Total coins";
        UpdatePriceText();
        HandleUICanvas();
    }

    private void OnEnable()
    {
        detectItem.OnItemEnter += AddItem;
        detectItem.OnItemExit += RemoveItem;

        detectItem.OnCustomerEnter += SetCustomerRequest;
        detectItem.OnCustomerExit += RemoveCustomerRequest;
    }

    private void OnDisable()
    {
        detectItem.OnItemEnter -= AddItem;
        detectItem.OnItemExit -= RemoveItem;

        detectItem.OnCustomerEnter -= SetCustomerRequest;
        detectItem.OnCustomerExit -= RemoveCustomerRequest;
    }

    private void SetCustomerRequest(CustomerRequest customerRequest)
    {
        if (customerRequestsList.Exists(x => x.GetRequestID() == customerRequest.GetCustomerRequest().GetRequestID()))
            return;

        customerRequestsList.Add(customerRequest.GetCustomerRequest());

        CheckForDelivery();
    }

    private void RemoveCustomerRequest(CustomerRequest customerRequest)
    {
        customerRequestsList.Remove(customerRequest.GetCustomerRequest());
    }

    private void AddItem(ItemCostData itemData)
    {
        if (itemsOnTableList.Contains(itemData))
            return;

        itemsOnTableList.Add(itemData);

        CheckForDelivery();

        CalculateTotalCost();
        UpdatePriceText();
        HandleUICanvas();
    }

    //[Header("DEBUG ONLY")]
    //[SerializeField] private List<ItemCostData> deliverableItemsList = new();
    private void CheckForDelivery()
    {
        bool isReadyForDelivery = false;

        CustomerRequestData deliveryRequest = default;
        List<ItemCostData> deliverableItemsList = new();
        //deliverableItemsList.Clear(); // Clear deliverable items list // When Debugging

        foreach (var customerRequest in customerRequestsList)
        {
            isReadyForDelivery = true;
            foreach (var item in customerRequest.requestedItemsList)
            {
                if (!itemsOnTableList.Exists(x => x.Id == item.GetItemID())) // Check if item is on table
                {
                    isReadyForDelivery = false; // If item is not on table, break the loop
                    itemsOnTableList.AddRange(deliverableItemsList); // Add deliverable items back to table
                    deliverableItemsList.Clear(); // Clear deliverable items list // When not Debugging
                    break;
                }

                var delivrableItem = itemsOnTableList.Find(x => x.Id == item.GetItemID()); // Find deliverable item
                deliverableItemsList.Add(delivrableItem); // Add deliverable item to list
                // Remove deliverable item from table so duplicate items are not counted.
                // eg. suppose Axe has Id 1001 then it should be removed from list, so next time the other Axe with Id 1001 is added to the delivery list.
                itemsOnTableList.Remove(delivrableItem);
            }
            if (isReadyForDelivery)
            {
                //Debug.Log("Ready for delivery with ID: " + customerRequest.GetRequestID());
                deliveryRequest = customerRequest; // Set delivery request for deletion as we can't delete from list while iterating
                break;
            }
        }

        if (isReadyForDelivery)
        {
            customerRequestsList.Remove(deliveryRequest); // Remove delivery request from list
            deliverableItemsList.ForEach(x => Destroy(x.gameObject)); // Destroy deliverable items
            OnDeliveryComplete?.Invoke(); // Invoke delivery complete event
        }
    }

    private void CalculateTotalCost()
    {
        totalCost = 0;
        itemsOnTableList.ForEach(x => totalCost += x.Cost);
    }

    private void RemoveItem(ItemCostData itemData)
    {
        if (!itemsOnTableList.Contains(itemData))
            return;

        itemsOnTableList.Remove(itemData);

        // Update cost
        totalCost -= itemData.Cost;
        UpdatePriceText();
        HandleUICanvas();
    }

    private void UpdatePriceText()
    {
        priceText.text = Mathf.Max(0, totalCost).ToString();
    }

    private void HandleUICanvas()
    {
        var alpha = itemsOnTableList.Count > 0 ? 1 : 0;
        canvasGroup.DOFade(alpha, duration: 1f);
    }
}
