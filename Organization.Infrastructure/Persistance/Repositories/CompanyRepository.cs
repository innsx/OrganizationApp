using Organization.Application.Commons.DTOs;
using Organization.Application.Commons.Interfaces.Persistance;
using Organization.Application.Commons.Utilities;
using Organization.Domain.Commons.Utilities;
using Organization.Domain.Company;
using Organization.Domain.Company.Models;
using Organization.Domain.Employees.Models;
using Organization.Infrastructure.Persistance.DataContext;

namespace Organization.Infrastructure.Persistance.Repositories
{
    public sealed class CompanyRepository : GenericRepository<Company>, ICompanyRepository
    {        
        public CompanyRepository(DapperDataContext dapperDataContext) : base(dapperDataContext)
        {
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
    }
}
