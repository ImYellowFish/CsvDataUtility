﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CSVDataUtility.Action
{
    /// <summary>
    /// Represents an action during code generation and deserialization time
    /// </summary>
    public class ActionItem
    {

        public ActionItem(string key, int index, string paramsTypeStr)
        {
            this.key = key;
            this.index = index;
            ParseParameterInfo(paramsTypeStr);

        }

        /// <summary>
        /// name specified in type field info
        /// </summary>
        public string key;

        /// <summary>
        /// index of this action
        /// </summary>
        public int index;


        /// <summary>
        /// Types of parameters
        /// </summary>
        public List<IDataType> parameterTypes;

        /// <summary>
        /// numbers of action parameters
        /// </summary>
        public int parameterCount
        {
            get { return parameterTypes.Count; }
        }

        /// <summary>
        /// Get the field name of the action, used in generated scripts.
        /// </summary>
        public string GetFieldName(string variableName)
        {
            return variableName + "_action_" + key;
        }


        public void Deserialize(string parameterInfo, string variableName, ActionInfo actionInfo)
        {
            actionInfo.callList.Add(index);

            if (Helper.CorrectHeadItemString(parameterInfo) == "")
            {
                // No parameters
                return;
            }

            string[] paramStringArray = parameterInfo.Split(CSVConstant.ARRAY_DELIMITER);
            if (paramStringArray.Length != parameterCount)
            {
                throw new CSVParseException("Wrong action param count, expected: " + parameterCount + ", actual: " + paramStringArray.Length);
            }

            for (int i = 0; i < paramStringArray.Length; i++)
            {
                var param = paramStringArray[i];
                var actionParam = new ActionParameter(param, parameterTypes[i].GetTypeNameForWriter(variableName));
                actionParam.DeserializeValue();
                actionInfo.paramList.Add(actionParam);
            }
        }


        private void ParseParameterInfo(string paramsTypeStr)
        {
            parameterTypes = new List<IDataType>();

            string[] rawValues = paramsTypeStr.Split(';');
            foreach (string childTypeField in rawValues)
            {
                var correctedType = Helper.CorrectHeadItemString(childTypeField);
                if (correctedType == "")
                    continue;
                var childDataType = DataTypeFactory.GetBaseDataType(correctedType);
                parameterTypes.Add(childDataType);
            }
        }
    }

}
