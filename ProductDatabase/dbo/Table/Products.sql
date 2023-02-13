CREATE TABLE [dbo].[Products]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[ProductName] NVARCHAR(100) NOT NULL,
	[Price] MONEY NULL,
	[Quantity] VARCHAR(50) NOT NULL,
	[RelativePrice] VARCHAR(60) NOT NULL,
	[Image] VARBINARY(max) NOT NULL,
    [Visibility] VARCHAR(20) NOT NULL
)
