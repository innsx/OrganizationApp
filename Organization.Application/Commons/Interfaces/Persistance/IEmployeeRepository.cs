using Organization.Application.Commons.DTOs;
using Organization.Application.Commons.Utilities;
using Organization.Domain.Company;
using Organization.Domain.Company.Models;
using Organization.Domain.Employees;
using Organization.Domain.Employees.Models;

namespace Organization.Application.Commons.Interfaces.Persistance
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        public Task<PageList<EmployeeResponseDto>> GetEmployeesByQueryAsync(EmployeeQueryParameters employeeQueryParameters);

        public Task<EmployeeResponseDto> QueryOneToManyParentChildRelationshipAsync(string guid);
    }
}
