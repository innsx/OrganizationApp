using Dapper;
using Organization.Application.Commons.DTOs;
using Organization.Application.Commons.Interfaces.Persistance;
using Organization.Application.Commons.Utilities;
using Organization.Domain.Commons.Utilities;
using Organization.Domain.Employees;
using Organization.Domain.Employees.Models;
using Organization.Infrastructure.Persistance.DataContext;

namespace Organization.Infrastructure.Persistance.Repositories
{
    public sealed class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly DapperDataContext _dapperDataContext;
        public EmployeeRepository(DapperDataContext dapperDataContext) : base(dapperDataContext)
        {
            _dapperDataContext = dapperDataContext;
        }

        public async Task<PageList<EmployeeResponseDto>> GetEmployeesByQueryAsync(EmployeeQueryParameters employeeQueryParameters)
        {
            //using pass-in EmployeeQueryParameters & specified needed columns that we SPECIFIED in DTO EmployeeResponse 
            var employees = (await GetAsync(employeeQueryParameters, "Name", "Age", "Position", "Salary", "CreatedOn", "ModifiedOn", "CompanyId"))
                                .AsQueryable()
                                .Select(e => new EmployeeResponseDto(e.Name, e.Age, e.Position, e.Salary, e.CreatedOn, e.ModifiedOn, e.CompanyId));


            //DEMO only: manually hard-coded employees total Counts = 200000000; from tblEmployee table
            //OPTION: we can also do a CALL to "await GetTotalCountAsync();" to RETURN tblEmployee total counts,
            //because of the 2 millions records, its RESOURCES INTESIVE, we do not wanted that
            int employeesTotalCount = await GetTotalCountAsync();    //= 200000000;


            // we check if EmployeeQueryParameters FilterBy Property is NOT NULL or EMPTY
            if (!string.IsNullOrEmpty(employeeQueryParameters.FilterBy))
            {

                //NOT EMPTY, then Filter the returning Employees by Name
                employees = employees.Where(e => e.Name!.ToLowerInvariant()
                                                        .Contains(employeeQueryParameters.FilterBy.ToLowerInvariant())
                                              || e.Position!.ToLowerInvariant()
                                                        .Contains(employeeQueryParameters.FilterBy.ToLowerInvariant())
                                           
                                           );

                //string targetProperty = "Age";
                //employees = employees.Where(e => (int)e.GetType().GetProperty(targetProperty).GetValue(e)) == employeeQueryParameters.FilterBy;

            }


            if (!string.IsNullOrEmpty(employeeQueryParameters.SortBy))
            {
                if (typeof(Employee).GetProperty(employeeQueryParameters.SortBy) is not null)
                {
                    employees = employees.OrderByCustom(employeeQueryParameters.SortBy,
                                                        employeeQueryParameters.SortOrder);
                }
            }

            //this line will get CALL every time 
            //so this will create TRAFFICE Bottle-neck
            //SOLUTION: we can request it ONLY ONCE and
            //"CACHE" it and save the response in-memory,
            //so we can ACCESS the return reponse from in-memory instead
            //the PageList.cs STATIC Create( ) is REFERENCED
            var pagedEmployees = PageList<EmployeeResponseDto>.Create(employees, 
                                                                    employeeQueryParameters.PageNumber, 
                                                                    employeeQueryParameters.PageSize, 
                                                                    employeesTotalCount);

            return pagedEmployees;

        }

        //https://www.youtube.com/watch?v=rpBmUqrDH8M
        public async Task<EmployeeResponseDto> QueryOneToManyParentChildRelationshipAsync(string id)
        {

            string sql = @$" select e.Name, e.Age, c.Name as CompanyName, e.Position, e.Salary, e.CreatedOn
                                      From tblCompanies c
                                      inner join tblEmployees e
                                      on c.Id = e.CompanyId
                                      where e.Id = '{id}'";

            //Get connected thru DapperDataContext object & Dispose object after
            using var connection = _dapperDataContext.Connection;

            var employee = await connection!.QueryFirstOrDefaultAsync<EmployeeResponseDto>(sql, new { Id = id });

            return employee!;
        }

    }
}
