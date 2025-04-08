using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(Collider))]
public class CoinPouchHandler : MonoBehaviour
{
    public static CoinPouchHandler Instance { private set; get; }

    public const int COIN_WORTH = 5;
    [SerializeField] private XRSocketInteractor coinPouchSocketInteractor;

    [SerializeField] private int coinCount;

    public System.Action OnCoinPouchGrabbed;
    public System.Action OnCoinPouchReleased;
    public System.Action<int> OnCoinCountChanged;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        GetComponent<Collider>().isTrigger = true;
        coinPouchSocketInteractor.selectEntered.AddListener(OnPouchDropped);
        coinPouchSocketInteractor.selectExited.AddListener(OnPouchPickedUp);
    }

    private void OnDestroy()
    {
        coinPouchSocketInteractor.selectEntered.RemoveListener(OnPouchDropped);
        coinPouchSocketInteractor.selectExited.RemoveListener(OnPouchPickedUp);
    }

    public int GetCoinCount() => coinCount;

    public void DeductCoins(int count)
    {
        coinCount -= count;
        OnCoinCountChanged?.Invoke(coinCount);
    }

    private void OnPouchDropped(SelectEnterEventArgs eventArgs)
    {
        Debug.Log("Coin Pouch Released");
        OnCoinPouchReleased?.Invoke();
    }

    private void OnPouchPickedUp(SelectExitEventArgs eventArgs)
    {
        Debug.Log("Coin Pouch Grabbed");
        OnCoinPouchGrabbed?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        var itemTag = other.GetComponentInParent<ItemTag>();
        if (itemTag && itemTag.itemTag == ItemType.Coin)
        {
            coinCount += COIN_WORTH;
            OnCoinCountChanged?.Invoke(coinCount);
            Debug.Log("Coin Dropped In Pouch: " + coinCount);
            Destroy(itemTag.gameObject);
        }
    }
}
