namespace CSVDataUtility {
    public class CSVParseException : System.Exception {
        public CSVParseException(string message) : base(message) {

        }

        public CSVParseException(string message, int row, int column) :
            base(">>>csv error coord: (" + row + "," + column + "), " + message) {
        }

        public CSVParseException(string message, string item, int row, int column) :
           base(message + ">>> item: " + item + ">>> error coord: (" + row + "," + column + ")") {
        }
    }
}