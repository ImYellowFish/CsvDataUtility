using System;
using System.Collections.Generic;

namespace CSVDataUtility
{
    public class RefDataType : IDataType
    {
        private string refVariableName;
        private IDataType refType;

        public RefDataType(string refVariableName, Dictionary<string, IDataType> historyDataTypes)
        {
            refVariableName = Helper.CorrectHeadItemString(refVariableName);
            this.refVariableName = refVariableName;

            if (!historyDataTypes.ContainsKey(refVariableName))
            {
                throw new CSVParseException("Cannot find ref variable: " + refVariableName);
            }
            refType = historyDataTypes[refVariableName];
        }

        public bool IsType(string csvTypeField)
        {
            return csvTypeField == CSVConstant.ENUM_TYPE;
        }


        public string TypeName
        {
            get
            {
                return refType.TypeName;
            }
        }


        public string GetTypeNameForWriter(string variableName)
        {
            return refType.GetTypeNameForWriter(refVariableName);
        }


        public virtual string GetAdditionalInfoForWriter(string variableName)
        {
            return "";
        }


        public virtual string GetExtensionMethodForWriter(string dataEntryName, string variableName)
        {
            return "";
        }

        public virtual object Deserialize(string rawItem, Type expectedType)
        {
            return refType.Deserialize(rawItem, expectedType);
        }

        public DataTypeDeserializeExtraInfo deserializeExtraInfo
        {
            get { return refType.deserializeExtraInfo; }
            set
            {
                var info = value;
                info.variableName = refVariableName;
                refType.deserializeExtraInfo = info;
            }
        }
    }
}