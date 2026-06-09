using Organization.Application.Commons.Interfaces.Persistance;
using Organization.Domain.Employees.Models;
using Organization.Infrastructure.Persistance.DataContext;

namespace Organization.Infrastructure.Persistance.Repositories
{
    public sealed class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(DapperDataContext dapperDataContext) : base(dapperDataContext)
        {
        }
    }
}
