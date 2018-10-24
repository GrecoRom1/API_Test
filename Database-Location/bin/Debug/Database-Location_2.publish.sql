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
/*
Le type pour la colonne Id de la table [dbo].[AdminAreas] est actuellement  VARCHAR (50) NOT NULL, mais il est en cours de modification en  INT NOT NULL. Une perte de données est susceptible de se produire.
*/

IF EXISTS (select top 1 1 from [dbo].[AdminAreas])
    RAISERROR (N'Lignes détectées. Arrêt de la mise à jour du schéma en raison d''''un risque de perte de données.', 16, 127) WITH NOWAIT

GO
/*
La colonne [dbo].[MainCities].[IdAdminArea] est en cours de suppression, des données risquent d'être perdues.

La colonne [dbo].[MainCities].[IdCountry] est en cours de suppression, des données risquent d'être perdues.

Le type pour la colonne Id de la table [dbo].[MainCities] est actuellement  VARCHAR (50) NOT NULL, mais il est en cours de modification en  INT NOT NULL. Une perte de données est susceptible de se produire.
*/

IF EXISTS (select top 1 1 from [dbo].[MainCities])
    RAISERROR (N'Lignes détectées. Arrêt de la mise à jour du schéma en raison d''''un risque de perte de données.', 16, 127) WITH NOWAIT

GO
/*
La colonne [dbo].[SecondaryCities].[IdAdminArea] est en cours de suppression, des données risquent d'être perdues.

La colonne [dbo].[SecondaryCities].[IdCountry] est en cours de suppression, des données risquent d'être perdues.

La colonne [dbo].[SecondaryCities].[Population] est en cours de suppression, des données risquent d'être perdues.

Le type pour la colonne Id de la table [dbo].[SecondaryCities] est actuellement  VARCHAR (50) NOT NULL, mais il est en cours de modification en  INT NOT NULL. Une perte de données est susceptible de se produire.
*/

IF EXISTS (select top 1 1 from [dbo].[SecondaryCities])
    RAISERROR (N'Lignes détectées. Arrêt de la mise à jour du schéma en raison d''''un risque de perte de données.', 16, 127) WITH NOWAIT

GO
/*
La colonne [dbo].[USERS].[AdminAreaId] est en cours de suppression, des données risquent d'être perdues.

La colonne [dbo].[USERS].[CountryId] est en cours de suppression, des données risquent d'être perdues.

La colonne [dbo].[USERS].[MainCityId] est en cours de suppression, des données risquent d'être perdues.

La colonne [dbo].[USERS].[SecondaryCityId] est en cours de suppression, des données risquent d'être perdues.
*/

IF EXISTS (select top 1 1 from [dbo].[USERS])
    RAISERROR (N'Lignes détectées. Arrêt de la mise à jour du schéma en raison d''''un risque de perte de données.', 16, 127) WITH NOWAIT

GO
PRINT N'Suppression de contrainte sans nom sur [dbo].[AdminAreas]...';


GO
ALTER TABLE [dbo].[AdminAreas] DROP CONSTRAINT [FK__AdminArea__Count__5165187F];


GO
PRINT N'Suppression de [dbo].[FK_AdminArea]...';


GO
ALTER TABLE [dbo].[MainCities] DROP CONSTRAINT [FK_AdminArea];


GO
PRINT N'Suppression de [dbo].[FK_AdminAreaSecondaryCities]...';


GO
ALTER TABLE [dbo].[SecondaryCities] DROP CONSTRAINT [FK_AdminAreaSecondaryCities];


GO
PRINT N'Suppression de [dbo].[FK_MainCity]...';


GO
ALTER TABLE [dbo].[USERS] DROP CONSTRAINT [FK_MainCity];


GO
PRINT N'Suppression de [dbo].[FK_SecondaryCity]...';


GO
ALTER TABLE [dbo].[USERS] DROP CONSTRAINT [FK_SecondaryCity];


GO
PRINT N'Début de la régénération de la table [dbo].[AdminAreas]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_AdminAreas] (
    [Id]      INT          NOT NULL,
    [Name]    VARCHAR (50) NULL,
    [Country] VARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[AdminAreas])
    BEGIN
        INSERT INTO [dbo].[tmp_ms_xx_AdminAreas] ([Id], [Name], [Country])
        SELECT   [Id],
                 [Name],
                 [Country]
        FROM     [dbo].[AdminAreas]
        ORDER BY [Id] ASC;
    END

DROP TABLE [dbo].[AdminAreas];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_AdminAreas]', N'AdminAreas';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Début de la régénération de la table [dbo].[MainCities]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_MainCities] (
    [Id]         INT          NOT NULL,
    [Name]       VARCHAR (50) NULL,
    [Latitude]   FLOAT (53)   NULL,
    [Longitude]  FLOAT (53)   NULL,
    [Population] INT          NULL,
    [AdminArea]  INT          NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[MainCities])
    BEGIN
        INSERT INTO [dbo].[tmp_ms_xx_MainCities] ([Id], [Name], [Latitude], [Longitude], [Population])
        SELECT   [Id],
                 [Name],
                 [Latitude],
                 [Longitude],
                 [Population]
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
    [Id]        INT          NOT NULL,
    [Name]      VARCHAR (50) NULL,
    [Latitude]  FLOAT (53)   NULL,
    [Longitude] FLOAT (53)   NULL,
    [AdminArea] INT          NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[SecondaryCities])
    BEGIN
        INSERT INTO [dbo].[tmp_ms_xx_SecondaryCities] ([Id], [Name], [Latitude], [Longitude])
        SELECT   [Id],
                 [Name],
                 [Latitude],
                 [Longitude]
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
ALTER TABLE [dbo].[USERS] DROP COLUMN [AdminAreaId], COLUMN [CountryId], COLUMN [MainCityId], COLUMN [SecondaryCityId];


GO
ALTER TABLE [dbo].[USERS]
    ADD [MainCity]      INT NULL,
        [SecondaryCity] INT NULL;


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
