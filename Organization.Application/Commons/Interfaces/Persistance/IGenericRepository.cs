using Organization.Domain.Commons.Models;
using Organization.Domain.Commons.Utilities;

namespace Organization.Application.Commons.Interfaces.Persistance
{
    public interface IGenericRepository<TEntity> where TEntity : IDbEntity
    {
        Task<IEnumerable<TEntity>> GetAsync(QueryParameters queryParameters, params string[] selectData);
        Task<TEntity> GetByIdAsync(string guid, params string[] selectData);
        Task<string> AddAsnyc(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task SoftDeleteAsync(string id, bool isSoftDeleteRecordHasRelatedChildTableColumn = false); 
        Task<int> GetTotalCountAsync();
    }
}
