using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class TestRegexSpli : MonoBehaviour {
    public string original = "action1<param1,param2> action2<param1>";
    public string pattern = @"[\w\d_]+<[\w\d_;]+>";

    public string[] result;

    [ContextMenu("Parse")]
    public void Parse()
    {
        var matches = Regex.Matches(original, pattern);
        result = new string[matches.Count];
        for(int i = 0; i < matches.Count; i++)
        {
            result[i] = matches[i].Value;
        }
    }
}
