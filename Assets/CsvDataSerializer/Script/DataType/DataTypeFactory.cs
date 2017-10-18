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
        private IDataType GetNestedDataType(string typeInfo, string prefix, string nesting, string csvFieldName)
        {
            if (prefix == CSVConstant.ARRAY_TYPE)
            {
                IDataType arrayType;
                if (!TryGetSpecialArrayType(nesting, out arrayType))
                {
                    // use default ArrayDataType
                    IDataType baseType = GetDataType(nesting, csvFieldName);
                    arrayType = new ArrayDataType(baseType);
                }
                return arrayType;
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

            else if(prefix == CSVConstant.STRUCT_TYPE)
            {
                return new StructDataType(prefix, nesting);
            }
            
            throw new CSVParseException("Unknown data type in csv: " + prefix + "+" + nesting);
        }

        // Add types which need a special treatment for array case, instead of the default ArrayDataType
        public bool TryGetSpecialArrayType(string nestedTypeInfo, out IDataType specialDataTypes)
        {
            specialDataTypes = null;

            string prefix, nesting;
            if (!Helper.AnalyzeNestingTypeInfo(nestedTypeInfo, out prefix, out nesting))
                return false;
            prefix = Helper.CorrectHeadItemString(prefix);

            // add new cases here
            if (prefix == CSVConstant.STRUCT_TYPE)
            {
                specialDataTypes = new StructArrayDataType(prefix, nesting);
                return true;
            }
            return false;
        }

        #endregion
        

        public IDataType GetDataType(string csvTypeField, string csvFieldName) {
            string prefix;
            string nesting;
            // Check for nesting type first, e.g. list<> and enum<>
            if(Helper.AnalyzeNestingTypeInfo(csvTypeField, out prefix, out nesting))
            {
                var dataType = GetNestedDataType(csvTypeField, Helper.CorrectHeadItemString(prefix), nesting, csvFieldName);
                if(!(dataType is RefDataType) && !historyDataTypes.ContainsKey(csvFieldName))
                {
                    historyDataTypes.Add(csvFieldName, dataType);
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