namespace Organization.Domain.Commons.Utilities
{
    // This attribute can be used to specify the column name for a property in a database table.
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnNameAttribute : Attribute
    {
        // This attribute can be used to specify the column name for a property in a database table.
        public string NameValue { get; }

        // Constructor to initialize the NameValue property.
        public ColumnNameAttribute(string nameValue)
        {
            NameValue = nameValue;
        }
    }
}
