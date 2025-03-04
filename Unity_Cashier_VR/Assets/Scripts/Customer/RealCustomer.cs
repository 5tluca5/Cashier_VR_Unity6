using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class RealCustomer : Customer
{
    [SerializeField] GameObject desiredItemImage;
    [SerializeField] StoreItemData desiredItemData;
    [SerializeField] Transform handTransform;

    bool isAwitingItem = false;

    CustomerDesireItemIndicator desireItemIndicator;
    Transform playerTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        desireItemIndicator = GameObject.FindGameObjectWithTag("CustomerDesireItemIndicator").GetComponent<CustomerDesireItemIndicator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        if (desiredItemData == null)
            Debug.LogError("CustomerDesireItemIndicator is missing!");
    }

    public void SetDesireItem(StoreItemData storeItemData)
    {
        desiredItemData = storeItemData;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"{name} collided with {collision.gameObject.name}");
    }

    public async void CatchItem(StoreItem storeItem)
    {
        if (!isAwitingItem) return;

        storeItem.SetKinematic(true);
        storeItem.transform.SetParent(handTransform);
        storeItem.transform.DOMove(handTransform.position, 0.5f).SetEase(Ease.InOutSine);
        
        await Task.Delay(1000);

        if(storeItem.storeItemData == desiredItemData)
        {
            //desireItemIndicator.SetDesireItem(null);
            desiredItemImage.SetActive(false);

            isAwitingItem = false;

            StartCoroutine(GoToNextPathPoint(0));
        }
        else
        {
            // Throw it to player

            await Task.Delay(1000);

            storeItem.SetKinematic(false);
            storeItem.AddForce((playerTransform.position - storeItem.transform.position).normalized * 1000);
        }
    }

    protected override void Checkout()
    {
        base.Checkout();

        isAwitingItem = true;

        desireItemIndicator.SetDesireItem(desiredItemData);
        desiredItemImage.SetActive(true);
    }

}
