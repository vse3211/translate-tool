using System.Collections;
using System.Xml;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Newtonsoft.Json;
using TranslateTool.Shared.Data.Stationeers;

namespace TranslateTool.Shared.Pages.Stationeers;

public partial class Stationeers : ComponentBase
{
    FluentInputFile? myFileByStream = default!;
    private bool isFileLoaded = false;
    private MemoryStream file = new();
    private MemoryStream outFile = new();
    private string json = string.Empty;
    private Data.Stationeers.JSON.Main? translateFile = null;
    private XmlDocument? sourceFile = null;
    private Dictionary<string, Dictionary<string, Data.Stationeers.XML.UniversalRecord>> records = new();
    private Dictionary<string, bool> _hideGroups = new();
    private bool _hideCounts = true;
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
            records.Clear();
            file.Position = 0;
            sourceFile = new XmlDocument();
            sourceFile.Load(file);

            var nodes = sourceFile.ChildNodes.GetEnumerator();
            while (nodes.MoveNext()) 
            {
                if (nodes.Current is not null)
                {
                    //if (typeof(System.Xml.XmlChildEnumerator) == nodes.Current.GetType())
                    var n = nodes.Current;
                    if (n.GetType() == typeof(XmlElement) && ((XmlElement)n).HasChildNodes)
                    {
                        var nE = ((XmlElement)n).ChildNodes.GetEnumerator();
                        bool nE_Exists = false;
                        while (nE.MoveNext())
                        {
                            if (nE.Current.GetType() == typeof(XmlElement) && ((XmlElement)nE.Current).HasChildNodes)
                            {
                                var nD = ((XmlElement)nE.Current).ChildNodes.GetEnumerator();
                                while (nD.MoveNext())
                                {
                                    var type = nD.Current.GetType().ToString();
                                    if (nD.Current.GetType() == typeof(XmlText)) break;
                                    if (!nE_Exists)
                                    {
                                        if (!records.ContainsKey(((XmlElement)nE.Current).Name))
                                        {
                                            records.Add(((XmlElement)nE.Current).Name, new());
                                            _hideGroups.Add(((XmlElement)nE.Current).Name, true);
                                        }
                                        nE_Exists = true;
                                    }
                                    switch (((XmlElement)nD.Current).Name)
                                    {
                                        case "RecordReagent":
                                            var v0 = ((XmlElement)nD.Current).ChildNodes[0]?.InnerXml;
                                            var v1 = ((XmlElement)nD.Current).ChildNodes[1]?.InnerXml;
                                            var v2 = ((XmlElement)nD.Current).ChildNodes[2]?.InnerXml;
                                            if (v0 is not null && !records[((XmlElement)nE.Current).Name]
                                                .ContainsKey(v0))
                                            {
                                                records[((XmlElement)nE.Current).Name].Add(v0, new XML.UniversalRecord(v0, v1, v2));
                                            }
                                            break;
                                        case "Record":
                                            var vr0 = ((XmlElement)nD.Current).ChildNodes[0]?.InnerXml;
                                            var vr1 = ((XmlElement)nD.Current).ChildNodes[1]?.InnerXml;
                                            if (vr0 is not null && !records[((XmlElement)nE.Current).Name]
                                                    .ContainsKey(vr0))
                                            {
                                                records[((XmlElement)nE.Current).Name].Add(vr0, new XML.UniversalRecord(vr0, vr1));
                                            }
                                            break;
                                        case "RecordThing":
                                            var vt0 = ((XmlElement)nD.Current).ChildNodes[0]?.InnerXml;
                                            var vt1 = ((XmlElement)nD.Current).ChildNodes[1]?.InnerXml;
                                            var vt2 = ((XmlElement)nD.Current).ChildNodes[2]?.InnerXml;
                                            if (vt0 is not null && !records[((XmlElement)nE.Current).Name]
                                                    .ContainsKey(vt0))
                                            {
                                                records[((XmlElement)nE.Current).Name].Add(vt0, new XML.UniversalRecord(vt0, vt1, Description:vt2));
                                            }
                                            break;
                                    }
                                }
                            }
                            nE_Exists = false;
                        }
                    }
                }
            }
            
            
            
            // outFile = new();
            // sourceFile.Save(outFile);

            json = JsonConvert.SerializeXmlNode(sourceFile);
            translateFile = JsonConvert.DeserializeObject<Data.Stationeers.JSON.Main>(json);
            isFileLoaded = true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private string getIsSavedStateStyle(Data.Stationeers.XML.UniversalRecord record, int valueType)
    {
        switch (valueType)
        {
            case 0:
                if (record.IsValueChanged) return _isSavedStateStyle;
                break;
            case 1:
                if (record.IsUnitChanged) return _isSavedStateStyle;
                break;
            case 2:
                if (record.IsDescriptionChanged) return _isSavedStateStyle;
                break;
        }
        return _isNotSavedStateStyle;
    }

    private string _isSavedStateStyle = "border-color: black; border-width: 1px;";
    private string _isNotSavedStateStyle = "border-color: palegreen; border-width: 2px;";
}