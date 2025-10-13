using System.Xml;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Newtonsoft.Json;

namespace TranslateTool.Shared.Pages;

public partial class Stationeers : ComponentBase
{
    FluentInputFile? myFileByStream = default!;
    private bool isFileLoaded = false;
    private MemoryStream file = new();
    private string json = string.Empty;
    private Data.Stationeers.JSON.Main? translateFile = null;
    protected override async Task OnInitializedAsync()
    {
    }
    
    async Task OnFileUploadedAsync(FluentInputFileEventArgs inputFile)
    {
        file = new();
        await inputFile.Stream!.CopyToAsync(file);
        await inputFile.Stream!.DisposeAsync();
    }

    void OnCompleted(IEnumerable<FluentInputFileEventArgs> files)
    {
        try
        {
            file.Position = 0;
            XmlDocument sourceFile = new XmlDocument();
            sourceFile.Load(file);
            json = JsonConvert.SerializeXmlNode(sourceFile);
            translateFile = JsonConvert.DeserializeObject<Data.Stationeers.JSON.Main>(json);
            isFileLoaded = true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}