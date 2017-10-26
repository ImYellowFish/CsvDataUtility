using System.Collections.Generic;

namespace CSVDataUtility.Action
{
    public class ActionWriter
    {
        private string actionInfoName;
        private string variableName;
        private string actionPrefix;
        private List<ActionItem> actions;

        public ActionWriter(List<ActionItem> actions, string actionInfoName, string variableName, string actionPrefix)
        {
            this.actions = actions;
            this.actionInfoName = actionInfoName;
            this.variableName = variableName;
            this.actionPrefix = actionPrefix;
        }


        #region Writer methods
        public string GetAdditionalInfoForDataTypeWriter()
        {
            return GetActionsDeclaration() + GetInvokeMethodDefinition();
        }

        
        public string GetActionsDeclaration()
        {
            string result = "";
            for (int i = 0; i < actions.Count; i++)
            {
                var action = actions[i];
                result += ActionItemDeclarationTemplate.
                    Replace("ACTION_NAME", action.GetFieldName(actionPrefix)).
                    Replace("ACTION_PARAMETER_TYPES", GetActionParameterTypes(action)).
                    Replace("<>", "");  // case for zero parameters
            }
            return result;
        }

        public string GetInvokeMethodDefinition()
        {
            string invokeBody = "";
            for (int i = 0; i < actions.Count; i++)
            {
                var action = actions[i];
                invokeBody += GetInvokeMethodCase(action);
            }

            return InvokeMethodTemplate.
                Replace("VARIABLE_NAME", variableName).
                Replace("INVOKE_BODY", invokeBody).
                Replace("ACTION_INFO_NAME", actionInfoName);
        }

        private string GetActionParameterTypes(ActionItem action)
        {
            string result = "";
            for(int i = 0; i < action.parameterTypes.Count; i++)
            {
                result += action.parameterTypes[i].GetTypeNameForWriter(variableName);
                if (i < action.parameterCount - 1)
                    result += ", ";
            }
            return result;
        }

        private string GetInvokeMethodCase(ActionItem action)
        {
            string result = InvokeActionCaseTemplate
                .Replace("ACTION_INDEX", action.index.ToString())
                .Replace("ACTION_FIELD_NAME", action.GetFieldName(actionPrefix))
                .Replace("PARAM_COUNT", action.parameterCount.ToString())
                .Replace("ACTION_PARAMETERS", GetInvokeMethodParameters(action));
            return result;
        }

        private string GetInvokeMethodParameters(ActionItem action)
        {
            string result = "";
            for (int i = 0; i < action.parameterCount; i++)
            {
                var paramType = action.parameterTypes[i];
                result += InvokeActionParameterTemplate
                    .Replace("PARAM_TYPE", paramType.GetTypeNameForWriter(variableName))
                    .Replace("ACTION_INFO_NAME", actionInfoName)
                    .Replace("PARAM_OFFSET", i.ToString());
                if (i < action.parameterCount - 1)
                    result += ", ";
            }
            return result;
        }
        #endregion


        #region templates
        
        private static readonly string ActionItemDeclarationTemplate = @"
    [System.NonSerialized]
    public static System.Action<ACTION_PARAMETER_TYPES> ACTION_NAME = delegate { };
";
        private static readonly string InvokeMethodTemplate = @"
    public void Invoke_VARIABLE_NAME()
    {
        int paramIndex = 0;

        for (int i = 0; i < ACTION_INFO_NAME.callList.Count; i++)
        {
            switch (ACTION_INFO_NAME.callList[i])
            {
                INVOKE_BODY
            }
        }
    }
";
        private static readonly string InvokeActionCaseTemplate = @"
                case ACTION_INDEX:
                    ACTION_FIELD_NAME.Invoke(ACTION_PARAMETERS);
                    paramIndex += PARAM_COUNT;
                    break; 
";

        private static readonly string InvokeActionParameterTemplate = "(PARAM_TYPE)ACTION_INFO_NAME.paramList[paramIndex + PARAM_OFFSET].Value";
        #endregion

    }


    public class TestActionDataEntry
    {
        public ActionInfo ACTION_INFO_NAME = new ActionInfo();
        public static System.Action<string, int> static_action_name = delegate { };
        public static System.Action<int, float, string> static_action_2_name;

        public void InvokeActions()
        {
            int paramIndex = 0;

            for (int i = 0; i < ACTION_INFO_NAME.callList.Count; i++)
            {
                switch (ACTION_INFO_NAME.callList[i])
                {
                    case 1:
                        static_action_name.Invoke((string)ACTION_INFO_NAME.paramList[paramIndex].Value, (int)ACTION_INFO_NAME.paramList[paramIndex + 1].Value);
                        paramIndex += 2;
                        break;
                    case 2:
                        static_action_2_name.Invoke((int)ACTION_INFO_NAME.paramList[paramIndex].Value, (float)ACTION_INFO_NAME.paramList[paramIndex + 1].Value, (string)ACTION_INFO_NAME.paramList[paramIndex + 2].Value);
                        paramIndex += 3;
                        break;
                }
            }
        }

    }

}