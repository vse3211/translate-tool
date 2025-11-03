using Microsoft.AspNetCore.Components;
using TranslateTool.Shared.Data.Stationeers;
using System.Linq;

namespace TranslateTool.Shared.Pages.Stationeers;

public partial class Word : ComponentBase
{
    [Parameter]
    public required State AppState { get; set; }
    [Parameter]
    public required Dictionary<string, TranslateContent>
        Localizations { get; set; }

    private void Close()
    {
        AppState.SelectedWord.Group = null;
        AppState.SelectedWord.Word = null;
        StateHasChanged();
    }
}