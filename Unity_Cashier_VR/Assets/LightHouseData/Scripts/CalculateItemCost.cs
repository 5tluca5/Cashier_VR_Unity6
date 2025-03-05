using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CalculateItemCost : MonoBehaviour
{
    [Header("Cost Data")]
    [SerializeField] private float totalCost;

    [Header("UI settings")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI priceText;

    [Header("Items Data")]
    [SerializeField] private List<ItemCostData> itemsOnTableList;

    private DetectItemInArea detectArea;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        detectArea = GetComponentInChildren<DetectItemInArea>();
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
        detectArea.OnItemEnter += AddItem;
        detectArea.OnItemExit += RemoveItem;
    }

    private void OnDisable()
    {
        detectArea.OnItemEnter -= AddItem;
        detectArea.OnItemExit -= RemoveItem;
    }

    private void AddItem(ItemCostData itemData)
    {
        if (itemsOnTableList.Contains(itemData))
            return;

        itemsOnTableList.Add(itemData);

        // Update cost
        totalCost += itemData.Cost;
        UpdatePriceText();
        HandleUICanvas();
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
        priceText.text = MathF.Max(0, totalCost).ToString();
    }

    private void HandleUICanvas()
    {
        var alpha = itemsOnTableList.Count > 0 ? 1 : 0;
        canvasGroup.alpha = alpha;
    }
}
