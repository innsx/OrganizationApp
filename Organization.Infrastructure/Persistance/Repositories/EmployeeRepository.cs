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
        public EmployeeRepository(DapperDataContext dapperDataContext) : base(dapperDataContext)
        {
        }

        public async Task<PageList<EmployeeResponseDto>> GetEmployeesByQueryAsync(EmployeeQueryParameters employeeQueryParameters)
        {
            //using pass-in EmployeeQueryParameters & specified needed columns that we SPECIFIED in DTO EmployeeResponse 
            var employees = (await GetAsync(employeeQueryParameters, "Name", "Age", "Position", "Salary"))
                                .AsQueryable()
                                .Select(e => new EmployeeResponseDto  //manually converting EmployeeResponse object
                                {
                                    //mapping the PROPERTIES
                                    Name = e.Name,
                                    Age = e.Age,
                                    Position = e.Position,
                                    Salary = e.Salary,
                                }); //in future, we will use MAPPester tool to AUTOMATIC converting an OBJECT into another OBJECT


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

    }
}
