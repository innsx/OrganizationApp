using Organization.Application.Commons.Interfaces.Persistance;
using Organization.Application.Commons.Interfaces.Persistance.RepositoriesFactory;
using Organization.Domain.Commons.Models;
using Organization.Infrastructure.Persistance.DataContext;
using Organization.Infrastructure.Persistance.Repositories;

namespace Organization.Infrastructure.Persistance
{
    public class UnitOfWork : IUnitOfWork
    {
        //public ICompanyRepository Companies { get; private set; }
        //public IEmployeeRepository Employees { get; private set;  }
        private readonly IRepositoryFactory _repositoryFactory;

        //specified boolean _isDisposed to 'false'
        public bool _isDisposed = false;
        private readonly DapperDataContext _dapperDataContext;

        public UnitOfWork(DapperDataContext dapperDataContext, IRepositoryFactory repositoryFactory)
        {
            _dapperDataContext = dapperDataContext;

            // RepositoryFactory is injected
            _repositoryFactory = repositoryFactory;

            //InitailizeRepositories();
        }

        //private void InitailizeRepositories()
        //{
        //    Companies = new CompanyRepository(_dapperDataContext);
        //    Employees = new EmployeeRepository(_dapperDataContext);
        //}

        public void OpenConnectionAndBeginTransaction()
        {
            _dapperDataContext.Connection?.Open();
            _dapperDataContext.Transaction = _dapperDataContext.Connection?.BeginTransaction();
        }

        public void CommitTransactionDisposeAndCloseConnectionDispose()
        {
            _dapperDataContext.Transaction?.Commit();
            _dapperDataContext.Transaction?.Dispose();

            //setting IDbTransaction 'Transaction' to Null will nullify it
            _dapperDataContext.Transaction = null;

            _dapperDataContext.Connection?.Close();
            _dapperDataContext.Connection?.Dispose();
        }


        //virtual (Inheritance Modifier): This signals that the method has a default implementation
        //in the base class,
        //but allows any derived (child) class to override it to provide custom behavior
        public virtual void Dispose(bool isDisposing)
        {
            //we've initialized _isDisposed = false
            //if (!_isDisposed)  //!_isDisposed means _isDisposed IS NOT true
            if (_isDisposed is false)
            {
                if(isDisposing is true)
                {
                    _dapperDataContext.Transaction?.Dispose();
                    _dapperDataContext.Connection?.Dispose();
                }

                _isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        public void RollbackTransactionAndDispose()
        {
            _dapperDataContext.Transaction?.Rollback();
            _dapperDataContext.Transaction?.Dispose();
            _dapperDataContext.Transaction = null;
        }


        //In C#, where T : IDbEntity is a generic type constraint.
        //It restricts the generic placeholder T so that it can only represent classes or structs
        //that implement the IDbEntity interface
        public IGenericRepository<TEntity> RepositoryFactory<TEntity>() where TEntity : IDbEntity
        {
            return _repositoryFactory.CreateRepository<TEntity>(_dapperDataContext);
        }
    }
}





//public void CommitTransaction()
//{
//    _dapperDataContext.Transaction?.Commit();
//    _dapperDataContext.Transaction?.Dispose();
//    _dapperDataContext.Transaction = null;
//}