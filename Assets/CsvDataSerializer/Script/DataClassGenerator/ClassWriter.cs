﻿using System.IO;

namespace CSVDataUtility {
    public class ClassWriter {
        private StreamWriter mainClassFile;
        private StreamWriter assetClassFile;
        private StreamWriter assetClassEditorFile;

        private string savePath;
        private string dataTableName;
        private string dataEntryName;
        private string keyVariableName;
        private string csvFilename;


        public ClassWriter(string savePath,
            string csvFilename,
            string dataTableName, 
            string dataEntryName,
            string keyVariableName) {

            this.savePath = savePath;
            this.csvFilename = csvFilename;
            this.dataTableName = dataTableName;
            this.dataEntryName = dataEntryName;
            this.keyVariableName = keyVariableName;
            
    }


        public void Prepare()
        {
            PrepareFiles();
            PrepareDataEntry();
        }


        public void ProcessVariable(string csvFieldName, string variableName, IDataType type)
        {
            if (type == null)
            {
                return;
            }

            string typeDefinition = "\t[CSVField(\"" + csvFieldName + "\")]\n\tpublic " + type.GetTypeNameForWriter(variableName) + " " + variableName + ";\n";

            dataEntryClassContent += (typeDefinition + type.GetAdditionalInfoForWriter(variableName) + "\n");
            extensionContent += type.GetExtensionMethodForWriter(dataEntryName, variableName);
        }

        public void Finish()
        {
            WriteDataEntryClass();

            mainClassFile.Write("\n");
            WriteDataTableClass(dataTableName, dataEntryName);

            mainClassFile.Write("\n");
            WriteExtensionClass();

            WriteDataAssetClass();
            WriteDataAssetClassEditor();

            CloseFiles();
        }


        private void PrepareFiles()
        {
            var assetClassSavePath = savePath + "\\DataAsset";
            var editorClassSavePath = savePath + "\\Editor";
            
            Helper.CreateDirectoryIfNotExist(savePath);
            Helper.CreateDirectoryIfNotExist(assetClassSavePath);
            Helper.CreateDirectoryIfNotExist(editorClassSavePath);

            mainClassFile = File.CreateText(savePath + "\\" + dataEntryName + ".cs");
            assetClassFile = File.CreateText(assetClassSavePath + "\\" + dataTableName + "_Asset.cs");
            assetClassEditorFile = File.CreateText(editorClassSavePath + "\\" + dataTableName + "_AssetEditor.cs");
        }

        private void CloseFiles()
        {
            mainClassFile.Close();

            if(assetClassFile != null)
                assetClassFile.Close();

            if (assetClassEditorFile != null)
                assetClassEditorFile.Close();
        }


        #region DataEntry class
        private const string dataEntryTemplate = @"using System.Collections.Generic;
[System.Serializable]
[CSVFilename(""CSV_FILE_NAME"")]
public class DATA_ENTRY_NAME : CSVDataUtility.IDataEntry{
            
DATA_ENTRY_CONTENT

    public string internal_dataEntryID { 
        get {
            return KEY_VARIABLE_NAME;    
        } 
    }
}
";
        private string dataEntryClassFramework;
        private string dataEntryClassContent;

        
        private void PrepareDataEntry()
        {
            dataEntryClassFramework = dataEntryTemplate.
                Replace("CSV_FILE_NAME", csvFilename).
                Replace("DATA_ENTRY_NAME", dataEntryName).
                Replace("KEY_VARIABLE_NAME", keyVariableName);

            dataEntryClassContent = "";
        }


        private void WriteDataEntryClass()
        {
            string tmp = dataEntryClassFramework.Replace("DATA_ENTRY_CONTENT", dataEntryClassContent);
            mainClassFile.Write(tmp);
        }
        
        

        #endregion
        

        #region DataTable class
        private const string dataTableClassTemplate = @"

[System.Serializable]
public class DATATABLE_NAME : CSVDataUtility.DataTable<DATA_ENTRY_NAME>{
    public static DATATABLE_NAME Create(){
        DATATABLE_NAME datatable = new DATATABLE_NAME();
        datatable.Read();
        return datatable;
    }
}
";

        private void WriteDataTableClass(string dataTableName, string dataEntryName) {
            string tmp = dataTableClassTemplate.Replace("DATATABLE_NAME", dataTableName);
            tmp = tmp.Replace("DATA_ENTRY_NAME", dataEntryName);
            mainClassFile.Write(tmp);
        }
        #endregion


        #region DataAsset class
        
        private const string dataAssetTemplate = 
@"public class DATATABLE_NAME_Asset : CSVDataUtility.DataAsset<DATA_ENTRY_NAME>
{

}
";
        
        private void WriteDataAssetClass()
        {
            string tmp = dataAssetTemplate.
                Replace("DATATABLE_NAME", dataTableName).
                Replace("DATA_ENTRY_NAME", dataEntryName);
            assetClassFile.Write(tmp);
        }

        private const string dataAssetEditorTemplate = 
@"using UnityEngine;
using UnityEditor;

public class DATATABLE_NAME_AssetEditor
{
    [MenuItem(""Assets/CSV Data Utility/DataAsset/Create DATATABLE_NAME"")]
    public static void CreateAsset()
    {
        var asset = ScriptableObject.CreateInstance<DATATABLE_NAME_Asset>();
        CSVDataUtility.DataAssetUtility.ReadAsset(asset);
    }
}";

        private void WriteDataAssetClassEditor()
        {
            string tmp = dataAssetEditorTemplate.
                Replace("DATATABLE_NAME", dataTableName);
            assetClassEditorFile.Write(tmp);
        }

        #endregion


        #region Extension method class
        private string extensionContent = "";

        private const string extensionClassTemplate = @"
namespace CSVDataUtility.Extension {
    public static class DATATABLE_NAME_Extension {
        EXTENSION_CONTENT
    }
}
";      

        private void WriteExtensionClass() {
            if (extensionContent == "")
                return;

            string tmp = extensionClassTemplate.Replace("DATATABLE_NAME", dataTableName);
            tmp = tmp.Replace("EXTENSION_CONTENT", extensionContent);
            mainClassFile.Write(tmp);
        }
        #endregion
    }
}

