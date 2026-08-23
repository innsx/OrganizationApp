namespace Organization.Application.Commons.Interfaces.Persistance
{
    public interface IUnitOfWork : IDisposable
    {
        // get, set Properties
        public ICompanyRepository Companies { get; }
        public IEmployeeRepository Employees { get; }
        public IUserRepository Users { get; set; }


        //In C#, where T : IDbEntity is a generic type constraint.
        //It restricts the generic placeholder T so that it can only represent classes or structs
        //that implement the IDbEntity interface
        //public IGenericRepository<TEntity> RepositoryFactory<TEntity>() where TEntity : IDbEntity;

        public void OpenConnectionAndBeginDbTransaction();
        //public void CommitTransaction();
        public void CommitDbTransactionDisposeAndCloseConnectionDispose();
        public void RollbackDbTransactionAndDispose();
    }
}
 