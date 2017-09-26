using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSVDataUtility
{
    public interface IDataAsset
    {
        void Read();
    }

    [System.Serializable]
    public class DataAsset<T> : ScriptableObject, IDataAsset where T : IDataEntry
    {
        public T[] data;
        

        public void Read()
        {
            DataTable<T> dataTable = new DataTable<T>();
            dataTable.Read();
            
            var values = dataTable.Values;
            int dataCount = values.Count;

            data = new T[dataCount];
            int index = 0;
            foreach (var entry in values)
            {
                data[index] = entry;
                index++;
            }
        }
    }
}