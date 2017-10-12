using System.Reflection;
using System;

[AttributeUsage(AttributeTargets.Field)]
public class CSVFieldAttribute : Attribute {
    private string csvFieldName;

    public CSVFieldAttribute(string csvFieldName) {
        this.csvFieldName = csvFieldName;
    }

    public static string GetCsvFieldName(MemberInfo m) {
        object[] attributes = m.GetCustomAttributes(true);
        for (int i = 0; i < attributes.Length; i++) {
            if (attributes[i] is CSVFieldAttribute) {
                return ((CSVFieldAttribute) attributes[i]).csvFieldName;
            }
        }
        return null;
    }
}


[AttributeUsage(AttributeTargets.Class)]
public class CSVFilenameAttribute : Attribute {
    private string csvFilename;

    public CSVFilenameAttribute(string csvFilename) {
        this.csvFilename = csvFilename;
    }

    public static string GetCsvFilename(Type m) {
        object[] attributes = m.GetCustomAttributes(true);
        for (int i = 0; i < attributes.Length; i++) {
            if (attributes[i] is CSVFilenameAttribute) {
                return ((CSVFilenameAttribute) attributes[i]).csvFilename;
            }
        }
        return null;
    }

    public static string GetCsvFilename(object target) {
        return GetCsvFilename(target.GetType());
    }
}

[AttributeUsage(AttributeTargets.Class)]
public class CSVDataAssetAttribute : Attribute
{
    private string dataTableName;

    public CSVDataAssetAttribute(string dataTableName)
    {
        this.dataTableName = dataTableName;
    }

    public static string GetDataTableName(Type m)
    {
        object[] attributes = m.GetCustomAttributes(true);
        for (int i = 0; i < attributes.Length; i++)
        {
            if (attributes[i] is CSVDataAssetAttribute)
            {
                return ((CSVDataAssetAttribute)attributes[i]).dataTableName;
            }
        }
        return null;
    }

    public static string GetDataTableName(object target)
    {
        return GetDataTableName(target.GetType());
    }
}

[AttributeUsage(AttributeTargets.Field)]
public class CSVInternalIndexAttribute : Attribute
{
    public static bool IsInternalIndex(MemberInfo m)
    {
        object[] attributes = m.GetCustomAttributes(true);
        for (int i = 0; i < attributes.Length; i++)
        {
            if (attributes[i] is CSVInternalIndexAttribute)
            {
                return true;
            }
        }
        return false;
    }
}
