CREATE TABLE [dbo].[MainCities]
(
	[Id] VARCHAR(50) NOT NULL PRIMARY KEY, 
    [Name] VARCHAR(50) NULL, 
    [Latitude] FLOAT NULL, 
    [Longitude] FLOAT NULL, 
    [Population] INT NULL, 
    [AdminArea] INT REFERENCES AdminAreas(Id) NOT NULL,
)
