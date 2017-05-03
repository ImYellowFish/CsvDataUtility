# CsvDataUtility
https://github.com/ImYellowFish/CsvDataUtility.git

This library is a series of csv parsing functions made for Unity game data management. It can auto generate csharp files of data classes based on csv files, and deserialize csv files into these classes. Deserialized data can be accessed using class and variable name instead of string keys, which is very simple to use. Can parse base types including int, float, string, bool, enum, Vector3, and arrays that contain them.


# Usage

In your unity project view, right click on any csv files. In the pop-up menu, choose CsvDataUtility -> generate class for csv. If successful, a csharp file will be generated in Script/Data folder (refresh is required for Unity to find the file). The file will contain definition of two classes. CSVNAME_DataEntry and CSVNAME_DataTable.

To load data from csv, only one line of code is needed:
   ```
   var heroDataTable = CSVNAME_DataTable.create();
   ```

The dataTable will contain all the data from the corresponding csv file, which can be accessed via data keys:
   ```
   var heroJim = skillDataTable.GetEntry("Jim");
   Debug.Log ("hero's damage is: " + heroJim.damage);
   Debug.Log ("hero's skills are " + heroJim.skills[0].ToString() + heroJim.skills[1].ToString());
   ```
Note that most of the info, like csv file path and the structure of data, is saved during class generation phase. You don't need to worry about it when using the data class.

You should probably save the loaded dataTable in some DataManager class for later use. Repeat loading may cause performance issues.

To specify csv folder path and generated csharp file path, you can modify the CsvImportSetting.asset file under Resources folder.
You can further control the class names of generated csharp files, by adding additional rules in CsvImportSetting.

# Csv format

To use the library, your csv files must follow a few format rules:

- csv files should use ',' as delimeter
- First row of csv contains the field name.
- Second row contains the type info. viable types include:
    int, float, bool, string, enum, Vector3, array
- Data starts from third row, with the types defined in the second row.
- Array, enum and vector uses ';' as delimeter

Example csv:

```
ID,     Damage,   Skills,          Type,                    IsHuman,    Scale
string, float,    array<string>,   enum<warrior; civilian>, bool,       vector
Jim,    10,       chop;slash,      warrior,                 true,       1;1;2
PigKing,20,       eat;sleep,       civilian,                false,      5;5;5  

```

# Extension

You can implement your own data types by implementing IDataType interface. You also need to modify DataTypeFactory.cs to detect your customized data type.
