using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CSVDataUtility
{
    public static class DataAssetUtility
    {

        public static void ReadAsset<T>(T asset) where T : ScriptableObject, IDataAsset
        {
            asset.Read();
            string folderPath = Helper.ImportSetting.DataAssetRelativePath;
            Helper.CreateDirectoryIfNotExist(folderPath);

            AssetDatabase.CreateAsset(asset, folderPath + "/TestEnumDataTable.asset");
            AssetDatabase.SaveAssets();

            //EditorUtility.FocusProjectWindow();
            //Selection.activeObject = asset;
        }
    }
}