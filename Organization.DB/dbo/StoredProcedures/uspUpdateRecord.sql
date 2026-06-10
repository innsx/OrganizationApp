CREATE PROCEDURE [dbo].[uspUpdateRecord]
(
    @tableName 		VARCHAR(50),
    @columnsToUpdate 	VARCHAR(MAX),
    @id 			VARCHAR(22)
)
AS
BEGIN
    DECLARE @sql NVARCHAR(MAX)

    SET @sql = N'UPDATE ' + QUOTENAME(@tableName) + ' SET ' +  @columnsToUpdate + ' WHERE Id = @id'

    EXEC sp_executesql @sql,N'@id VARCHAR(22)',@id
END






--CREATE PROCEDURE [dbo].[uspUpdateRecord] 
--( 
--    @tableName VARCHAR(50), 
--    @columnsToUpdate VARCHAR(50), -- Column to update
--    @columnsToUpdateValue NVARCHAR(MAX), -- New value
--    @id VARCHAR(22) 
--) 
--AS 
--BEGIN 
--    DECLARE @sql NVARCHAR(MAX);

--    -- Build the dynamic SQL statement, escaping the column name with QUOTENAME
--    SET @sql = N'UPDATE ' + QUOTENAME(@tableName) + 
--               N' SET ' + QUOTENAME(@columnsToUpdate) + 
--               N' = @Value WHERE Id = @id';

--    -- Execute with parameters to prevent SQL injection
--    EXEC sp_executesql 
--        @stmt = @sql, 
--        @params = N'@Value NVARCHAR(MAX), @id VARCHAR(22)', 
--        @Value = @columnsToUpdateValue, 
--        @id = @id;
--END
--GO





