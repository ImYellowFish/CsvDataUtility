//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by CSVDataUtility.ClassWriter.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System.Collections.Generic;
[System.Serializable]
[CSVFilename("TestRef")]
[CSVDataAsset("TestRefDataTable")]
public class TestRefDataEntry : CSVDataUtility.IDataEntry{
            
	[CSVField("id")]
	public string id;

	[CSVField("original field")]
	public original_field_values original_field;

	public enum original_field_values { a = 0, b = 1, c = 2, }

	[CSVField("refer")]
	public original_field_values refer;



    public string internal_dataEntryID { 
        get {
            return id;    
        } 
    }
    
    [CSVInternalIndex]
    public int m_internal_dataEntryIndex;
    public int internal_dataEntryIndex { get { return m_internal_dataEntryIndex; } }
}



[System.Serializable]
public class TestRefDataTable : CSVDataUtility.DataTable<TestRefDataEntry>{
    public static TestRefDataTable Create(){
        TestRefDataTable datatable = new TestRefDataTable();
        datatable.Read();
        return datatable;
    }
}

