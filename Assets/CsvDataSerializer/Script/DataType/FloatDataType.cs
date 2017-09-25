using System;

namespace CSVDataUtility {
    public class FloatDataType : SingleDataTypeBase {
        public override string TypeName {
            get { return CSVConstant.FLOAT_TYPE; }
        }

        public override string GetTypeNameForWriter(string variableName)
        {
            return CSVConstant.FLOAT_TYPE;
        }

        public override object Deserialize(string rawItem, Type expectedType) {

            try {
                if (rawItem == CSVConstant.EMPTY_ITEM)
                    return 0f;
                return float.Parse(rawItem);
            } catch (System.FormatException) {
                throw CreateItemParseException(TypeName);
            }
        }
    }
}