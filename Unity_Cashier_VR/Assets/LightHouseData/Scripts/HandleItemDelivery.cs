using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class HandleItemDelivery : MonoBehaviour
{
    [Header("Cost Data")]
    [SerializeField] private float totalCost;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private Transform coinSpawnPoint;

    [Header("UI settings")] // JD: I want to move UI part to its own script to seperate UI and Logic, yet that's the problem for future JD.
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI priceText;
    private CanvasGroup canvasGroup;

    [Header("Detect Area")]
    [SerializeField] private DetectItemInArea detectItem;

    [Header("Items Data")]
    [SerializeField] private List<ItemCostData> itemsOnTableList;

    [Header("Customer Request List")]
    [SerializeField] private List<CustomerRequestData> customerRequestsList;

    public System.Action<int> OnDeliveryComplete;

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
    //[SerializeField] private List<ItemCostData> deliverableItemsList = new(); // when Debugging
    private void CheckForDelivery()
    {
        int cost = 0;
        bool isReadyForDelivery = false;

        CustomerRequestData deliveryRequest = default;
        List<ItemCostData> deliverableItemsList = new(); // When not Debugging
        //deliverableItemsList.Clear(); // Clear deliverable items list // When Debugging

        foreach (var customerRequest in customerRequestsList)
        {
            if (customerRequest.requestedItemsList.Count != itemsOnTableList.Count)
                continue; // If item count is not equal, skip to next request

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
                cost += delivrableItem.Cost; // Add cost of deliverable item to total cost

                // Remove deliverable item from table so duplicate items are not counted.
                // eg. suppose Axe has Id 1001 then it should be removed from list, so next time the other Axe with Id 1001 is added to the delivery list.
                itemsOnTableList.Remove(delivrableItem);
            }
            if (isReadyForDelivery)
            {
                Debug.Log("Ready for delivery with ID: " + customerRequest.GetRequestID());
                deliveryRequest = customerRequest; // Set delivery request for deletion as we can't delete from list while iterating
                break;
            }
        }

        if (isReadyForDelivery)
        {
            customerRequestsList.Remove(deliveryRequest); // Remove delivery request from list
            SpawnCoins(cost);
            deliverableItemsList.ForEach(x => Destroy(x.gameObject)); // Destroy deliverable items
            OnDeliveryComplete?.Invoke(deliveryRequest.requestID); // Invoke delivery complete event
        }
    }

    private void SpawnCoins(int cost)
    {
        int spawnCount = cost / 5;
        Debug.Log("Spawn Count: " + spawnCount + " Cost: " + cost);
        for (int i = 0; i < spawnCount; i++)
        {
            var spawnPosition = new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 2f), Random.Range(-.5f, .5f));
            var spawnRotation = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
            var coin = Instantiate(coinPrefab, coinSpawnPoint);
            coin.transform.SetLocalPositionAndRotation(spawnPosition, spawnRotation);
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
