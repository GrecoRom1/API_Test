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
La colonne [dbo].[AdminAreas].[Latitude] est en cours de suppression, des données risquent d'être perdues.

La colonne [dbo].[AdminAreas].[Longitude] est en cours de suppression, des données risquent d'être perdues.

La colonne Country de la table [dbo].[AdminAreas] doit être modifiée de NULL à NOT NULL. Si la table contient des données, le script ALTER peut ne pas fonctionner. Pour éviter ce problème, vous devez ajouter des valeurs à cette colonne pour toutes les lignes, marquer la colonne comme autorisant les valeurs NULL ou activer la génération de smart-defaults en tant qu'option de déploiement.

Le type pour la colonne Id de la table [dbo].[AdminAreas] est actuellement  VARCHAR (50) NOT NULL, mais il est en cours de modification en  INT IDENTITY (1, 1) NOT NULL. Une perte de données est susceptible de se produire.
*/

IF EXISTS (select top 1 1 from [dbo].[AdminAreas])
    RAISERROR (N'Lignes détectées. Arrêt de la mise à jour du schéma en raison d''''un risque de perte de données.', 16, 127) WITH NOWAIT

GO
/*
La colonne AdminArea de la table [dbo].[MainCities] doit être modifiée de NULL à NOT NULL. Si la table contient des données, le script ALTER peut ne pas fonctionner. Pour éviter ce problème, vous devez ajouter des valeurs à cette colonne pour toutes les lignes, marquer la colonne comme autorisant les valeurs NULL ou activer la génération de smart-defaults en tant qu'option de déploiement.

Le type pour la colonne AdminArea de la table [dbo].[MainCities] est actuellement  VARCHAR (50) NULL, mais il est en cours de modification en  INT NOT NULL. Une perte de données est susceptible de se produire.
*/

IF EXISTS (select top 1 1 from [dbo].[MainCities])
    RAISERROR (N'Lignes détectées. Arrêt de la mise à jour du schéma en raison d''''un risque de perte de données.', 16, 127) WITH NOWAIT

GO
/*
La colonne AdminArea de la table [dbo].[SecondaryCities] doit être modifiée de NULL à NOT NULL. Si la table contient des données, le script ALTER peut ne pas fonctionner. Pour éviter ce problème, vous devez ajouter des valeurs à cette colonne pour toutes les lignes, marquer la colonne comme autorisant les valeurs NULL ou activer la génération de smart-defaults en tant qu'option de déploiement.

Le type pour la colonne AdminArea de la table [dbo].[SecondaryCities] est actuellement  VARCHAR (50) NULL, mais il est en cours de modification en  INT NOT NULL. Une perte de données est susceptible de se produire.
*/

IF EXISTS (select top 1 1 from [dbo].[SecondaryCities])
    RAISERROR (N'Lignes détectées. Arrêt de la mise à jour du schéma en raison d''''un risque de perte de données.', 16, 127) WITH NOWAIT

GO
PRINT N'Suppression de contrainte sans nom sur [dbo].[AdminAreas]...';


GO
ALTER TABLE [dbo].[AdminAreas] DROP CONSTRAINT [FK__AdminArea__Count__5BE2A6F2];


GO
PRINT N'Suppression de contrainte sans nom sur [dbo].[MainCities]...';


GO
ALTER TABLE [dbo].[MainCities] DROP CONSTRAINT [FK__MainCitie__Admin__76969D2E];


GO
PRINT N'Suppression de contrainte sans nom sur [dbo].[SecondaryCities]...';


GO
ALTER TABLE [dbo].[SecondaryCities] DROP CONSTRAINT [FK__Secondary__Admin__787EE5A0];


GO
PRINT N'Début de la régénération de la table [dbo].[AdminAreas]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_AdminAreas] (
    [Id]        INT          IDENTITY (1, 1) NOT NULL,
    [Name]      VARCHAR (50) NULL,
    [Country]   VARCHAR (50) NOT NULL,
    [GeonameId] VARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[AdminAreas])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_AdminAreas] ON;
        INSERT INTO [dbo].[tmp_ms_xx_AdminAreas] ([Id], [Name], [Country])
        SELECT   [Id],
                 [Name],
                 [Country]
        FROM     [dbo].[AdminAreas]
        ORDER BY [Id] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_AdminAreas] OFF;
    END

DROP TABLE [dbo].[AdminAreas];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_AdminAreas]', N'AdminAreas';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Modification de [dbo].[MainCities]...';


GO
ALTER TABLE [dbo].[MainCities] ALTER COLUMN [AdminArea] INT NOT NULL;


GO
PRINT N'Modification de [dbo].[SecondaryCities]...';


GO
ALTER TABLE [dbo].[SecondaryCities] ALTER COLUMN [AdminArea] INT NOT NULL;


GO
PRINT N'Création de contrainte sans nom sur [dbo].[AdminAreas]...';


GO
ALTER TABLE [dbo].[AdminAreas] WITH NOCHECK
    ADD FOREIGN KEY ([Country]) REFERENCES [dbo].[Countries] ([GeonameId]);


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
