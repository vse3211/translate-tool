﻿using System.Xml;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Newtonsoft.Json;
using TranslateTool.Shared.Data.Stationeers;

namespace TranslateTool.Shared.Pages.Stationeers;

public partial class Translate : ComponentBase
{
    [Parameter]
    public required string MyLanguage { get; set; }
    
    [Parameter]
    public required State AppState { get; set; }

    [Parameter]
    public required Dictionary<string, ComponentContent>
        Localizations { get; set; }

    async Task OnFileUploadedAsync(FluentInputFileEventArgs inputFile)
    {
        Localizations[MyLanguage].file = new();
        await inputFile.Stream!.CopyToAsync(Localizations[MyLanguage].file);
        await inputFile.Stream!.DisposeAsync();
    }

    void OnCompleted(IEnumerable<FluentInputFileEventArgs> files)
    {
        try
        {
            Localizations[MyLanguage].records.Clear();
            Localizations[MyLanguage].file.Position = 0;
            Localizations[MyLanguage].sourceFile = new XmlDocument();
            Localizations[MyLanguage].sourceFile?.Load(Localizations[MyLanguage].file);

            var nodes = Localizations[MyLanguage].sourceFile?.ChildNodes.GetEnumerator();
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
                                        if (!Localizations[MyLanguage].records.ContainsKey(((XmlElement)nE.Current).Name))
                                        {
                                            Localizations[MyLanguage].records.Add(((XmlElement)nE.Current).Name, new());
                                            Localizations[MyLanguage]._hideGroups.Add(((XmlElement)nE.Current).Name, true);
                                        }
                                        nE_Exists = true;
                                    }
                                    switch (((XmlElement)nD.Current).Name)
                                    {
                                        case "RecordReagent":
                                            var v0 = ((XmlElement)nD.Current).ChildNodes[0]?.InnerXml;
                                            var v1 = ((XmlElement)nD.Current).ChildNodes[1]?.InnerXml;
                                            var v2 = ((XmlElement)nD.Current).ChildNodes[2]?.InnerXml;
                                            if (v0 is not null && !Localizations[MyLanguage].records[((XmlElement)nE.Current).Name]
                                                .ContainsKey(v0))
                                            {
                                                Localizations[MyLanguage].records[((XmlElement)nE.Current).Name].Add(v0, new XML.UniversalRecord(v0, v1, v2));
                                            }
                                            break;
                                        case "Record":
                                            var vr0 = ((XmlElement)nD.Current).ChildNodes[0]?.InnerXml;
                                            var vr1 = ((XmlElement)nD.Current).ChildNodes[1]?.InnerXml;
                                            if (vr0 is not null && !Localizations[MyLanguage].records[((XmlElement)nE.Current).Name]
                                                    .ContainsKey(vr0))
                                            {
                                                Localizations[MyLanguage].records[((XmlElement)nE.Current).Name].Add(vr0, new XML.UniversalRecord(vr0, vr1));
                                            }
                                            break;
                                        case "RecordThing":
                                            var vt0 = ((XmlElement)nD.Current).ChildNodes[0]?.InnerXml;
                                            var vt1 = ((XmlElement)nD.Current).ChildNodes[1]?.InnerXml;
                                            var vt2 = ((XmlElement)nD.Current).ChildNodes[2]?.InnerXml;
                                            if (vt0 is not null && !Localizations[MyLanguage].records[((XmlElement)nE.Current).Name]
                                                    .ContainsKey(vt0))
                                            {
                                                Localizations[MyLanguage].records[((XmlElement)nE.Current).Name].Add(vt0, new XML.UniversalRecord(vt0, vt1, Description:vt2));
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

            string newLanguage = Localizations[MyLanguage].sourceFile?.SelectSingleNode("//Language/Name")?.InnerText ?? "Undefined";

            Localizations[MyLanguage].isFileLoaded = true;
            while (Localizations.ContainsKey(MyLanguage))
            {
                Localizations.Add(newLanguage, Localizations[MyLanguage]);
                Localizations.Remove(MyLanguage);
            }

            MyLanguage = newLanguage;
            
            // outFile = new();
            // sourceFile.Save(outFile);

            //json = JsonConvert.SerializeXmlNode(sourceFile);
            //translateFile = JsonConvert.DeserializeObject<Data.Stationeers.JSON.Main>(json);
            
            StateHasChanged();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private void TrimUnusedValues()
    {
        foreach (var l in Localizations[MyLanguage].records)
        {
            foreach (var r in l.Value)
            {
                if (!Localizations[AppState.MainLocalization].records[l.Key].ContainsKey(r.Key))
                {
                    l.Value.Remove(r.Key);
                    var node = Localizations[MyLanguage].sourceFile
                        .SelectSingleNode(@$"//Language/{l.Key}/RecordReagent[Key='{r.Key}']");
                    node.ParentNode.RemoveChild(node);
                }
            }
        }
    }

    private string getIsSavedStateStyle(Data.Stationeers.XML.UniversalRecord record, int valueType)
    {
        switch (valueType)
        {
            case 0:
                if (record.IsValueChanged) return _isNotSavedStateStyle;
                break;
            case 1:
                if (record.IsUnitChanged) return _isNotSavedStateStyle;
                break;
            case 2:
                if (record.IsDescriptionChanged) return _isNotSavedStateStyle;
                break;
        }
        return _isSavedStateStyle;
    }

    private string _isSavedStateStyle = "border-color: black; border-width: 1px;";
    private string _isNotSavedStateStyle = "border-color: palegreen; border-width: 2px;";
}