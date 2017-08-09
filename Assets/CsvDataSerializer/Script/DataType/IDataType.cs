namespace CSVDataUtility {
    public interface IDataType {
        /// <summary>
        /// corresponding system type
        /// </summary>
        System.Type SystemType { get; }

        /// <summary>
        /// Type name that matches the type info from csv file
        /// </summary>
        string TypeIdentifier { get; }

        /// <summary>
        /// Type name that is used for code generation.
        /// </summary>
        string TypeName { get; }


        /// <summary>
        /// Process the definition from the code generator, with csvVariableName provided
        /// The original definition will be replaced by the result from this method
        /// Useful to customerize class generation process
        /// used in EnumDataType
        /// </summary>
        string OverrideGeneratedVariableDefinition(string definition, string csvVariableName);


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

    }


}