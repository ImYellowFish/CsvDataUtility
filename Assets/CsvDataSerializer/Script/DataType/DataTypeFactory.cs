using System.Collections.Generic;
using CSVDataUtility.Action;

namespace CSVDataUtility {
    public class DataTypeFactory {
        #region Extensible
        // Add new base types here 
        // (A type is a base type if its csv identifier does not include "<>")
        private static IDataType[] baseTypes = new IDataType[] {
                new IntDataType(),
                new FloatDataType(),
                new BoolDataType(),
                new StringDataType(),
                new Vector3DataType(),
            };
        
        // Add new nested types here 
        // (A type is a base type if its csv identifier includes "<>")
        private IDataType GetNestedDataType(string typeInfo, string prefix, string nesting, string variableName)
        {
            if (prefix == CSVConstant.ARRAY_TYPE)
            {
                IDataType baseType = GetDataType(nesting, variableName);
                return new ArrayDataType(baseType);
            }

            else if (prefix == CSVConstant.ENUM_TYPE)
            {
                return new EnumDataType(typeInfo, nesting);
            }

            else if (prefix == CSVConstant.FLAG_TYPE)
            {
                return new FlagDataType(typeInfo, nesting);
            }

            else if (prefix == CSVConstant.ACTION_TYPE)
            {
                return new ActionDataType(prefix, nesting);
            }

            else if (prefix == CSVConstant.REF_TYPE)
            {
                return new RefDataType(nesting, historyDataTypes);
            }

            throw new CSVParseException("Unknown data type in csv: " + prefix + "+" + nesting);
        }
        #endregion
        

        public IDataType GetDataType(string csvTypeField, string variableName) {
            string prefix;
            string nesting;
            // Check for nesting type first, e.g. list<> and enum<>
            if(Helper.AnalyzeNestingTypeInfo(csvTypeField, out prefix, out nesting))
            {
                var dataType = GetNestedDataType(csvTypeField, Helper.CorrectHeadItemString(prefix), nesting, variableName);
                if(!(dataType is RefDataType) && !historyDataTypes.ContainsKey(variableName))
                {
                    historyDataTypes.Add(variableName, dataType);
                }

                return dataType;
            }
            else
            {
                return GetBaseDataType(csvTypeField);
            }
        }

        

        public static IDataType GetBaseDataType(string csvTypeField)
        {
            for (int i = 0; i < baseTypes.Length; i++)
            {
                if (baseTypes[i].IsType(csvTypeField))
                    return baseTypes[i];
            }
            throw new CSVParseException("Unknown data type in csv: " + csvTypeField);
        }

        private Dictionary<string, IDataType> historyDataTypes = new Dictionary<string, IDataType>();

    }
}