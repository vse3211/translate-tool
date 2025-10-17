using Microsoft.AspNetCore.Components;

namespace TranslateTool.Shared.Pages.Stationeers;

public partial class Stationeers : ComponentBase
{
    private string _mainLocalization = "";
    private string _selectedWord = "";
    private Dictionary<string, Data.Stationeers.ComponentContent>
        _localizations = new();
    
}