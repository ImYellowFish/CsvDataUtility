using System;
using System.Reflection;
using System.Collections.Generic;

namespace CSVDataUtility {
    
    public class DataTableSerializer {
        
        private DataTypeFactory dataTypeFactory = new DataTypeFactory();

        private List<string[]> splitContent;
        
        private List<string> csvFields;
        private List<string> csvTypes;
        private int keyIndex;

        private int currentRow;
        private int currentColumn;
        
        public DataTableSerializer(string csvContentText) {
            CSVReader reader = new CSVReader(csvContentText);
            splitContent = reader.Read();
            InitializeDataTableHeadInfo();
        }

        /// <summary>
        /// Deserialize csv content into DataTable class
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Dictionary<string, T> Deserialize<T>() {
            Dictionary<string, T> result = new Dictionary<string, T>();
            for(int i = 2; i < splitContent.Count; i++) {
                object rowObj = DeserializeRow(i, typeof(T));
                string key = GetKeyStringForRow(i);

                if (rowObj != null) {
                    try {
                        result.Add(key, (T) rowObj);
                    } catch (ArgumentException) {
                        throw new CSVParseException("The Datatable contains duplicate key: " + key);
                    }
                } else {
                    throw new CSVParseException(
                        "Failed to serialize row. key: " +
                        key +
                        ", Expected type: " +
                        typeof(T).Name +
                        ", Result type: " +
                        rowObj.GetType().Name);
                }
            }
            
            return result;
        }
        
        /// <summary>
        /// Deserialize a row of content into DataEntry class
        /// </summary>
        /// <param name="row"></param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public object DeserializeRow(int row, Type targetType) {
            currentRow = row;
            string[] rowData = splitContent[row];

            // object activator
            object resultRowObj = Activator.CreateInstance(targetType);
            FieldInfo[] targetFieldInfos = resultRowObj.GetType().GetFields();


            for (int i = 0; i < targetFieldInfos.Length; i++) {
                FieldInfo field = targetFieldInfos[i];
                currentColumn = i;

                // If this field is marked with NonSerialized, skip
                if (!IsSerializable(field))
                    continue;

                // if this field is internal index, assign the row number.
                if (IsInternalIndexField(field))
                {
                    field.SetValue(resultRowObj, currentRow + CSVConstant.INTERNAL_INDEX_OFFSET);
                    continue;
                }
                
                // check whether the class field matches csv fields
                string expectedCsvFieldName = GetCSVFieldNameByFieldInfo(field);
                
                if (expectedCsvFieldName == null) {
                    // CSVHelper.LogWarning("Field skipped for type: " + targetType.Name + ", field: " + field.Name);

                    // all fields in the class must be deserialized
                    throw new CSVParseException("Field not marked for deserialization: " + 
                        targetType.Name + ", field: " + field.Name, currentColumn, currentRow);
                }
                
                if (!csvFields.Contains(expectedCsvFieldName)) {
                    throw new CSVParseException(
                        "Cannot find csv field for class: " +
                        targetType.Name +
                        ", field name: " +
                        field.Name +
                        ", expected csv field: " +
                        expectedCsvFieldName, currentRow, currentColumn);
                }

                // find the right column according to expected field name
                int index = csvFields.IndexOf(expectedCsvFieldName);

                // assign item info
                string typeInfo = csvTypes[index];
                string rawItem = rowData[index];
                string variableName = csvFields[index];

                // Deserialize item
                object targetValue = DeserializeItem(rawItem, typeInfo, variableName, field.FieldType, targetType);
                
                if (targetValue.GetType() != field.FieldType) {
                    throw new CSVParseException(
                        "Class Field type dismatch with csv field type. class: " +
                         targetType.Name +
                        ", Class field type: " +
                        field.FieldType.ToString() +
                        ", Csv field type: " +
                        targetValue.GetType().ToString(),
                        currentRow,
                        currentColumn);
                }

                field.SetValue(resultRowObj, targetValue);
            }

            return resultRowObj;
        }



        /// <summary>
        /// Given a string element in the csv and target type
        /// Deserialize it into an object of the target type
        /// </summary>
        /// <param name="item"></param>
        /// <param name="typeInfo"></param>
        /// <param name="expectedItemType"></param>
        /// <returns></returns>
        public object DeserializeItem(string item, string typeInfo, string variableName, Type expectedItemType, Type dataEntryType)
        {
            IDataType dataType = dataTypeFactory.GetDataType(typeInfo, variableName);
            dataType.deserializeExtraInfo = new DataTypeDeserializeExtraInfo(variableName, dataEntryType);

            try
            {
                return dataType.Deserialize(Helper.CorrectDataItemString(item), expectedItemType);
            }
            catch(CSVParseException e)
            {
                throw new CSVParseException(e.ToString(), currentRow, currentColumn);
            }
            catch (Exception e)
            {
                throw new CSVParseException("Unknown exception: " + e.ToString(), currentRow, currentColumn);
            }
        }


        /// <summary>
        /// process fields and types info from csv content
        /// </summary>
        private void InitializeDataTableHeadInfo()
        {
            // save fields row and types row as list
            csvFields = new List<string>(splitContent[0]);
            csvTypes = new List<string>(splitContent[1]);
            keyIndex = Helper.GetKeyColumnIndex(splitContent[1]);

            // make sure the two list have the same count
            if (csvFields.Count != csvTypes.Count)
                throw new CSVParseException("Field and types count dont match, fields: " + 
                    csvFields.Count + "types: " + csvTypes.Count);

            // trim field and type
            // change them to lower string (same as the rule in code generator)
            for(int i = 0; i < csvTypes.Count; i++)
            {
                csvTypes[i] = Helper.CorrectHeadItemString(csvTypes[i]);
                csvFields[i] = Helper.CorrectHeadItemString(csvFields[i]);
            }

        }



        private static string GetCSVFieldNameByFieldInfo(FieldInfo field) {
            return CSVFieldAttribute.GetCsvFieldName(field);
        }

        private static bool IsSerializable(FieldInfo field)
        {
            object[] attributes = field.GetCustomAttributes(true);
            for (int i = 0; i < attributes.Length; i++)
            {
                if (attributes[i] is NonSerializedAttribute)
                {
                    return false;
                }
            }
            return true;
        }
        
        private static bool IsInternalIndexField(FieldInfo field)
        {
            return CSVInternalIndexAttribute.IsInternalIndex(field);
        }

        private string GetKeyStringForRow(int row)
        {
            return splitContent[row][keyIndex];
        }

    }
}