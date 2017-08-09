using System.Collections.Generic;

namespace CSVDataUtility {
    
    /// <summary>
    /// Generate a table dictionary from raw CSV text
    /// Split the lines and do type conversion, and map them with keys.
    /// The items stored will be transformed from string to the expected types. e.g. int, string, bool, list
    /// </summary>
    public class CSVReader {
        public CSVReader(string csvContent) {
            rawContent = csvContent;
        }

        /// <summary>
        /// read CSV text and split them into string elements by columns and rows
        /// </summary>
        public List<string[]> Read() {
            if (rawContent == null) {
                throw new CSVParseException("csv raw content missing!");
            }
            
            var content = CSVFile.CSV.LoadString(rawContent, true, false);
            return content;
        }
        
        private string rawContent;        
    }
}