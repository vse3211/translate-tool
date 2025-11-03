namespace TranslateTool.Shared.Data.Stationeers;

public class SharedFunctions
{
    public static string GetIsSavedStateStyle(Data.Stationeers.XML.UniversalRecord record, int valueType)
    {
        switch (valueType)
        {
            case 0:
                if (record.IsValueChanged) return IsNotSavedStateStyle;
                break;
            case 1:
                if (record.IsUnitChanged) return IsNotSavedStateStyle;
                break;
            case 2:
                if (record.IsDescriptionChanged) return IsNotSavedStateStyle;
                break;
        }
        return IsSavedStateStyle;
    }

    private const string IsSavedStateStyle = "border-color: black; border-width: 1px;";
    private const string IsNotSavedStateStyle = "border-color: palegreen; border-width: 2px;";
}