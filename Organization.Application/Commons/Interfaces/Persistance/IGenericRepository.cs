using Organization.Domain.Commons.Models;

namespace Organization.Application.Commons.Interfaces.Persistance
{
    public interface IGenericRepository<TEntity> where TEntity : IDbEntity
    {
        Task<IEnumerable<TEntity>> GetAsync(params string[] selectData);
        Task<TEntity> GetByIdAsync(string guid, params string[] selectData);
        Task<string> AddAsnyc(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task SoftDeleteAsync(string id, bool isSoftDeleteFromRelatedChildTables = false);
        Task<int> GetTotalCountAsync();
    }
}
