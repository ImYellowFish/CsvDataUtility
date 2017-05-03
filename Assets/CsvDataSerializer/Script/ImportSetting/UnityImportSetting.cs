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

        public string DataEntryFolderPath {
            get {
                return Application.dataPath + "/" + dataEntryFolderPath;
            }
        }
        public string dataEntryFolderPath = "Script/Data/";

        public string CsvFolderPath {
            get { return csvFolderPath; }
        }
        public string csvFolderPath = "Data";

        public List<GenerationRule> additionalRules;
        public GenerationRule FindRuleByCsvName(string csvName) {
            return additionalRules.Find((r) => r.csvName.ToLower() == csvName.ToLower());
        }
    }
}
