namespace CSVDataUtility {
    public abstract class SingleDataTypeBase : IDataType, ISingleDataType {
        public abstract string TypeName {
            get;
        }

        public abstract string GetTypeNameForWriter(string variableName);

        public virtual string GetAdditionalInfoForWriter(string variableName)
        {
            return "";
        }

        public virtual string GetExtensionMethodForWriter(string dataEntryName, string variableName) {
            return "";
        }
        
        public abstract System.Type SystemType { get; }

        public virtual bool IsType(string csvTypeField) {
            return csvTypeField.Contains(TypeName);
        }

        public abstract object Deserialize(string rawItem, System.Type expectedType);

        protected static CSVParseException CreateItemParseException(string type) {
            return new CSVParseException("Item parse error for type: " + type);
        }

        public DataTypeDeserializeExtraInfo deserializeExtraInfo { get; set; }

    }
}