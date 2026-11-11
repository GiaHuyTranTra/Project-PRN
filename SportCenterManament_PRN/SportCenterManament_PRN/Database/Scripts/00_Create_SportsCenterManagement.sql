/* FRESH INSTALL ONLY: creates a new database. Do not execute on an existing database. */
USE [master]
GO
/****** Object:  Database [SportsCenterManagement]    Script Date: 10/15/2026 10:24:55 AM ******/
CREATE DATABASE [SportsCenterManagement]
 CONTAINMENT = NONE
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [SportsCenterManagement] SET COMPATIBILITY_LEVEL = 170
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SportsCenterManagement].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [SportsCenterManagement] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [SportsCenterManagement] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [SportsCenterManagement] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [SportsCenterManagement] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [SportsCenterManagement] SET ARITHABORT OFF 
GO
ALTER DATABASE [SportsCenterManagement] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [SportsCenterManagement] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [SportsCenterManagement] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [SportsCenterManagement] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [SportsCenterManagement] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [SportsCenterManagement] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [SportsCenterManagement] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [SportsCenterManagement] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [SportsCenterManagement] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [SportsCenterManagement] SET  ENABLE_BROKER 
GO
ALTER DATABASE [SportsCenterManagement] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [SportsCenterManagement] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [SportsCenterManagement] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [SportsCenterManagement] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [SportsCenterManagement] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [SportsCenterManagement] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [SportsCenterManagement] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [SportsCenterManagement] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [SportsCenterManagement] SET  MULTI_USER 
GO
ALTER DATABASE [SportsCenterManagement] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [SportsCenterManagement] SET DB_CHAINING OFF 
GO
ALTER DATABASE [SportsCenterManagement] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [SportsCenterManagement] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [SportsCenterManagement] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [SportsCenterManagement] SET OPTIMIZED_LOCKING = OFF 
GO
ALTER DATABASE [SportsCenterManagement] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [SportsCenterManagement] SET QUERY_STORE = ON
GO
ALTER DATABASE [SportsCenterManagement] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [SportsCenterManagement]
GO
/****** Object:  UserDefinedFunction [dbo].[NormalizeFlow2Name]    Script Date: 10/15/2026 10:24:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[NormalizeFlow2Name](@value nvarchar(100)) RETURNS nvarchar(100) WITH SCHEMABINDING AS
BEGIN
    DECLARE @name nvarchar(100) = TRANSLATE(@value,N'ÀÁÂÃÄÅÇÈÉÊËÌÍÎÏÑÒÓÔÕÖÙÚÛÜÝàáâãäåçèéêëìíîïñòóôõöùúûüýÿĀāĂăĄąĆćĈĉĊċČčĎďĒēĔĕĖėĘęĚěĜĝĞğĠġĢģĤĥĨĩĪīĬĭĮįİĴĵĶķĹĺĻļĽľŃńŅņŇňŌōŎŏŐőŔŕŖŗŘřŚśŜŝŞşŠšŢţŤťŨũŪūŬŭŮůŰűŲųŴŵŶŷŸŹźŻżŽžƠơƯưǍǎǏǐǑǒǓǔǕǖǗǘǙǚǛǜǞǟǠǡǦǧǨǩǪǫǬǭǰǴǵǸǹǺǻȀȁȂȃȄȅȆȇȈȉȊȋȌȍȎȏȐȑȒȓȔȕȖȗȘșȚțȞȟȦȧȨȩȪȫȬȭȮȯȰȱȲȳḀḁḂḃḄḅḆḇḈḉḊḋḌḍḎḏḐḑḒḓḔḕḖḗḘḙḚḛḜḝḞḟḠḡḢḣḤḥḦḧḨḩḪḫḬḭḮḯḰḱḲḳḴḵḶḷḸḹḺḻḼḽḾḿṀṁṂṃṄṅṆṇṈṉṊṋṌṍṎṏṐṑṒṓṔṕṖṗṘṙṚṛṜṝṞṟṠṡṢṣṤṥṦṧṨṩṪṫṬṭṮṯṰṱṲṳṴṵṶṷṸṹṺṻṼṽṾṿẀẁẂẃẄẅẆẇẈẉẊẋẌẍẎẏẐẑẒẓẔẕẖẗẘẙẠạẢảẤấẦầẨẩẪẫẬậẮắẰằẲẳẴẵẶặẸẹẺẻẼẽẾếỀềỂểỄễỆệỈỉỊịỌọỎỏỐốỒồỔổỖỗỘộỚớỜờỞởỠỡỢợỤụỦủỨứỪừỬửỮữỰựỲỳỴỵỶỷỸỹđĐ',N'AAAAAACEEEEIIIINOOOOOUUUUYaaaaaaceeeeiiiinooooouuuuyyAaAaAaCcCcCcCcDdEeEeEeEeEeGgGgGgGgHhIiIiIiIiIJjKkLlLlLlNnNnNnOoOoOoRrRrRrSsSsSsSsTtTtUuUuUuUuUuUuWwYyYZzZzZzOoUuAaIiOoUuUuUuUuUuAaAaGgKkOoOojGgNnAaAaAaEeEeIiIiOoOoRrRrUuUuSsTtHhAaEeOoOoOoOoYyAaBbBbBbCcDdDdDdDdDdEeEeEeEeEeFfGgHhHhHhHhHhIiIiKkKkKkLlLlLlLlMmMmMmNnNnNnNnOoOoOoOoPpPpRrRrRrRrSsSsSsSsSsTtTtTtTtUuUuUuUuUuVvVvWwWwWwWwWwXxXxYyZzZzZzhtwyAaAaAaAaAaAaAaAaAaAaAaAaEeEeEeEeEeEeEeEeIiIiOoOoOoOoOoOoOoOoOoOoOoOoUuUuUuUuUuUuUuYyYyYyYydD');
    SET @name=TRANSLATE(@name,NCHAR(9)+NCHAR(10)+NCHAR(11)+NCHAR(12)+NCHAR(13)+NCHAR(133)+NCHAR(160)+NCHAR(5760)+NCHAR(8192)+NCHAR(8193)+NCHAR(8194)+NCHAR(8195)+NCHAR(8196)+NCHAR(8197)+NCHAR(8198)+NCHAR(8199)+NCHAR(8200)+NCHAR(8201)+NCHAR(8202)+NCHAR(8232)+NCHAR(8233)+NCHAR(8239)+NCHAR(8287)+NCHAR(12288),REPLICATE(N' ',24));
    DECLARE @combining int=768;
    WHILE @combining<=879 BEGIN SET @name=REPLACE(@name,NCHAR(@combining),N''); SET @combining+=1; END;
    WHILE CHARINDEX(N'  ',@name)>0 SET @name=REPLACE(@name,N'  ',N' ');
    RETURN UPPER(LTRIM(RTRIM(@name)) COLLATE Latin1_General_100_CI_AS);
END
GO
/****** Object:  Table [dbo].[Discipline]    Script Date: 10/15/2026 10:24:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Discipline](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](80) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[NormalizedName]  AS (CONVERT([nvarchar](80),[dbo].[NormalizeFlow2Name]([Name]))) PERSISTED,
	[RowVersion] [timestamp] NOT NULL,
 CONSTRAINT [PK_Discipline] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Room]    Script Date: 10/15/2026 10:24:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Room](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](80) NOT NULL,
	[Location] [nvarchar](120) NULL,
	[RoomType] [nvarchar](20) NULL,
	[Capacity] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[NormalizedName]  AS (CONVERT([nvarchar](80),[dbo].[NormalizeFlow2Name]([Name]))) PERSISTED,
	[RowVersion] [timestamp] NOT NULL,
 CONSTRAINT [PK_Room] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Class]    Script Date: 10/15/2026 10:24:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Class](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DisciplineId] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[Capacity] [int] NOT NULL,
	[Status] [varchar](20) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[CreatedBy] [varchar](400) NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[NormalizedName]  AS (CONVERT([nvarchar](100),[dbo].[NormalizeFlow2Name]([Name]))) PERSISTED,
	[RowVersion] [timestamp] NOT NULL,
 CONSTRAINT [PK_Class] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 10/15/2026 10:24:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[Id] [varchar](400) NOT NULL,
	[Email] [varchar](150) NOT NULL,
	[PasswordHash] [varchar](255) NOT NULL,
	[Status] [varchar](20) NOT NULL,
	[FailedLoginCount] [int] NOT NULL,
	[IsLocked] [bit] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[Phone] [varchar](10) NULL,
	[RoleId] [int] NOT NULL,
	[DeletedAt] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AuditLog]    Script Date: 10/15/2026 10:24:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditLog](
	[Id] [varchar](400) NOT NULL,
	[AccountId] [varchar](400) NULL,
	[Action] [varchar](100) NOT NULL,
	[EntityType] [varchar](100) NULL,
	[EntityId] [varchar](400) NULL,
	[Description] [nvarchar](500) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[ClassId] [int] NULL,
	[SessionId] [int] NULL,
	[OperationId] [uniqueidentifier] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CenterManager]    Script Date: 10/15/2026 10:24:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CenterManager](
	[AccountId] [varchar](400) NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ClassChangeEvent]    Script Date: 10/15/2026 10:24:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClassChangeEvent](
	[Id] [uniqueidentifier] NOT NULL,
	[OperationId] [uniqueidentifier] NOT NULL,
	[SessionId] [int] NOT NULL,
	[EventType] [varchar](50) NOT NULL,
	[ActorAccountId] [varchar](400) NULL,
	[OccurredAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_ClassChangeEvent] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ClassRegistration]    Script Date: 10/15/2026 10:24:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClassRegistration](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SessionId] [int] NOT NULL,
	[MemberId] [varchar](400) NOT NULL,
	[Status] [varchar](20) NOT NULL,
	[RegisteredAt] [datetime2](7) NOT NULL,
	[RegisteredVia] [varchar](20) NOT NULL,
	[RegisteredBy] [varchar](400) NOT NULL,
	[CanceledAt] [datetime2](7) NULL,
	[CanceledBy] [varchar](400) NULL,
	[CancelReason] [nvarchar](500) NULL,
	[MemberSubscriptionId] [int] NULL,
	[RowVersion] [timestamp] NOT NULL,
 CONSTRAINT [PK_ClassRegistration] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ClassSession]    Script Date: 10/15/2026 10:24:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClassSession](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClassId] [int] NOT NULL,
	[CoachId] [varchar](400) NOT NULL,
	[RoomId] [int] NOT NULL,
	[SessionDate] [date] NOT NULL,
	[StartTime] [time](0) NOT NULL,
	[EndTime] [time](0) NOT NULL,
	[Capacity] [int] NOT NULL,
	[BookedCount] [int] NOT NULL,
	[Status] [varchar](20) NOT NULL,
	[Notes] [nvarchar](500) NULL,
	[CancelReason] [nvarchar](500) NULL,
	[CanceledAt] [datetime2](7) NULL,
	[CanceledBy] [varchar](400) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[CreatedBy] [varchar](400) NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[RowVersion] [timestamp] NOT NULL,
 CONSTRAINT [PK_ClassSession] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Coach]    Script Date: 10/15/2026 10:24:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Coach](
	[AccountId] [varchar](400) NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[Specialization] [nvarchar](255) NULL,
	[WorkSchedule] [nvarchar](500) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CoachDiscipline]    Script Date: 10/15/2026 10:24:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CoachDiscipline](
	[CoachAccountId] [varchar](400) NOT NULL,
	[DisciplineId] [int] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_CoachDiscipline] PRIMARY KEY CLUSTERED 
(
	[CoachAccountId] ASC,
	[DisciplineId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Member]    Script Date: 10/15/2026 10:24:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Member](
	[AccountId] [varchar](400) NOT NULL,
	[MemberCode] [varchar](30) NOT NULL,
	[FullName] [nvarchar](100) NULL,
	[DateOfBirth] [date] NULL,
	[AvatarUrl] [varchar](500) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[MemberCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MembershipInvoice]    Script Date: 10/15/2026 10:24:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MembershipInvoice](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[InvoiceNumber] [varchar](50) NOT NULL,
	[SubscriptionId] [int] NOT NULL,
	[MemberId] [varchar](400) NOT NULL,
	[Amount] [decimal](18, 0) NOT NULL,
	[Status] [varchar](20) NOT NULL,
	[PaymentMethod] [varchar](30) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[CreatedBy] [varchar](400) NOT NULL,
	[PaidAt] [datetime2](7) NULL,
	[PaidBy] [varchar](400) NULL,
 CONSTRAINT [PK_MembershipInvoice] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_MembershipInvoice_InvoiceNumber] UNIQUE NONCLUSTERED 
(
	[InvoiceNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_MembershipInvoice_SubscriptionId] UNIQUE NONCLUSTERED 
(
	[SubscriptionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MembershipPackage]    Script Date: 10/15/2026 10:24:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MembershipPackage](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](80) NOT NULL,
	[Price] [decimal](18, 0) NOT NULL,
	[DurationMonths] [int] NOT NULL,
	[Benefits] [nvarchar](max) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
 CONSTRAINT [PK_MembershipPackage] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MembershipPackageSport]    Script Date: 10/15/2026 10:24:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MembershipPackageSport](
	[PackageId] [int] NOT NULL,
	[DisciplineId] [int] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_MembershipPackageSport] PRIMARY KEY CLUSTERED 
(
	[PackageId] ASC,
	[DisciplineId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MemberSubscription]    Script Date: 10/15/2026 10:24:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MemberSubscription](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MemberId] [varchar](400) NOT NULL,
	[PackageId] [int] NOT NULL,
	[PackageName] [nvarchar](80) NOT NULL,
	[PackagePrice] [decimal](18, 0) NOT NULL,
	[DurationMonths] [int] NOT NULL,
	[Benefits] [nvarchar](max) NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NOT NULL,
	[Kind] [varchar](20) NOT NULL,
	[Status] [varchar](20) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[IsSuspended] [bit] NOT NULL,
	[SuspensionReason] [nvarchar](500) NULL,
 CONSTRAINT [PK_MemberSubscription] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notification]    Script Date: 10/15/2026 10:24:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notification](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RecipientId] [varchar](400) NOT NULL,
	[Type] [varchar](50) NOT NULL,
	[Title] [nvarchar](200) NOT NULL,
	[Body] [nvarchar](1000) NOT NULL,
	[EntityType] [varchar](50) NOT NULL,
	[EntityId] [varchar](100) NOT NULL,
	[EventAt] [datetime2](7) NOT NULL,
	[IsRead] [bit] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[ReadAt] [datetime2](7) NULL,
	[EventId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Notification] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PasswordChangeOtp]    Script Date: 10/15/2026 10:24:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PasswordChangeOtp](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AccountId] [varchar](400) NOT NULL,
	[OtpHash] [varchar](255) NOT NULL,
	[FailedAttempts] [int] NOT NULL,
	[MaxAttempts] [int] NOT NULL,
	[ExpiresAt] [datetime2](7) NOT NULL,
	[IsUsed] [bit] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Receptionist]    Script Date: 10/15/2026 10:24:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Receptionist](
	[AccountId] [varchar](400) NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[WorkShift] [nvarchar](100) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 10/15/2026 10:24:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Account_Phone]    Script Date: 10/15/2026 10:24:55 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UQ_Account_Phone] ON [dbo].[Account]
(
	[Phone] ASC
)
WHERE ([Phone] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_AuditLog_Class_Time]    Script Date: 10/15/2026 10:24:55 AM ******/
CREATE NONCLUSTERED INDEX [IX_AuditLog_Class_Time] ON [dbo].[AuditLog]
(
	[ClassId] ASC,
	[CreatedAt] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_AuditLog_Session_Time]    Script Date: 10/15/2026 10:24:55 AM ******/
