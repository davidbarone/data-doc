USE [master]
GO
/****** Object:  Database [MetadataRepository]    Script Date: 28/02/2021 8:05:39 PM ******/
CREATE DATABASE [MetadataRepository]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'MetadataRepository', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\MetadataRepository.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'MetadataRepository_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\MetadataRepository_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [MetadataRepository] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [MetadataRepository].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [MetadataRepository] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [MetadataRepository] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [MetadataRepository] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [MetadataRepository] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [MetadataRepository] SET ARITHABORT OFF 
GO
ALTER DATABASE [MetadataRepository] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [MetadataRepository] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [MetadataRepository] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [MetadataRepository] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [MetadataRepository] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [MetadataRepository] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [MetadataRepository] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [MetadataRepository] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [MetadataRepository] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [MetadataRepository] SET  DISABLE_BROKER 
GO
ALTER DATABASE [MetadataRepository] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [MetadataRepository] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [MetadataRepository] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [MetadataRepository] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [MetadataRepository] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [MetadataRepository] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [MetadataRepository] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [MetadataRepository] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [MetadataRepository] SET  MULTI_USER 
GO
ALTER DATABASE [MetadataRepository] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [MetadataRepository] SET DB_CHAINING OFF 
GO
ALTER DATABASE [MetadataRepository] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [MetadataRepository] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [MetadataRepository] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [MetadataRepository] SET QUERY_STORE = OFF
GO
USE [MetadataRepository]
GO
/****** Object:  Table [dbo].[Project]    Script Date: 28/02/2021 8:05:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Project](
	[ProjectName] [varchar](50) NOT NULL,
	[ConnectionString] [varchar](250) NOT NULL,
	[Version] [int] NOT NULL,
	[LastUpdated] [datetime2](7) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[FactCruises]    Script Date: 28/02/2021 8:05:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[FactCruises]
AS
	SELECT * FROM PROJECT
GO
/****** Object:  Table [dbo].[Attribute]    Script Date: 28/02/2021 8:05:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Attribute](
	[ProjectName] [varchar](250) NOT NULL,
	[EntityName] [varchar](250) NOT NULL,
	[AttributeName] [varchar](250) NOT NULL,
	[Order] [int] NOT NULL,
	[IsPrimaryKey] [bit] NOT NULL,
	[DataType] [varchar](9) NOT NULL,
	[DataLength] [smallint] NOT NULL,
	[Precision] [tinyint] NOT NULL,
	[Scale] [tinyint] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Entity]    Script Date: 28/02/2021 8:05:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Entity](
	[ProjectName] [varchar](50) NOT NULL,
	[EntityName] [sysname] NOT NULL,
	[EntityType] [varchar](50) NOT NULL,
	[RowCount] [int] NULL,
	[CreatedDt] [datetime2](7) NOT NULL,
	[ModifiedDt] [datetime2](7) NOT NULL,
	[UpdatedDt] [datetime2](7) NULL,
	[Definition] [varchar](max) NULL,
 CONSTRAINT [PK_SERVER_OBJECT] PRIMARY KEY CLUSTERED 
(
	[ProjectName] ASC,
	[EntityName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EntityPublication]    Script Date: 28/02/2021 8:05:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EntityPublication](
	[ProjectName] [varchar](50) NOT NULL,
	[EntityName] [sysname] NOT NULL,
	[EntityAlias] [varchar](50) NOT NULL,
	[EntityDescription] [varchar](50) NOT NULL,
	[IsPublished] [bit] NOT NULL
) ON [PRIMARY]
GO
USE [master]
GO
ALTER DATABASE [MetadataRepository] SET  READ_WRITE 
GO
