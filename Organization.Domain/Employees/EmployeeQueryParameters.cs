using Organization.Domain.Commons.Utilities;

namespace Organization.Domain.Employees
{
    public sealed class EmployeeQueryParameters : QueryParameters
    {
        public string Name { get; set; } = string.Empty;
    }
}
