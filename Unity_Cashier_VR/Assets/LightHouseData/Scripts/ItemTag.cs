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
    Weapon,
    Potion,
    Shield,
}
