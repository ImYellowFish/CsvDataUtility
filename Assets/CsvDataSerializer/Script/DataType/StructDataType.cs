using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSVDataUtility
{
    public class StructDataType : IDataType
    {
        private List<IDataType> parameterDataTypes;
        private List<string> parameterNames;
        private string nesting;

        public StructDataType(string prefix, string nesting)
        {
            this.nesting = nesting;
            parameterDataTypes = new List<IDataType>();
            parameterNames = new List<string>();

            string[] typeAndNames = nesting.Split(CSVConstant.ARRAY_DELIMITER);
            for(int i = 0; i < typeAndNames.Length; i++)
            {
                ParseParameter(typeAndNames[i], i);
            }
        }

        public int ParameterCount
        {
            get { return parameterDataTypes.Count; }
        }

        private void ParseParameter(string typeAndName, int index)
        {
            string[] split = typeAndName.Split(CSVConstant.STRUCT_DELIMITER);
            string paramTypeName = "", paramFieldName = "";

            foreach (string item in split)
            {
                var correctedItem = Helper.CorrectHeadItemString(item);
                if (correctedItem == "")
                    continue;
                if (paramTypeName == "")
                    paramTypeName = correctedItem;
                else
                {
                    paramFieldName = Helper.GetValidScriptVariableName(correctedItem, true);
                }
            }

            if (paramTypeName == "")
                throw new CSVParseException("Error type def when parsing struct: " + TypeName + ", type: " + typeAndName);

            if (paramFieldName == "")
                paramFieldName = "member_" + index.ToString();

            var dataType = DataTypeFactory.GetBaseDataType(paramTypeName);
            parameterDataTypes.Add(dataType);
            parameterNames.Add(paramFieldName);
        }

        public string TypeName
        {
            get { return "struct<" + nesting + ">"; }
        }

        public bool IsType(string typeInfo)
        {
            return typeInfo.Contains("struct");
        }

        public DataTypeDeserializeExtraInfo deserializeExtraInfo { get; set; }


        public object Deserialize(string item, System.Type expectedType)
        {
            var subItems = item.Split(CSVConstant.ARRAY_DELIMITER);
            return DeserializeItemArray(subItems, 0, expectedType);
        }

        public object DeserializeItemArray(string[] items, int startIndex, Type expectedType)
        {
            if(items.Length < parameterDataTypes.Count + startIndex)
            {
                throw new CSVParseException("Struct params number mismatch, type: " + TypeName + ", expected: " + 
                    parameterDataTypes.Count + ", actual: " + (items.Length - startIndex).ToString());
            }

            object structObj = Activator.CreateInstance(expectedType);
            for(int i = 0; i < parameterDataTypes.Count; i++)
            {
                var item = items[i + startIndex];
                var fieldInfo = expectedType.GetField(parameterNames[i]);
                var fieldDataType = parameterDataTypes[i];
                fieldInfo.SetValue(structObj, fieldDataType.Deserialize(item, ((ISingleDataType)fieldDataType).SystemType));
            }
            return structObj;
        }

        public string GetTypeNameForWriter(string variableName)
        {
            return variableName + "_struct";
        }

        public string GetAdditionalInfoForWriter(string variableName)
        {
            string membersInfo = "";
            for(int i = 0; i < parameterDataTypes.Count; i++)
            {
                var memberType = parameterDataTypes[i];
                string memberInfoLine = memberInfoTemplate.
                    Replace("MEMBER_TYPE", memberType.GetTypeNameForWriter(variableName)).
                    Replace("MEMBER_NAME", parameterNames[i]);
                membersInfo += memberInfoLine;
            }

            return additionalInfoTemplate.Replace("VARIABLE_NAME", variableName).
                Replace("MEMBERS_INFO", membersInfo);
        }

        public string GetExtensionMethodForWriter(string dataEntryName, string variableName)
        {
            return "";
        }

        
        
        private static readonly string additionalInfoTemplate = @"
    [System.Serializable]
    public struct VARIABLE_NAME_struct
    {
MEMBERS_INFO
    }
";
        private static readonly string memberInfoTemplate = "\t\tpublic MEMBER_TYPE MEMBER_NAME;\n";
    }
}