CREATE NONCLUSTERED INDEX [IX_AuditLog_Session_Time] ON [dbo].[AuditLog]
(
	[SessionId] ASC,
	[CreatedAt] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Class_DisciplineId]    Script Date: 10/15/2026 10:24:55 AM ******/
CREATE NONCLUSTERED INDEX [IX_Class_DisciplineId] ON [dbo].[Class]
(
	[DisciplineId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ARITHABORT ON
SET CONCAT_NULL_YIELDS_NULL ON
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
SET NUMERIC_ROUNDABORT OFF
GO
/****** Object:  Index [UQ_Class_NormalizedName_Active]    Script Date: 10/15/2026 10:24:55 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UQ_Class_NormalizedName_Active] ON [dbo].[Class]
(
	[NormalizedName] ASC
)
WHERE ([Status]<>'ARCHIVED')
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_ClassChangeEvent_Operation]    Script Date: 10/15/2026 10:24:55 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UQ_ClassChangeEvent_Operation] ON [dbo].[ClassChangeEvent]
(
	[OperationId] ASC,
	[SessionId] ASC,
	[EventType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_ClassRegistration_Member]    Script Date: 10/15/2026 10:24:55 AM ******/
CREATE NONCLUSTERED INDEX [IX_ClassRegistration_Member] ON [dbo].[ClassRegistration]
(
	[MemberId] ASC,
	[Status] ASC,
	[SessionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_ClassRegistration_Session]    Script Date: 10/15/2026 10:24:55 AM ******/
CREATE NONCLUSTERED INDEX [IX_ClassRegistration_Session] ON [dbo].[ClassRegistration]
(
	[SessionId] ASC,
	[Status] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_ClassRegistration_ActiveBooking]    Script Date: 10/15/2026 10:24:55 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UQ_ClassRegistration_ActiveBooking] ON [dbo].[ClassRegistration]
(
	[SessionId] ASC,
	[MemberId] ASC
)
WHERE ([Status]='BOOKED')
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ClassSession_ClassId]    Script Date: 10/15/2026 10:24:55 AM ******/
CREATE NONCLUSTERED INDEX [IX_ClassSession_ClassId] ON [dbo].[ClassSession]
(
	[ClassId] ASC,
	[SessionDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_ClassSession_Date_Coach]    Script Date: 10/15/2026 10:24:55 AM ******/
CREATE NONCLUSTERED INDEX [IX_ClassSession_Date_Coach] ON [dbo].[ClassSession]
(
	[CoachId] ASC,
	[SessionDate] ASC,
	[StartTime] ASC
)
WHERE ([Status]='SCHEDULED')
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ClassSession_Date_Room]    Script Date: 10/15/2026 10:24:55 AM ******/
CREATE NONCLUSTERED INDEX [IX_ClassSession_Date_Room] ON [dbo].[ClassSession]
(
	[RoomId] ASC,
	[SessionDate] ASC,
	[StartTime] ASC
)
WHERE ([Status]='SCHEDULED')
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Coach_Pagination]    Script Date: 10/15/2026 10:24:55 AM ******/
CREATE NONCLUSTERED INDEX [IX_Coach_Pagination] ON [dbo].[Coach]
(
	[CreatedAt] DESC,
	[AccountId] DESC
)
INCLUDE([FullName]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_CoachDiscipline_DisciplineId]    Script Date: 10/15/2026 10:24:55 AM ******/
CREATE NONCLUSTERED INDEX [IX_CoachDiscipline_DisciplineId] ON [dbo].[CoachDiscipline]
(
	[DisciplineId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Discipline_Name]    Script Date: 10/15/2026 10:24:55 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UQ_Discipline_Name] ON [dbo].[Discipline]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ARITHABORT ON
