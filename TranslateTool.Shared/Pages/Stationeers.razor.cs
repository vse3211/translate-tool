using System.Xml;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace TranslateTool.Shared.Pages;

public partial class Stationeers : ComponentBase
{
    protected override async Task OnInitializedAsync()
    {
        XmlDocument sourceFile = new XmlDocument();
        sourceFile.Load("source.xml");
        string json = JsonConvert.SerializeXmlNode(sourceFile);
    }
}