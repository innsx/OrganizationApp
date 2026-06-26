using Organization.Application.Commons.DTOs;
using Organization.Application.Commons.Utilities;
using Organization.Domain.Company;
using Organization.Domain.Company.Models;

namespace Organization.Application.Commons.Interfaces.Persistance
{
    public interface ICompanyRepository : IGenericRepository<Company>
    {
        public Task<PageList<CompanyResponseDto>> GetCompaniesByQueryAsync(CompanyQueryParameters companyqueryParameters);
        public Task<ICollection<Company>> QueryOneToManyParentChildRelationshipAsync(string guid);

    }
}

