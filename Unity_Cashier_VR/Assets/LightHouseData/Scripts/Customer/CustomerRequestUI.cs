using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class CustomerRequestUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI totalCostTMP;
    [SerializeField] private ItemRequestSingleUI itemRequestSingleUITemplate;
    [SerializeField] private Transform itemRequestListParent;
    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private CustomerRequest customerRequest;

    private void Awake()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponentInParent<CanvasGroup>();
        }
        itemRequestSingleUITemplate.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        customerRequest.OnRequestedItemsListCompleted += SetRequestList;
    }

    private void OnDisable()
    {
        customerRequest.OnRequestedItemsListCompleted -= SetRequestList;
    }

    private void Start()
    {
        Hide();
    }

    public void Show()
    {
        canvasGroup.DOFade(1f, .5f);
    }

    public void Hide()
    {
        canvasGroup.DOFade(0f, .5f);
    }

    public void SetRequestList(CustomerRequestData customerRequestData)
    {
        foreach (Transform child in itemRequestListParent)
        {
            if (child == itemRequestSingleUITemplate.transform) continue;
            Destroy(child.gameObject);
        }

        int cost = 0;
        foreach (var item in customerRequestData.requestedItemsList)
        {
            var itemUI = Instantiate(itemRequestSingleUITemplate, itemRequestListParent);
            itemUI.SetItem(item);
            itemUI.gameObject.SetActive(true);

            var itemCost = item.itemPrefab.GetComponent<ItemCostData>();
            cost += itemCost.Cost;
        }

        totalCostTMP.text = cost.ToString();
    }
}