SET CONCAT_NULL_YIELDS_NULL ON
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
SET NUMERIC_ROUNDABORT OFF
GO
/****** Object:  Index [UQ_Discipline_NormalizedName]    Script Date: 10/15/2026 10:24:55 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UQ_Discipline_NormalizedName] ON [dbo].[Discipline]
(
	[NormalizedName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_MembershipInvoice_MemberId_Status]    Script Date: 10/15/2026 10:24:55 AM ******/
CREATE NONCLUSTERED INDEX [IX_MembershipInvoice_MemberId_Status] ON [dbo].[MembershipInvoice]
(
	[MemberId] ASC,
	[Status] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_MembershipPackage_Name]    Script Date: 10/15/2026 10:24:55 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UQ_MembershipPackage_Name] ON [dbo].[MembershipPackage]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_MembershipPackageSport_DisciplineId]    Script Date: 10/15/2026 10:24:55 AM ******/
CREATE NONCLUSTERED INDEX [IX_MembershipPackageSport_DisciplineId] ON [dbo].[MembershipPackageSport]
(
	[DisciplineId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_MemberSubscription_Member_Confirmed_EndDate]    Script Date: 10/15/2026 10:24:55 AM ******/
CREATE NONCLUSTERED INDEX [IX_MemberSubscription_Member_Confirmed_EndDate] ON [dbo].[MemberSubscription]
(
	[MemberId] ASC,
	[Status] ASC,
	[EndDate] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_MemberSubscription_MemberId_Status]    Script Date: 10/15/2026 10:24:55 AM ******/
CREATE NONCLUSTERED INDEX [IX_MemberSubscription_MemberId_Status] ON [dbo].[MemberSubscription]
(
	[MemberId] ASC,
	[Status] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_MemberSubscription_Pending_Member]    Script Date: 10/15/2026 10:24:55 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UQ_MemberSubscription_Pending_Member] ON [dbo].[MemberSubscription]
