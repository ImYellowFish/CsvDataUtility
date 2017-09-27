using UnityEngine;
using System.Collections.Generic;

namespace CSVDataUtility {
    [CreateAssetMenu(fileName = SETTING_FILENAME, menuName = "CsvDataUtility/CsvImportSetting")]
    public class UnityImportSetting : ScriptableObject, IImportSetting {
        #region Configuration
        [Tooltip("Path for generated scripts, relative to project folder")]
        public string dataEntryFolderPath = "Assets/Script/Data/";

        [Tooltip("Path of excel files, relative to project folder")]
        public string csvFolderPath = "Assets/Resources/Data/";

        [Tooltip("Path of generated scriptableObjects, relative to project folder")]
        public string dataAssetFolderPath = "Assets/Resources/DataAsset/";

        [Tooltip("If true, data will be transferred into scriptableObjects, instead of excel files")]
        public bool useScriptableObject = false;

        [Tooltip("Additional rules for generating script files")]
        public List<GenerationRule> additionalRules;
        
        #endregion

        #region Interface
        public const string SETTING_FILENAME = "CsvImportSetting";

        public static UnityImportSetting Instance {
            get {
                var result = Resources.Load<UnityImportSetting>(SETTING_FILENAME);
                if (result == null)
                    throw new System.NullReferenceException("Cannot find generator setting in resources folder.");
                return result;
            }
        }

        /// <summary>
        /// Full path for the folder which contains generated DataEntry class
        /// </summary>
        public string DataEntryFolderFullPath {
            get {
                return Application.dataPath + "/" + Helper.GetRelativePathToAssetsFolder(dataEntryFolderPath);
            }
        }

        /// <summary>
        /// Path for the folder which contains generated DataEntry class, relative to the project folder
        /// </summary>
        public string DataEntryFolderRelativePathToProject
        {
            get
            {
                return dataEntryFolderPath;
            }
        }

        /// <summary>
        /// Path for the folder which contains csv files, relative to the Resources folder
        /// </summary>
        public string CsvFolderRelativePathToResources {
            get { return Helper.GetRelativePathToResourcesFolder(csvFolderPath); }
        }


        /// <summary>
        /// Path for the folder which contains csv files, relative to the project folder
        /// </summary>
        public string DataAssetRelativePathToProject
        {
            get { return dataAssetFolderPath; }
        }


        public string DataAssetRelativePathToResources
        {
            get { return Helper.GetRelativePathToResourcesFolder(dataAssetFolderPath); }
        }


        public bool UseScriptableObject
        {
            get { return useScriptableObject; }
        }


        public GenerationRule FindRuleByCsvName(string csvName) {
            return additionalRules.Find((r) => r.csvName.ToLower() == csvName.ToLower());
        }
        #endregion
    }
}
