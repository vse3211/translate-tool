using Microsoft.AspNetCore.Components;

namespace TranslateTool.Shared.Pages.Stationeers;

public partial class Stationeers : ComponentBase
{
    private Data.Stationeers.State AppState = new();
    private Dictionary<string, Data.Stationeers.ComponentContent>
        _localizations = new();
    
}