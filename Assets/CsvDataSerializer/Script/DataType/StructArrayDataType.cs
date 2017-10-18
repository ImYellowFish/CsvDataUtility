using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSVDataUtility
{
    public class StructArrayDataType : IDataType
    {

        private StructDataType baseDataType;
        
        public StructArrayDataType(string prefix, string nesting)
        {
            baseDataType = new StructDataType(prefix, nesting);
        }

        
        public string TypeName
        {
            get { return "array<" + baseDataType.TypeName + ">"; }
        }

        public bool IsType(string typeInfo)
        {
            return typeInfo.Contains("struct_array");
        }

        public DataTypeDeserializeExtraInfo deserializeExtraInfo { get; set; }


        private MethodInfo listAddMethod;
        private Type baseSystemType;

        public object Deserialize(string item, Type expectedType)
        {
            if (listAddMethod == null)
                listAddMethod = Helper.GetAddMethodForListType(expectedType);
            if (baseSystemType == null)
                baseSystemType = Helper.GetBaseTypeForListType(expectedType);

            var listObj = System.Activator.CreateInstance(expectedType);
            var subItems = item.Split(CSVConstant.ARRAY_DELIMITER);
            for(int i = 0; i < subItems.Length; i += baseDataType.ParameterCount)
            {
                var structItem = baseDataType.DeserializeItemArray(subItems, i, baseSystemType);
                listAddMethod.Invoke(listObj, new object[] { structItem });
            }

            return listObj;
        }
        
        public string GetTypeNameForWriter(string variableName)
        {
            return "List<" + baseDataType.GetTypeNameForWriter(variableName)+ ">";
        }

        public string GetAdditionalInfoForWriter(string variableName)
        {
            return baseDataType.GetAdditionalInfoForWriter(variableName);
        }

        public string GetExtensionMethodForWriter(string dataEntryName, string variableName)
        {
            return "";
        }
        
    }
}