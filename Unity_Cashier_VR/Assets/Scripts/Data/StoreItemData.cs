using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

[Serializable, CreateAssetMenu(fileName = "StoreItemData", menuName = "Store Item Data")]
public class StoreItemData : ScriptableObject
{
    [SerializeField] public int id;    // Unique identifier for the store item
    [SerializeField] public string itemName;    // Name of the store item
    [SerializeField] public GameObject itemPrefab;    // Prefab of the store item

    public static bool operator ==(StoreItemData a, StoreItemData b)
    {
        if (ReferenceEquals(a, b))
        {
            return true;
        }

        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
        {
            return false;
        }

        return a.id == b.id;
    }

    public static bool operator !=(StoreItemData a, StoreItemData b)
    {
        return !(a == b);
    }

    public override bool Equals(object other)
    {
        return base.Equals(other);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

public static class StoreItemDataTools
{
    [MenuItem("Tools/StoreItem/Check duplicated IDs for Store Items")]
    public static void CheckDuplicatedIds()
    {
        // Find all StoreItemData assets in the project
        string[] guids = AssetDatabase.FindAssets("t:StoreItemData");
        HashSet<int> uniqueIds = new HashSet<int>();
        List<StoreItemData> checkedItems = new List<StoreItemData>();
        bool hasDuplicatedIds = false;

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            StoreItemData storeItemData = AssetDatabase.LoadAssetAtPath<StoreItemData>(path);

            if (storeItemData != null)
            {
                if(uniqueIds.Contains(storeItemData.id))
                {
                    hasDuplicatedIds = true;
                    Debug.LogError($"Duplicated ID found for StoreItemData: {storeItemData.name} with ID: {storeItemData.id} | StoreItemData: {checkedItems.First(x => x.id == storeItemData.id).name} has same ID.");
                }
                else
                {
                    uniqueIds.Add(storeItemData.id);
                }

                checkedItems.Add(storeItemData);
            }
        }

        if (!hasDuplicatedIds)
        {
            Debug.Log("No duplicated IDs found for StoreItemData assets.");
        }
    }

    [MenuItem("Tools/StoreItem/Generate Unique IDs for Store Items")]
    public static void GenerateUniqueIds()
    {
        // Find all StoreItemData assets in the project
        string[] guids = AssetDatabase.FindAssets("t:StoreItemData");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            StoreItemData storeItemData = AssetDatabase.LoadAssetAtPath<StoreItemData>(path);

            // Assign a unique id if it hasn't been assigned yet
            if (storeItemData.id == 0)
            {
                storeItemData.id = (GenerateUniqueId());
                EditorUtility.SetDirty(storeItemData); // Mark the object as dirty to save changes
            }
        }

        // Save all changes to assets
        AssetDatabase.SaveAssets();
        Debug.Log("Unique IDs generated for all StoreItemData assets.");
    }

    public static int GenerateUniqueId()
    {
        // Implement your logic to generate a unique id
        // For example, you could use a static counter or a GUID
        return Guid.NewGuid().GetHashCode();
    }


}