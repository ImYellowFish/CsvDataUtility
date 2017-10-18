using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStruct : MonoBehaviour {

    public TestStructDataTable structTest;
    public TestStructDataEntry[] structArray;

	// Use this for initialization
	void Start () {
        structTest = TestStructDataTable.Create();
        structArray = structTest.ValueArray;
	}
	
    
}
