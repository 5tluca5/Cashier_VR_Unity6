using System;
using UnityEngine;

public class ItemCostData : MonoBehaviour
{
    [SerializeField] private int cost = 10;

    public int Cost => cost;

    [field:SerializeField] public int Id { get; private set; }

    internal void SetItemID(int id)
    {
        Id = id;
    }
}
