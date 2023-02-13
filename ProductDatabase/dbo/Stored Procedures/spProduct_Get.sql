CREATE PROCEDURE [dbo].[spProduct_Get]
	@Id int
AS
BEGIN
	SELECT *
	FROM dbo.[Products]
	WHERE Id = @Id;
END
