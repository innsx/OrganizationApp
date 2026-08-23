using Organization.Application.Commons.Interfaces.Persistance;
using Organization.Domain.Users.Models;
using Organization.Infrastructure.Persistance.DataContext;

namespace Organization.Infrastructure.Persistance.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(DapperDataContext dapperDataContext) : base(dapperDataContext)
        {
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return (await GetBySpecificColumnAsync("Email", email)).AsQueryable().FirstOrDefault();
        }
    }
}
