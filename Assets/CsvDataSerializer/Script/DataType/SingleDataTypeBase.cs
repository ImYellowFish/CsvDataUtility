namespace CSVDataUtility {
    public abstract class SingleDataTypeBase : IDataType {
        public abstract string TypeName {
            get;
        }

        public abstract string GetTypeNameForWriter(string variableName);

        public virtual string GetAdditionalInfoForWriter(string variableName)
        {
            return "";
        }
        
        public virtual bool IsType(string csvTypeField) {
            return csvTypeField.Contains(TypeName);
        }

        public abstract object Deserialize(string rawItem, System.Type expectedType);

        protected static CSVParseException CreateItemParseException(string type) {
            return new CSVParseException("Item parse error for type: " + type);
        }
        
    }
}