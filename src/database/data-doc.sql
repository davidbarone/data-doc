USE [master]
GO
/****** Object:  Database [data-doc]    Script Date: 3/20/2021 3:04:21 PM ******/
CREATE DATABASE [data-doc]
 CONTAINMENT = NONE
GO
ALTER DATABASE [data-doc] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [data-doc].[dbo].[sp_fulltext_database] @action = 'disable'
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
ALTER DATABASE [data-doc] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [data-doc] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [data-doc] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [data-doc] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [data-doc] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [data-doc] SET DB_CHAINING OFF 
GO
ALTER DATABASE [data-doc] SET ACCELERATED_DATABASE_RECOVERY = ON  (PERSISTENT_VERSION_STORE_FILEGROUP = [PRIMARY]) 
GO
EXEC sys.sp_db_vardecimal_storage_format N'data-doc', N'ON'
GO
ALTER DATABASE [data-doc] SET ENCRYPTION ON
GO
ALTER DATABASE [data-doc] SET QUERY_STORE = ON
GO
ALTER DATABASE [data-doc] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 100, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [data-doc]
GO
/****** Object:  User [DataDoc]    Script Date: 3/20/2021 3:04:21 PM ******/
CREATE USER [DataDoc] FOR LOGIN [DataDoc] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [DataDoc]
GO
/****** Object:  Table [dbo].[Attribute]    Script Date: 3/20/2021 3:04:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Attribute](
	[ProjectId] [int] NOT NULL,
	[EntityName] [varchar](250) NOT NULL,
	[AttributeName] [varchar](250) NOT NULL,
	[Order] [int] NOT NULL,
	[IsPrimaryKey] [bit] NOT NULL,
	[DataType] [varchar](50) NOT NULL,
	[DataLength] [smallint] NOT NULL,
	[Precision] [tinyint] NOT NULL,
	[Scale] [tinyint] NOT NULL,
	[IsNullable] [bit] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AttributeConfig]    Script Date: 3/20/2021 3:04:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AttributeConfig](
	[AttributeConfigId] [int] IDENTITY(1,1) NOT NULL,
	[ProjectId] [int] NOT NULL,
	[EntityName] [sysname] NOT NULL,
	[AttributeName] [sysname] NOT NULL,
	[AttributeDesc] [varchar](1000) NULL,
	[AttributeComment] [varchar](max) NULL,
	[IsPrimaryKey] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_AttributeConfig] PRIMARY KEY CLUSTERED 
(
	[AttributeConfigId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Entity]    Script Date: 3/20/2021 3:04:21 PM ******/
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
/****** Object:  Table [dbo].[EntityConfig]    Script Date: 3/20/2021 3:04:21 PM ******/
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
/****** Object:  Table [dbo].[EntityDependency]    Script Date: 3/20/2021 3:04:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EntityDependency](
	[ProjectId] [int] NOT NULL,
	[ParentEntityName] [sysname] NOT NULL,
	[ChildEntityName] [sysname] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Project]    Script Date: 3/20/2021 3:04:21 PM ******/
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
/****** Object:  Table [dbo].[project2]    Script Date: 3/20/2021 3:04:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[project2](
	[ProjectId] [int] IDENTITY(1,1) NOT NULL,
	[ProjectName] [varchar](50) NOT NULL,
	[ProjectDesc] [varchar](1000) NULL,
	[ConnectionString] [varchar](250) NOT NULL,
	[ScanVersion] [int] NOT NULL,
	[ScanUpdatedDt] [datetime2](7) NOT NULL,
	[ConfigVersion] [int] NOT NULL,
	[ConfigUpdatedDt] [datetime2](7) NOT NULL,
	[IsActive] [bit] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Relationship]    Script Date: 3/20/2021 3:04:21 PM ******/
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
	[IsScanned] [bit] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RelationshipAttribute]    Script Date: 3/20/2021 3:04:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RelationshipAttribute](
	[RelationshipAttributeId] [int] IDENTITY(1,1) NOT NULL,
	[RelationshipId] [int] NOT NULL,
	[ParentAttributeName] [sysname] NULL,
	[ReferencedAttributeName] [sysname] NULL
) ON [PRIMARY]
GO
