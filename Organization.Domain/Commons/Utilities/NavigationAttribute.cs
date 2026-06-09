namespace Organization.Domain.Commons.Utilities
{
    // This attribute can be used to specify that a property is a navigation property in a database table.
    [AttributeUsage(AttributeTargets.Property)]
    public class NavigationAttribute : Attribute
    {
        // This attribute can be used to specify that a property is a navigation property in a database table.
        public Type AssociatedType { get; }
        public string AssociatedProperty { get; } = string.Empty;

        // Constructor to initialize the NameValue property.
        public NavigationAttribute(Type associatedType, string associatedProperty)
        {
            AssociatedType = associatedType;
            AssociatedProperty = associatedProperty;
        }
    }
}
