using System.Collections.Generic;
using System;
using System.Reflection;

namespace CSVDataUtility {
    public abstract class ComplexDataTypeBase : IDataType {
        protected IDataType baseDataType;
        protected string typeIdentifier;
        protected string typeName;
        
        public ComplexDataTypeBase(IDataType baseDataType) {
            if (baseDataType == null)
                throw new ArgumentException("base type of complex type cannot be null!");

            this.baseDataType = baseDataType;
            
            typeIdentifier = ComplexTypeIdentifierPrefix + "<" + this.baseDataType.TypeIdentifier + ">";
            typeName = ComplexTypeNamePrefix + "<" + this.baseDataType.TypeName + ">";
        }

        public string TypeIdentifier {
            get {
                return typeIdentifier;
            }
        }

        public string GetDecoratedTypeDefinition(string definition, string csvVaraibleName) { 
            return definition; 
        }

        public string TypeName {
            get {
                return typeName;
            }
        }

        public bool IsType(string csvTypeField) {
            return csvTypeField.Contains(ComplexTypeIdentifierPrefix) && baseDataType.IsType(csvTypeField);
        }


        public abstract string ComplexTypeIdentifierPrefix {
            get;
        }

        public abstract string ComplexTypeNamePrefix {
            get;
        }


        public abstract Type SystemType {
            get;
        }

        public abstract object Serialize(string rawItem, Type expectedType);

        protected void EnforceTypeMatch(System.Type expectedType) {
            Helper.EnforceTypeMatch(this, expectedType);
        }


    }
}