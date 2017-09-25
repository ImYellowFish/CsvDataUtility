using System;

namespace CSVDataUtility {
    public class StringDataType : SingleDataTypeBase {
        public override string TypeName {
            get {
                return CSVConstant.STRING_TYPE;
            }
        }

        public override string GetTypeNameForWriter(string variableName)
        {
            return CSVConstant.STRING_TYPE;
        }

        // string type, return as it is
        public override object Deserialize(string rawItem, Type expectedType) {
            return rawItem;
        }
    }
}