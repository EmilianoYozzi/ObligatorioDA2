USE [master]
GO
/****** Object:  Database [Obligatorio1Db]    Script Date: 14/06/2023 17:05:18 ******/
CREATE DATABASE [Obligatorio1Db]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Obligatorio1Db', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\Obligatorio1Db.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Obligatorio1Db_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\Obligatorio1Db_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [Obligatorio1Db] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Obligatorio1Db].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Obligatorio1Db] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Obligatorio1Db] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Obligatorio1Db] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Obligatorio1Db] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Obligatorio1Db] SET ARITHABORT OFF 
GO
ALTER DATABASE [Obligatorio1Db] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [Obligatorio1Db] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Obligatorio1Db] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Obligatorio1Db] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Obligatorio1Db] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Obligatorio1Db] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Obligatorio1Db] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Obligatorio1Db] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Obligatorio1Db] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Obligatorio1Db] SET  ENABLE_BROKER 
GO
ALTER DATABASE [Obligatorio1Db] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Obligatorio1Db] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Obligatorio1Db] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Obligatorio1Db] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Obligatorio1Db] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Obligatorio1Db] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [Obligatorio1Db] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Obligatorio1Db] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Obligatorio1Db] SET  MULTI_USER 
GO
ALTER DATABASE [Obligatorio1Db] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Obligatorio1Db] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Obligatorio1Db] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Obligatorio1Db] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Obligatorio1Db] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Obligatorio1Db] SET QUERY_STORE = OFF
GO
USE [Obligatorio1Db]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 14/06/2023 17:05:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Articles]    Script Date: 14/06/2023 17:05:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Articles](
	[Id] [nvarchar](450) NOT NULL,
	[OwnerUsername] [nvarchar](max) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Text] [nvarchar](max) NOT NULL,
	[Template] [int] NOT NULL,
	[Visibility] [int] NOT NULL,
	[Deleted] [bit] NOT NULL,
	[Edited] [bit] NOT NULL,
	[WaitingForRevision] [bit] NOT NULL,
	[Username] [nvarchar](450) NULL,
 CONSTRAINT [PK_Articles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Comments]    Script Date: 14/06/2023 17:05:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comments](
	[Id] [nvarchar](450) NOT NULL,
	[OwnerUsername] [nvarchar](max) NULL,
	[Text] [nvarchar](max) NULL,
	[IdAttachedTo] [nvarchar](max) NULL,
	[AnswerId] [nvarchar](450) NULL,
	[Edited] [bit] NULL,
	[Deleted] [bit] NULL,
	[WaitingForRevision] [bit] NOT NULL,
	[ArticleId] [nvarchar](450) NULL,
 CONSTRAINT [PK_Comments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Image]    Script Date: 14/06/2023 17:05:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Image](
	[Id] [uniqueidentifier] NOT NULL,
	[IdAttachedTo] [nvarchar](max) NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
	[ArticleId] [nvarchar](450) NULL,
 CONSTRAINT [PK_Image] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notifications]    Script Date: 14/06/2023 17:05:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notifications](
	[Id] [nvarchar](450) NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
	[Uri] [nvarchar](max) NOT NULL,
	[Date] [datetime2](7) NOT NULL,
	[Username] [nvarchar](450) NULL,
 CONSTRAINT [PK_Notifications] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OffensiveWordCollection]    Script Date: 14/06/2023 17:05:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OffensiveWordCollection](
	[Id] [int] NOT NULL,
	[offensiveWords] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_OffensiveWordCollection] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sessions]    Script Date: 14/06/2023 17:05:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sessions](
	[Token] [uniqueidentifier] NOT NULL,
	[Username] [nvarchar](max) NOT NULL,
	[Deleted] [bit] NOT NULL,
	[Role] [int] NOT NULL,
 CONSTRAINT [PK_Sessions] PRIMARY KEY CLUSTERED 
(
	[Token] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 14/06/2023 17:05:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Username] [nvarchar](450) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Lastname] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[Role] [int] NOT NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Articles_Username]    Script Date: 14/06/2023 17:05:18 ******/
CREATE NONCLUSTERED INDEX [IX_Articles_Username] ON [dbo].[Articles]
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Comments_AnswerId]    Script Date: 14/06/2023 17:05:18 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Comments_AnswerId] ON [dbo].[Comments]
(
	[AnswerId] ASC
)
WHERE ([AnswerId] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Comments_ArticleId]    Script Date: 14/06/2023 17:05:18 ******/
CREATE NONCLUSTERED INDEX [IX_Comments_ArticleId] ON [dbo].[Comments]
(
	[ArticleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Image_ArticleId]    Script Date: 14/06/2023 17:05:18 ******/
CREATE NONCLUSTERED INDEX [IX_Image_ArticleId] ON [dbo].[Image]
(
	[ArticleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Notifications_Username]    Script Date: 14/06/2023 17:05:18 ******/
CREATE NONCLUSTERED INDEX [IX_Notifications_Username] ON [dbo].[Notifications]
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Articles]  WITH CHECK ADD  CONSTRAINT [FK_Articles_Users_Username] FOREIGN KEY([Username])
REFERENCES [dbo].[Users] ([Username])
GO
ALTER TABLE [dbo].[Articles] CHECK CONSTRAINT [FK_Articles_Users_Username]
GO
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_Articles_ArticleId] FOREIGN KEY([ArticleId])
REFERENCES [dbo].[Articles] ([Id])
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comments_Articles_ArticleId]
GO
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_Comments_AnswerId] FOREIGN KEY([AnswerId])
REFERENCES [dbo].[Comments] ([Id])
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comments_Comments_AnswerId]
GO
ALTER TABLE [dbo].[Image]  WITH CHECK ADD  CONSTRAINT [FK_Image_Articles_ArticleId] FOREIGN KEY([ArticleId])
REFERENCES [dbo].[Articles] ([Id])
GO
ALTER TABLE [dbo].[Image] CHECK CONSTRAINT [FK_Image_Articles_ArticleId]
GO
ALTER TABLE [dbo].[Notifications]  WITH CHECK ADD  CONSTRAINT [FK_Notifications_Users_Username] FOREIGN KEY([Username])
REFERENCES [dbo].[Users] ([Username])
GO
ALTER TABLE [dbo].[Notifications] CHECK CONSTRAINT [FK_Notifications_Users_Username]
GO
USE [master]
GO
ALTER DATABASE [Obligatorio1Db] SET  READ_WRITE 
GO
