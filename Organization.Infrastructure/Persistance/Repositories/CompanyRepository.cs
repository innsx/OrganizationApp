using Organization.Application.Commons.DTOs;
using Organization.Application.Commons.Interfaces.Persistance;
using Organization.Application.Commons.Utilities;
using Organization.Domain.Commons.Utilities;
using Organization.Domain.Company;
using Organization.Domain.Company.Models;
using Organization.Domain.Employees.Models;
using Organization.Infrastructure.Persistance.DataContext;
using static Dapper.SqlMapper;

namespace Organization.Infrastructure.Persistance.Repositories
{
    public sealed class CompanyRepository : GenericRepository<Company>, ICompanyRepository
    {
        private readonly DapperDataContext _dapperDataContext;
        public CompanyRepository(DapperDataContext dapperDataContext) : base(dapperDataContext)
        {
            _dapperDataContext = dapperDataContext;
        }

        public async Task<PageList<CompanyResponseDto>> GetCompaniesByQueryAsync(CompanyQueryParameters companyQueryParameters)
        {
            //using pass-in EmployeeQueryParameters & specified needed columns that we SPECIFIED in DTO EmployeeResponse 
            var companies = (await GetAsync(companyQueryParameters, "Name", "Address", "Country"))
                                .AsQueryable()
                                .Select(e => new CompanyResponseDto  //manually converting EmployeeResponse object
                                {
                                    //mapping the PROPERTIES
                                    Name = e.Name,
                                    Address = e.Address,
                                    Country = e.Country
                                }); //in future, we will use MAPPester tool to AUTOMATIC converting an OBJECT into another OBJECT

            //manually hard-coded company total Counts from tblCompany table for DEMO only
            //OPTION: we can also do a CALL to a method to RETURN tblCompany total counts,
            //but its RESOURCES INTESIVE, we do not wanted that
            int companyTotalCount = await GetTotalCountAsync();  //will use spGetTotalRecordsCount in GenericRepository.cs to return the counts


            // we check if EmployeeQueryParameters FilterBy Property is NOT NULL or EMPTY
            if (!string.IsNullOrEmpty(companyQueryParameters.FilterBy))
            {
                //NOT EMPTY, then Filter the returning Employees by Name
                companies = companies.Where(e => e.Name!.ToLowerInvariant()
                                                        .Contains(companyQueryParameters.FilterBy.ToLowerInvariant())
                                               || e.Address!.ToLowerInvariant()
                                                        .Contains(companyQueryParameters.FilterBy.ToLowerInvariant())
                                               || e.Country!.ToLowerInvariant()
                                                        .Contains(companyQueryParameters.FilterBy.ToLowerInvariant())
                                           );
            }

            if (!string.IsNullOrEmpty(companyQueryParameters.SortBy))
            {
                if (typeof(Employee).GetProperty(companyQueryParameters.SortBy) is not null)
                {
                    companies = companies.OrderByCustom(companyQueryParameters.SortBy,
                                                        companyQueryParameters.SortOrder);
                }
            }

            //this line will get CALL every time 
            //so this will create TRAFFICE Bottle-neck
            //SOLUTION: we can request it ONLY ONCE and
            //"CACHE" it and save the response in-memory,
            //so we can ACCESS the return reponse from in-memory instead
            //the PageList.cs STATIC Create( ) is REFERENCED 
            var pagedCompany = PageList<CompanyResponseDto>.Create(companies,
                                                                    companyQueryParameters.PageNumber,
                                                                    companyQueryParameters.PageSize,
                                                                    companyTotalCount);

            return pagedCompany;

        }


        //https://www.youtube.com/watch?v=rpBmUqrDH8M
        public async Task<ICollection<Company>> QueryOneToManyParentChildRelationshipAsync(string guid)
        {

            string sql = @$" select *
                                      From tblCompanies c
                                      inner join tblEmployees e
                                      on c.Id = e.CompanyId
                                      where c.Id = '{guid}'";

            // Dictionary tracks parents we've already created/seen
            var companyDictionary = new Dictionary<string, Company>();

            //Get connected thru DapperDataContext object & Dispose object after
            using var connection = _dapperDataContext.Connection;

            IEnumerable<Company> companies = await connection!.QueryAsync<Company, Employee, Company>(
                 sql,
                (company, employee) =>
                {
                    // 1. Check if the parent company is already in our dictionary
                    if (!companyDictionary.TryGetValue(company.Id, out var currentCompany))
                    {
                        currentCompany = company;
                        currentCompany.Employees = new List<Employee>();
                        companyDictionary.Add(company.Id, currentCompany);
                    }

                    // 2. Add the related child to the parent's collection
                    if (employee.Id != null)
                    {
                        currentCompany.Employees.Add(employee);
                    }

                    return currentCompany;
                },

                // Adjust if your child table's primary key has a different name as a Primary Key
                splitOn: "Id"

                );

            // Return the distinct parent record
            var companyList = companyDictionary.Values.ToList();

            return companyList;
        }

    }
}




//public async Task<IEnumerable<Company>> QueryJoinTablesAsync(string guid)
//{

//    string sql = @" select *
//                     From tblCompanies c
//                     select *
//                     from tblEmployees e
//                     ";

//    using var connection = _dapperDataContext.Connection;

//    using (var multi = await connection.QueryMultipleAsync(sql))
//    {
//        // Read the tables sequentially in the order they are queried
//        var companies = (await multi.ReadAsync<Company>()).ToList();
//        var employees = (await multi.ReadAsync<Employee>()).ToList();

//        // Map the relationships manually in C# memory
//        foreach (var company in companies)
//        {
//            company.Employees = employees.Where(l => l.CompanyId == company.Id).ToList();
//        }
//    }
//}



//public async Task<Company> QueryJoinTablesAsync(string guid)
//{

//    var sql = @"
//                    SELECT * FROM tblCompanies WHERE Id = @Id;";

//    var sql1 = @"SELECT * FROM tblEmployees WHERE CompanyId = @Id;";

//    using var multi = await _dapperDataContext.Connection!.QueryMultipleAsync(sql + sql1, new { @Id = guid });

//    var company = await multi.ReadSingleOrDefaultAsync<Company>();

//    if (company != null)
//    {
//        var employees = await multi.ReadAsync<Employee>();
//        company.Employees = employees.ToList();
//    }

//    return company;

//}