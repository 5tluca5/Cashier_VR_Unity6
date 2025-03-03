using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class StoreItemDataGenerator : MonoBehaviour
{
    [SerializeField] string path = "Assets/Resources/StoreItemData/";
    [SerializeField] List<GameObject> itemPrefabs;
 
    int uniqueId = 100001;
    public void GenerateItemData()
    {
        foreach (var item in itemPrefabs) {
            StoreItemData storeItemData = ScriptableObject.CreateInstance<StoreItemData>();
            storeItemData.itemPrefab = item;
            storeItemData.itemName = item.name;
            storeItemData.id = uniqueId++;
            AssetDatabase.CreateAsset(storeItemData, path + item.name + ".asset");
        }

        AssetDatabase.SaveAssets();
        Debug.Log("Item data generated successfully. Path: " + path);
    }
}

