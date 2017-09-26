using UnityEngine;
using System.Collections.Generic;

namespace CSVDataUtility {
    
    [System.Serializable]
    public class DataTable<T> where T : class {

        Dictionary<string, T> data;
        public int dataCount;

        public Dictionary<string, T>.KeyCollection Keys {
            get {
                return data.Keys;
            }
        }

        public Dictionary<string, T>.ValueCollection Values {
            get {
                return data.Values;
            }
        }

        private T[] valueArray;
        public T[] ValueArray {
            get {
                if(valueArray == null) {
                    valueArray = new T[Values.Count];
                    Values.CopyTo(valueArray, 0);
                }
                return valueArray;
            }
        }


        public T GetEntry(string key) {
            try {
                return data[key];
            } catch (KeyNotFoundException) {
                throw new CSVParseException("cannot find key:" + key + " in datatable: " + typeof(T).Name);
                
            }
        }
        public bool ExistKey(string key) {
            try
            {
                return data[key] != null;
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
        }
        public Dictionary<string, T> GetAllEntry()
        {
            return data;
        }


        protected void Read() {
            string csvFilename = CSVFilenameAttribute.GetCsvFilename(typeof(T));
            if (Helper.ImportSetting.CsvFolderPath.Trim() != "") {
                csvFilename = Helper.ImportSetting.CsvFolderPath + "/" + csvFilename;
            }

            if (csvFilename == null)
                throw new CSVParseException("Not a valid csv data class format: " + typeof(T).Name);

            TextAsset csv = Resources.Load<TextAsset>(csvFilename);
            if (csv == null) {
                throw new CSVParseException("Cannot find expected csv: " + csvFilename);
            }

            data = Serializer.Deserialize<T>(csv.text);
            dataCount = data.Keys.Count;
        }
    }
}