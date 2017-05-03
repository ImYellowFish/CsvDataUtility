using System.Collections.Generic;
[System.Serializable]
[CSVFilename("Skill1")]
public class SkillDataEntry{

	[CSVField("ID")]
	public string id;

	[CSVField("Interrupt")]
	public bool interrupt;

	[CSVField("Action")]
	public string action;

	[CSVField("1Distance Condition")]
	public float _1distance_condition;

	[CSVField("Distance")]
	public float distance;

	[CSVField("Sing")]
	public float sing;

	[CSVField("Front Time")]
	public float front_time;

	[CSVField("Hit Time")]
	public float hit_time;

	[CSVField("Post Time")]
	public float post_time;

	[CSVField("Hurt")]
	public float hurt;

	[CSVField("Priority")]
	public int priority;

	[CSVField("Attack Type")]
	public List<string> attack_type;

	[CSVField("Type ID")]
	public List<string> type_id;

	[CSVField("HasCombo")]
	public bool hascombo;

	[CSVField("Combo ID")]
	public string combo_id;

	[CSVField("Reduce Endurance")]
	public int reduce_endurance;

	[CSVField("Release Effect")]
	public List<string> release_effect;

	[CSVField("Mirror")]
	public bool mirror;

	[CSVField("Release Sound")]
	public string release_sound;

	[CSVField("Gethit  Effect")]
	public List<string> gethit__effect;

	[CSVField("Gethit  Effect Rotate")]
	public List<int> gethit__effect_rotate;

	[CSVField("Gethit  Sound")]
	public string gethit__sound;

	[CSVField("Icon")]
	public string icon;

	[CSVField("Name")]
	public string name;

	[CSVField("Describe")]
	public string describe;


}

[System.Serializable]
public class SkillDataTable : CSVDataUtility.DataTable<SkillDataEntry>{
    public static SkillDataTable Create(){
        SkillDataTable datatable = new SkillDataTable();
        datatable.Read();
        return datatable;
    }
}
