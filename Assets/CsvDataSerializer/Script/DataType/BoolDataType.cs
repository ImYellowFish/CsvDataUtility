using System;

namespace CSVDataUtility {
    public class BoolDataType : SingleDataTypeBase {
        public override string TypeIdentifier {
            get { return CSVConstant.BOOL_TYPE; }
        }

        public override Type SystemType {
            get {
                return typeof(bool);
            }
        }

        public override object Serialize(string rawItem, Type expectedType) {
            EnforceTypeMatch(expectedType);

            string lower = rawItem.ToLower();
            if (lower.Contains(CSVConstant.BOOL_TRUE)) {
                return true;
            }
            if (lower.Contains(CSVConstant.BOOL_FALSE) || lower == CSVConstant.EMPTY_ITEM) {
                return false;
            }
            throw new CSVParseException("Unrecognized input for bool type: " + rawItem);
        }
    }
}