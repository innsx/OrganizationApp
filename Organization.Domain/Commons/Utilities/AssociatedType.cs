using System.Reflection;

namespace Organization.Domain.Commons.Utilities
{
    public class AssociatedType
    {
        public Type Type { get; }
        public PropertyInfo ForeignKeyProperty { get; }

        public AssociatedType(Type type, PropertyInfo foreignKeyProperty)
        {
            Type = type;
            ForeignKeyProperty = foreignKeyProperty;
        }
    }
}
