using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DetectDeliveryStation : MonoBehaviour
{
    [SerializeField] private CustomerRequest customerRequest;
    [SerializeField] private CustomerRequestUI requestUI;

    private HandleItemDelivery currentDelieveryStation;
    private Collider detectionCollider;

    private void Awake()
    {
        detectionCollider = GetComponent<Collider>();
        detectionCollider.isTrigger = true;
        detectionCollider.enabled = false;
    }

    private void OnEnable()
    {
        customerRequest.OnRequestedItemsInitilized += StartDetectingDeliveryStation;
    }

    private void OnDisable()
    {
        customerRequest.OnRequestedItemsInitilized -= StartDetectingDeliveryStation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!detectionCollider.enabled) return;

        var deliveryStation = other.GetComponentInParent<HandleItemDelivery>();
        Debug.Log("Delivery station entered: " + deliveryStation);
        if (deliveryStation && currentDelieveryStation != deliveryStation)
        {
            currentDelieveryStation = deliveryStation;
            requestUI.Show();
            deliveryStation.OnDeliveryComplete += HandleDeliveryComplete;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!detectionCollider.enabled) return;

        var deliveryStation = other.GetComponentInParent<HandleItemDelivery>();
        //Debug.Log("Delivery station exited: " + deliveryStation);
        if (currentDelieveryStation && currentDelieveryStation == deliveryStation)
        {
            currentDelieveryStation = null;
            requestUI.Hide();
            deliveryStation.OnDeliveryComplete -= HandleDeliveryComplete;
        }
    }

    private void StartDetectingDeliveryStation()
    {
        detectionCollider.enabled = true;
    }

    private void HandleDeliveryComplete()
    {
        requestUI.Hide();
        currentDelieveryStation.OnDeliveryComplete -= HandleDeliveryComplete;
        detectionCollider.enabled = false;
    }
}
