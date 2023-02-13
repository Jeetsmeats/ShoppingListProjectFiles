CREATE PROCEDURE [dbo].[spDelete_Selected]
	@Id int
AS
BEGIN
	DELETE
	FROM dbo.[Products]
	WHERE Id = @Id;
END

