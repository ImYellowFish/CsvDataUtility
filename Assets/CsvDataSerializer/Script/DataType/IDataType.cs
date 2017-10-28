namespace CSVDataUtility {
    public interface IDataType {
        /// <summary>
        /// The name of this type.
        /// </summary>
        string TypeName { get; }


        /// <summary>
        /// Type name that is used in generated class file.
        /// </summary>
        string GetTypeNameForWriter(string variableName);


        /// <summary>
        /// Additional info in generated file that is appended after type definition
        /// </summary>
        string GetAdditionalInfoForWriter(string variableName);

        
        /// <summary>
        /// Extension method in generated file that is appended after class definition
        /// </summary>
        string GetExtensionMethodForWriter(string dataEntryName, string variableName);

        
        /// <summary>
        /// read string and check if it is a value of this type
        /// </summary>
        /// <param name="csvTypeField"></param>
        /// <returns></returns>
        bool IsType(string csvTypeField);
        

        /// <summary>
        /// read string and return a value of this type (same as its SystemType)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        object Deserialize(string item, System.Type expectedType);


        /// <summary>
        /// Extra info needed for deserialization.
        /// Set before calling Deserialize(string,Type).
        /// </summary>
        DataTypeDeserializeExtraInfo deserializeExtraInfo { get; set; }
    }


}