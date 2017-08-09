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

            file.Close();
        }

        public void WriteVariable(string csvFieldName, string variableName, IDataType type) {
            if (type == null) {
                return;
            }

            string typeDefinition = "\t[CSVField(\"" + csvFieldName + "\")]\n\tpublic " + type.TypeName + " " + variableName + ";\n";

            file.WriteLine(type.OverrideGeneratedVariableDefinition(typeDefinition, variableName));
        }

        private const string _template = @"

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
            string tmp = _template.Replace("DATATABLE_NAME", dataTableName);
            tmp = tmp.Replace("DATA_ENTRY_NAME", dataEntryName);
            file.Write(tmp);
        }
        
    }
}