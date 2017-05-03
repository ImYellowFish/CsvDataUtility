using UnityEditor;
using UnityEngine;
using System.IO;

namespace CSVDataUtility {
    public class GenerateCsvMenu : MonoBehaviour {
        [MenuItem("Assets/CSV Data Utility/Generate class for csv")]
        private static void GenerateSingle() {
            var selected = Selection.activeObject as TextAsset;
            Generate(selected);
        }

        [MenuItem("Assets/CSV Data Utility/Generate class for csv", true)]
        private static bool GenerateSingleValidate() {
            var selected = Selection.activeObject;
            return Validate(selected);
        }

        [MenuItem("Assets/CSV Data Utility/Generate class for folder(non-recursive)")]
        private static void GenerateForFolder() {
            var selected = Selection.activeObject;
            int successCount = 0;
            int failCount = 0;

            string[] files = Directory.GetFiles(AssetDatabase.GetAssetPath(selected));
            foreach(string path in files) {
                Object current = AssetDatabase.LoadAssetAtPath<Object>(path);
                try {
                    // ignore .meta files
                    if (path.Contains(".meta") || path.Contains(".xlsm"))
                        continue;

                    if (Validate(current)) {
                        Generate(current as TextAsset);
                        successCount++;
                    } else {
                        Debug.LogWarning("Skipped file because it is not csv format: " + path);
                        failCount++;
                    }
                }catch(CSVParseException e) {
                    Debug.LogWarning("Parse failed for file: " + path);
                    Debug.LogWarning(e.ToString());
                    failCount++;
                }
            }

            Debug.Log("Generate Complte. Success count = " + successCount + ", Failed count = " + failCount);
        }

        [MenuItem("Assets/CSV Data Utility/Generate class for folder(non-recursive)", true)]
        private static bool GenerateForFolderValidate() {
            var selected = Selection.activeObject;
            return IsDirectory(selected);
        }

        private static void Generate(TextAsset selected) {
            string folderPath = Helper.ImportSetting.DataEntryFolderPath;
            if (!Directory.Exists(folderPath)) {
                Directory.CreateDirectory(folderPath);
            }

            DataClassGenerator gen = new DataClassGenerator();
            gen.Generate(selected.name, selected.text, folderPath);
            Debug.Log("Class generated for: " + selected.name);
        }

        private static bool Validate(Object selected) {
            //Debug.Log("selected type: " + selected.GetType().Name);
            //Debug.Log("selected path: " + AssetDatabase.GetAssetPath(selected));
            return selected != null && selected is TextAsset && AssetDatabase.GetAssetPath(selected).ToLower().Contains(".csv");
        }
        
        private static char[] slash = { '\\', '/' };
        private static string GetAssetParentFolderPath(string assetPath) {
            int lastSlash = assetPath.LastIndexOfAny(slash);
            return assetPath.Substring(0, lastSlash);
        }

        private static bool IsDirectory(Object selected) {
            // get the file attributes for file or directory
            FileAttributes attr = File.GetAttributes(AssetDatabase.GetAssetPath(selected));
            //detect whether its a directory or file
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                return true;
            else
                return false;
        }
    }
}