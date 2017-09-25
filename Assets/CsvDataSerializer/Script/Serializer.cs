using System.Collections;
using System.Collections.Generic;

namespace CSVDataUtility {
    public static class Serializer {            
        public static Dictionary<string, T> Deserialize<T>(string csvContent) where T : class {
            DataTableSerializer ser = new DataTableSerializer(csvContent);
            return ser.Deserialize<T>();
        }        
    }
}