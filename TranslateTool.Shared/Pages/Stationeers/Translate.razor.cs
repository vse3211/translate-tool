using System.Reflection.Metadata;
using System.Text;
using System.Xml;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using TranslateTool.Shared.Data.Stationeers;
using TranslateTool.Shared.Services;

namespace TranslateTool.Shared.Pages.Stationeers;

public partial class Translate : ComponentBase
{
    [Inject] IJSRuntime JSRuntime { get; set; }
    [Inject] IFormFactor FormFactor { get; set; }
    [Parameter]
    public required string MyLanguage { get; set; }
    
    [Parameter]
    public required State AppState { get; set; }

    [Parameter]
    public required Dictionary<string, TranslateContent>
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
            Localizations[MyLanguage].FileName = files.First().Name;

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
                    var node = Localizations[MyLanguage].sourceFile?
                        .SelectSingleNode(@$"//Language/{l.Key}/RecordReagent[Key='{r.Key}']");
                    node?.ParentNode?.RemoveChild(node);
                }
            }
        }
    }

    private async Task Save()
    {
        foreach (var l in Localizations[MyLanguage].records)
        {
            foreach (var r in l.Value)
            {
                switch (l.Key)
                {
                    case "Things":
                        var thing = Localizations[MyLanguage].sourceFile?
                            .SelectSingleNode(@$"//Language/{l.Key}/RecordThing[Key='{r.Key}']");
                        if (r.Value.IsValueChanged)
                        {
                            thing.ChildNodes[1].InnerText = r.Value.Value;
                            r.Value.SaveValue();
                        }
                        if (r.Value.IsDescriptionChanged)
                        {
                            thing.ChildNodes[2].InnerText = r.Value.Description;
                            r.Value.SaveDescription();
                        }
                        break;
                    case "Reagents":
                        var reagent = Localizations[MyLanguage].sourceFile?
                            .SelectSingleNode(@$"//Language/{l.Key}/RecordReagent[Key='{r.Key}']");
                        if (r.Value.IsValueChanged)
                        {
                            reagent.ChildNodes[1].InnerText = r.Value.Value;
                            r.Value.SaveValue();
                        }
                        if (r.Value.IsUnitChanged)
                        {
                            reagent.ChildNodes[2].InnerText = r.Value.Unit;
                            r.Value.SaveUnit();
                        }
                        break;
                    default:
                        var record = Localizations[MyLanguage].sourceFile?
                            .SelectSingleNode(@$"//Language/{l.Key}/Record[Key='{r.Key}']");
                        if (r.Value.IsValueChanged)
                        {
                            record.ChildNodes[1].InnerText = r.Value.Value;
                            r.Value.SaveValue();
                        }
                        break;
                }
            }
        }
        
        Localizations[MyLanguage].outFile = new MemoryStream();
        Localizations[MyLanguage].sourceFile?.Save(Localizations[MyLanguage].outFile);
        switch (FormFactor.GetFormFactor())
        {
            case "Desktop":
            case "Web":
                await JSRuntime.InvokeVoidAsync(
                    "saveFile",
                    Localizations[MyLanguage].FileName,
                    Encoding.UTF8.GetString(Localizations[MyLanguage].outFile.ToArray()));
                break;
            default:
                break;
        }
    }

    private bool CanShowRow(KeyValuePair<string, Dictionary<string, XML.UniversalRecord>> group, KeyValuePair<string, XML.UniversalRecord> val)
    {
        if (val.Key == "Corn")
        {
        }
        if (!Localizations[MyLanguage]._hideTranslated) return true;
        if (val.Value.IsDescriptionChanged ||
            val.Value.IsUnitChanged ||
            val.Value.IsValueChanged) return false;
        if (!String.IsNullOrWhiteSpace(AppState.MainLocalization))
        {
            if (Localizations[AppState.MainLocalization].records[group.Key].ContainsKey(val.Key))
            {
                if (Localizations[AppState.MainLocalization].records[group.Key][val.Key].Value !=
                    val.Value.Value)
                {
                    if (group.Key == "Things")
                    {
                        if (String.IsNullOrWhiteSpace(Localizations[AppState.MainLocalization].records[group.Key][val.Key]
                                .Description))
                        {
                            if (Localizations[MyLanguage]._hideEmpty) return false;
                        }
                        if (Localizations[AppState.MainLocalization].records[group.Key][val.Key].Description !=
                            val.Value.Description) return false;
                    }
                    if (group.Key == "Reagents")
                    {
                        if (String.IsNullOrWhiteSpace(
                                Localizations[AppState.MainLocalization].records[group.Key][val.Key].Unit))
                        {
                            if (Localizations[MyLanguage]._hideEmpty) return false;
                        }
                        if (Localizations[AppState.MainLocalization].records[group.Key][val.Key].Unit != val.Value.Unit) return false;
                    }

                    return false;
                }
            }
        }
        return true;
    }

    private void SelectWord(string group, string word)
    {
        AppState.SelectedWord.Group = group;
        AppState.SelectedWord.Word = word;
        StateHasChanged();
    }
}