using System;

namespace CSVDataUtility {
    public class BoolDataType : SingleDataTypeBase {
        public override string TypeName {
            get { return CSVConstant.BOOL_TYPE; }
        }

        public override string GetTypeNameForWriter(string variableName)
        {
            return CSVConstant.BOOL_TYPE;
        }

        public override object Deserialize(string rawItem, Type expectedType) {
            string lower = rawItem.ToLower();
            if (lower.Contains(CSVConstant.BOOL_TRUE)) {
                return true;
            }
            if (lower.Contains(CSVConstant.BOOL_FALSE) || lower == CSVConstant.EMPTY_ITEM) {
                return false;
            }
            throw new CSVParseException("Unrecognized input for bool type: " + rawItem);
        }

        public override Type SystemType
        {
            get
            {
                return typeof(bool);
            }
        }
    }
}