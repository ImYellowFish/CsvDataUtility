using System;
namespace CSVDataUtility.Action
{
    [Serializable]
    public class ActionParameter
    {
        public string serializedValue;
        public string typeInfo;

        public ActionParameter(string serializedValue, string typeInfo)
        {
            this.serializedValue = serializedValue;
            this.typeInfo = typeInfo;
        }

        [NonSerialized]
        private object boxedValue;
        public object Value
        {
            get
            {
                if (boxedValue == null)
                {
                    DeserializeValue();
                }
                return boxedValue;
            }
        }

        public void DeserializeValue() {
            var dataType = DataTypeFactory.GetBaseDataType(typeInfo);
            boxedValue = dataType.Deserialize(serializedValue, null);
        }
    }
}