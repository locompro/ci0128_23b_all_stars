USE [Locompro]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 10/2/2023 9:19:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](40) NULL,
	[Address] [nvarchar](50) NULL,
	[Rating] [real] NOT NULL,
	[Status] [int] NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Canton]    Script Date: 10/2/2023 9:19:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Canton](
	[CountryName] [nvarchar](450) NOT NULL,
	[ProvinceName] [nvarchar](60) NOT NULL,
	[Name] [nvarchar](60) NOT NULL,
 CONSTRAINT [PK_Canton] PRIMARY KEY CLUSTERED 
(
	[CountryName] ASC,
	[ProvinceName] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 10/2/2023 9:19:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[Name] [nvarchar](60) NOT NULL,
	[ParentName] [nvarchar](60) NULL,
	[ProductId] [int] NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Country]    Script Date: 10/2/2023 9:19:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Country](
	[Name] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_Country] PRIMARY KEY CLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 10/2/2023 9:19:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](60) NOT NULL,
	[Model] [nvarchar](60) NULL,
	[Brand] [nvarchar](60) NULL,
	[BarCode] [nvarchar](13) NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Province]    Script Date: 10/2/2023 9:19:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Province](
	[CountryName] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](60) NOT NULL,
 CONSTRAINT [PK_Province] PRIMARY KEY CLUSTERED 
(
	[CountryName] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Store]    Script Date: 10/2/2023 9:19:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Store](
	[Name] [nvarchar](60) NOT NULL,
	[CantonCountryName] [nvarchar](450) NOT NULL,
	[CantonProvinceName] [nvarchar](60) NOT NULL,
	[CantonName] [nvarchar](60) NOT NULL,
	[Address] [nvarchar](35) NOT NULL,
	[Telephone] [nvarchar](15) NOT NULL,
	[Status] [int] NOT NULL,
	[Latitude] [decimal](18, 2) NOT NULL,
	[Longitude] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_Store] PRIMARY KEY CLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Submission]    Script Date: 10/2/2023 9:19:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Submission](
	[Username] [nvarchar](450) NOT NULL,
	[EntryTime] [datetime2](7) NOT NULL,
	[Status] [int] NOT NULL,
	[Price] [int] NOT NULL,
	[Rating] [real] NOT NULL,
	[Description] [int] NOT NULL,
	[StoreName] [nvarchar](60) NOT NULL,
	[ProductId] [int] NOT NULL,
 CONSTRAINT [PK_Submission] PRIMARY KEY CLUSTERED 
(
	[Username] ASC,
	[EntryTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Canton]  WITH CHECK ADD  CONSTRAINT [FK_Canton_Province_CountryName_ProvinceName] FOREIGN KEY([CountryName], [ProvinceName])
REFERENCES [dbo].[Province] ([CountryName], [Name])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Canton] CHECK CONSTRAINT [FK_Canton_Province_CountryName_ProvinceName]
GO
ALTER TABLE [dbo].[Category]  WITH CHECK ADD  CONSTRAINT [FK_Category_Category_ParentName] FOREIGN KEY([ParentName])
REFERENCES [dbo].[Category] ([Name])
GO
ALTER TABLE [dbo].[Category] CHECK CONSTRAINT [FK_Category_Category_ParentName]
GO
ALTER TABLE [dbo].[Category]  WITH CHECK ADD  CONSTRAINT [FK_Category_Product_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
GO
ALTER TABLE [dbo].[Category] CHECK CONSTRAINT [FK_Category_Product_ProductId]
GO
ALTER TABLE [dbo].[Province]  WITH CHECK ADD  CONSTRAINT [FK_Province_Country_CountryName] FOREIGN KEY([CountryName])
REFERENCES [dbo].[Country] ([Name])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Province] CHECK CONSTRAINT [FK_Province_Country_CountryName]
GO
ALTER TABLE [dbo].[Store]  WITH CHECK ADD  CONSTRAINT [FK_Store_Canton_CantonCountryName_CantonProvinceName_CantonName] FOREIGN KEY([CantonCountryName], [CantonProvinceName], [CantonName])
REFERENCES [dbo].[Canton] ([CountryName], [ProvinceName], [Name])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Store] CHECK CONSTRAINT [FK_Store_Canton_CantonCountryName_CantonProvinceName_CantonName]
GO
ALTER TABLE [dbo].[Submission]  WITH CHECK ADD  CONSTRAINT [FK_Submission_Product_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Submission] CHECK CONSTRAINT [FK_Submission_Product_ProductId]
GO
ALTER TABLE [dbo].[Submission]  WITH CHECK ADD  CONSTRAINT [FK_Submission_Store_StoreName] FOREIGN KEY([StoreName])
REFERENCES [dbo].[Store] ([Name])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Submission] CHECK CONSTRAINT [FK_Submission_Store_StoreName]
GO
