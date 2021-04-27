USE [master]
GO
/****** Object:  Database [data-doc]    Script Date: 27/04/2021 8:59:47 PM ******/
CREATE DATABASE [data-doc]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'data-doc', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\data-doc.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'data-doc_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\data-doc_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [data-doc] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [data-doc].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [data-doc] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [data-doc] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [data-doc] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [data-doc] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [data-doc] SET ARITHABORT OFF 
GO
ALTER DATABASE [data-doc] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [data-doc] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [data-doc] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [data-doc] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [data-doc] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [data-doc] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [data-doc] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [data-doc] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [data-doc] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [data-doc] SET  DISABLE_BROKER 
GO
ALTER DATABASE [data-doc] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [data-doc] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [data-doc] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [data-doc] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [data-doc] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [data-doc] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [data-doc] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [data-doc] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [data-doc] SET  MULTI_USER 
GO
ALTER DATABASE [data-doc] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [data-doc] SET DB_CHAINING OFF 
GO
ALTER DATABASE [data-doc] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [data-doc] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [data-doc] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [data-doc] SET QUERY_STORE = OFF
GO
USE [data-doc]
GO
/****** Object:  UserDefinedTableType [dbo].[ValuesUDT]    Script Date: 27/04/2021 8:59:47 PM ******/
CREATE TYPE [dbo].[ValuesUDT] AS TABLE(
	[Value] [varchar](250) NOT NULL,
	[Desc] [varchar](250) NOT NULL
)
GO
/****** Object:  Table [dbo].[AttributePrimaryKeyConfig]    Script Date: 27/04/2021 8:59:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AttributePrimaryKeyConfig](
	[AttributePrimaryKeyConfigId] [int] IDENTITY(1,1) NOT NULL,
	[ProjectId] [int] NOT NULL,
	[EntityName] [sysname] NOT NULL,
	[AttributeName] [sysname] NOT NULL,
	[IsPrimaryKey] [bit] NOT NULL,
 CONSTRAINT [PK_AttributePrimaryKeyConfig] PRIMARY KEY CLUSTERED 
(
	[AttributePrimaryKeyConfigId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AttributeDescConfig]    Script Date: 27/04/2021 8:59:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AttributeDescConfig](
	[AttributeDescConfigId] [int] IDENTITY(1,1) NOT NULL,
	[ProjectId] [int] NOT NULL,
	[EntityName] [sysname] NOT NULL,
	[AttributeName] [sysname] NOT NULL,
	[AttributeDesc] [varchar](1000) NULL,
	[AttributeComment] [varchar](max) NULL,
	[ValueGroupId] [int] NULL,
 CONSTRAINT [PK_AttributeDescConfig] PRIMARY KEY CLUSTERED 
(
	[AttributeDescConfigId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AttributeConfig]    Script Date: 27/04/2021 8:59:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AttributeConfig](
	[AttributeConfigId] [int] IDENTITY(1,1) NOT NULL,
	[ProjectId] [int] NOT NULL,
	[EntityName] [sysname] NOT NULL,
	[AttributeName] [sysname] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_AttributeConfig] PRIMARY KEY CLUSTERED 
(
	[AttributeConfigId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Attribute]    Script Date: 27/04/2021 8:59:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Attribute](
	[ProjectId] [int] NOT NULL,
	[EntityName] [sysname] NOT NULL,
	[AttributeName] [sysname] NOT NULL,
	[Order] [int] NOT NULL,
	[IsPrimaryKey] [bit] NOT NULL,
	[DataType] [varchar](50) NOT NULL,
	[DataLength] [smallint] NOT NULL,
	[Precision] [tinyint] NOT NULL,
	[Scale] [tinyint] NOT NULL,
	[IsNullable] [bit] NOT NULL,
 CONSTRAINT [PK_Attribute] PRIMARY KEY CLUSTERED 
(
	[ProjectId] ASC,
	[EntityName] ASC,
	[AttributeName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[AttributeDetails]    Script Date: 27/04/2021 8:59:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE VIEW [dbo].[AttributeDetails]
AS
	-- Returns all attributes (scanned + config)
	-- Also deals with description / comments at
	-- different scopes (local, project, global)
	SELECT
		COALESCE(AC.ProjectId, A.ProjectId) ProjectId,
		COALESCE(AC.EntityName, A.EntityName) EntityName,
		COALESCE(AC.AttributeName, A.AttributeName) AttributeName,

		-- Scanned Attribute Values (read only)
		A.[ORDER] [Order],
		A.DataType,
		A.DataLength,
		A.[Precision],
		A.Scale,
		A.IsNullable,

		-- Attribute Configuration
		C.AttributeConfigId,
		COALESCE(C.IsActive, 1) IsActive,
		
		-- Attribute Configuration (Description)
		COALESCE(AC.AttributeDescConfigId, ACProject.AttributeDescConfigId, ACGlobal.AttributeDescConfigId) AttributeDescConfigId,
		COALESCE(AC.AttributeDesc, ACProject.AttributeDesc, ACGlobal.AttributeDesc, '') AttributeDesc,
		COALESCE(AC.AttributeComment, ACProject.AttributeComment, ACGlobal.AttributeComment, '') AttributeComment,
		COALESCE(AC.ValueGroupId, ACProject.ValueGroupId, ACGlobal.ValueGroupId) ValueGroupId,
		
		CASE
			WHEN AC.AttributeDescConfigId IS NOT NULL THEN 'Local'
			WHEN ACProject.AttributeDescConfigId IS NOT NULL THEN 'Project'
			WHEN ACGlobal.AttributeDescConfigId IS NOT NULL THEN 'Global'
			ELSE 'Undefined'
		END DescScope,

		-- Attribute Primary Key Configuration
		APKC.AttributePrimaryKeyConfigId,
		COALESCE(APKC.IsPrimaryKey, A.IsPrimaryKey) IsPrimaryKey
		
	FROM
		Attribute A
	LEFT OUTER JOIN
		AttributeConfig C
	ON
		A.ProjectId = C.ProjectId AND
		A.EntityName = C.EntityName AND
		A.AttributeName = C.AttributeName
	LEFT OUTER JOIN
		AttributeDescConfig AC
	ON
		A.ProjectId = AC.ProjectId AND
		A.EntityName = AC.EntityName AND
		A.AttributeName = AC.AttributeName
	LEFT OUTER JOIN
		AttributePrimaryKeyConfig APKC
	ON
		A.ProjectId = APKC.ProjectId AND
		A.EntityName = APKC.EntityName AND
		A.AttributeName = APKC.AttributeName
	LEFT OUTER JOIN
		(SELECT * FROM AttributeDescConfig WHERE EntityName = '*') ACProject
	ON
		A.ProjectId = ACProject.ProjectId AND
		A.AttributeName = ACProject.AttributeName
	LEFT OUTER JOIN
		(SELECT * FROM AttributeDescConfig WHERE EntityName = '*' AND ProjectId = -1) ACGlobal
	ON
		A.AttributeName = ACGlobal.AttributeName
GO
/****** Object:  Table [dbo].[Entity]    Script Date: 27/04/2021 8:59:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Entity](
	[ProjectId] [int] NOT NULL,
	[EntityName] [sysname] NOT NULL,
	[EntityType] [varchar](50) NOT NULL,
	[RowCount] [int] NULL,
	[CreatedDt] [datetime2](7) NOT NULL,
	[ModifiedDt] [datetime2](7) NOT NULL,
	[UpdatedDt] [datetime2](7) NULL,
	[Definition] [varchar](max) NULL,
 CONSTRAINT [PK_SERVER_OBJECT] PRIMARY KEY CLUSTERED 
(
	[ProjectId] ASC,
	[EntityName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EntityConfig]    Script Date: 27/04/2021 8:59:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EntityConfig](
	[EntityConfigId] [int] IDENTITY(1,1) NOT NULL,
	[ProjectId] [int] NOT NULL,
	[EntityName] [sysname] NOT NULL,
	[EntityAlias] [varchar](128) NOT NULL,
	[EntityDesc] [varchar](1000) NOT NULL,
	[EntityComment] [varchar](max) NULL,
	[ShowData] [bit] NOT NULL,
	[ShowDefinition] [bit] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_EntityConfig] PRIMARY KEY CLUSTERED 
(
	[EntityConfigId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[EntityDetails]    Script Date: 27/04/2021 8:59:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[EntityDetails]
AS
	SELECT
		COALESCE(E.ProjectId, EC.ProjectId) ProjectId,
		COALESCE(E.EntityName, EC.EntityName) EntityName,
		COALESCE(E.EntityType, 'Unknown') EntityType,
		E.[RowCount],
		E.CreatedDt,
		E.ModifiedDt,
		E.UpdatedDt,
		E.Definition,
		EC.EntityConfigId,
		COALESCE(EC.EntityAlias, E.EntityName) EntityAlias,
		COALESCE(EC.EntityDesc, '[No description currently available for this entity.]') EntityDesc,
		COALESCE(EC.EntityComment, '') EntityComment,
		COALESCE(EC.ShowData, CAST(0 AS BIT)) ShowData,
		COALESCE(EC.ShowDefinition, CAST(0 AS BIT)) ShowDefinition,
		COALESCE(EC.IsActive, CAST(0 AS BIT)) IsActive
	FROM
		Entity E
	FULL OUTER JOIN
		EntityConfig EC
	ON
		E.ProjectId = EC.ProjectId AND
		E.EntityName = EC.EntityName
	WHERE
		E.EntityName <> '*'	-- Ignore the wild-card entities
GO
/****** Object:  Table [dbo].[EntityDependency]    Script Date: 27/04/2021 8:59:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EntityDependency](
	[ProjectId] [int] NOT NULL,
	[ParentEntityName] [sysname] NOT NULL,
	[ChildEntityName] [sysname] NOT NULL,
 CONSTRAINT [PK_EntityDependency] PRIMARY KEY CLUSTERED 
(
	[ProjectId] ASC,
	[ParentEntityName] ASC,
	[ChildEntityName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EntityHierarchy]    Script Date: 27/04/2021 8:59:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EntityHierarchy](
	[ProjectId] [int] NOT NULL,
	[EntityName] [varchar](128) NOT NULL,
	[AttributeName] [varchar](128) NOT NULL,
	[ParentAttributeName] [varchar](128) NOT NULL,
	[RelationshipType] [char](1) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Project]    Script Date: 27/04/2021 8:59:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Project](
	[ProjectId] [int] IDENTITY(1,1) NOT NULL,
	[ProjectName] [varchar](50) NOT NULL,
	[ProjectDesc] [varchar](1000) NULL,
	[ProjectComment] [varchar](max) NULL,
	[ConnectionString] [varchar](250) NOT NULL,
	[ScanVersion] [int] NOT NULL,
	[ScanUpdatedDt] [datetime2](7) NOT NULL,
	[ConfigVersion] [int] NOT NULL,
	[ConfigUpdatedDt] [datetime2](7) NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Project] PRIMARY KEY CLUSTERED 
(
	[ProjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Relationship]    Script Date: 27/04/2021 8:59:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Relationship](
	[RelationshipId] [int] IDENTITY(1,1) NOT NULL,
	[ProjectId] [int] NOT NULL,
	[RelationshipName] [sysname] NOT NULL,
	[ParentEntityName] [varchar](250) NULL,
	[ReferencedEntityName] [varchar](250) NULL,
	[IsScanned] [bit] NOT NULL,
 CONSTRAINT [PK_Relationship] PRIMARY KEY CLUSTERED 
(
	[RelationshipId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RelationshipAttribute]    Script Date: 27/04/2021 8:59:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RelationshipAttribute](
	[RelationshipAttributeId] [int] IDENTITY(1,1) NOT NULL,
	[RelationshipId] [int] NOT NULL,
	[ParentAttributeName] [sysname] NULL,
	[ReferencedAttributeName] [sysname] NULL,
 CONSTRAINT [PK_RelationshipAttribute] PRIMARY KEY CLUSTERED 
(
	[RelationshipAttributeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Value]    Script Date: 27/04/2021 8:59:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Value](
	[ValueId] [int] IDENTITY(1,1) NOT NULL,
	[ValueGroupId] [int] NOT NULL,
	[Value] [varchar](250) NOT NULL,
	[Desc] [varchar](250) NOT NULL,
 CONSTRAINT [PK_Value] PRIMARY KEY CLUSTERED 
(
	[ValueId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ValueGroup]    Script Date: 27/04/2021 8:59:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ValueGroup](
	[ValueGroupId] [int] IDENTITY(1,1) NOT NULL,
	[ProjectId] [int] NOT NULL,
	[ValueGroupName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_ValueGroup] PRIMARY KEY CLUSTERED 
(
	[ValueGroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Attribute]  WITH NOCHECK ADD  CONSTRAINT [FK_Attribute_Entity] FOREIGN KEY([ProjectId], [EntityName])
REFERENCES [dbo].[Entity] ([ProjectId], [EntityName])
GO
ALTER TABLE [dbo].[Attribute] CHECK CONSTRAINT [FK_Attribute_Entity]
GO
ALTER TABLE [dbo].[AttributeDescConfig]  WITH CHECK ADD  CONSTRAINT [FK_AttributeDescConfig_ValueGroup] FOREIGN KEY([ValueGroupId])
REFERENCES [dbo].[ValueGroup] ([ValueGroupId])
GO
ALTER TABLE [dbo].[AttributeDescConfig] CHECK CONSTRAINT [FK_AttributeDescConfig_ValueGroup]
GO
ALTER TABLE [dbo].[Entity]  WITH NOCHECK ADD  CONSTRAINT [FK_Entity_Project] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Project] ([ProjectId])
GO
ALTER TABLE [dbo].[Entity] CHECK CONSTRAINT [FK_Entity_Project]
GO
ALTER TABLE [dbo].[EntityDependency]  WITH NOCHECK ADD  CONSTRAINT [FK_EntityDependency_Project] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Project] ([ProjectId])
GO
ALTER TABLE [dbo].[EntityDependency] CHECK CONSTRAINT [FK_EntityDependency_Project]
GO
ALTER TABLE [dbo].[Relationship]  WITH CHECK ADD  CONSTRAINT [FK_Relationship_Project] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Project] ([ProjectId])
GO
ALTER TABLE [dbo].[Relationship] CHECK CONSTRAINT [FK_Relationship_Project]
GO
ALTER TABLE [dbo].[RelationshipAttribute]  WITH CHECK ADD  CONSTRAINT [FK_RelationshipAttribute_Relationship] FOREIGN KEY([RelationshipId])
REFERENCES [dbo].[Relationship] ([RelationshipId])
GO
ALTER TABLE [dbo].[RelationshipAttribute] CHECK CONSTRAINT [FK_RelationshipAttribute_Relationship]
GO
ALTER TABLE [dbo].[Value]  WITH CHECK ADD  CONSTRAINT [FK_Value_ValueGroup] FOREIGN KEY([ValueGroupId])
REFERENCES [dbo].[ValueGroup] ([ValueGroupId])
GO
ALTER TABLE [dbo].[Value] CHECK CONSTRAINT [FK_Value_ValueGroup]
GO
ALTER TABLE [dbo].[ValueGroup]  WITH CHECK ADD  CONSTRAINT [FK_ValueGroup_Project] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Project] ([ProjectId])
GO
ALTER TABLE [dbo].[ValueGroup] CHECK CONSTRAINT [FK_ValueGroup_Project]
GO
/****** Object:  StoredProcedure [dbo].[MergeValues_sp]    Script Date: 27/04/2021 8:59:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		d.barone
-- Create date: 11-apr-2021
-- Description:	Merges values from source db
--              into the Values table
-- =============================================
CREATE PROCEDURE [dbo].[MergeValues_sp]
	-- Add the parameters for the stored procedure here
	@ValueGroupId INT,
	@Values ValuesUDT READONLY
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO Value (ValueGroupId, Value, [Desc])
	SELECT
		@ValueGroupId,
		Value,
		[Desc]
	FROM
		@Values V1
	WHERE
		NOT EXISTS (SELECT NULL FROM Value V2 WHERE V1.Value = V2.Value AND V2.ValueGroupId = @ValueGroupId)

	UPDATE Value
	SET [Desc] = V.[Desc]
	FROM
		@Values V
	WHERE
		Value.Value = V.Value AND
		NULLIF(V.[Desc],'') IS NOT NULL

END
GO
USE [master]
GO
ALTER DATABASE [data-doc] SET  READ_WRITE 
GO
