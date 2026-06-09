namespace Organization.Domain.Commons.Utilities
{
    // This attribute can be used to specify that a property is a primary key in a database table.
    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKeyAttribute : Attribute
    {
    }
}
