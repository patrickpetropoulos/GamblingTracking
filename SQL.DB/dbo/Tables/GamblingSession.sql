CREATE TABLE [dbo].[GamblingSession]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[UserId] UNIQUEIDENTIFIER,
	[CasinoId] INT,
	[GameTypeId] INT,
	[GameSubTypeId] INT,
	[StartAmount] MONEY,
	[EndAmount] MONEY

	--CONSTRAINT FK_GamblingSession_AspNetUsers FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id)
	CONSTRAINT FK_GamblingSession_Casinos FOREIGN KEY (CasinoId) REFERENCES Casinos(Id),
	CONSTRAINT FK_GamblingSession_GameType FOREIGN KEY (GameTypeId) REFERENCES GameType(Id),
	--add game subtype constraint to that table, have sub types for slots, video keno, video poker

)
