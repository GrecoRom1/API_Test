﻿CREATE TABLE [dbo].[AdminAreas]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[Name] VARCHAR(50) NULL,
	[Country] VARCHAR(50) REFERENCES COUNTRIES(GeonameId) NOT NULL, 
    [GeonameId] VARCHAR(50) NULL, 
    
)
