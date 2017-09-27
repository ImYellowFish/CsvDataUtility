using UnityEngine;
using UnityEditor;

public class SkillDataTable_AssetEditor
{
    [MenuItem("Assets/CSV Data Utility/DataAsset/Create SkillDataTable")]
    public static void CreateAsset()
    {
        var asset = ScriptableObject.CreateInstance<SkillDataTable_Asset>();
        CSVDataUtility.DataAssetUtility.ReadAsset(asset);
    }
}