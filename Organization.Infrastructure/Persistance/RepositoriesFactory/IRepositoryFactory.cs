using Organization.Domain.Commons.Models;
using Organization.Infrastructure.Persistance.DataContext;

namespace Organization.Application.Commons.Interfaces.Persistance.RepositoriesFactory
{
    public interface IRepositoryFactory
    {
        // IGenericRepository interface leverages C# Generics to accept any type of entity,
        // typically enforcing that TEntity must be a class
        IGenericRepository<TEntity> CreateRepository<TEntity>(DapperDataContext dapperDbContext) where TEntity : IDbEntity;
    }
}
