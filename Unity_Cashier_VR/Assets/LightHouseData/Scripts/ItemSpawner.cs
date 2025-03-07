using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private float spawnTime;
    [SerializeField] private Transform spawnLocation;
    [SerializeField] private ItemGrabHandler prefabToSpawn;
    [SerializeField] private XRSocketInteractor socketInteractor;

    [SerializeField] private Image progressBarImage;

    private float currentSpawnTime;
    [SerializeField] private ItemGrabHandler spawnedItem;

    private void Start()
    {
        progressBarImage.fillAmount = 0;
        spawnedItem = GetComponentInChildren<ItemGrabHandler>();
        AttachToSocket();
    }

    private void Update()
    {
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
