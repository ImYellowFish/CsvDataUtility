﻿using UnityEngine;
using System.Collections.Generic;

namespace CSVDataUtility {
    
    [System.Serializable]
    public class DataTable<T> where T : IDataEntry {

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
                    foreach(var value in Values)
                    {
                        valueArray[value.internal_dataEntryIndex] = value;
                    }
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


        public void Read() {
            if (Helper.ImportSetting.UseScriptableObject)
            {
                try
                {
                    ReadFromDataAsset();
                }
                catch
                {
                    ReadFromCsv();
                }
            }
            else
                ReadFromCsv();
        }

        public void ReadFromCsv()
        {
            string csvFilename = CSVFilenameAttribute.GetCsvFilename(typeof(T));
            if (Helper.ImportSetting.CsvFolderRelativePathToResources.Trim() != "")
            {
                csvFilename = Helper.ImportSetting.CsvFolderRelativePathToResources + "/" + csvFilename;
            }

            if (csvFilename == null)
                throw new CSVParseException("Not a valid csv data class format: " + typeof(T).Name);

            TextAsset csv = Resources.Load<TextAsset>(csvFilename);
            if (csv == null)
            {
                throw new CSVParseException("Cannot find expected csv: " + csvFilename);
            }

            data = Serializer.Deserialize<T>(csv.text);
            dataCount = data.Keys.Count;
        }


        public void ReadFromDataAsset()
        {
            string dataAssetPath = CSVDataAssetAttribute.GetDataTableName(typeof(T));
            if (Helper.ImportSetting.DataAssetRelativePathToProject.Trim() != "")
            {
                dataAssetPath = Helper.ImportSetting.DataAssetRelativePathToProject + "/" + dataAssetPath;
            }

            dataAssetPath = Helper.GetRelativePathToResourcesFolder(dataAssetPath);

            var obj = Resources.Load(dataAssetPath);
            DataAsset<T> dataAsset = obj as DataAsset<T>;

            data = new Dictionary<string, T>();
            for (int i = 0; i < dataAsset.data.Length; i++)
            {
                var entry = dataAsset.data[i];
                var key = entry.internal_dataEntryID;

                data.Add(key, entry);
            }
            dataCount = data.Keys.Count;
        }


    }
}