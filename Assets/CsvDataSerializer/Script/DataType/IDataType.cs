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
        /// Type name that is used in generated class.
        /// </summary>
        string TypeName { get; }


        /// <summary>
        /// Process the definition from the class writer, with csvVariableName provided
        /// Useful to customerize class generation process
        /// used in EnumDataType
        /// </summary>
        string GetDecoratedTypeDefinition(string definition, string csvVariableName);


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
        object Serialize(string item, System.Type expectedType);

    }


}