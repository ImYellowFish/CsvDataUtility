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

	[CSVField("original enum")]
	public original_enum_values original_enum;

	public enum original_enum_values { a = 0, b = 1, c = 2, }

	[CSVField("refer enum")]
	public original_enum_values refer_enum;

	[CSVField("original actiong")]
	public CSVDataUtility.Action.ActionInfo original_actiong;

    [System.NonSerialized]
    public static System.Action<string> original_actiong_action_call = delegate { };

    [System.NonSerialized]
    public static System.Action<int, int> original_actiong_action_dial = delegate { };

    public void Invoke_original_actiong()
    {
        int paramIndex = 0;

        for (int i = 0; i < original_actiong.callList.Count; i++)
        {
            switch (original_actiong.callList[i])
            {
                
                case 0:
                    original_actiong_action_call.Invoke((string)original_actiong.paramList[paramIndex + 0].Value);
                    paramIndex += 1;
                    break; 

                case 1:
                    original_actiong_action_dial.Invoke((int)original_actiong.paramList[paramIndex + 0].Value, (int)original_actiong.paramList[paramIndex + 1].Value);
                    paramIndex += 2;
                    break; 

            }
        }
    }

	[CSVField("refer actiong")]
	public CSVDataUtility.Action.ActionInfo refer_actiong;

    public void Invoke_refer_actiong()
    {
        int paramIndex = 0;

        for (int i = 0; i < refer_actiong.callList.Count; i++)
        {
            switch (refer_actiong.callList[i])
            {
                
                case 0:
                    original_actiong_action_call.Invoke((string)refer_actiong.paramList[paramIndex + 0].Value);
                    paramIndex += 1;
                    break; 

                case 1:
                    original_actiong_action_dial.Invoke((int)refer_actiong.paramList[paramIndex + 0].Value, (int)refer_actiong.paramList[paramIndex + 1].Value);
                    paramIndex += 2;
                    break; 

            }
        }
    }



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

