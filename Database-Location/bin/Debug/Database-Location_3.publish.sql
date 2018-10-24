/*
Script de déploiement pour Db-Location

Ce code a été généré par un outil.
La modification de ce fichier peut provoquer un comportement incorrect et sera perdue si
le code est régénéré.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "Db-Location"
:setvar DefaultFilePrefix "Db-Location"
:setvar DefaultDataPath ""
:setvar DefaultLogPath ""

GO
:on error exit
GO
/*
Détectez le mode SQLCMD et désactivez l'exécution du script si le mode SQLCMD n'est pas pris en charge.
Pour réactiver le script une fois le mode SQLCMD activé, exécutez ce qui suit :
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'Le mode SQLCMD doit être activé de manière à pouvoir exécuter ce script.';
        SET NOEXEC ON;
    END


GO
PRINT N'Création de [dbo].[AdminAreas]...';


GO
CREATE TABLE [dbo].[AdminAreas] (
    [Id]      INT          NOT NULL,
    [Name]    VARCHAR (50) NULL,
    [Country] VARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Création de [dbo].[Countries]...';


GO
CREATE TABLE [dbo].[Countries] (
    [Name]      VARCHAR (50) NOT NULL,
    [GeonameId] VARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([Name] ASC)
);


GO
PRINT N'Création de [dbo].[MainCities]...';


GO
CREATE TABLE [dbo].[MainCities] (
    [Id]         INT          NOT NULL,
    [Name]       VARCHAR (50) NULL,
    [Latitude]   FLOAT (53)   NULL,
    [Longitude]  FLOAT (53)   NULL,
    [Population] INT          NULL,
    [AdminArea]  INT          NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Création de [dbo].[SecondaryCities]...';


GO
CREATE TABLE [dbo].[SecondaryCities] (
    [Id]        INT          NOT NULL,
    [Name]      VARCHAR (50) NULL,
    [Latitude]  FLOAT (53)   NULL,
    [Longitude] FLOAT (53)   NULL,
    [AdminArea] INT          NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Création de [dbo].[USERS]...';


GO
CREATE TABLE [dbo].[USERS] (
    [IdMail]        VARCHAR (50) NOT NULL,
    [Pseudo]        VARCHAR (50) NULL,
    [Latitude]      FLOAT (53)   NULL,
    [Longitude]     NCHAR (10)   NULL,
    [Nationality]   VARCHAR (50) NULL,
    [MainCity]      INT          NULL,
    [SecondaryCity] INT          NULL,
    PRIMARY KEY CLUSTERED ([IdMail] ASC)
);


GO
PRINT N'Création de contrainte sans nom sur [dbo].[AdminAreas]...';


GO
ALTER TABLE [dbo].[AdminAreas] WITH NOCHECK
    ADD FOREIGN KEY ([Country]) REFERENCES [dbo].[Countries] ([Name]);


GO
PRINT N'Création de contrainte sans nom sur [dbo].[MainCities]...';


GO
ALTER TABLE [dbo].[MainCities] WITH NOCHECK
    ADD FOREIGN KEY ([AdminArea]) REFERENCES [dbo].[AdminAreas] ([Id]);


GO
PRINT N'Création de contrainte sans nom sur [dbo].[SecondaryCities]...';


GO
ALTER TABLE [dbo].[SecondaryCities] WITH NOCHECK
    ADD FOREIGN KEY ([AdminArea]) REFERENCES [dbo].[AdminAreas] ([Id]);


GO
PRINT N'Création de contrainte sans nom sur [dbo].[USERS]...';


GO
ALTER TABLE [dbo].[USERS] WITH NOCHECK
    ADD FOREIGN KEY ([MainCity]) REFERENCES [dbo].[MainCities] ([Id]);


GO
PRINT N'Création de contrainte sans nom sur [dbo].[USERS]...';


GO
ALTER TABLE [dbo].[USERS] WITH NOCHECK
    ADD FOREIGN KEY ([SecondaryCity]) REFERENCES [dbo].[SecondaryCities] ([Id]);


GO
