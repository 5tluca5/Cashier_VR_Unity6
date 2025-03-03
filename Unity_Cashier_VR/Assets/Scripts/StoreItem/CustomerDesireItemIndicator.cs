using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomerDesireItemIndicator : MonoBehaviour
{
    [SerializeField] List<StoreItemData> storeItemDataList = new();
    [SerializeField] Transform itemParent;
    [SerializeField] GameObject item;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        storeItemDataList = Resources.LoadAll<StoreItemData>("StoreItemData").ToList();
    }

    public void SetDesireItem(StoreItemData storeItemData)
    {
        if (storeItemDataList.Contains(storeItemData))
        {
            if(item != null)
                Destroy(item);

            item = Instantiate(storeItemData.itemPrefab, itemParent);
            item.transform.localPosition = Vector3.zero;
        }
    }
}
