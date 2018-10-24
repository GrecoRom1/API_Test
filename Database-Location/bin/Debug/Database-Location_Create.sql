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
USE [master];


GO
PRINT N'Création de $(DatabaseName)...'
GO
CREATE DATABASE [$(DatabaseName)] COLLATE SQL_Latin1_General_CP1_CI_AS
GO
DECLARE  @job_state INT = 0;
DECLARE  @index INT = 0;
DECLARE @EscapedDBNameLiteral sysname = N'$(DatabaseName)'
WAITFOR DELAY '00:00:30';
WHILE (@index < 60) 
BEGIN
	SET @job_state = ISNULL( (SELECT SUM (result)  FROM (
		SELECT TOP 1 [state] AS result
		FROM sys.dm_operation_status WHERE resource_type = 0 
		AND operation = 'CREATE DATABASE' AND major_resource_id = @EscapedDBNameLiteral AND [state] = 2
		ORDER BY start_time DESC
		) r), -1);

	SET @index = @index + 1;

	IF @job_state = 0 /* pending */ OR @job_state = 1 /* in progress */ OR @job_state = -1 /* job not found */ OR (SELECT [state] FROM sys.databases WHERE name = @EscapedDBNameLiteral) <> 0
		WAITFOR DELAY '00:00:30';
	ELSE 
    	BREAK;
END
GO
USE [$(DatabaseName)];


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
PRINT N'Création de [dbo].[Countries]...';


GO
CREATE TABLE [dbo].[Countries] (
    [Name]      VARCHAR (50) NOT NULL,
    [GeonameId] VARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([Name] ASC)
);


GO
PRINT N'Création de contrainte sans nom sur [dbo].[AdminAreas]...';


GO
ALTER TABLE [dbo].[AdminAreas]
    ADD FOREIGN KEY ([Country]) REFERENCES [dbo].[Countries] ([Name]);


GO
PRINT N'Création de contrainte sans nom sur [dbo].[USERS]...';


GO
ALTER TABLE [dbo].[USERS]
    ADD FOREIGN KEY ([MainCity]) REFERENCES [dbo].[MainCities] ([Id]);


GO
PRINT N'Création de contrainte sans nom sur [dbo].[USERS]...';


GO
ALTER TABLE [dbo].[USERS]
    ADD FOREIGN KEY ([SecondaryCity]) REFERENCES [dbo].[SecondaryCities] ([Id]);


GO
PRINT N'Création de contrainte sans nom sur [dbo].[SecondaryCities]...';


GO
ALTER TABLE [dbo].[SecondaryCities]
    ADD FOREIGN KEY ([AdminArea]) REFERENCES [dbo].[AdminAreas] ([Id]);


GO
PRINT N'Création de contrainte sans nom sur [dbo].[MainCities]...';


GO
ALTER TABLE [dbo].[MainCities]
    ADD FOREIGN KEY ([AdminArea]) REFERENCES [dbo].[AdminAreas] ([Id]);


GO
-- Étape de refactorisation pour mettre à jour le serveur cible avec des journaux de transactions déployés

IF OBJECT_ID(N'dbo.__RefactorLog') IS NULL
BEGIN
    CREATE TABLE [dbo].[__RefactorLog] (OperationKey UNIQUEIDENTIFIER NOT NULL PRIMARY KEY)
END
GO
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '1ea85a5a-c656-4ef4-864b-9653abd5ca2b')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('1ea85a5a-c656-4ef4-864b-9653abd5ca2b')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'e0398096-604b-48c9-9dcd-d70269ac6b88')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('e0398096-604b-48c9-9dcd-d70269ac6b88')

GO

GO
PRINT N'Mise à jour terminée.';


GO
