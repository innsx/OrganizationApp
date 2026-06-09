namespace Organization.Domain.Commons.Utilities
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TableNameAttribute : Attribute
    {
        // This attribute can be used to specify the table name for a class in a database.
        public string NameValue { get; }

        // Constructor to initialize the NameValue property.
        public TableNameAttribute(string nameValue)
        {
            NameValue = nameValue;
        }
    }
}
