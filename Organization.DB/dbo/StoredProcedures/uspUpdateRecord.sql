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
--    @tableName 		VARCHAR(50),
--    @columnsToUpdate 	VARCHAR(MAX),
--    @columnsToUpdateValue VARCHAR(MAX),
--    @id 			VARCHAR(22)
--)
--AS
--BEGIN
--    DECLARE @sql NVARCHAR(MAX)

--    SET @sql = N'UPDATE ' + QUOTENAME(@tableName) + 
--              N' SET ' + @columnsToUpdate + 
--              N' = ' + @columnsToUpdateValue + 
--              N' WHERE Id=@id'

--    EXEC sp_executesql @sql,N'@id VARCHAR(22)',@id
--END






