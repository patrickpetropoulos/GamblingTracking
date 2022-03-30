CREATE TABLE [dbo].[GameType]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Name] varchar(50) NOT NULL,
	[HasSubType] bit DEFAULT 0 
)
