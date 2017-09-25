using System;

namespace CSVDataUtility {
    public class IntDataType : SingleDataTypeBase {
        public override string TypeName {
            get { return CSVConstant.INT_TYPE; }
        }

        public override string GetTypeNameForWriter(string variableName)
        {
            return CSVConstant.INT_TYPE;
        }

        public override object Deserialize(string rawItem, Type expectedType) {           
            try {
                if (rawItem == CSVConstant.EMPTY_ITEM)
                    return 0;
                return int.Parse(rawItem);
            } catch (System.FormatException) {
                throw CreateItemParseException(TypeName);
            }
        }
    }
}