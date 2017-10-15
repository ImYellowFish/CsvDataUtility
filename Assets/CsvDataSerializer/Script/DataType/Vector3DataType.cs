using UnityEngine;
using System.Collections.Generic;
using System;

namespace CSVDataUtility {
    public class Vector3DataType : SingleDataTypeBase {
        private ArrayDataType arrayType;

        public Vector3DataType() {
            arrayType = new ArrayDataType(new FloatDataType(), ';');
        }

        public override string TypeName {
            get {
                return "vector3";
            }
        }

        public override string GetTypeNameForWriter(string variableName) {
            return "UnityEngine.Vector3";
        }
        
        public override object Deserialize(string rawItem, Type expectedType) {
            List<float> floatList = arrayType.Deserialize(rawItem, typeof(List<float>)) as List<float>;

            if (floatList == null)
                throw new CSVParseException("Failed to parse Vector3 format: " + rawItem);

            Vector3 result = Vector3.zero;
            for(int i = 0; i < Mathf.Min(3, floatList.Count); i++)
            {
                result[i] = floatList[i];
            }

            return result;
        }

        public override Type SystemType
        {
            get
            {
                return typeof(Vector3);
            }
        }
    }
}