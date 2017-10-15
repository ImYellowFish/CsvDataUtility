using System;
using System.Collections.Generic;

namespace CSVDataUtility {
    public class EnumDataType : IDataType
    {
        protected string typeInfo;
        protected List<string> values;

        
        public EnumDataType(string typeInfo, string nested)
        {
            this.typeInfo = typeInfo;
            string[] rawValues = nested.Split(';');

            values = new List<string>();
            for (int i = 0; i < rawValues.Length; i++)
            {
                rawValues[i] = Helper.CorrectHeadItemString(rawValues[i]);
                if (rawValues[i] == "")
                    return;
                values.Add(rawValues[i]);
            }
        }

        public bool IsType(string csvTypeField)
        {
            return csvTypeField == CSVConstant.ENUM_TYPE;
        }


        public string TypeName {
            get {
                return typeInfo;
            }
        }


        public string GetTypeNameForWriter(string variableName)
        {
            return variableName + "_values";
        }


        public virtual string GetAdditionalInfoForWriter(string variableName)
        {
            string additional = "\n\tpublic enum ";
            additional = additional + GetTypeNameForWriter(variableName) + " { ";

            for (int i = 0; i < values.Count; i++)
            {
                additional = additional + values[i] + " = " + i.ToString() + ", ";
            }

            additional += "}\n";
            return additional;
        }


        public virtual string GetExtensionMethodForWriter(string dataEntryName, string variableName) {
            return "";
        }

        public virtual object Deserialize(string rawItem, Type expectedType) {
            try {
                var str = Helper.CorrectHeadItemString(rawItem);
                if (str == CSVConstant.EMPTY_ITEM)
                    return Enum.Parse(expectedType, "0");

                return Enum.Parse(expectedType, str);
            } catch {
                throw new CSVParseException("Unrecognized enum value: " + rawItem + ", expected: " + typeInfo);
            }

        }

        public DataTypeDeserializeExtraInfo deserializeExtraInfo { get; set; }

    }
}