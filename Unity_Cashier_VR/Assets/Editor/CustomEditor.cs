using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StoreItemDataGenerator))]
public class StoreItemDataGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        StoreItemDataGenerator storeItemDataGenerator = (StoreItemDataGenerator)target;

        if (GUILayout.Button("Generate Item Data"))
        {
            storeItemDataGenerator.GenerateItemData();
        }
    }
}