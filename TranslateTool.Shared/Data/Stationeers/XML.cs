namespace TranslateTool.Shared.Data.Stationeers;

public class XML
{
    public class UniversalRecord
    {
        public UniversalRecord(string Key, string? Value, string? Unit = null, string? Description = null)
        {
            this.Key = Key;
            
            _savedValue = Value;
            _currentValue = Value;
            
            _savedUnit =  Unit;
            _currentUnit = Unit;
            
            _savedDescription = Description;
            _currentDescription = Description;
        }
        
        /// <summary>
        /// Basic Key value
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Basic translation value
        /// </summary>
        public string? Value
        {
            get => _currentValue;
            set => _currentValue = value;
        }
        /// <summary>
        /// Can be null if unused! Used in reagents
        /// </summary>
        public string? Unit
        {
            get => _currentUnit;
            set => _currentUnit =  value;
        }

        /// <summary>
        /// Can be null if unused! Used in things
        /// </summary>
        public string? Description
        {
            get => _currentDescription;
            set => _currentDescription = value;
        }

        /// <summary>
        /// Changed state for
        /// </summary>
        public bool IsValueChanged => _savedValue != _currentValue;
        
        /// <summary>
        /// Changed state for
        /// </summary>
        public bool IsUnitChanged => _savedUnit != _currentUnit;
        
        /// <summary>
        /// Changed state for
        /// </summary>
        public bool IsDescriptionChanged => _savedDescription != _currentDescription;
        
        /// <summary>
        /// Make Value state is saved
        /// </summary>
        public void SaveValue() => _savedValue = _currentValue;
        // Saved (init) value and current value of Value
        private string? _savedValue;
        private string? _currentValue;
        
        /// <summary>
        /// Make Unit state is saved
        /// </summary>
        public void SaveUnit() => _savedUnit = _currentUnit;
        // Saved (init) value and current value of Unit
        private string? _savedUnit;
        private string? _currentUnit;
        
        /// <summary>
        /// Make Description state is saved
        /// </summary>
        public void SaveDescription() => _savedDescription = _currentDescription;
        // Saved (init) value and current value of Description
        private string? _savedDescription;
        private string? _currentDescription;
    }
}