using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System;

namespace CSVDataUtility {
    public static class Helper {
        public static void LogWarning(object message) {
            Debug.LogWarning(message);
        }

        public static void Log(object message) {
            Debug.Log(message);
        }

        public static bool CompareTypeInfo(string typeIdentifier, string csvTypeField) {
            return csvTypeField.Contains(typeIdentifier);
        }

        public static CSVParseException CreateItemParseException(string type) {
            return new CSVParseException("Item parse error for type: " + type);
        }

        public static string GetScriptFriendlyName(string rawName, bool toLower) {
            string result;

            result = rawName.Trim().Replace(" ", "_");
            if (toLower) {
                result = result.ToLower();
            }

            int firstChar;

            // if first character is number, add an underscore in the front
            if (int.TryParse(result[0].ToString(), out firstChar)) {
                result = "_" + result;
            }
            return result;
        }

        public static void EnforceTypeMatch(IDataType dataType, Type expectedType) {
            if (expectedType != dataType.SystemType) {
                throw new CSVParseException("Deserialize type dismatch: csv type: " + dataType.TypeName + ", class type: " + expectedType.Name);
            }
        }

        public static IImportSetting ImportSetting {
            get { return UnityImportSetting.Instance; }
        }
    }

}