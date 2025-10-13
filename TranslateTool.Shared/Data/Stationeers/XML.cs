namespace TranslateTool.Shared.Data.Stationeers;

public class XML
{
    public class UniversalRecord
    {
        /// <summary>
        /// Basic Key value
        /// </summary>
        public required string Key { get; set; }
        /// <summary>
        /// Basic translation value
        /// </summary>
        public required string Value { get; set; }
        /// <summary>
        /// Can be null if unused! Used in reagents
        /// </summary>
        public string? Unit { get; set; }
        /// <summary>
        /// Can be null if unused! Used in things
        /// </summary>
        public string? Description { get; set; }
    }
}