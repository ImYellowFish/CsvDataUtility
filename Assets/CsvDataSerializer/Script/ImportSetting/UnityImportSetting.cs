using UnityEngine;
using System.Collections.Generic;

namespace CSVDataUtility {
    [CreateAssetMenu(fileName = SETTING_FILENAME, menuName = "CsvDataUtility/CsvImportSetting")]
    public class UnityImportSetting : ScriptableObject, IImportSetting {
        public const string SETTING_FILENAME = "CsvImportSetting";

        public static UnityImportSetting Instance {
            get {
                var result = Resources.Load<UnityImportSetting>(SETTING_FILENAME);
                if (result == null)
                    throw new System.NullReferenceException("Cannot find generator setting in resources folder.");
                return result;
            }
        }

        public string DataEntryFolderFullPath {
            get {
                return Application.dataPath + "/" + dataEntryFolderPath;
            }
        }

        public string DataEntryFolderRelativePath
        {
            get
            {
                return "Assets/" + dataEntryFolderPath;
            }
        }

        [Tooltip("Path for generated scripts, relative to Assets folder")]
        public string dataEntryFolderPath = "Script/Data/";

        public string CsvFolderRelativeResourcesPath {
            get { return csvFolderPath; }
        }

        [Tooltip("Path of excel files, relative to Resources folder")]
        public string csvFolderPath = "Data";


        public string DataAssetRelativePath
        {
            get { return "Assets/" + dataAssetFolderPath; }
        }
        

        [Tooltip("Path of generated scriptableObjects, relative to Asset folder")]
        public string dataAssetFolderPath = "Resources/DataAsset";
        
        public List<GenerationRule> additionalRules;
        public GenerationRule FindRuleByCsvName(string csvName) {
            return additionalRules.Find((r) => r.csvName.ToLower() == csvName.ToLower());
        }
    }
}
