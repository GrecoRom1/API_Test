CREATE TABLE [dbo].[USERS]
(
	[IdMail] VARCHAR(50) NOT NULL PRIMARY KEY, 
    [Pseudo] VARCHAR(50) NULL, 
    [Latitude] FLOAT NULL, 
    [Longitude] NCHAR(10) NULL, 
    [Nationality] VARCHAR(50) NULL, 
    [MainCity] VARCHAR(50) NULL REFERENCES MainCities(Id), 
    [SecondaryCity] VARCHAR(50)  NULL REFERENCES SecondaryCities(Id)
)
