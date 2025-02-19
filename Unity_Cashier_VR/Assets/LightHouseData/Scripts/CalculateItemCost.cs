using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CalculateItemCost : MonoBehaviour
{
    [SerializeField] private float totalCost;

    [Header("UI settings")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI priceText;

    private DetectItemInArea detectArea;

    [SerializeField] private List<Transform> itemsOnTableList;

    private void Awake()
    {
        detectArea = GetComponentInChildren<DetectItemInArea>();
    }

    private void Start()
    {
        titleText.text = "Total coins";
        UpdatePriceText();
    }

    private void UpdatePriceText()
    {
        priceText.text = MathF.Max(0, totalCost).ToString();
    }

    private void OnEnable()
    {
        detectArea.OnItemEnter += AddItem;
        detectArea.OnItemExit += RemoveItem;
    }

    private void RemoveItem(Transform itemTransform)
    {
        if (!itemsOnTableList.Contains(itemTransform))
            return;

        itemsOnTableList.Remove(itemTransform);

        // Update cost
        var costData = itemTransform.GetComponent<ItemCostData>();
        totalCost -= costData.Cost;
        UpdatePriceText();
    }

    private void AddItem(Transform itemTransform)
    {
        if (itemsOnTableList.Contains(itemTransform))
            return;

        itemsOnTableList.Add(itemTransform);

        // Update cost
        var costData = itemTransform.GetComponent<ItemCostData>();
        totalCost += costData.Cost;
        UpdatePriceText();
    }

    private void OnDisable()
    {
        detectArea.OnItemEnter -= AddItem;
        detectArea.OnItemExit -= RemoveItem;
    }
}
