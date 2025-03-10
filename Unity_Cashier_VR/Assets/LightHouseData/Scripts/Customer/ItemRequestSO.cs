using UnityEngine;

[CreateAssetMenu(fileName = "ItemRequest", menuName = "LightHouseData/ItemRequestSO", order = 0)]
public class ItemRequestSO : ScriptableObject
{
    public Sprite itemSprite;
    public GameObject itemPrefab;

    public int GetItemID()
    {
        return itemPrefab.GetComponent<ItemCostData>().Id;
    }

    public void SetItemID(int id)
    {
        itemPrefab.GetComponent<ItemCostData>().SetItemID(id);
    }
}
