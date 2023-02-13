CREATE PROCEDURE [dbo].[spProduct_Insert]
	@ProductName NVARCHAR(50),
	@Price MONEY NULL,
	@Quantity VARCHAR(50),
	@RelativePrice VARCHAR(60),
	@Image VARBINARY(max),
	@Visibility VARCHAR(50),
	@Id INT
AS
BEGIN
IF NOT EXISTS (SELECT Id FROM dbo.[Products] WHERE Id = @Id)
BEGIN
	INSERT INTO dbo.[Products] (ProductName, Price, 
	Quantity, RelativePrice, Image, Visibility)
	VALUES (@ProductName, @Price, @Quantity, @RelativePrice,
	@Image, @Visibility)
	SET @Id = @@IDENTITY
END
ELSE
BEGIN
	UPDATE dbo.[Products]
	SET ProductName = @ProductName,
	Price = @Price,
	Quantity = @Quantity,
	RelativePrice = @RelativePrice,
	Image = @Image,
	Visibility = @Visibility
	WHERE Id = @ID;
END
END