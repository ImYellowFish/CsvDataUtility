using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TestEnumDataTable_AssetEditor
{
    [MenuItem("Assets/CSV Data Utility/DataAsset/Create TestEnumDataTable")]
    public static void CreateAsset()
    {
        var asset = ScriptableObject.CreateInstance<TestEnumDataTable_Asset>();
        CSVDataUtility.DataAssetUtility.ReadAsset(asset);
    }
}