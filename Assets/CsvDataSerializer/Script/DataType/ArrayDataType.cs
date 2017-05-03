using System.Collections.Generic;
using System;
using System.Reflection;

namespace CSVDataUtility {
    public class ArrayDataType : ComplexDataTypeBase {
        private Type listType;
        private MethodInfo listAddMethod;
        private char arrayDelimeter;

        public ArrayDataType(IDataType baseDataType) : base(baseDataType) {
            Init(baseDataType, CSVConstant.ARRAY_DELIMITER);

        }

        public ArrayDataType(IDataType baseDataType, char arrayDelimeter) : base(baseDataType) {
            Init(baseDataType, arrayDelimeter);
        }

        
        public override string ComplexTypeIdentifierPrefix {
            get {
                return CSVConstant.ARRAY_TYPE;
            }
        }

        public override string ComplexTypeNamePrefix {
            get {
                return "List";
            }
        }


        public override Type SystemType {
            get {
                return listType;
            }
        }

        public override object Serialize(string rawItem, Type expectedType) {
            EnforceTypeMatch(expectedType);

            // create array of baseType
            object array =  Activator.CreateInstance(listType);

            if (rawItem == CSVConstant.EMPTY_ITEM)
                return array;

            string[] rawElements = rawItem.Split(arrayDelimeter);
            for (int i = 0; i < rawElements.Length; i++) {
                object element = baseDataType.Serialize(rawElements[i], baseDataType.SystemType);
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
            this.arrayDelimeter = arrayDelimeter;

            if (baseDataType == null)
                throw new ArgumentException("base type of array cannot be null!");

            Type[] typeArgs = { baseDataType.SystemType };
            listType = typeof(List<>).MakeGenericType(typeArgs);
            listAddMethod = listType.GetMethod("Add");
        }

    }
}