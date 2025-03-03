using UnityEngine;
using UnityEngine.UI;

public class RealCustomer : Customer
{
    [SerializeField] RawImage desiredItemImage;
    [SerializeField] StoreItemData desiredItemData;
    bool isAwitingItem = false;

    CustomerDesireItemIndicator desireItemIndicator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        desireItemIndicator = GameObject.FindGameObjectWithTag("CustomerDesireItemIndicator").GetComponent<CustomerDesireItemIndicator>();

        if(desiredItemData == null)
            Debug.LogError("CustomerDesireItemIndicator is missing!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDesireItem(StoreItemData storeItemData)
    {
        desiredItemData = storeItemData;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"{name} collided with {collision.gameObject.name}");
    }

    public void CatchItem(StoreItem storeItem)
    {
        if (!isAwitingItem) return;


    }
}
