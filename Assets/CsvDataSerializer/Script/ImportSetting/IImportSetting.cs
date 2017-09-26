using UnityEngine;
using System.Collections.Generic;

namespace CSVDataUtility {

    public interface IImportSetting {
        /// <summary>
        /// Full path of the folder containing generated scripts
        /// </summary>
        string DataEntryFolderFullPath { get; }

        /// <summary>
        /// Relative path to Project folder containing generated scripts
        /// </summary>
        string DataEntryFolderRelativePath { get; }

        /// <summary>
        /// Relative path to Resources folder containing excel files
        /// </summary>
        string CsvFolderRelativeResourcesPath { get; }

        /// <summary>
        /// Relative path to Project folder containing generated asset files
        /// </summary>
        string DataAssetRelativePath { get; }

        /// <summary>
        /// Look for special rules about code generation
        /// </summary>
        GenerationRule FindRuleByCsvName(string csvName);
    }

    [System.Serializable]
    public class GenerationRule {
        public string csvName;
        public string tableName;
        public string entryName;
    }  
}