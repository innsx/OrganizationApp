CREATE PROCEDURE [dbo].[uspGetRecords]
(
	@tableName 	VARCHAR(50),
	@columns 	VARCHAR(MAX) = NULL
)
AS
BEGIN
	DECLARE @sql NVARCHAR(MAX);	

	IF @columns IS NULL
	   SET @columns = '*';

	SET @sql= N'SELECT '+ @columns + ' FROM ' + QUOTENAME(@tableName)+ ' WHERE IsDeleted = 0';

	EXEC sp_executesql @sql
END

