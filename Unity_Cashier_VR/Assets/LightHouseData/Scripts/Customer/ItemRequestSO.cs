using UnityEngine;

[CreateAssetMenu(fileName = "ItemRequest", menuName = "LightHouseData/ItemRequestSO", order = 0)]
public class ItemRequestSO : ScriptableObject
{
    [SerializeField] private Sprite itemSprite;
    [SerializeField] private GameObject itemPrefab;
}
