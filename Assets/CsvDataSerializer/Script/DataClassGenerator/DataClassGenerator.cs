using System.Collections.Generic;
using System.IO;

namespace CSVDataUtility {
    public class DataClassGenerator{
        private string csvFilename;
        private string dataEntryName;
        private string dataTableName;

        private List<string[]> data;
        private string[] fields;
        private string[] types;

        private DataTypeFactory dataTypeFactory = new DataTypeFactory();
        private ClassWriter classWriter;

        /// <summary>
        /// Generate a data class based on csv content
        /// Class will be written to the given path
        /// </summary>
        /// <param name="csvFilename"></param>
        /// <param name="csvContent"></param>
        /// <param name="savePath"></param>
        public void Generate(string csvFilename, string csvContent, string savePath) {
            Init(csvFilename, csvContent);
            Write(savePath);   
        }

        private void Init(string csvFilename, string csvContent) {
            // Initiation
            this.csvFilename = csvFilename;
            InitDataEntryTableName(this.csvFilename);

            CSVReader reader = new CSVReader(csvContent);
            data = reader.Read();

            fields = data[0];
            types = data[1];
        }


        private void Write(string savePath) {
            // Write class head
            classWriter = new ClassWriter(savePath, dataEntryName);
            classWriter.WriteHead(csvFilename, dataEntryName);

            // Write class body
            for (int i = 0; i < fields.Length; i++) {
                WriteVariable(fields[i], types[i]);
            }

            // Write class end
            classWriter.WriteEnd(dataTableName, dataEntryName);
        }


        private void InitDataEntryTableName(string csvName) {
            var rule = Helper.ImportSetting.FindRuleByCsvName(csvName);
            if(rule == null) {
                string defaultDataName = Helper.GetScriptFriendlyName(csvName, false);
                dataEntryName = defaultDataName + "DataEntry";
                dataTableName = defaultDataName + "DataTable";
            } else {
                dataEntryName = rule.entryName;
                dataTableName = rule.tableName;
            }
        }
        
        private void WriteVariable(string fieldName, string typeInfo) {
            fieldName = fieldName.Trim();

            string variableName = Helper.GetScriptFriendlyName(fieldName, true);
            if (variableName.Contains(CSVConstant.IDENTIFIER_OMIT_COLUMN))
                return;

            IDataType dataType = dataTypeFactory.GetDataType(typeInfo);
            classWriter.WriteVariable(fieldName, variableName, dataType);
        }
        
    }
}