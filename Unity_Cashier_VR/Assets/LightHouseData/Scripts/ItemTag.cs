using UnityEngine;

public class ItemTag : MonoBehaviour
{
    public ItemType itemTag;
}

public enum ItemType
{
    None,
    Coin,
    CoinPouch,
    Weapon_Common,
    Weapon_Uncommon,
    Weapon_Golden,
    Potion_Small,
    Potion_Medium,
    Potion_Large,
    Shield_Common,
    Shield_Uncommon,
    Shield_Golden,
    Arrow,
}
