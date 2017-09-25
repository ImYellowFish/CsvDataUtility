using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSVDataUtility
{
    public class FlagDataType : EnumDataType
    {
        public FlagDataType(string typeInfo, string nested) : base(typeInfo, nested)
        {
            
        }

        
        public override string GetAdditionalInfoForWriter(string variableName)
        {
            string additional = "\n\t[System.Flags]\n\tpublic enum ";
            additional = additional + GetTypeNameForWriter(variableName) + " { ";
            int enumValue = 0;

            for (int i = 0; i < values.Count; i++)
            {
                additional = additional + values[i] + " = " + enumValue.ToString() + ", ";

                if (enumValue == 0)
                    enumValue = 1;
                else
                    enumValue *= 2;
            }

            additional += "}\n";
            return additional;
        }

        public override object Deserialize(string rawItem, Type expectedType)
        {
            int enumValue = 0;
            string[] rawElements = rawItem.Split(CSVConstant.FLAG_DELIMITER);
            for(int i = 0; i < rawElements.Length; i++)
            {
                object flag = base.Deserialize(rawElements[i], expectedType);
                enumValue = enumValue | (int)flag;
            }

            return Enum.Parse(expectedType, enumValue.ToString());
        }
    }
}