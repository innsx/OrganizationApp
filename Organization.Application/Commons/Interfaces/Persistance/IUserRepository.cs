using Organization.Domain.Users.Models;

namespace Organization.Application.Commons.Interfaces.Persistance
{
    public interface IUserRepository : IGenericRepository<User>
    {
        public Task<User> GetUserByEmail(string email);
    }
}
