using System.Collections.Generic;
[System.Serializable]
[CSVFilename("Skill1")]
public class SkillDataEntry{

	[CSVField("id")]
	public string id;

	[CSVField("interrupt")]
	public bool interrupt;

	[CSVField("action")]
	public string action;

	[CSVField("1distance condition")]
	public float _1distance_condition;

	[CSVField("distance")]
	public float distance;

	[CSVField("sing")]
	public float sing;

	[CSVField("front time")]
	public float front_time;

	[CSVField("hit time")]
	public float hit_time;

	[CSVField("post time")]
	public float post_time;

	[CSVField("hurt")]
	public float hurt;

	[CSVField("priority")]
	public int priority;

	[CSVField("attack type")]
	public List<string> attack_type;

	[CSVField("type id")]
	public List<string> type_id;

	[CSVField("hascombo")]
	public bool hascombo;

	[CSVField("combo id")]
	public string combo_id;

	[CSVField("reduce endurance")]
	public int reduce_endurance;

	[CSVField("release effect")]
	public List<string> release_effect;

	[CSVField("mirror")]
	public bool mirror;

	[CSVField("release sound")]
	public string release_sound;

	[CSVField("gethit  effect")]
	public List<string> gethit__effect;

	[CSVField("gethit  effect rotate")]
	public List<int> gethit__effect_rotate;

	[CSVField("gethit  sound")]
	public string gethit__sound;

	[CSVField("icon")]
	public string icon;

	[CSVField("name")]
	public string name;

	[CSVField("describe")]
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