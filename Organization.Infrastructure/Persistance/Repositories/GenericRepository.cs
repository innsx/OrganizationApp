using Dapper;
using Organization.Application.Commons.Interfaces.Persistance;
using Organization.Domain.Commons.Models;
using Organization.Domain.Commons.Utilities;
using Organization.Infrastructure.Persistance.DataContext;
using System.Data;

namespace Organization.Infrastructure.Persistance.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : IDbEntity
    {
        private readonly DapperDataContext _dapperDataContext;
        public GenericRepository(DapperDataContext dapperDataContext)
        {
            _dapperDataContext = dapperDataContext;
        }

        public async Task<string> AddAsnyc(TEntity entity)
        {
            var parameters = new DynamicParameters();
            parameters.Add("tableName", typeof(TEntity).GetDbTableName(), DbType.String, ParameterDirection.Input, size: 50);
            parameters.Add("columnNames", typeof(TEntity).GetDbTableColumnNames(new string[0]), DbType.String, ParameterDirection.Input);
            parameters.Add("columnValues", typeof(TEntity).GetColumnValuesForInsert(entity), DbType.String, ParameterDirection.Input);

            //ExecuteScalarAsync returns a SINGLE value
            string? newRecordId = await _dapperDataContext.Connection!.ExecuteScalarAsync<string>(
                "uspInsertRecord", 
                parameters, 
                _dapperDataContext.Transaction, 
                commandType: CommandType.StoredProcedure
            );

            if (newRecordId == null)
            {
                throw new Exception("An Unexpected Error Occured; no record added.");
            }

            return newRecordId;
        }

        public async Task<IEnumerable<TEntity>> GetAsync(params string[] selectedTableColumns)
        {
            var parameters = new DynamicParameters();

            parameters.Add("tableName", typeof(TEntity).GetDbTableName(), DbType.String, ParameterDirection.Input);

            if (selectedTableColumns.Length > 0)
            {
                parameters.Add("columns", typeof(TEntity)
                          .GetDbTableColumnNames(selectedTableColumns), 
                            DbType.String, 
                            ParameterDirection.Input
                );
            }

            using (var connection = _dapperDataContext.Connection)
            {
                //QueryAsync Returns an enumerable of dynamic types asynchronously
                //return a collection of data (SELECT *)
                IEnumerable<TEntity> records = await connection!.QueryAsync<TEntity>(
                    "uspGetRecords", 
                    parameters, 
                    commandType: CommandType.StoredProcedure
                );

                return records;
            }
        }

        public async Task<TEntity> GetByIdAsync(string guid, params string[] selectData)
        {
            var parameters = new DynamicParameters();
            parameters.Add("tableName", typeof(TEntity).GetDbTableName(), DbType.String, ParameterDirection.Input, size: 50);
            parameters.Add("id", guid, DbType.String, ParameterDirection.Input, size: 22);

            if (selectData.Length > 0)
            {
                parameters.Add("columns", typeof(TEntity).GetDbTableColumnNames(selectData), DbType.String, ParameterDirection.Input);
            }

            using (var connection = _dapperDataContext.Connection)
            {
                // QuerySingleOrDefaultAsync returns zero or one row of an instance of the type specified by the TEntity type parameter or null
                var record = await connection!.QuerySingleOrDefaultAsync<TEntity>(
                    "uspGetRecordsById", 
                    parameters, 
                    commandType: CommandType.StoredProcedure
                );

                return record!;
            }

        }

        public async Task<int> GetTotalCountAsync()
        {
            var parameters = new DynamicParameters();
            parameters.Add("tableName", typeof(TEntity).GetDbTableName(), DbType.String, ParameterDirection.Input, size: 50);

            using (var connection = _dapperDataContext.Connection)
            {
                // QuerySingleOrDefaultAsync returns an integer value or default value of type integer
                int recordsCount = await connection!.QuerySingleOrDefaultAsync<int>(
                    "uspGetTotalRecordsCount", 
                    parameters, 
                    commandType: CommandType.StoredProcedure
                );

                return recordsCount;
            }

        }

        public async Task<bool> IsExistingAsync(string distinguishingUniqueKeyValue)
        {
            var parameters = new DynamicParameters();
            parameters.Add("tableName", typeof(TEntity).GetDbTableName(), DbType.String, ParameterDirection.Input, size: 50);
            parameters.Add("distinguishingUniqueKeyColumnName", typeof(TEntity).GetDistinguishingUniqueKeyName(), DbType.String, ParameterDirection.Input, size: 100);
            parameters.Add("distinguishingUniquekeyColumnValue", distinguishingUniqueKeyValue, DbType.String, ParameterDirection.Input, size: 100);

            using (var connection = _dapperDataContext.Connection)
            {
                // QuerySingleOrDefaultAsync returns a single value or a default value of type bool
                bool isRecordExisted = await connection!.QuerySingleOrDefaultAsync<bool>(
                    "uspDoesRecordExist", 
                    parameters, 
                    commandType: CommandType.StoredProcedure
                );

                return isRecordExisted;
            }
        }

        public async Task SoftDeleteAsync(string id, bool isSoftDeleteColumnHasRelatedChildTableColumn)
        {
            var parameters = new DynamicParameters();
            parameters.Add("tableName", typeof(TEntity).GetDbTableName(), DbType.String, ParameterDirection.Input, size: 50);
            parameters.Add("id", id, DbType.String, ParameterDirection.Input, size: 22);

            await _dapperDataContext.Connection!.ExecuteAsync(
                "uspSoftDeleteRecord", 
                parameters, 
                _dapperDataContext.Transaction, 
                commandType: CommandType.StoredProcedure
            );

            if (isSoftDeleteColumnHasRelatedChildTableColumn == true)
            {
                foreach (var associatedType in typeof(TEntity).GetAssociatedTypes())
                {
                    parameters = new DynamicParameters();
                    parameters.Add("tableName", associatedType.Type.GetDbTableName(), DbType.String, ParameterDirection.Input, size: 50);
                    parameters.Add("foreignkeyColumnName", associatedType.ForeignKeyProperty.GetDbColumnName(), DbType.String, ParameterDirection.Input, size: 50);
                    parameters.Add("foreignkeyColumnValue", id, DbType.String, ParameterDirection.Input, size: 22);

                    //ExecuteAsync is an asynchronous extension method for the IDbConnection interface.
                    // It is specifically designed to execute commands that do not return a result set
                    // (such as INSERT, UPDATE, DELETE, or stored procedures)
                    // and it returns the number of rows affected.
                    //the ASYNC is a great way to improve performance
                    //when dealing with multiple queries.
                    //It allows you to execute them in parallel without
                    //waiting for one to finish before proceeding with the next
                    await _dapperDataContext.Connection!.ExecuteAsync(
                        "uspSoftDeleteForeignKeyRecord", 
                        parameters, 
                        _dapperDataContext.Transaction, 
                        commandType: CommandType.StoredProcedure
                    );
                }
            }

            return;
        }

        public async Task UpdateAsync(TEntity entity)
        {
            var parameters = new DynamicParameters();
            parameters.Add("tableName", typeof(TEntity).GetDbTableName(), DbType.String, ParameterDirection.Input, size: 50);
            parameters.Add("columnsToUpdate", typeof(TEntity).GetColumnValuesForUpdate(entity), DbType.String, ParameterDirection.Input);
            parameters.Add("id", entity.Id, DbType.String, ParameterDirection.Input, size: 22);

            //ExecuteAsync is an asynchronous extension method for the IDbConnection interface.
            // It is specifically designed to execute commands that do not return a result set
            // (such as INSERT, UPDATE, DELETE, or stored procedures)
            // and it returns the number of rows affected.
            //the ASYNC is a great way to improve performance
            //when dealing with multiple queries.
            //It allows you to execute them in parallel without
            //waiting for one to finish before proceeding with the next
            await _dapperDataContext.Connection!.ExecuteAsync(
                "uspUpdateRecord", 
                parameters, 
                _dapperDataContext.Transaction, 
                commandType: CommandType.StoredProcedure
            );

            return;
        }

        //ExecuteScalarAsync	Returns the first column of the first row as a dynamic type asynchronously
    }
}
