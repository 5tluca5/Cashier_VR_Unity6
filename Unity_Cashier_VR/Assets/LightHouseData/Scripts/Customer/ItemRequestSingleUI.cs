using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemRequestSingleUI : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemQuantityTMP; // If we want to show the quantity of the item in future!

    public void SetItem(ItemRequestSO itemSO)
    {
        itemImage.sprite = itemSO.itemSprite;
    }
}
