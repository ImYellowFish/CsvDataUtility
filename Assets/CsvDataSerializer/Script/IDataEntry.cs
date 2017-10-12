using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CSVDataUtility
{
    public interface IDataEntry
    {
        string internal_dataEntryID { get; }
        int internal_dataEntryIndex { get; }
    }
}