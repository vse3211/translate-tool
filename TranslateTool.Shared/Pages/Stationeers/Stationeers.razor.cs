using Microsoft.AspNetCore.Components;

namespace TranslateTool.Shared.Pages.Stationeers;

public partial class Stationeers : ComponentBase
{
    private Data.Stationeers.State AppState = new();
    private Dictionary<string, Data.Stationeers.TranslateContent>
        _localizations = new();
    
}