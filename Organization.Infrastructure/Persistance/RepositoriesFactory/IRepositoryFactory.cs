using Organization.Domain.Commons.Models;
using Organization.Infrastructure.Persistance.DataContext;

namespace Organization.Application.Commons.Interfaces.Persistance.RepositoriesFactory
{
    public interface IRepositoryFactory
    {

        //In C#, where T : IDbEntity is a generic type constraint.
        //It restricts the generic placeholder T so that it can only represent classes or structs
        //that implement the IDbEntity interface
        IGenericRepository<TEntity> CreateRepository<TEntity>(DapperDataContext dapperDbContext) where TEntity : IDbEntity;
    }
}
