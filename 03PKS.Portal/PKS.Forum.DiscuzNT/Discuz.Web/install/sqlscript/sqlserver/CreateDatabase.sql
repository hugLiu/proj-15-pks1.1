﻿USE [master]
GO

/****** Object:  Database [PKS_DNT3]    Script Date: 11/27/2017 09:44:22 ******/
CREATE DATABASE [PKS_DNT3] ON  PRIMARY 
( NAME = N'PKS_DNT3', FILENAME = N'E:\PKSData\PKS_DNT\PKS_DNT3.mdf' , SIZE = 10240KB , FILEGROWTH = 10240KB )
 LOG ON 
( NAME = N'PKS_DNT3_log', FILENAME = N'E:\PKSData\PKS_DNT\PKS_DNT3_log.LDF' , SIZE = 10240KB , FILEGROWTH = 20480KB)
GO

ALTER DATABASE [PKS_DNT3] SET COMPATIBILITY_LEVEL = 100
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [PKS_DNT3].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [PKS_DNT3] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [PKS_DNT3] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [PKS_DNT3] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [PKS_DNT3] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [PKS_DNT3] SET ARITHABORT OFF 
GO

ALTER DATABASE [PKS_DNT3] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [PKS_DNT3] SET AUTO_CREATE_STATISTICS ON 
GO

ALTER DATABASE [PKS_DNT3] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [PKS_DNT3] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [PKS_DNT3] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [PKS_DNT3] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [PKS_DNT3] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [PKS_DNT3] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [PKS_DNT3] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [PKS_DNT3] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [PKS_DNT3] SET  DISABLE_BROKER 
GO

ALTER DATABASE [PKS_DNT3] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [PKS_DNT3] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [PKS_DNT3] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [PKS_DNT3] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [PKS_DNT3] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [PKS_DNT3] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [PKS_DNT3] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [PKS_DNT3] SET  READ_WRITE 
GO

ALTER DATABASE [PKS_DNT3] SET RECOVERY FULL 
GO

ALTER DATABASE [PKS_DNT3] SET  MULTI_USER 
GO

ALTER DATABASE [PKS_DNT3] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [PKS_DNT3] SET DB_CHAINING OFF 
GO

USE [PKS_DNT3]
GO

CREATE USER [PKSUser] FOR LOGIN [PKSUser] WITH DEFAULT_SCHEMA=[dbo]
GO
