CREATE PROCEDURE [dbo].[spDelete_All]
	
AS
BEGIN
	DELETE
	FROM dbo.[Products];
END
RETURN 0
