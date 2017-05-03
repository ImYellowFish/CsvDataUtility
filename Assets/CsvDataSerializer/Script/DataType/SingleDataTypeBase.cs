namespace CSVDataUtility {
    public abstract class SingleDataTypeBase : IDataType {
        public abstract System.Type SystemType {
            get;
        }

        public abstract string TypeIdentifier {
            get;
        }

        public virtual string TypeName {
            get {
                return TypeIdentifier;
            }
        }

        public virtual string GetDecoratedTypeDefinition(string definition, string csvVariableName) {
            return definition;
        }

        public virtual bool IsType(string csvTypeField) {
            return csvTypeField.Contains(TypeIdentifier);
        }

        public abstract object Serialize(string rawItem, System.Type expectedType);

        protected static CSVParseException CreateItemParseException(string type) {
            return new CSVParseException("Item parse error for type: " + type);
        }

        protected void EnforceTypeMatch(System.Type expectedType) {
            Helper.EnforceTypeMatch(this, expectedType);
        }
    }
}