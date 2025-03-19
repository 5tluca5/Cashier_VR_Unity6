using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private float spawnTime;
    [SerializeField] private Transform spawnLocation;
    [SerializeField] private XRGrabInteractable prefabToSpawn;
    [SerializeField] private XRSocketInteractor socketInteractor;

    [SerializeField] private Image progressBarImage;

    private float currentSpawnTime;
    [SerializeField] private XRGrabInteractable spawnedItem;

    private void Start()
    {
        progressBarImage.fillAmount = 0;
        spawnedItem = GetComponentInChildren<XRGrabInteractable>();
        AttachToSocket();
    }

    private void Update()
    {
        Debug.Log(socketInteractor.hasSelection);
        if (!socketInteractor.hasSelection)
        {
            currentSpawnTime += Time.deltaTime;
            progressBarImage.fillAmount = currentSpawnTime / spawnTime;
            if (currentSpawnTime > spawnTime)
            {
                currentSpawnTime = 0f;
                progressBarImage.fillAmount = 0f;
                SpawnItem();
            }
        }
    }

    private void SpawnItem()
    {
        if (spawnedItem != null) return;

        spawnedItem = Instantiate(prefabToSpawn, spawnLocation);
        AttachToSocket();
    }

    private void AttachToSocket()
    {
        IXRSelectInteractable interactable = spawnedItem.GetComponent<IXRSelectInteractable>();
        socketInteractor.StartManualInteraction(interactable);
    }
}
