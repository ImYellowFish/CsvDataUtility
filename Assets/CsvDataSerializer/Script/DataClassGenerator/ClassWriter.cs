using System.IO;

namespace CSVDataUtility {
    public class ClassWriter {
        private StreamWriter file;
        
        public ClassWriter(string savePath, string dataEntryName) {
            file = File.CreateText(savePath + "\\" + dataEntryName + ".cs");
        }

        
        public void WriteHead(string csvFilename, string dataEntryName) {
            string head = @"using System.Collections.Generic;
[System.Serializable]
[CSVFilename(" + "\"" + csvFilename + "\")]\n" +
"public class ";
            file.Write(head);
            file.Write(dataEntryName);
            file.Write("{\n\n");
        }

        public void WriteEnd(string dataTableName, string dataEntryName) {
            file.Write("\n}");

            WriteDataTableClass(dataTableName, dataEntryName);

            file.Write("\n");
            WriteExtensionClass(dataTableName);

            file.Close();
        }

        public void WriteVariable(string dataEntryName, string csvFieldName, string variableName, IDataType type) {
            if (type == null) {
                return;
            }

            string typeDefinition = "\t[CSVField(\"" + csvFieldName + "\")]\n\tpublic " + type.GetTypeNameForWriter(variableName) + " " + variableName + ";\n";
            file.WriteLine(typeDefinition + type.GetAdditionalInfoForWriter(variableName));

            extensionContent = extensionContent + type.GetExtensionMethodForWriter(dataEntryName, variableName);
        }


        #region DataTableClass
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
            file.Write(tmp);
        }
        #endregion


        #region Extension Method Class
        private string extensionContent = "";

        private const string extensionClassTemplate = @"
namespace CSVDataUtility.Extension {
    public static class DATATABLE_NAME_Extension {
        EXTENSION_CONTENT
    }
}
";      

        private void WriteExtensionClass(string dataTableName) {
            if (extensionContent == "")
                return;

            string tmp = extensionClassTemplate.Replace("DATATABLE_NAME", dataTableName);
            tmp = tmp.Replace("EXTENSION_CONTENT", extensionContent);
            file.Write(tmp);
        }
        #endregion
    }
}

