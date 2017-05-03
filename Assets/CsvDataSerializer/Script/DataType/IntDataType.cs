using System;

namespace CSVDataUtility {
    public class IntDataType : SingleDataTypeBase {
        public override string TypeIdentifier {
            get { return CSVConstant.INT_TYPE; }
        }

        public override Type SystemType {
            get {
                return typeof(int);
            }
        }

        public override object Serialize(string rawItem, Type expectedType) {
            EnforceTypeMatch(expectedType);

            try {
                if (rawItem == CSVConstant.EMPTY_ITEM)
                    return 0;
                return int.Parse(rawItem);
            } catch (System.FormatException) {
                throw CreateItemParseException(TypeIdentifier);
            }
        }
    }
}