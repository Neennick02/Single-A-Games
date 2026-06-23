using UnityEditor;
using UnityEngine;

public class ReadWeiteHelper : MonoBehaviour
{
    [MenuItem("Tools/Enable ReadWrite On Selected Meshes")]
    static void EnableRW()
    {
        foreach (Object obj in Selection.objects)
        {
            string path = AssetDatabase.GetAssetPath(obj);
            ModelImporter importer = AssetImporter.GetAtPath(path) as ModelImporter;

            if (importer != null)
            {
                importer.isReadable = true;
                importer.SaveAndReimport();
                Debug.Log("Enabled Read/Write on: " + obj.name);
            }
        }
    }
}
