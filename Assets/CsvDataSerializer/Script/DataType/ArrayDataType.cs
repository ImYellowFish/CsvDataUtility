using System.Collections.Generic;
using System;
using System.Reflection;

namespace CSVDataUtility {
    public class ArrayDataType : IDataType
    {
        protected IDataType baseDataType;
        protected string typeIdentifier;
        protected string typeName;

        private Type listType;
        private MethodInfo listAddMethod;
        private char arrayDelimeter;

        public ArrayDataType(IDataType baseDataType) {            
            Init(baseDataType, CSVConstant.ARRAY_DELIMITER);
        }

        public ArrayDataType(IDataType baseDataType, char arrayDelimeter) {
            Init(baseDataType, arrayDelimeter);
        }

        
        public string ArrayTypeIdentifierPrefix {
            get {
                return CSVConstant.ARRAY_TYPE;
            }
        }
        
        public string ArrayTypeNamePrefix
        {
            get
            {
                return "List";
            }
        }

        public string TypeIdentifier
        {
            get
            {
                return typeIdentifier;
            }
        }

        public string OverrideGeneratedVariableDefinition(string definition, string csvVaraibleName)
        {
            return definition;
        }

        public string TypeName
        {
            get
            {
                return typeName;
            }
        }

        public bool IsType(string csvTypeField)
        {
            return csvTypeField.Contains(ArrayTypeIdentifierPrefix) && baseDataType.IsType(csvTypeField);
        }

        
        public Type SystemType {
            get {
                return listType;
            }
        }

        public object Deserialize(string rawItem, Type expectedType) {
            EnforceTypeMatch(expectedType);

            // create array of baseType
            object array =  Activator.CreateInstance(listType);

            if (rawItem == CSVConstant.EMPTY_ITEM)
                return array;

            string[] rawElements = rawItem.Split(arrayDelimeter);
            for (int i = 0; i < rawElements.Length; i++) {
                object element = baseDataType.Deserialize(rawElements[i], baseDataType.SystemType);
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

            typeIdentifier = ArrayTypeIdentifierPrefix + "<" + this.baseDataType.TypeIdentifier + ">";
            typeName = ArrayTypeNamePrefix + "<" + this.baseDataType.TypeName + ">";

            this.arrayDelimeter = arrayDelimeter;

            if (baseDataType == null)
                throw new ArgumentException("base type of array cannot be null!");

            Type[] typeArgs = { baseDataType.SystemType };
            listType = typeof(List<>).MakeGenericType(typeArgs);
            listAddMethod = listType.GetMethod("Add");
        }

        private void EnforceTypeMatch(System.Type expectedType)
        {
            Helper.EnforceTypeMatch(this, expectedType);
        }


    }
}