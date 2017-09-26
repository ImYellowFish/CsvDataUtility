using System.Collections.Generic;
using System;
using System.Reflection;

namespace CSVDataUtility {
    public class ArrayDataType : IDataType
    {
        // base type info
        protected IDataType baseDataType;
        protected string typeName;
        protected char arrayDelimeter;

        // reflection info
        protected Type baseSystemType;
        protected Type listSystemType;
        private MethodInfo listAddMethod;
        private bool reflectionInitiated = false;

        
        public ArrayDataType(IDataType baseDataType) {            
            Init(baseDataType, CSVConstant.ARRAY_DELIMITER);
        }

        public ArrayDataType(IDataType baseDataType, char arrayDelimeter) {
            Init(baseDataType, arrayDelimeter);
        }

        
        public static readonly string ArrayTypeIdentifierPrefix = CSVConstant.ARRAY_TYPE;
        public static readonly string ArrayTypeNamePrefix = "List";
      

        public string TypeName
        {
            get
            {
                return typeName;
            }
        }

        public string GetTypeNameForWriter(string variableName)
        {
            var typeNameForWriter = ArrayTypeNamePrefix + "<" + baseDataType.GetTypeNameForWriter(variableName) + ">";
            return typeNameForWriter;
        }

        public string GetAdditionalInfoForWriter(string variableName)
        {
            return baseDataType.GetAdditionalInfoForWriter(variableName);
        }


        public string GetExtensionMethodForWriter(string dataEntryName, string variableName) {
            return baseDataType.GetExtensionMethodForWriter(dataEntryName, variableName);
        }


        public bool IsType(string csvTypeField)
        {
            return csvTypeField == ArrayTypeIdentifierPrefix;
        }

        private void InitReflectionTypeInfo(Type expectedListType)
        {
            if (reflectionInitiated)
                return;

            try
            {
                this.listSystemType = expectedListType;
                this.baseSystemType = expectedListType.GetGenericArguments()[0];
                listAddMethod = expectedListType.GetMethod("Add");

            }
            catch (System.Exception e)
            {
                Helper.LogWarning(e);                
            }

            if (listSystemType == null || baseSystemType == null || listAddMethod == null)
            {
                throw new CSVParseException("Cannot init reflection for array type: List+" + baseDataType.TypeName +
                ", expected type: " + expectedListType.Name);
            }

            reflectionInitiated = true;
        }


        public object Deserialize(string rawItem, Type expectedType) {
            InitReflectionTypeInfo(expectedType);
            
            // create array of baseType
            object array =  Activator.CreateInstance(expectedType);

            if (rawItem == CSVConstant.EMPTY_ITEM)
                return array;

            string[] rawElements = rawItem.Split(arrayDelimeter);
            for (int i = 0; i < rawElements.Length; i++) {
                object element = baseDataType.Deserialize(rawElements[i], baseSystemType);
                AddToArray(array, element);
            }

            return array;
        }

        private void AddToArray(object array, object value) {
            try {
                listAddMethod.Invoke(array, new[] { value });
            } catch (Exception) {
                // TODO: catch specific kind of exception
                throw new CSVParseException("Failure when adding value to array");
            }
        }

        private void Init(IDataType baseDataType, char arrayDelimeter) {
            if (baseDataType == null)
                throw new ArgumentException("base type of complex type cannot be null!");

            this.baseDataType = baseDataType;

            typeName = ArrayTypeIdentifierPrefix + "<" + this.baseDataType.TypeName + ">";
            
            this.arrayDelimeter = arrayDelimeter;

            if (baseDataType == null)
                throw new ArgumentException("base type of array cannot be null!");
            
        }        
    }
}