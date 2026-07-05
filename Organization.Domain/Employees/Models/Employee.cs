using Organization.Domain.Commons.BaseEntity;
using Organization.Domain.Commons.Utilities;

namespace Organization.Domain.Employees.Models
{
    [TableName("tblEmployees")]
    public sealed class Employee : IDbEntity
    {
        [PrimaryKey]
        [ColumnName("Id")]
        public string Id { get; set; } = ShortGuid.NewGuid();

        [DistinctUniqueKey]
        [ColumnName("Name")]
        public string? Name { get; set; }

        [ColumnName("Age")]
        public int Age { get; set; }

        [ColumnName("Position")]
        public string? Position { get; set; }

        [ForeignKey]
        [ColumnName("CompanyId")]
        public string? CompanyId { get; set; }

        [ColumnName("CreatedOn")]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [ColumnName("ModifiedOn")]
        public DateTime ModifiedOn { get; set; } = DateTime.Now;

        [ColumnName("Salary")]
        public Decimal Salary { get; set; }

        [ColumnName("IsDeleted")]
        public bool IsDeleted { get; set; }

    }
}
