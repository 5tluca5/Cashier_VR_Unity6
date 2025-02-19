using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private float spawnTime;
    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private Transform spawnLocation;

    [SerializeField] private Image progressBarImage;

    private float currentSpawnTime;
    private bool hasItem = true;

    private void Start()
    {
        progressBarImage.fillAmount = 0;
    }

    private void Update()
    {
        if (spawnLocation.childCount == 0)
        {
            hasItem = false;
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
        // If we already have item then we don't do anything
        if (hasItem) return;

        var spawnedItem = Instantiate(prefabToSpawn, spawnLocation);
        hasItem = true;
    }
}
