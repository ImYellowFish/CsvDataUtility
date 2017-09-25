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
        

        public static string GetValidScriptVariableName(string rawName, bool toLower) {
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
        
        public static IImportSetting ImportSetting {
            get { return UnityImportSetting.Instance; }
        }

        public static string ToTrimedLower(string item)
        {
            return item.Trim().ToLower();
        }

        public static string CorrectDataItemString(string item)
        {
            return item.Trim();
        }

        public static string CorrectHeadItemString(string item)
        {
            return item.Trim().ToLower();
        }

        /// <summary>
        /// Look for a nested typeInfo like A<B>
        /// </summary>
        public static bool AnalyzeNestingTypeInfo(string typeInfo, out string prefix, out string nesting)
        {
            int startIndex = typeInfo.IndexOf('<') + 1;
            int endIndex = typeInfo.LastIndexOf('>') - 1;
            if (startIndex <= 0 || startIndex >= typeInfo.Length || endIndex < 0)
            {
                prefix = typeInfo;
                nesting = "";
                return false;
            }

            prefix = typeInfo.Substring(0, startIndex - 1);
            nesting = typeInfo.Substring(startIndex, endIndex - startIndex + 1);
            return true;
        }
    }

}