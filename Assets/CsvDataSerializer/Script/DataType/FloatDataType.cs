using System;

namespace CSVDataUtility {
    public class FloatDataType : SingleDataTypeBase {
        public override string TypeIdentifier {
            get { return CSVConstant.FLOAT_TYPE; }
        }

        public override Type SystemType {
            get {
                return typeof(float);
            }
        }

        public override object Serialize(string rawItem, Type expectedType) {
            EnforceTypeMatch(expectedType);

            try {
                if (rawItem == CSVConstant.EMPTY_ITEM)
                    return 0f;
                return float.Parse(rawItem);
            } catch (System.FormatException) {
                throw CreateItemParseException(TypeIdentifier);
            }
        }
    }
}