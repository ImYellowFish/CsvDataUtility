using UnityEngine;
using System.Collections.Generic;
using System;

namespace CSVDataUtility {
    public class Vector3DataType : SingleDataTypeBase {
        private ArrayDataType arrayType;

        public Vector3DataType() {
            arrayType = new ArrayDataType(new FloatDataType(), ';');
        }

        public override string TypeIdentifier {
            get {
                return "vector";
            }
        }

        public override string TypeName {
            get {
                return "UnityEngine.Vector3";
            }
        }

        public override Type SystemType {
            get {
                return typeof(Vector3);
            }
        }

        public override object Serialize(string rawItem, Type expectedType) {
            EnforceTypeMatch(expectedType);

            List<float> floatList = arrayType.Serialize(rawItem, typeof(List<float>)) as List<float>;

            if (floatList == null)
                throw new CSVParseException("Failed to parse Vector3 format: " + rawItem);

            if (floatList.Count != 3)
                throw new CSVParseException("Vector3 has incorrect number of elements: " + rawItem);

            return new Vector3(floatList[0], floatList[1], floatList[2]);
        }
    }
}