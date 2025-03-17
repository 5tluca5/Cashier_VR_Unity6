using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(Collider))]
public class CoinPouchHandler : MonoBehaviour
{
    [SerializeField] private XRSocketInteractor grabInteractable;

    [SerializeField] private int coinCount;

    public System.Action OnCoinPouchGrabbed;
    public System.Action OnCoinPouchReleased;
    public System.Action<int> OnCoinDroppedInPouch;

    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
        grabInteractable.selectEntered.AddListener(OnPouchDropped);
        grabInteractable.selectExited.AddListener(OnPouchPickedUp);
    }

    private void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnPouchDropped);
        grabInteractable.selectExited.RemoveListener(OnPouchPickedUp);
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
        Debug.Log(other.transform.name);
        var itemTag = other.GetComponentInParent<ItemTag>();
        if (itemTag && itemTag.itemTag == ItemType.Coin)
        {
            OnCoinDroppedInPouch?.Invoke(++coinCount);
            Debug.Log("Coin Dropped In Pouch: " + coinCount);
            Destroy(other.gameObject);
        }
    }
}
