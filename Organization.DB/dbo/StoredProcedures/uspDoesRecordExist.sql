CREATE PROCEDURE [dbo].[uspDoesRecordExist]
    @tableName                    VARCHAR(50),
    @distinctUniqueKeyColumnName  VARCHAR(100),
    @distinctUniqueKeyColumnValue VARCHAR(100)
AS
BEGIN
	DECLARE @sql NVARCHAR(MAX)

     SET @sql = N'SELECT COUNT(id) FROM ' + QUOTENAME(@tableName) + ' WHERE ' + @distinctUniqueKeyColumnName + '= @distinctUniqueKeyColumnValue';

     EXEC sp_executesql @sql,N'@distinctUniqueKeyColumnValue VARCHAR(100)', @distinctUniqueKeyColumnValue   

END