(
	[MemberId] ASC
)
WHERE ([Status]='PENDING_PAYMENT')
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Notification_Recipient_ReadAt]    Script Date: 10/15/2026 10:24:55 AM ******/
CREATE NONCLUSTERED INDEX [IX_Notification_Recipient_ReadAt] ON [dbo].[Notification]
(
	[RecipientId] ASC,
	[ReadAt] ASC,
	[CreatedAt] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Notification_EventRecipient]    Script Date: 10/15/2026 10:24:55 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UQ_Notification_EventRecipient] ON [dbo].[Notification]
(
	[EventId] ASC,
	[RecipientId] ASC
)
WHERE ([EventId] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_PasswordChangeOtp_ActiveAccount]    Script Date: 10/15/2026 10:24:55 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UQ_PasswordChangeOtp_ActiveAccount] ON [dbo].[PasswordChangeOtp]
(
	[AccountId] ASC
)
WHERE ([IsUsed]=(0))
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ARITHABORT ON
SET CONCAT_NULL_YIELDS_NULL ON
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
SET NUMERIC_ROUNDABORT OFF
GO
/****** Object:  Index [UQ_Room_NormalizedName]    Script Date: 10/15/2026 10:24:55 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UQ_Room_NormalizedName] ON [dbo].[Room]
(
	[NormalizedName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Account] ADD  DEFAULT ('Active') FOR [Status]
GO
ALTER TABLE [dbo].[Account] ADD  DEFAULT ((0)) FOR [FailedLoginCount]
GO
ALTER TABLE [dbo].[Account] ADD  DEFAULT ((0)) FOR [IsLocked]
GO
ALTER TABLE [dbo].[Account] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[AuditLog] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[CenterManager] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Class] ADD  CONSTRAINT [DF_Class_Status]  DEFAULT ('DRAFT') FOR [Status]
GO
ALTER TABLE [dbo].[Class] ADD  CONSTRAINT [DF_Class_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[ClassRegistration] ADD  CONSTRAINT [DF_ClassRegistration_Status]  DEFAULT ('BOOKED') FOR [Status]
GO
ALTER TABLE [dbo].[ClassRegistration] ADD  CONSTRAINT [DF_ClassRegistration_RegisteredAt]  DEFAULT (sysutcdatetime()) FOR [RegisteredAt]
GO
ALTER TABLE [dbo].[ClassRegistration] ADD  CONSTRAINT [DF_ClassRegistration_RegisteredVia]  DEFAULT ('MEMBER') FOR [RegisteredVia]
GO
ALTER TABLE [dbo].[ClassSession] ADD  CONSTRAINT [DF_ClassSession_BookedCount]  DEFAULT ((0)) FOR [BookedCount]
GO
ALTER TABLE [dbo].[ClassSession] ADD  CONSTRAINT [DF_ClassSession_Status]  DEFAULT ('SCHEDULED') FOR [Status]
GO
ALTER TABLE [dbo].[ClassSession] ADD  CONSTRAINT [DF_ClassSession_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Coach] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[CoachDiscipline] ADD  CONSTRAINT [DF_CoachDiscipline_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Discipline] ADD  CONSTRAINT [DF_Discipline_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Discipline] ADD  CONSTRAINT [DF_Discipline_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Member] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[MembershipInvoice] ADD  CONSTRAINT [DF_MembershipInvoice_Status]  DEFAULT ('PENDING_PAYMENT') FOR [Status]
GO
ALTER TABLE [dbo].[MembershipInvoice] ADD  CONSTRAINT [DF_MembershipInvoice_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[MembershipPackage] ADD  CONSTRAINT [DF_MembershipPackage_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[MembershipPackage] ADD  CONSTRAINT [DF_MembershipPackage_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[MembershipPackageSport] ADD  CONSTRAINT [DF_MembershipPackageSport_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[MemberSubscription] ADD  CONSTRAINT [DF_MemberSubscription_Status]  DEFAULT ('PENDING_PAYMENT') FOR [Status]
GO
ALTER TABLE [dbo].[MemberSubscription] ADD  CONSTRAINT [DF_MemberSubscription_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[MemberSubscription] ADD  CONSTRAINT [DF_MemberSubscription_IsSuspended]  DEFAULT ((0)) FOR [IsSuspended]
GO
ALTER TABLE [dbo].[Notification] ADD  CONSTRAINT [DF_Notification_IsRead]  DEFAULT ((0)) FOR [IsRead]
GO
ALTER TABLE [dbo].[Notification] ADD  CONSTRAINT [DF_Notification_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[PasswordChangeOtp] ADD  DEFAULT ((0)) FOR [FailedAttempts]
GO
ALTER TABLE [dbo].[PasswordChangeOtp] ADD  DEFAULT ((3)) FOR [MaxAttempts]
GO
ALTER TABLE [dbo].[PasswordChangeOtp] ADD  DEFAULT ((0)) FOR [IsUsed]
GO
ALTER TABLE [dbo].[PasswordChangeOtp] ADD  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Receptionist] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Room] ADD  CONSTRAINT [DF_Room_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Room] ADD  CONSTRAINT [DF_Room_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD  CONSTRAINT [FK_Account_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([Id])
GO
ALTER TABLE [dbo].[Account] CHECK CONSTRAINT [FK_Account_Role]
GO
ALTER TABLE [dbo].[AuditLog]  WITH CHECK ADD  CONSTRAINT [FK_AuditLog_Account] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[AuditLog] CHECK CONSTRAINT [FK_AuditLog_Account]
GO
ALTER TABLE [dbo].[AuditLog]  WITH CHECK ADD  CONSTRAINT [FK_AuditLog_Class] FOREIGN KEY([ClassId])
REFERENCES [dbo].[Class] ([Id])
GO
ALTER TABLE [dbo].[AuditLog] CHECK CONSTRAINT [FK_AuditLog_Class]
GO
ALTER TABLE [dbo].[AuditLog]  WITH CHECK ADD  CONSTRAINT [FK_AuditLog_Session] FOREIGN KEY([SessionId])
REFERENCES [dbo].[ClassSession] ([Id])
GO
ALTER TABLE [dbo].[AuditLog] CHECK CONSTRAINT [FK_AuditLog_Session]
GO
ALTER TABLE [dbo].[CenterManager]  WITH CHECK ADD  CONSTRAINT [FK_CenterManager_Account] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[CenterManager] CHECK CONSTRAINT [FK_CenterManager_Account]
GO
ALTER TABLE [dbo].[Class]  WITH CHECK ADD  CONSTRAINT [FK_Class_CreatedBy] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[Class] CHECK CONSTRAINT [FK_Class_CreatedBy]
GO
ALTER TABLE [dbo].[Class]  WITH CHECK ADD  CONSTRAINT [FK_Class_Discipline] FOREIGN KEY([DisciplineId])
REFERENCES [dbo].[Discipline] ([Id])
GO
ALTER TABLE [dbo].[Class] CHECK CONSTRAINT [FK_Class_Discipline]
GO
ALTER TABLE [dbo].[ClassChangeEvent]  WITH CHECK ADD  CONSTRAINT [FK_ClassChangeEvent_Actor] FOREIGN KEY([ActorAccountId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[ClassChangeEvent] CHECK CONSTRAINT [FK_ClassChangeEvent_Actor]
GO
ALTER TABLE [dbo].[ClassChangeEvent]  WITH CHECK ADD  CONSTRAINT [FK_ClassChangeEvent_Session] FOREIGN KEY([SessionId])
REFERENCES [dbo].[ClassSession] ([Id])
GO
ALTER TABLE [dbo].[ClassChangeEvent] CHECK CONSTRAINT [FK_ClassChangeEvent_Session]
GO
ALTER TABLE [dbo].[ClassRegistration]  WITH CHECK ADD  CONSTRAINT [FK_ClassRegistration_CanceledBy] FOREIGN KEY([CanceledBy])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[ClassRegistration] CHECK CONSTRAINT [FK_ClassRegistration_CanceledBy]
GO
ALTER TABLE [dbo].[ClassRegistration]  WITH CHECK ADD  CONSTRAINT [FK_ClassRegistration_ClassSession] FOREIGN KEY([SessionId])
REFERENCES [dbo].[ClassSession] ([Id])
GO
ALTER TABLE [dbo].[ClassRegistration] CHECK CONSTRAINT [FK_ClassRegistration_ClassSession]
GO
ALTER TABLE [dbo].[ClassRegistration]  WITH CHECK ADD  CONSTRAINT [FK_ClassRegistration_Member] FOREIGN KEY([MemberId])
REFERENCES [dbo].[Member] ([AccountId])
GO
ALTER TABLE [dbo].[ClassRegistration] CHECK CONSTRAINT [FK_ClassRegistration_Member]
GO
ALTER TABLE [dbo].[ClassRegistration]  WITH CHECK ADD  CONSTRAINT [FK_ClassRegistration_RegisteredBy] FOREIGN KEY([RegisteredBy])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[ClassRegistration] CHECK CONSTRAINT [FK_ClassRegistration_RegisteredBy]
GO
ALTER TABLE [dbo].[ClassRegistration]  WITH CHECK ADD  CONSTRAINT [FK_ClassRegistration_Subscription] FOREIGN KEY([MemberSubscriptionId])
REFERENCES [dbo].[MemberSubscription] ([Id])
GO
ALTER TABLE [dbo].[ClassRegistration] CHECK CONSTRAINT [FK_ClassRegistration_Subscription]
GO
ALTER TABLE [dbo].[ClassSession]  WITH CHECK ADD  CONSTRAINT [FK_ClassSession_CanceledBy] FOREIGN KEY([CanceledBy])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[ClassSession] CHECK CONSTRAINT [FK_ClassSession_CanceledBy]
GO
ALTER TABLE [dbo].[ClassSession]  WITH CHECK ADD  CONSTRAINT [FK_ClassSession_Class] FOREIGN KEY([ClassId])
REFERENCES [dbo].[Class] ([Id])
GO
ALTER TABLE [dbo].[ClassSession] CHECK CONSTRAINT [FK_ClassSession_Class]
GO
ALTER TABLE [dbo].[ClassSession]  WITH CHECK ADD  CONSTRAINT [FK_ClassSession_Coach] FOREIGN KEY([CoachId])
REFERENCES [dbo].[Coach] ([AccountId])
GO
ALTER TABLE [dbo].[ClassSession] CHECK CONSTRAINT [FK_ClassSession_Coach]
GO
ALTER TABLE [dbo].[ClassSession]  WITH CHECK ADD  CONSTRAINT [FK_ClassSession_CreatedBy] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[ClassSession] CHECK CONSTRAINT [FK_ClassSession_CreatedBy]
GO
ALTER TABLE [dbo].[ClassSession]  WITH CHECK ADD  CONSTRAINT [FK_ClassSession_Room] FOREIGN KEY([RoomId])
REFERENCES [dbo].[Room] ([Id])
GO
ALTER TABLE [dbo].[ClassSession] CHECK CONSTRAINT [FK_ClassSession_Room]
GO
ALTER TABLE [dbo].[Coach]  WITH CHECK ADD  CONSTRAINT [FK_Coach_Account] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[Coach] CHECK CONSTRAINT [FK_Coach_Account]
GO
ALTER TABLE [dbo].[CoachDiscipline]  WITH CHECK ADD  CONSTRAINT [FK_CoachDiscipline_Coach] FOREIGN KEY([CoachAccountId])
REFERENCES [dbo].[Coach] ([AccountId])
GO
ALTER TABLE [dbo].[CoachDiscipline] CHECK CONSTRAINT [FK_CoachDiscipline_Coach]
GO
ALTER TABLE [dbo].[CoachDiscipline]  WITH CHECK ADD  CONSTRAINT [FK_CoachDiscipline_Discipline] FOREIGN KEY([DisciplineId])
REFERENCES [dbo].[Discipline] ([Id])
GO
ALTER TABLE [dbo].[CoachDiscipline] CHECK CONSTRAINT [FK_CoachDiscipline_Discipline]
GO
ALTER TABLE [dbo].[Member]  WITH CHECK ADD  CONSTRAINT [FK_Member_Account] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[Member] CHECK CONSTRAINT [FK_Member_Account]
GO
ALTER TABLE [dbo].[MembershipInvoice]  WITH CHECK ADD  CONSTRAINT [FK_MembershipInvoice_CreatedBy] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[MembershipInvoice] CHECK CONSTRAINT [FK_MembershipInvoice_CreatedBy]
GO
ALTER TABLE [dbo].[MembershipInvoice]  WITH CHECK ADD  CONSTRAINT [FK_MembershipInvoice_Member] FOREIGN KEY([MemberId])
REFERENCES [dbo].[Member] ([AccountId])
GO
ALTER TABLE [dbo].[MembershipInvoice] CHECK CONSTRAINT [FK_MembershipInvoice_Member]
GO
ALTER TABLE [dbo].[MembershipInvoice]  WITH CHECK ADD  CONSTRAINT [FK_MembershipInvoice_PaidBy] FOREIGN KEY([PaidBy])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[MembershipInvoice] CHECK CONSTRAINT [FK_MembershipInvoice_PaidBy]
GO
ALTER TABLE [dbo].[MembershipInvoice]  WITH CHECK ADD  CONSTRAINT [FK_MembershipInvoice_Subscription] FOREIGN KEY([SubscriptionId])
REFERENCES [dbo].[MemberSubscription] ([Id])
GO
ALTER TABLE [dbo].[MembershipInvoice] CHECK CONSTRAINT [FK_MembershipInvoice_Subscription]
GO
ALTER TABLE [dbo].[MembershipPackageSport]  WITH CHECK ADD  CONSTRAINT [FK_MembershipPackageSport_Discipline] FOREIGN KEY([DisciplineId])
REFERENCES [dbo].[Discipline] ([Id])
GO
ALTER TABLE [dbo].[MembershipPackageSport] CHECK CONSTRAINT [FK_MembershipPackageSport_Discipline]
GO
ALTER TABLE [dbo].[MembershipPackageSport]  WITH CHECK ADD  CONSTRAINT [FK_MembershipPackageSport_Package] FOREIGN KEY([PackageId])
REFERENCES [dbo].[MembershipPackage] ([Id])
GO
ALTER TABLE [dbo].[MembershipPackageSport] CHECK CONSTRAINT [FK_MembershipPackageSport_Package]
GO
ALTER TABLE [dbo].[MemberSubscription]  WITH CHECK ADD  CONSTRAINT [FK_MemberSubscription_Member] FOREIGN KEY([MemberId])
REFERENCES [dbo].[Member] ([AccountId])
GO
ALTER TABLE [dbo].[MemberSubscription] CHECK CONSTRAINT [FK_MemberSubscription_Member]
GO
ALTER TABLE [dbo].[MemberSubscription]  WITH CHECK ADD  CONSTRAINT [FK_MemberSubscription_MembershipPackage] FOREIGN KEY([PackageId])
REFERENCES [dbo].[MembershipPackage] ([Id])
GO
ALTER TABLE [dbo].[MemberSubscription] CHECK CONSTRAINT [FK_MemberSubscription_MembershipPackage]
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD  CONSTRAINT [FK_Notification_Event] FOREIGN KEY([EventId])
REFERENCES [dbo].[ClassChangeEvent] ([Id])
GO
ALTER TABLE [dbo].[Notification] CHECK CONSTRAINT [FK_Notification_Event]
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD  CONSTRAINT [FK_Notification_Recipient] FOREIGN KEY([RecipientId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[Notification] CHECK CONSTRAINT [FK_Notification_Recipient]
GO
ALTER TABLE [dbo].[PasswordChangeOtp]  WITH CHECK ADD  CONSTRAINT [FK_PasswordChangeOtp_Account] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[PasswordChangeOtp] CHECK CONSTRAINT [FK_PasswordChangeOtp_Account]
GO
ALTER TABLE [dbo].[Receptionist]  WITH CHECK ADD  CONSTRAINT [FK_Receptionist_Account] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[Receptionist] CHECK CONSTRAINT [FK_Receptionist_Account]
GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD  CONSTRAINT [CK_Account_Phone_Format] CHECK  (([Phone] IS NULL OR datalength([Phone])=(10) AND NOT ([Phone]) collate Latin1_General_100_BIN2 like '%[^0-9]%'))
GO
ALTER TABLE [dbo].[Account] CHECK CONSTRAINT [CK_Account_Phone_Format]
GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD  CONSTRAINT [CK_Account_Status] CHECK  (([Status]='Inactive' OR [Status]='Active'))
GO
ALTER TABLE [dbo].[Account] CHECK CONSTRAINT [CK_Account_Status]
GO
ALTER TABLE [dbo].[Class]  WITH CHECK ADD  CONSTRAINT [CK_Class_Capacity] CHECK  (([Capacity]>(0)))
GO
ALTER TABLE [dbo].[Class] CHECK CONSTRAINT [CK_Class_Capacity]
GO
ALTER TABLE [dbo].[Class]  WITH CHECK ADD  CONSTRAINT [CK_Class_Status] CHECK  (([Status]='ARCHIVED' OR [Status]='CLOSED' OR [Status]='OPEN' OR [Status]='DRAFT'))
GO
ALTER TABLE [dbo].[Class] CHECK CONSTRAINT [CK_Class_Status]
GO
ALTER TABLE [dbo].[ClassRegistration]  WITH CHECK ADD  CONSTRAINT [CK_ClassRegistration_RegisteredVia] CHECK  (([RegisteredVia]='RECEPTIONIST' OR [RegisteredVia]='MEMBER'))
GO
ALTER TABLE [dbo].[ClassRegistration] CHECK CONSTRAINT [CK_ClassRegistration_RegisteredVia]
GO
ALTER TABLE [dbo].[ClassRegistration]  WITH CHECK ADD  CONSTRAINT [CK_ClassRegistration_Status] CHECK  (([Status]='CANCELED' OR [Status]='BOOKED'))
GO
ALTER TABLE [dbo].[ClassRegistration] CHECK CONSTRAINT [CK_ClassRegistration_Status]
GO
ALTER TABLE [dbo].[ClassSession]  WITH CHECK ADD  CONSTRAINT [CK_ClassSession_BookedCount] CHECK  (([BookedCount]>=(0) AND [BookedCount]<=[Capacity]))
GO
ALTER TABLE [dbo].[ClassSession] CHECK CONSTRAINT [CK_ClassSession_BookedCount]
GO
ALTER TABLE [dbo].[ClassSession]  WITH CHECK ADD  CONSTRAINT [CK_ClassSession_Capacity] CHECK  (([Capacity]>(0)))
GO
ALTER TABLE [dbo].[ClassSession] CHECK CONSTRAINT [CK_ClassSession_Capacity]
GO
ALTER TABLE [dbo].[ClassSession]  WITH CHECK ADD  CONSTRAINT [CK_ClassSession_Status] CHECK  (([Status]='COMPLETED' OR [Status]='CANCELED' OR [Status]='SCHEDULED'))
GO
ALTER TABLE [dbo].[ClassSession] CHECK CONSTRAINT [CK_ClassSession_Status]
GO
ALTER TABLE [dbo].[ClassSession]  WITH CHECK ADD  CONSTRAINT [CK_ClassSession_Time] CHECK  (([StartTime]<[EndTime]))
GO
ALTER TABLE [dbo].[ClassSession] CHECK CONSTRAINT [CK_ClassSession_Time]
GO
ALTER TABLE [dbo].[Discipline]  WITH CHECK ADD  CONSTRAINT [CK_Discipline_Name] CHECK  ((len(ltrim(rtrim([Name])))>=(2) AND len(ltrim(rtrim([Name])))<=(80) AND datalength([Name])=datalength(ltrim(rtrim([Name])))))
GO
ALTER TABLE [dbo].[Discipline] CHECK CONSTRAINT [CK_Discipline_Name]
GO
ALTER TABLE [dbo].[MembershipInvoice]  WITH CHECK ADD  CONSTRAINT [CK_MembershipInvoice_Amount] CHECK  (([Amount]>=(1) AND [Amount]<=(1000000000)))
GO
ALTER TABLE [dbo].[MembershipInvoice] CHECK CONSTRAINT [CK_MembershipInvoice_Amount]
GO
ALTER TABLE [dbo].[MembershipInvoice]  WITH CHECK ADD  CONSTRAINT [CK_MembershipInvoice_PaymentMethod] CHECK  (([PaymentMethod]='CARD' OR [PaymentMethod]='BANK_TRANSFER' OR [PaymentMethod]='CASH'))
GO
ALTER TABLE [dbo].[MembershipInvoice] CHECK CONSTRAINT [CK_MembershipInvoice_PaymentMethod]
GO
ALTER TABLE [dbo].[MembershipInvoice]  WITH CHECK ADD  CONSTRAINT [CK_MembershipInvoice_Status] CHECK  (([Status]='CANCELED' OR [Status]='PAID' OR [Status]='PENDING_PAYMENT'))
GO
ALTER TABLE [dbo].[MembershipInvoice] CHECK CONSTRAINT [CK_MembershipInvoice_Status]
GO
ALTER TABLE [dbo].[MembershipPackage]  WITH CHECK ADD  CONSTRAINT [CK_MembershipPackage_Benefits_Json] CHECK  ((isjson([Benefits])=(1)))
GO
ALTER TABLE [dbo].[MembershipPackage] CHECK CONSTRAINT [CK_MembershipPackage_Benefits_Json]
GO
ALTER TABLE [dbo].[MembershipPackage]  WITH CHECK ADD  CONSTRAINT [CK_MembershipPackage_DurationMonths] CHECK  (([DurationMonths]=(12) OR [DurationMonths]=(3) OR [DurationMonths]=(1)))
GO
ALTER TABLE [dbo].[MembershipPackage] CHECK CONSTRAINT [CK_MembershipPackage_DurationMonths]
GO
ALTER TABLE [dbo].[MembershipPackage]  WITH CHECK ADD  CONSTRAINT [CK_MembershipPackage_Name] CHECK  ((len(ltrim(rtrim([Name])))>=(2) AND len(ltrim(rtrim([Name])))<=(80) AND datalength([Name])=datalength(ltrim(rtrim([Name])))))
GO
ALTER TABLE [dbo].[MembershipPackage] CHECK CONSTRAINT [CK_MembershipPackage_Name]
GO
ALTER TABLE [dbo].[MembershipPackage]  WITH CHECK ADD  CONSTRAINT [CK_MembershipPackage_Price] CHECK  (([Price]>=(1) AND [Price]<=(1000000000)))
GO
ALTER TABLE [dbo].[MembershipPackage] CHECK CONSTRAINT [CK_MembershipPackage_Price]
GO
ALTER TABLE [dbo].[MemberSubscription]  WITH CHECK ADD  CONSTRAINT [CK_MemberSubscription_Benefits_Json] CHECK  ((isjson([Benefits])=(1)))
GO
ALTER TABLE [dbo].[MemberSubscription] CHECK CONSTRAINT [CK_MemberSubscription_Benefits_Json]
GO
ALTER TABLE [dbo].[MemberSubscription]  WITH CHECK ADD  CONSTRAINT [CK_MemberSubscription_Dates] CHECK  (([EndDate]>=[StartDate]))
GO
ALTER TABLE [dbo].[MemberSubscription] CHECK CONSTRAINT [CK_MemberSubscription_Dates]
GO
ALTER TABLE [dbo].[MemberSubscription]  WITH CHECK ADD  CONSTRAINT [CK_MemberSubscription_DurationMonths] CHECK  (([DurationMonths]=(12) OR [DurationMonths]=(3) OR [DurationMonths]=(1)))
GO
ALTER TABLE [dbo].[MemberSubscription] CHECK CONSTRAINT [CK_MemberSubscription_DurationMonths]
GO
ALTER TABLE [dbo].[MemberSubscription]  WITH CHECK ADD  CONSTRAINT [CK_MemberSubscription_Kind] CHECK  (([Kind]='RENEW' OR [Kind]='REGISTER'))
GO
ALTER TABLE [dbo].[MemberSubscription] CHECK CONSTRAINT [CK_MemberSubscription_Kind]
GO
ALTER TABLE [dbo].[MemberSubscription]  WITH CHECK ADD  CONSTRAINT [CK_MemberSubscription_PackagePrice] CHECK  (([PackagePrice]>=(1) AND [PackagePrice]<=(1000000000)))
GO
ALTER TABLE [dbo].[MemberSubscription] CHECK CONSTRAINT [CK_MemberSubscription_PackagePrice]
GO
ALTER TABLE [dbo].[MemberSubscription]  WITH CHECK ADD  CONSTRAINT [CK_MemberSubscription_Status] CHECK  (([Status]='CANCELED' OR [Status]='CONFIRMED' OR [Status]='PENDING_PAYMENT'))
GO
ALTER TABLE [dbo].[MemberSubscription] CHECK CONSTRAINT [CK_MemberSubscription_Status]
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD  CONSTRAINT [CK_Notification_Type] CHECK  (([Type]='SESSION_CANCELED' OR [Type]='SESSION_COACH_CHANGED' OR [Type]='SESSION_RESCHEDULED' OR [Type]='CLASS_REGISTERED'))
GO
ALTER TABLE [dbo].[Notification] CHECK CONSTRAINT [CK_Notification_Type]
GO
ALTER TABLE [dbo].[PasswordChangeOtp]  WITH CHECK ADD  CONSTRAINT [CK_PasswordChangeOtp_Attempts] CHECK  (([FailedAttempts]>=(0) AND [MaxAttempts]>(0) AND [FailedAttempts]<=[MaxAttempts]))
GO
ALTER TABLE [dbo].[PasswordChangeOtp] CHECK CONSTRAINT [CK_PasswordChangeOtp_Attempts]
GO
ALTER TABLE [dbo].[PasswordChangeOtp]  WITH CHECK ADD  CONSTRAINT [CK_PasswordChangeOtp_Dates] CHECK  (([ExpiresAt]>[CreatedAt]))
GO
ALTER TABLE [dbo].[PasswordChangeOtp] CHECK CONSTRAINT [CK_PasswordChangeOtp_Dates]
GO
ALTER TABLE [dbo].[Room]  WITH CHECK ADD  CONSTRAINT [CK_Room_Capacity] CHECK  (([Capacity]>(0)))
GO
ALTER TABLE [dbo].[Room] CHECK CONSTRAINT [CK_Room_Capacity]
GO
ALTER TABLE [dbo].[Room]  WITH CHECK ADD  CONSTRAINT [CK_Room_RoomType] CHECK  (([RoomType] IS NULL OR ([RoomType]='OTHER' OR [RoomType]='COURT' OR [RoomType]='POOL' OR [RoomType]='GYM' OR [RoomType]='STUDIO')))
GO
ALTER TABLE [dbo].[Room] CHECK CONSTRAINT [CK_Room_RoomType]
GO
USE [SportsCenterManagement]
GO

CREATE TABLE [dbo].[ClassAttendance]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [ClassRegistrationId] int NOT NULL,
    [SessionId] int NOT NULL,
    [MemberId] varchar(400) NOT NULL,
    [IsPresent] bit NOT NULL CONSTRAINT [DF_ClassAttendance_IsPresent] DEFAULT ((0)),
    [Notes] nvarchar(500) NULL,
    [MarkedBy] varchar(400) NULL,
    [MarkedAt] datetime2(7) NULL CONSTRAINT [DF_ClassAttendance_MarkedAt] DEFAULT (sysutcdatetime()),
    CONSTRAINT [PK_ClassAttendance] PRIMARY KEY CLUSTERED ([Id]),
    CONSTRAINT [UQ_ClassAttendance_Registration] UNIQUE ([ClassRegistrationId]),
    CONSTRAINT [FK_ClassAttendance_Registration] FOREIGN KEY ([ClassRegistrationId]) REFERENCES [dbo].[ClassRegistration] ([Id]),
    CONSTRAINT [FK_ClassAttendance_Session] FOREIGN KEY ([SessionId]) REFERENCES [dbo].[ClassSession] ([Id]),
    CONSTRAINT [FK_ClassAttendance_Member] FOREIGN KEY ([MemberId]) REFERENCES [dbo].[Member] ([AccountId]),
    CONSTRAINT [FK_ClassAttendance_MarkedBy] FOREIGN KEY ([MarkedBy]) REFERENCES [dbo].[Account] ([Id])
);
GO

CREATE TABLE [dbo].[Feedback]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [ClassAttendanceId] int NOT NULL,
    [Rating] int NOT NULL,
    [Comment] nvarchar(1000) NULL,
    [CreatedAt] datetime2(7) NOT NULL CONSTRAINT [DF_Feedback_CreatedAt] DEFAULT (sysutcdatetime()),
    CONSTRAINT [PK_Feedback] PRIMARY KEY CLUSTERED ([Id]),
    CONSTRAINT [UQ_Feedback_ClassAttendance] UNIQUE ([ClassAttendanceId]),
    CONSTRAINT [CK_Feedback_Rating] CHECK ([Rating] BETWEEN 1 AND 5),
    CONSTRAINT [FK_Feedback_ClassAttendance] FOREIGN KEY ([ClassAttendanceId]) REFERENCES [dbo].[ClassAttendance] ([Id])
);
GO

USE [master]
GO
ALTER DATABASE [SportsCenterManagement] SET READ_WRITE
GO
