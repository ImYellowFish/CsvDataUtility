using System.Collections.Generic;
[System.Serializable]
[CSVFilename("TestEnum")]
public class TestEnumDataEntry{

	[CSVField("id")]
	public string id;

	[CSVField("skilltype")]
	public skilltype_values skilltype;

	public enum skilltype_values { range = 0, dart = 1, }

	[CSVField("skillpos")]
	public UnityEngine.Vector3 skillpos;

	[CSVField("skillflag")]
	public skillflag_values skillflag;

	[System.Flags]
	public enum skillflag_values { na = 0, a = 1, b = 2, c = 4, }

	[CSVField("enumarraytest")]
	public List<enumarraytest_values> enumarraytest;

	public enum enumarraytest_values { first = 0, second = 1, third = 2, }

	[CSVField("skillflag2")]
	public skillflag2_values skillflag2;

	[System.Flags]
	public enum skillflag2_values { na = 0, a = 1, b = 2, c = 4, }


}

[System.Serializable]
public class TestEnumDataTable : CSVDataUtility.DataTable<TestEnumDataEntry>{
    public static TestEnumDataTable Create(){
        TestEnumDataTable datatable = new TestEnumDataTable();
        datatable.Read();
        return datatable;
    }
}


namespace CSVDataUtility.Extension {
    public static class TestEnumDataTable_Extension {
        

        public static bool ContainsFlag(this TestEnumDataEntry.skillflag_values self, TestEnumDataEntry.skillflag_values flag) {
            return (self & flag) == flag;
        }


        public static bool ContainsFlag(this TestEnumDataEntry.skillflag2_values self, TestEnumDataEntry.skillflag2_values flag) {
            return (self & flag) == flag;
        }

    }
}
