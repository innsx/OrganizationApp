using Organization.Domain.Commons.BaseEntity;
using Organization.Domain.Commons.Utilities;
using Organization.Domain.Employees.Models;

namespace Organization.Domain.Company.Models
{
    [TableName("tblCompanies")]
    public sealed class Company : IDbEntity
    {
        [PrimaryKey]   
        [ColumnName("Id")]
        public string Id { get; set; } = ShortGuid.NewGuid();

        [DistinctUniqueKey]
        [ColumnName("Name")]
        public string? Name { get; set; }


        [ColumnName("Address")]
        public string? Address { get; set; }


        [ColumnName("Country")]
        public string? Country { get; set; }


        [ColumnName("IsDeleted")]
        public bool IsDeleted { get; set; }

        //1:N relationship: 1 Company has MANY Employees
        [Navigation(typeof(Employee), "CompanyId")]
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
