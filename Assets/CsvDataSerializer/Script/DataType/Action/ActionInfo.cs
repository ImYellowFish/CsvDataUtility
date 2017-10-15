using System.Collections.Generic;
namespace CSVDataUtility.Action
{
    [System.Serializable]
    public class ActionInfo
    {
        public List<int> callList = new List<int>();
        public List<ActionParameter> paramList = new List<ActionParameter>();
    }
}
