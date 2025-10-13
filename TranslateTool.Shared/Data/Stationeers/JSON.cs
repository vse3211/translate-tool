namespace TranslateTool.Shared.Data.Stationeers;

public class JSON
{
    /// <summary>
    /// Root of file
    /// Warning! ?xml has been skipped! Add this manually
    /// </summary>
    public class Main
    {
        public required Language Language { get; set; }
    }

    /// <summary>
    /// Store all translation groups
    /// </summary>
    public class Language
    {
        public required Reagents Reagents { get; set; }
        public required Gases Gases { get; set; }
        public required Actions Actions { get; set; }
        public required Things Things { get; set; }
        public required Slots Slots { get; set; }
        public required Interactables Interactables { get; set; }
        public required Interface Interface { get; set; }
        public required Colors Colors { get; set; }
        public required Keys Keys { get; set; }
        public required Mineables Mineables { get; set; }
        public required GameStrings GameStrings { get; set; }
    }

    /// <summary>
    /// Basic class used for more eq. classes
    /// </summary>
    public class Basic
    {
        public required List<Record> Record { get; set; }
    }
    
    /// <summary>
    /// Used for store basic translation
    /// </summary>
    public class Record
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    /// <summary>
    /// Translates of Reagents
    /// </summary>
    public class Reagents
    {
        public required List<RecordReagents> RecordReagents { get; set; }
    }

    /// <summary>
    /// Used for store Reagents translation
    /// </summary>
    public class RecordReagents : Record
    {
        public string Unit { get; set; }
    }

    /// <summary>
    /// Translates of Gases
    /// </summary>
    public class Gases : Basic
    {
    }

    /// <summary>
    /// Translates of Actions
    /// </summary>
    public class Actions : Basic
    {
    }

    /// <summary>
    /// Translates of Things
    /// </summary>
    public class Things
    {
        public required List<RecordThing> RecordThing { get; set; }
    }

    /// <summary>
    /// Used for store Thing translation
    /// </summary>
    public class RecordThing : Record
    {
        public string Description { get; set; }
    }

    /// <summary>
    /// Translates of Slots
    /// </summary>
    public class Slots : Basic
    {
    }

    /// <summary>
    /// Translates of Interactables
    /// </summary>
    public class Interactables : Basic
    {
    }

    /// <summary>
    /// Translates of Interface
    /// </summary>
    public class Interface : Basic
    {
    }

    /// <summary>
    /// Translates of Colors
    /// </summary>
    public class Colors : Basic
    {
    }

    /// <summary>
    /// Translates of Keys
    /// </summary>
    public class Keys : Basic
    {
    }

    /// <summary>
    /// Translates of Mineables
    /// </summary>
    public class Mineables : Basic
    {
        public dynamic ScreenSpaceToolTips { get; set; }
        public dynamic HelpPage { get; set; }
        public dynamic ThingPageOverride { get; set; }
        public dynamic HomePageButtonsOverride { get; set; }
    }

    /// <summary>
    /// Translates of GameStrings
    /// </summary>
    public class GameStrings : Basic
    {
        public dynamic GameTip { get; set; }
    }
}