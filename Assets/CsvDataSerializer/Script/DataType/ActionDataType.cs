using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CSVDataUtility.Action
{
    
    public class ActionDataType : IDataType, ICustomRefInfo
    {
        private List<ActionItem> actions;
        private string typeName;

        public ActionDataType(string prefix, string nesting)
        {
            typeName = prefix + ": " + nesting;
            ParseAllActions(nesting);   
        }
        
        private static readonly string actionMatchPattern = @"[\w\d_]+<[\w\d_;]*>";
        private void ParseAllActions(string actionListInfo)
        {
            var matches = Regex.Matches(actionListInfo, actionMatchPattern);
            actions = new List<ActionItem>(matches.Count);
            for (int i = 0; i < matches.Count; i++)
            {
                ParseAction(matches[i].Value, i);
            }
        }

        private void ParseAction(string actionNameAndParameters, int index)
        {
            string actionName;
            string parametersInfo;

            Helper.AnalyzeNestingTypeInfo(actionNameAndParameters, out actionName, out parametersInfo);
            var action = new ActionItem(Helper.CorrectHeadItemString(actionName), index, parametersInfo);
            actions.Add(action);
        }

        public string TypeName
        {
            get
            {
                return typeName;
            }
        }

        public string GetTypeNameForWriter(string variableName)
        {
            return "CSVDataUtility.Action.ActionInfo";
        }

        
        public string GetAdditionalInfoForWriter(string variableName)
        {
            ActionWriter writer = new ActionWriter(actions, variableName, variableName, variableName);
            return writer.GetAdditionalInfoForDataTypeWriter();
        }

        public string GetAdditionalInfoForRefType(string variableName, string refVariableName) {
            ActionWriter writer = new ActionWriter(actions, variableName, variableName, refVariableName);
            return writer.GetInvokeMethodDefinition();
        }


        public string GetExtensionMethodForWriter(string dataEntryName, string variableName)
        {
            return "";
        }


        public bool IsType(string csvTypeField)
        {
            // TODO: change this.
            return csvTypeField.Contains("action");
        }
        

        public object Deserialize(string rawItem, Type expectedType)
        {
            ActionInfo actionInfo = new ActionInfo();
            
            var matches = Regex.Matches(rawItem, actionMatchPattern);
            for(int i = 0; i < matches.Count; i++)
            {
                DeserializeActionItem(matches[i].Value, deserializeExtraInfo.variableName, actionInfo);
            }

            return actionInfo;
        }

        private void DeserializeActionItem(string actionItemAndParameterInfo, string variableName, ActionInfo actionInfo)
        {
            string actionItemName, parameters;
            Helper.AnalyzeNestingTypeInfo(actionItemAndParameterInfo, out actionItemName, out parameters);

            actionItemName = Helper.CorrectHeadItemString(actionItemName);
            var action = actions.Find((a) => a.key == actionItemName);
            if (action == null)
                throw new CSVParseException("Cannot find action: " + actionItemName);

            action.Deserialize(parameters, variableName, actionInfo);

        }

        public DataTypeDeserializeExtraInfo deserializeExtraInfo { get; set; }

        
    }
}