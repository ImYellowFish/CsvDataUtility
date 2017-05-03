using System;

namespace CSVDataUtility {
    public class StringDataType : SingleDataTypeBase {
        public override Type SystemType {
            get {
                return typeof(string);
            }
        }

        public override string TypeIdentifier {
            get {
                return CSVConstant.STRING_TYPE;
            }
        }

        // string type, return as it is
        public override object Serialize(string rawItem, Type expectedType) {
            EnforceTypeMatch(expectedType);
            return rawItem;
        }
    }
}