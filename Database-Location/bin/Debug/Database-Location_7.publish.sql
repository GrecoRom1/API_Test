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
PRINT N'Suppression de contrainte sans nom sur [dbo].[USERS]...';


GO
ALTER TABLE [dbo].[USERS] DROP CONSTRAINT [FK__USERS__MainCity__6FE99F9F];


GO
PRINT N'Suppression de contrainte sans nom sur [dbo].[MainCities]...';


GO
ALTER TABLE [dbo].[MainCities] DROP CONSTRAINT [FK__MainCitie__Admin__70DDC3D8];


GO
PRINT N'Suppression de contrainte sans nom sur [dbo].[USERS]...';


GO
ALTER TABLE [dbo].[USERS] DROP CONSTRAINT [FK__USERS__Secondary__534D60F1];


GO
PRINT N'Suppression de contrainte sans nom sur [dbo].[SecondaryCities]...';


GO
ALTER TABLE [dbo].[SecondaryCities] DROP CONSTRAINT [FK__Secondary__Admin__5CD6CB2B];


GO
PRINT N'Début de la régénération de la table [dbo].[MainCities]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_MainCities] (
    [Id]         VARCHAR (50) NOT NULL,
    [Name]       VARCHAR (50) NULL,
    [Latitude]   FLOAT (53)   NULL,
    [Longitude]  FLOAT (53)   NULL,
    [Population] INT          NULL,
    [AdminArea]  VARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[MainCities])
    BEGIN
        INSERT INTO [dbo].[tmp_ms_xx_MainCities] ([Id], [Name], [Latitude], [Longitude], [Population], [AdminArea])
        SELECT   [Id],
                 [Name],
                 [Latitude],
                 [Longitude],
                 [Population],
                 [AdminArea]
        FROM     [dbo].[MainCities]
        ORDER BY [Id] ASC;
    END

DROP TABLE [dbo].[MainCities];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_MainCities]', N'MainCities';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Début de la régénération de la table [dbo].[SecondaryCities]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_SecondaryCities] (
    [Id]        VARCHAR (50) NOT NULL,
    [Name]      VARCHAR (50) NULL,
    [Latitude]  FLOAT (53)   NULL,
    [Longitude] FLOAT (53)   NULL,
    [AdminArea] VARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[SecondaryCities])
    BEGIN
        INSERT INTO [dbo].[tmp_ms_xx_SecondaryCities] ([Id], [Name], [Latitude], [Longitude], [AdminArea])
        SELECT   [Id],
                 [Name],
                 [Latitude],
                 [Longitude],
                 [AdminArea]
        FROM     [dbo].[SecondaryCities]
        ORDER BY [Id] ASC;
    END

DROP TABLE [dbo].[SecondaryCities];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_SecondaryCities]', N'SecondaryCities';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Modification de [dbo].[USERS]...';


GO
ALTER TABLE [dbo].[USERS] ALTER COLUMN [MainCity] VARCHAR (50) NULL;

ALTER TABLE [dbo].[USERS] ALTER COLUMN [SecondaryCity] VARCHAR (50) NULL;


GO
PRINT N'Création de contrainte sans nom sur [dbo].[USERS]...';


GO
ALTER TABLE [dbo].[USERS] WITH NOCHECK
    ADD FOREIGN KEY ([MainCity]) REFERENCES [dbo].[MainCities] ([Id]);


GO
PRINT N'Création de contrainte sans nom sur [dbo].[MainCities]...';


GO
ALTER TABLE [dbo].[MainCities] WITH NOCHECK
    ADD FOREIGN KEY ([AdminArea]) REFERENCES [dbo].[AdminAreas] ([Id]);


GO
PRINT N'Création de contrainte sans nom sur [dbo].[USERS]...';


GO
ALTER TABLE [dbo].[USERS] WITH NOCHECK
    ADD FOREIGN KEY ([SecondaryCity]) REFERENCES [dbo].[SecondaryCities] ([Id]);


GO
PRINT N'Création de contrainte sans nom sur [dbo].[SecondaryCities]...';


GO
ALTER TABLE [dbo].[SecondaryCities] WITH NOCHECK
    ADD FOREIGN KEY ([AdminArea]) REFERENCES [dbo].[AdminAreas] ([Id]);


GO
