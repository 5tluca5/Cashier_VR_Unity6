using System.Collections.Generic;
using UnityEngine;

public class CustomerRequestProvider : MonoBehaviour
{
    public static CustomerRequestProvider Instance { private set; get; }

    [SerializeField] private List<ItemRequestSO> requestableItems;

    private int requestID = 10001;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public int GetRequestID()
    {
        return requestID++;
    }

    public ItemRequestSO GetRandomRequest()
    {
        return requestableItems[Random.Range(0, requestableItems.Count)];
    }

    [ContextMenu("Set Item Ids")]
    private void SetItemID()
    {
        int itemId = 1001;

        foreach (var item in requestableItems)
        {
            item.SetItemID(itemId++);
        }
    }
}
