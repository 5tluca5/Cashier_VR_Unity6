using UnityEngine;

public class ItemCostData : MonoBehaviour
{
    [SerializeField] private float cost = 10f;

    public float Cost => cost;
}
