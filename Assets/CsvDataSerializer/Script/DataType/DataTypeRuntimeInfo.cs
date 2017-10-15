using System;

namespace CSVDataUtility
{
    public class DataTypeDeserializeExtraInfo
    {
        public string variableName;
        public Type dataEntryType;

        public DataTypeDeserializeExtraInfo(string variableName, Type dataEntryType)
        {
            this.variableName = variableName;
            this.dataEntryType = dataEntryType;
        }
    }
}