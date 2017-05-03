using System;
using System.Collections.Generic;

namespace CSVDataUtility {
    public class EnumDataType : SingleDataTypeBase {
        private string typeInfo;
        private List<string> values;
        
        public EnumDataType(string typeInfo) {
            this.typeInfo = typeInfo;
            
            string[] rawValues = typeInfo.Split('<', '>',';');
            values = new List<string>();
            for (int i = 1; i < rawValues.Length; i++) {
                rawValues[i] = rawValues[i].Trim();
                if (rawValues[i] == "")
                    return;
                values.Add(rawValues[i]);
            }
        }

        public override Type SystemType {
            get {
                return typeof(Enum);
            }
        }
        

        public override string TypeIdentifier {
            get {
                return typeInfo;
            }
        }
        
        public enum field { field = 0, }
        public override string GetDecoratedTypeDefinition(string definition, string csvVariableName) {
            string enumTypeName = csvVariableName + "_values";

            definition = definition.Replace(TypeName, enumTypeName);

            string additional = "\n\tpublic enum ";
            additional = additional + enumTypeName + " { ";

            for(int i = 0; i < values.Count; i++) {
                additional = additional + values[i] + " = " + i.ToString() + ", ";
            }

            additional += "}\n";
            return definition + additional;
        }


        public override object Serialize(string rawItem, Type expectedType) {
            try {
                return Enum.Parse(expectedType, rawItem);
            } catch {
                throw new CSVParseException("Unrecognized enum value: " + rawItem + ", expected: " + typeInfo);
            }

        }
    }
}