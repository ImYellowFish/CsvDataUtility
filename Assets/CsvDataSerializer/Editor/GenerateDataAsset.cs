using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

namespace CSVDataUtility
{
    public static class DataAssetUtility
    {

        //public static void ReadAsset<T>(T asset) where T : ScriptableObject, IDataAsset
        //{
        //    asset.Read();
        //    string folderPath = Helper.ImportSetting.DataAssetRelativePath;
        //    Helper.CreateDirectoryIfNotExist(folderPath);

        //    AssetDatabase.CreateAsset(asset, folderPath + "/TestEnumDataTable.asset");
        //    AssetDatabase.SaveAssets();

        //    //EditorUtility.FocusProjectWindow();
        //    //Selection.activeObject = asset;
        //}

        public static void ReadAsset(UnityEngine.Object asset)
        {
            var dataAsset = asset as IDataAsset;
            dataAsset.Read();
            string folderPath = Helper.ImportSetting.DataAssetRelativePathToProject;
            Helper.CreateDirectoryIfNotExist(folderPath);

            string assetName = CSVDataAssetAttribute.GetDataTableName(asset);
            if (assetName == null)
                throw new CSVParseException("Cannot find name for asset: " + asset.GetType().Name);
            
            AssetDatabase.CreateAsset(asset, folderPath + "/" + assetName + ".asset");
            AssetDatabase.SaveAssets();

            //EditorUtility.FocusProjectWindow();
            //Selection.activeObject = asset;
        }

        [MenuItem("Assets/CSV Data Utility/DataAsset/Create All", priority = 2)]
        private static void CreateAllDataAssets()
        {
            var dataAssetInterface = typeof(IDataAsset);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => dataAssetInterface.IsAssignableFrom(p) && !p.IsAbstract && !p.IsInterface);

            foreach (var type in types) {
                var asset = ScriptableObject.CreateInstance(type.Name);
                ReadAsset(asset);
            }
        }

        //[MenuItem("Assets/CSV Data Utility/DataAsset/Create All", validate = true)]
        //private static bool CreateAllDataAssetsValidate()
        //{
        //    return Helper.ImportSetting.UseScriptableObject;
        //}
    }
}