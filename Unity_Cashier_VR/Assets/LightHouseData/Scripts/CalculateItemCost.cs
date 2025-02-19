using System;
using UnityEngine;

public class CalculateItemCost : MonoBehaviour
{
    [SerializeField] private float totalCost;

    private DetectItemInArea detectArea;

    private void Awake()
    {
        detectArea = GetComponentInChildren<DetectItemInArea>();
    }

    private void OnEnable()
    {
        detectArea.OnItemEnter += AddItemCost;
        detectArea.OnItemExit += SubtractItemCost;
    }

    private void SubtractItemCost()
    {
        totalCost -= 10;
    }

    private void AddItemCost()
    {
        totalCost += 10;
    }

    private void OnDisable()
    {
        detectArea.OnItemEnter -= AddItemCost;
        detectArea.OnItemExit -= SubtractItemCost;
    }
}
