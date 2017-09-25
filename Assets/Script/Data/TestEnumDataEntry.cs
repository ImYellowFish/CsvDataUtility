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


}

[System.Serializable]
public class TestEnumDataTable : CSVDataUtility.DataTable<TestEnumDataEntry>{
    public static TestEnumDataTable Create(){
        TestEnumDataTable datatable = new TestEnumDataTable();
        datatable.Read();
        return datatable;
    }
}
