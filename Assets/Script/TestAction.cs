using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestAction : MonoBehaviour
{
    public TestActionDataTable data;
    public TestActionDataEntry[] dataArray;

    private void Start()
    {
        data = TestActionDataTable.Create();
        dataArray = data.ValueArray;

        TestActionDataEntry.actiontest_action_tac1 = (s, i) => { Debug.Log("call1st, Name: " + s + ", age: " + i.ToString()); };
        TestActionDataEntry.actiontest_action_tac2 = (f) => { Debug.Log("call2nd, value: " + f.ToString()); };
    }

    [ContextMenu("Invoke First")]
    public void InvokeFirst()
    {
        dataArray[0].Invoke_actiontest();
    }

    [ContextMenu("Invoke Second")]
    public void InvokeSecond()
    {
        dataArray[1].Invoke_actiontest();
    }
}