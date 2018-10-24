CREATE TABLE [dbo].[SecondaryCities]
(
	[Id] VARCHAR(50) NOT NULL PRIMARY KEY, 
    [Name] VARCHAR(50) NULL, 
    [Latitude] FLOAT NULL, 
    [Longitude] FLOAT NULL,
	[AdminArea] INT REFERENCES ADMINAREAS(Id) NOT NULL,
)
