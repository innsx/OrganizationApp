using Organization.Domain.Commons.Models;

namespace Organization.Application.Commons.Interfaces.Persistance
{
    public interface IUnitOfWork : IDisposable
    {
        //public ICompanyRepository Companies { get; }
        //public IEmployeeRepository Employees { get; }
        public IGenericRepository<TEntity> RepositoryFactory<TEntity>() where TEntity : IDbEntity;

        public void OpenConnectionAndBeginTransaction();
        //public void CommitTransaction();
        public void CommitTransactionDisposeAndCloseConnectionDispose();
        public void RollbackTransactionAndDispose();
    }
}
