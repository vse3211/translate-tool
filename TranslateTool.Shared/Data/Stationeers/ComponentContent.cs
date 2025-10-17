using System.Xml;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using TranslateTool.Shared.Data.Stationeers;

namespace TranslateTool.Shared.Data.Stationeers;

public class ComponentContent
{
    public FluentInputFile? myFileByStream = default!;
    public bool isFileLoaded = false;
    public MemoryStream file = new();
    public MemoryStream outFile = new();
    public XmlDocument? sourceFile = null;
    public Dictionary<string, Dictionary<string, Data.Stationeers.XML.UniversalRecord>> records { get; set; } = new();
    public Dictionary<string, bool> _hideGroups = new();
    public bool _hideCounts = true;
    public bool _hideTranslated = false;
}