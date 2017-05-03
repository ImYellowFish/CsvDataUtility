using UnityEngine;
using System.Collections.Generic;

namespace CSVDataUtility {

    public interface IImportSetting {
        string DataEntryFolderPath { get; }
        string CsvFolderPath { get; }
        GenerationRule FindRuleByCsvName(string csvName);
    }

    [System.Serializable]
    public class GenerationRule {
        public string csvName;
        public string tableName;
        public string entryName;
    }  
}