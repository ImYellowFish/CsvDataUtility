using System;
using System.Reflection;
using System.Collections.Generic;

namespace CSVDataUtility {
    
    public class DataTableSerializer {
        public static readonly string KEY_IDENTIFIER = "key";
        public static readonly string ARRAY_IDENTIFIER = "array";
        public static readonly char ARRAY_DELIMITER = ';';

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

            csvFields = new List<string>();
            csvFields.AddRange(splitContent[0]);

            csvTypes = new List<string>();
            csvTypes.AddRange(splitContent[1]);

            // check for key column index
            for (int i = 0; i < csvTypes.Count; i++) {
                if (csvTypes[i].Contains(KEY_IDENTIFIER)) {
                    keyIndex = i;
                }
            }

            if (keyIndex == -1) {
                throw new CSVParseException("cannot find key column!");
            }

        }

        
        private string GetKey(int row) {
            return splitContent[row][keyIndex];
        }

        public Dictionary<string, T> Deserialize<T>() {
            Dictionary<string, T> result = new Dictionary<string, T>();
            for(int i = 2; i < splitContent.Count; i++) {
                object rowObj = DeserializeRow(i, typeof(T));
                string key = GetKey(i);

                if (rowObj is T) {
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
        

        public object DeserializeItem(string item, string typeInfo, Type expectedType) {
            IDataType dataType = dataTypeFactory.GetDataType(typeInfo);
            return dataType.Serialize(item, expectedType);
        }


        public object DeserializeRow(int row, Type targetType) {
            currentRow = row;
            string[] rowData = splitContent[row];

            // object activator
            object resultRowObj = Activator.CreateInstance(targetType);
            FieldInfo[] fieldInfos = resultRowObj.GetType().GetFields();


            for (int i = 0; i < fieldInfos.Length; i++) {
                FieldInfo field = fieldInfos[i];
                currentColumn = i;

                // check whether the class field matches csv fields
                string expectedCsvFieldName = GetCSVFieldName(field);

                if (expectedCsvFieldName == null) {
                    // CSVHelper.LogWarning("Field skipped for type: " + targetType.Name + ", field: " + field.Name);

                    // all fields in the class must be deserialized
                    throw new CSVParseException("Field not marked for deserialization: " + targetType.Name + ", field: " + field.Name);
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
                
                // Deserialize item
                object targetValue = DeserializeItem(rawItem, typeInfo, field.FieldType);
                
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

        

        private static string GetCSVFieldName(FieldInfo field) {
            return CSVFieldAttribute.GetCsvFieldName(field);
        }

        private static object ObjArrayToTypeArray(List<object> objArray, Type targetArrayType) {
            if (objArray == null) {
                throw new CSVParseException("Null object array!");
            }

            object resultArray = Activator.CreateInstance(targetArrayType);

            if (objArray.Count == 0)
                return resultArray;

            Type valueType = objArray[0].GetType();
            MethodInfo add = resultArray.GetType().GetMethod("Add");

            foreach (object element in objArray) {
                if (element.GetType() != valueType) {
                    throw new CSVParseException("Inconsistent value types for object array!");
                }
                add.Invoke(resultArray, new[] { Convert.ChangeType(element, valueType) });
            }

            return resultArray;
        }
    }
}