using UnityEngine;

public class TestData : MonoBehaviour
{
    public SkillDataTable skills;
    public SkillDataEntry[] skillList;

    public TestEnumDataTable enums;
    public TestEnumDataEntry[] enumList;

    public TestRefDataTable refs;
    public TestRefDataEntry[] refList;


    // Use this for initialization
    void Start()
    {
        skills = SkillDataTable.Create();
        skillList = skills.ValueArray;

        enums = TestEnumDataTable.Create();
        enumList = enums.ValueArray;

        refs = TestRefDataTable.Create();
        refList = refs.ValueArray;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
