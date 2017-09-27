using UnityEngine;
using System.Collections.Generic;

namespace CSVDataUtility {

    public interface IImportSetting {
        /// <summary>
        /// Full path of the folder containing generated scripts
        /// </summary>
        string DataEntryFolderFullPath { get; }

        /// <summary>
        /// Relative path to the Project folder containing generated scripts
        /// </summary>
        string DataEntryFolderRelativePathToProject { get; }

        /// <summary>
        /// Relative path to the Resources folder containing excel files
        /// </summary>
        string CsvFolderRelativePathToResources { get; }

        /// <summary>
        /// Relative path to the Project folder containing generated asset files
        /// </summary>
        string DataAssetRelativePathToProject { get; }

        /// <summary>
        /// Relative path to the Resources folder containing generated asset files
        /// </summary>
        string DataAssetRelativePathToResources { get; }

        /// <summary>
        /// If true, data will be transferred into scriptableObjects, instead of excel files
        /// </summary>
        bool UseScriptableObject { get; }

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