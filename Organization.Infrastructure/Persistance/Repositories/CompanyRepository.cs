using Organization.Application.Commons.Interfaces.Persistance;
using Organization.Domain.Company.Models;
using Organization.Infrastructure.Persistance.DataContext;

namespace Organization.Infrastructure.Persistance.Repositories
{
    public sealed class CompanyRepository : GenericRepository<Company>, ICompanyRepository
    {        
        public CompanyRepository(DapperDataContext dapperDataContext) : base(dapperDataContext)
        {
        }

    }
}
