﻿using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

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

        public static int GetKeyColumnIndex(string[] csvTypes)
        {
            // check for key column index
            int keyIndex = -1;
            for (int i = 0; i < csvTypes.Length; i++)
            {
                if (CorrectHeadItemString(csvTypes[i]).Contains(CSVConstant.KEY_IDENTIFIER))
                {
                    keyIndex = i;
                }
            }

            if (keyIndex == -1)
            {
                throw new CSVParseException("cannot find key column!");
            }
            return keyIndex;
        }

        public static void CreateDirectoryIfNotExist(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }


        public static string GetRelativePathToResourcesFolder(string path)
        {
            return GetRelativePathToFolder(path, "Resources");
        }

        public static string GetRelativePathToAssetsFolder(string path)
        {
            return GetRelativePathToFolder(path, "Assets");
        }

        private static string GetRelativePathToFolder(string path, string folderIdentifier)
        {
            if (path.Trim() == "")
            {
                return "";
            }

            int i = path.LastIndexOf(folderIdentifier);
            string result = path.Substring(i + folderIdentifier.Length);
            result = result.TrimStart('\\', '/');
            return result;
        }


        public static MethodInfo GetAddMethodForListType(System.Type listType)
        {
            return listType.GetMethod("Add");
        }

        public static System.Type GetBaseTypeForListType(System.Type listType)
        {
            return listType.GetGenericArguments()[0];
        }
    }

}