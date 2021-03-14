USE [master]
GO
/****** Object:  Database [data-doc]    Script Date: 14/03/2021 3:39:57 PM ******/
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
/****** Object:  Table [dbo].[Attribute]    Script Date: 14/03/2021 3:39:57 PM ******/
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
/****** Object:  Table [dbo].[AttributeConfig]    Script Date: 14/03/2021 3:39:57 PM ******/
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
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_AttributeConfig] PRIMARY KEY CLUSTERED 
(
	[AttributeConfigId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Entity]    Script Date: 14/03/2021 3:39:57 PM ******/
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
/****** Object:  Table [dbo].[EntityConfig]    Script Date: 14/03/2021 3:39:57 PM ******/
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
	[ShowData] [bit] NOT NULL,
	[ShowDefinition] [bit] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_EntityConfig] PRIMARY KEY CLUSTERED 
(
	[EntityConfigId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EntityDependency]    Script Date: 14/03/2021 3:39:57 PM ******/
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
/****** Object:  Table [dbo].[Project]    Script Date: 14/03/2021 3:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Project](
	[ProjectId] [int] IDENTITY(1,1) NOT NULL,
	[ProjectName] [varchar](50) NOT NULL,
	[ProjectDesc] [varchar](1000) NULL,
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
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Relationship]    Script Date: 14/03/2021 3:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Relationship](
	[ProjectId] [int] NOT NULL,
	[RelationshipName] [sysname] NOT NULL,
	[ParentEntityName] [varchar](250) NULL,
	[ParentAttributeName] [sysname] NULL,
	[ReferencedEntityName] [varchar](250) NULL,
	[ReferencedAttributeName] [sysname] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UK_AttributeConfig_Project_EntityName_AttributeName]    Script Date: 14/03/2021 3:39:57 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UK_AttributeConfig_Project_EntityName_AttributeName] ON [dbo].[AttributeConfig]
(
	[ProjectId] ASC,
	[EntityName] ASC,
	[AttributeName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UK_EntityConfig_Project_EntityAlias]    Script Date: 14/03/2021 3:39:57 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UK_EntityConfig_Project_EntityAlias] ON [dbo].[EntityConfig]
(
	[ProjectId] ASC,
	[EntityAlias] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UK_EntityConfig_Project_EntityName]    Script Date: 14/03/2021 3:39:57 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UK_EntityConfig_Project_EntityName] ON [dbo].[EntityConfig]
(
	[ProjectId] ASC,
	[EntityName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
USE [master]
GO
ALTER DATABASE [data-doc] SET  READ_WRITE 
GO
