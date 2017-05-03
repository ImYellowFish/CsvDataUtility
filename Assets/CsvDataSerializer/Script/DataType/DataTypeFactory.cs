using System.Collections.Generic;
using System;

namespace CSVDataUtility {
    public class DataTypeFactory {
        private IDataType[] baseTypes;
        private List<IDataType> typeList;
        
        public DataTypeFactory() {
            baseTypes = new IDataType[] {
                new IntDataType(),
                new FloatDataType(),
                new BoolDataType(),
                new StringDataType(),
                new Vector3DataType(),
            };

            typeList = new List<IDataType>();
            for(int i = 0; i < baseTypes.Length; i++) {
                typeList.Add(new ArrayDataType(baseTypes[i]));
            }

            typeList.AddRange(baseTypes);
        }

        public IDataType GetDataType(string csvTypeField) {
            for (int i = 0; i < typeList.Count; i++) {
                if (typeList[i].IsType(csvTypeField))
                    return typeList[i];
            }

            IDataType newType;
            if (CheckForNewEnumType(csvTypeField, out newType))
                return newType;
            
            
            throw new CSVParseException("Unknown data type in csv: " + csvTypeField);
        }

        private bool CheckForNewEnumType(string csvTypeField, out IDataType newType) {
            if (csvTypeField.Contains(CSVConstant.ENUM_TYPE)) {
                newType = new EnumDataType(csvTypeField);
                return true;
            }else {
                newType = null;
                return false;
            }
        }
        
    }
}