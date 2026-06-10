using Organization.Domain.Commons.Models;

namespace Organization.Application.Commons.Interfaces.Persistance
{
    public interface IUnitOfWork : IDisposable
    {
        //public ICompanyRepository Companies { get; }
        //public IEmployeeRepository Employees { get; }


        //In C#, where T : IDbEntity is a generic type constraint.
        //It restricts the generic placeholder T so that it can only represent classes or structs
        //that implement the IDbEntity interface
        public IGenericRepository<TEntity> RepositoryFactory<TEntity>() where TEntity : IDbEntity;

        public void OpenConnectionAndBeginTransaction();
        //public void CommitTransaction();
        public void CommitTransactionDisposeAndCloseConnectionDispose();
        public void RollbackTransactionAndDispose();
    }
}
