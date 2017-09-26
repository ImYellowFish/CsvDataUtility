using UnityEngine;

public class TestData : MonoBehaviour
{
    public SkillDataTable skills;
    public SkillDataEntry[] skillList;

    public TestEnumDataTable enums;
    public TestEnumDataEntry[] enumList;

    // Use this for initialization
    void Start()
    {
        skills = SkillDataTable.Create();
        skillList = skills.ValueArray;

        enums = TestEnumDataTable.Create();
        enumList = enums.ValueArray;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
