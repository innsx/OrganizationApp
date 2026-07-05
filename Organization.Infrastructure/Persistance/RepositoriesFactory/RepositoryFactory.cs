using Organization.Application.Commons.Interfaces.Persistance;
using Organization.Application.Commons.Interfaces.Persistance.RepositoriesFactory;
using Organization.Domain.Commons.BaseEntity;
using Organization.Infrastructure.Persistance.DataContext;
using Organization.Infrastructure.Persistance.Repositories;

namespace Organization.Infrastructure.Persistance.RepositoriesFactory
{
    public class RepositoryFactory : IRepositoryFactory
    {
        // Caches instantiated repositories to prevent duplicate creation
        private readonly Dictionary<Type, object> _repositories = new();


        //In software development, CreateRepository<TEntity> is a factory method
        //(or generic constructor) used to instantiate a data access class
        //specifically tailored for a given object or database model,
        //represented by the placeholder <TEntity>

        //<TEntity>: A generic type parameter.
        //It tells the repository which specific database model or domain object
        //(e.g., Customer, Product, User) it should manage

        //In C#, where T : IDbEntity is a generic type constraint.
        //It restricts the generic placeholder T so that it can only represent classes or structs
        //that implement the IDbEntity interface
        public IGenericRepository<TEntity> CreateRepository<TEntity>(DapperDataContext dapperDbContext) where TEntity : IDbEntity
        {
            var type = typeof(TEntity);

            // If the repository already exists for this context, return it
            if (_repositories.ContainsKey(type))
            {
                return (IGenericRepository<TEntity>)_repositories[type];
            }

            // Otherwise, manufacture a new repository instance
            var repositoryInstance = new GenericRepository<TEntity>(dapperDbContext);

            //add the new repository instance to the _repositories Dictionary to cache it
            _repositories.Add(type, repositoryInstance);

            return repositoryInstance;
        }
    }
}
