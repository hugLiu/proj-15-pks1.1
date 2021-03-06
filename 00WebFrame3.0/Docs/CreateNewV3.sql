/*该脚本用于创建一个全新的框架数据库中的所有表, 必须先建立一个空白数据库再在其上运行*/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[Base_Article](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](200) NULL,
	[Keywords] [nvarchar](200) NULL,
	[Abstract] [nvarchar](200) NULL,
	[UrlTitle] [varchar](50) NULL,
	[Author] [nvarchar](50) NULL,
	[EditorId] [int] NOT NULL,
	[CreateTime] [datetime2](7) NOT NULL,
	[EditTime] [datetime2](7) NOT NULL,
	[State] [int] NOT NULL,
	[Clicks] [int] NOT NULL CONSTRAINT [DF_Base_Article_Clicks]  DEFAULT ((0)),
	[Options] [int] NOT NULL DEFAULT ((0)),
	[IsDeleted] [bit] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_CT_Article] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Base_ArticleExt]    Script Date: 2017/4/20 14:00:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Base_ArticleExt](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ArticleId] [int] NOT NULL,
	[CatlogExtId] [int] NOT NULL,
	[Value] [nvarchar](400) NULL,
 CONSTRAINT [PK_Base_ArticleAttribute] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Base_ArticleRelation]    Script Date: 2017/4/20 14:00:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Base_ArticleRelation](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SourceId] [int] NOT NULL,
	[TargetId] [int] NOT NULL,
	[RelationType] [int] NOT NULL,
	[Ord] [int] NOT NULL,
 CONSTRAINT [PK_Base_ArticleRelation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Base_ArticleText]    Script Date: 2017/4/20 14:00:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Base_ArticleText](
	[Id] [int] NOT NULL,
	[TEXT] [nvarchar](max) NULL,
 CONSTRAINT [PK_Base_ArticleText] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Base_Catalog]    Script Date: 2017/4/20 14:00:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Base_Catalog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ParentId] [int] NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Abstract] [nvarchar](200) NULL,
	[Location] [varchar](200) NULL,
	[IconLocation] [varchar](200) NULL,
	[Language] [varchar](20) NULL,
	[Ord] [int] NOT NULL,
	[CreateTime] [datetime2](7) NULL,
	[EditTime] [datetime2](7) NULL,
	[EditorId] [int] NULL,
	[OwnerId] [int] NOT NULL DEFAULT ((0)),
	[OwnerType] [int] NOT NULL DEFAULT ((0)),
	[State] [int] NOT NULL,
	[Options] [int] NOT NULL DEFAULT ((0)),
	[IsDeleted] [bit] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_CT_Catalog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[ParentId] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Base_CatalogArticle]    Script Date: 2017/4/20 14:00:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Base_CatalogArticle](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CatalogId] [int] NOT NULL,
	[ArticleId] [int] NOT NULL,
	[CreateTime] [datetime2](7) NOT NULL,
	[Ord] [int] NOT NULL DEFAULT ((0)),
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[CatalogId] ASC,
	[ArticleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Base_CatalogExt]    Script Date: 2017/4/20 14:00:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Base_CatalogExt](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CatalogId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[DataType] [int] NOT NULL,
	[DefaultValue] [nvarchar](200) NULL,
	[DataSourceType] [int] NOT NULL,
	[DataSource] [nvarchar](max) NULL,
	[Ord] [int] NOT NULL,
	[State] [int] NOT NULL,
	[AllowNull] [bit] NULL,
	[MaxLength] [int] NULL,
	[Options] [int] NOT NULL DEFAULT ((0)),
	[IsDeleted] [bit] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_Base_CatlogExt] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[CatalogId] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Dep_Department]    Script Date: 2017/4/20 14:00:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dep_Department](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ParentId] [int] NULL,
	[DepHID] [nvarchar](50) NULL,
	[Ord] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Remark] [nvarchar](200) NULL,
	[DepType] [nvarchar](20) NULL,
	[ExamineType] [nvarchar](20) NULL,
	[IsActive] [int] NOT NULL CONSTRAINT [DF_Dep_Department_IsActive]  DEFAULT ((1)),
	[IsDisabled] [int] NOT NULL CONSTRAINT [DF_Dep_Department_IsDisabled]  DEFAULT ((0)),
	[IsDeleted] [int] NOT NULL CONSTRAINT [DF_Dep_Department_Deleted]  DEFAULT ((0)),
	[CreateDatetime] [datetime] NOT NULL CONSTRAINT [DF_Dep_Department_CreateDatetime]  DEFAULT (getdate()),
	[ModifiedDateTime] [datetime] NOT NULL CONSTRAINT [DF_Dep_Department_ModifiedDateTime]  DEFAULT (getdate()),
	[OrgNode] [varchar](100) NULL,
 CONSTRAINT [PK_Sys_Department] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dep_DepHistory]    Script Date: 2017/4/20 14:00:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dep_DepHistory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DepId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Dep_DepPost]    Script Date: 2017/4/20 14:00:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dep_DepPost](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DepId] [int] NOT NULL,
	[PostId] [int] NOT NULL,
	[Name] [nvarchar](50) NULL,
	[PlanNumber] [int] NULL,
	[ExamineType] [nvarchar](20) NULL,
	[Describe] [nvarchar](512) NULL,
	[Duty] [nvarchar](512) NULL,
	[Requirement] [nvarchar](512) NULL,
	[IsActive] [int] NOT NULL,
	[IsDisabled] [int] NOT NULL,
	[IsDeleted] [int] NOT NULL,
	[CreateDatetime] [datetime] NOT NULL,
 CONSTRAINT [PK__tmp_ms_x__3214EC07282DF8C2] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Dep_DepUser]    Script Date: 2017/4/20 14:00:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dep_DepUser](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DepId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[UserName] [nvarchar](10) NOT NULL,
	[ExamineType] [nvarchar](20) NOT NULL,
	[ContractType] [nvarchar](20) NULL,
	[ContractLenght] [int] NULL,
	[JoinDateTime] [date] NOT NULL,
	[OutDateTime] [date] NULL,
	[CreateDatetime] [datetime] NOT NULL CONSTRAINT [DF_Dep_DepUser_CreateDatetime]  DEFAULT (getdate()),
	[IsSuspension] [int] NOT NULL CONSTRAINT [DF_Dep_DepUser_IsSuspension]  DEFAULT ((0)),
	[IsDeleted] [int] NOT NULL CONSTRAINT [DF_Dep_DepUser_IsDeleted]  DEFAULT ((0)),
	[IsLeader] [int] NULL,
	[IsMain] [int] NULL,
	[PostId] [int] NULL,
 CONSTRAINT [PK_Dep_DepUser] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Dep_Post]    Script Date: 2017/4/20 14:00:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dep_Post](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PostName] [nvarchar](50) NOT NULL,
	[PostType] [nvarchar](20) NOT NULL,
	[PostLevelType] [nvarchar](20) NULL,
	[PostEngageType] [nvarchar](20) NULL,
	[OperatorID] [int] NOT NULL,
	[CreateDatetime] [datetime] NOT NULL,
	[IsDeleted] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Dep_PostHistory]    Script Date: 2017/4/20 14:00:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dep_PostHistory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DepPostId] [int] NOT NULL,
	[ChangeType] [nvarchar](20) NOT NULL,
	[ChangeExplain] [nvarchar](128) NULL,
	[DepId] [int] NOT NULL,
	[PostId] [int] NOT NULL,
	[Name] [nvarchar](50) NULL,
	[ExamineType] [nvarchar](20) NOT NULL,
	[IsActive] [int] NOT NULL,
	[IsDisabled] [int] NOT NULL,
	[IsDeleted] [int] NOT NULL,
	[CreateDatetime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Dep_PostUser]    Script Date: 2017/4/20 14:00:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dep_PostUser](
	[DepPostId] [int] NOT NULL,
	[DepUserId] [int] NOT NULL,
	[OperationId] [int] NOT NULL,
	[CreateDatetime] [datetime] NOT NULL,
	[ModifiedDateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Dep_PostUser] PRIMARY KEY CLUSTERED 
(
	[DepPostId] ASC,
	[DepUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Dep_UserHistory]    Script Date: 2017/4/20 14:00:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dep_UserHistory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_UserHistory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SchemaVersion]    Script Date: 2017/4/20 14:00:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SchemaVersion](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Version] [nvarchar](50) NULL,
	[UpdateTime] [datetime] NULL,
	[Remark] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Sys_BasicSettings]    Script Date: 2017/4/20 14:00:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sys_BasicSettings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Pid] [int] NOT NULL,
	[Scope] [nvarchar](50) NULL,
	[PreferencesItemName] [nvarchar](50) NOT NULL,
	[Sequence] [int] NOT NULL,
	[Value1] [nvarchar](128) NOT NULL,
	[Value2] [nvarchar](128) NULL,
	[Description] [nvarchar](512) NULL,
	[EffectivedStartDateTime] [datetime] NULL,
	[EffectivedEndDateTime] [datetime] NULL,
	[CreateDatetime] [datetime] NOT NULL,
	[ModifiedDateTime] [datetime] NOT NULL,
 CONSTRAINT [PK__Sys_Basi__3214EC070F624AF8] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Sys_Log]    Script Date: 2017/4/20 14:00:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Sys_Log](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ModuleName] [nvarchar](50) NOT NULL,
	[ActionName] [nvarchar](50) NOT NULL,
	[ClientIP] [varchar](20) NULL,
	[UserName] [varchar](50) NULL,
	[OpTime] [datetime2](7) NULL,
	[CatalogId] [int] NULL,
	[ObjectId] [int] NULL,
	[LogType] [varchar](20) NULL,
	[Request] [varchar](2000) NULL,
	[Costs] [float] NULL,
	[Message] [nvarchar](2000) NULL,
	[Browser] [varchar](50) NULL,
	[BrowserVersion] [numeric](18, 4) NULL,
	[Platform] [varchar](50) NULL,
 CONSTRAINT [PK_Sys_Log] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserProfile]    Script Date: 2017/4/20 14:00:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserProfile](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](56) NOT NULL,
	[Email] [varchar](200) NULL,
	[PhoneNumber] [varchar](50) NULL,
	[AvatarId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[webpages_Membership]    Script Date: 2017/4/20 14:00:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[webpages_Membership](
	[UserId] [int] NOT NULL,
	[CreateDate] [datetime] NULL,
	[ConfirmationToken] [nvarchar](128) NULL,
	[IsConfirmed] [bit] NULL DEFAULT ((0)),
	[LastPasswordFailureDate] [datetime] NULL,
	[PasswordFailuresSinceLastSuccess] [int] NOT NULL DEFAULT ((0)),
	[Password] [nvarchar](128) NOT NULL,
	[PasswordChangedDate] [datetime] NULL,
	[PasswordSalt] [nvarchar](128) NOT NULL,
	[PasswordVerificationToken] [nvarchar](128) NULL,
	[PasswordVerificationTokenExpirationDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[webpages_OAuthMembership]    Script Date: 2017/4/20 14:00:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[webpages_OAuthMembership](
	[Provider] [nvarchar](30) NOT NULL,
	[ProviderUserId] [nvarchar](100) NOT NULL,
	[UserId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Provider] ASC,
	[ProviderUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[webpages_Roles]    Script Date: 2017/4/20 14:00:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[webpages_Roles](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[RoleName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[webpages_UsersInRoles]    Script Date: 2017/4/20 14:00:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[webpages_UsersInRoles](
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [_dta_index_Base_Article_7_1474104292__K1_K2_K3_4_5_6_7_8_9_10_11_12_13]    Script Date: 2017/4/20 14:00:09 ******/
CREATE NONCLUSTERED INDEX [_dta_index_Base_Article_7_1474104292__K1_K2_K3_4_5_6_7_8_9_10_11_12_13] ON [dbo].[Base_Article]
(
	[Id] ASC,
	[Title] ASC,
	[Keywords] ASC
)
INCLUDE ( 	[Abstract],
	[UrlTitle],
	[Author],
	[EditorId],
	[CreateTime],
	[EditTime],
	[State],
	[Clicks],
	[Options],
	[IsDeleted]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [_dta_index_Base_Article_7_1474104292__K10_K7_K13_K1]    Script Date: 2017/4/20 14:00:09 ******/
CREATE NONCLUSTERED INDEX [_dta_index_Base_Article_7_1474104292__K10_K7_K13_K1] ON [dbo].[Base_Article]
(
	[State] ASC,
	[EditorId] ASC,
	[IsDeleted] ASC,
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [_dta_index_Base_Article_7_1474104292__K10_K7_K13_K1_2_12]    Script Date: 2017/4/20 14:00:09 ******/
CREATE NONCLUSTERED INDEX [_dta_index_Base_Article_7_1474104292__K10_K7_K13_K1_2_12] ON [dbo].[Base_Article]
(
	[State] ASC,
	[EditorId] ASC,
	[IsDeleted] ASC,
	[Id] ASC
)
INCLUDE ( 	[Title],
	[Options]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [_dta_index_Base_Article_7_1474104292__K13_K1]    Script Date: 2017/4/20 14:00:09 ******/
CREATE NONCLUSTERED INDEX [_dta_index_Base_Article_7_1474104292__K13_K1] ON [dbo].[Base_Article]
(
	[IsDeleted] ASC,
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Base_Article]    Script Date: 2017/4/20 14:00:09 ******/
CREATE NONCLUSTERED INDEX [IX_Base_Article] ON [dbo].[Base_Article]
(
	[State] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [_dta_index_Base_ArticleExt_7_459148681__K2_1_3_4]    Script Date: 2017/4/20 14:00:09 ******/
CREATE NONCLUSTERED INDEX [_dta_index_Base_ArticleExt_7_459148681__K2_1_3_4] ON [dbo].[Base_ArticleExt]
(
	[ArticleId] ASC
)
INCLUDE ( 	[Id],
	[CatlogExtId],
	[Value]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [_dta_index_Base_ArticleExt_7_459148681__K2_K3_K4]    Script Date: 2017/4/20 14:00:09 ******/
CREATE NONCLUSTERED INDEX [_dta_index_Base_ArticleExt_7_459148681__K2_K3_K4] ON [dbo].[Base_ArticleExt]
(
	[ArticleId] ASC,
	[CatlogExtId] ASC,
	[Value] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [_dta_index_Base_ArticleExt_7_459148681__K3_2]    Script Date: 2017/4/20 14:00:09 ******/
CREATE NONCLUSTERED INDEX [_dta_index_Base_ArticleExt_7_459148681__K3_2] ON [dbo].[Base_ArticleExt]
(
	[CatlogExtId] ASC
)
INCLUDE ( 	[ArticleId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [_dta_index_Base_ArticleExt_7_459148681__K3_K2]    Script Date: 2017/4/20 14:00:09 ******/
CREATE NONCLUSTERED INDEX [_dta_index_Base_ArticleExt_7_459148681__K3_K2] ON [dbo].[Base_ArticleExt]
(
	[CatlogExtId] ASC,
	[ArticleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [_dta_index_Base_ArticleRelation_7_1538104520__K2_K3_K4_1_5]    Script Date: 2017/4/20 14:00:09 ******/
CREATE NONCLUSTERED INDEX [_dta_index_Base_ArticleRelation_7_1538104520__K2_K3_K4_1_5] ON [dbo].[Base_ArticleRelation]
(
	[SourceId] ASC,
	[TargetId] ASC,
	[RelationType] ASC
)
INCLUDE ( 	[Id],
	[Ord]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Dep_DepPost] ADD  CONSTRAINT [DF_Dep_DepPost_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Dep_DepPost] ADD  CONSTRAINT [DF__Sys_DepPo__IsDis__395884C4]  DEFAULT ((0)) FOR [IsDisabled]
GO
ALTER TABLE [dbo].[Dep_DepPost] ADD  CONSTRAINT [DF__Sys_DepPo__IsDel__3A4CA8FD]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Dep_DepPost] ADD  CONSTRAINT [DF_Dep_DepPost_CreateDatetime]  DEFAULT (getdate()) FOR [CreateDatetime]
GO
ALTER TABLE [dbo].[Dep_Post] ADD  CONSTRAINT [DF_Dep_Post_CreateDatetime]  DEFAULT (getdate()) FOR [CreateDatetime]
GO
ALTER TABLE [dbo].[Dep_Post] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Dep_PostHistory] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Dep_PostHistory] ADD  DEFAULT ((0)) FOR [IsDisabled]
GO
ALTER TABLE [dbo].[Dep_PostHistory] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Dep_PostUser] ADD  CONSTRAINT [DF_Dep_PostUser_CreateDatetime]  DEFAULT (getdate()) FOR [CreateDatetime]
GO
ALTER TABLE [dbo].[Dep_PostUser] ADD  CONSTRAINT [DF_Dep_PostUser_ModifiedDateTime]  DEFAULT (getdate()) FOR [ModifiedDateTime]
GO
ALTER TABLE [dbo].[Sys_BasicSettings] ADD  CONSTRAINT [DF__Sys_Basic__Modif__160F4887]  DEFAULT (getdate()) FOR [ModifiedDateTime]
GO
ALTER TABLE [dbo].[Base_Article]  WITH CHECK ADD  CONSTRAINT [FK_Base_Article_UserProfile] FOREIGN KEY([EditorId])
REFERENCES [dbo].[UserProfile] ([UserId])
GO
ALTER TABLE [dbo].[Base_Article] CHECK CONSTRAINT [FK_Base_Article_UserProfile]
GO
ALTER TABLE [dbo].[Base_ArticleExt]  WITH CHECK ADD  CONSTRAINT [FK_Base_ArticleExt_Base_Article] FOREIGN KEY([ArticleId])
REFERENCES [dbo].[Base_Article] ([Id])
GO
ALTER TABLE [dbo].[Base_ArticleExt] CHECK CONSTRAINT [FK_Base_ArticleExt_Base_Article]
GO
ALTER TABLE [dbo].[Base_ArticleExt]  WITH CHECK ADD  CONSTRAINT [FK_Base_ArticleExt_Base_CatlogExt] FOREIGN KEY([CatlogExtId])
REFERENCES [dbo].[Base_CatalogExt] ([Id])
GO
ALTER TABLE [dbo].[Base_ArticleExt] CHECK CONSTRAINT [FK_Base_ArticleExt_Base_CatlogExt]
GO
ALTER TABLE [dbo].[Base_ArticleRelation]  WITH CHECK ADD  CONSTRAINT [FK_Base_ArticleRelation_Base_Article] FOREIGN KEY([SourceId])
REFERENCES [dbo].[Base_Article] ([Id])
GO
ALTER TABLE [dbo].[Base_ArticleRelation] CHECK CONSTRAINT [FK_Base_ArticleRelation_Base_Article]
GO
ALTER TABLE [dbo].[Base_ArticleRelation]  WITH CHECK ADD  CONSTRAINT [FK_Base_ArticleRelation_Base_Article1] FOREIGN KEY([TargetId])
REFERENCES [dbo].[Base_Article] ([Id])
GO
ALTER TABLE [dbo].[Base_ArticleRelation] CHECK CONSTRAINT [FK_Base_ArticleRelation_Base_Article1]
GO
ALTER TABLE [dbo].[Base_ArticleText]  WITH CHECK ADD  CONSTRAINT [FK_Base_ArticleText_Base_Article] FOREIGN KEY([Id])
REFERENCES [dbo].[Base_Article] ([Id])
GO
ALTER TABLE [dbo].[Base_ArticleText] CHECK CONSTRAINT [FK_Base_ArticleText_Base_Article]
GO
ALTER TABLE [dbo].[Base_Catalog]  WITH CHECK ADD  CONSTRAINT [FK_Base_Catalog_Base_Catalog] FOREIGN KEY([ParentId])
REFERENCES [dbo].[Base_Catalog] ([Id])
GO
ALTER TABLE [dbo].[Base_Catalog] CHECK CONSTRAINT [FK_Base_Catalog_Base_Catalog]
GO
ALTER TABLE [dbo].[Base_CatalogArticle]  WITH CHECK ADD  CONSTRAINT [FK_Base_Article_CatalogArticle] FOREIGN KEY([ArticleId])
REFERENCES [dbo].[Base_Article] ([Id])
GO
ALTER TABLE [dbo].[Base_CatalogArticle] CHECK CONSTRAINT [FK_Base_Article_CatalogArticle]
GO
ALTER TABLE [dbo].[Base_CatalogArticle]  WITH CHECK ADD  CONSTRAINT [FK_Base_Catalog_CatalogArticle] FOREIGN KEY([CatalogId])
REFERENCES [dbo].[Base_Catalog] ([Id])
GO
ALTER TABLE [dbo].[Base_CatalogArticle] CHECK CONSTRAINT [FK_Base_Catalog_CatalogArticle]
GO
ALTER TABLE [dbo].[Base_CatalogExt]  WITH CHECK ADD  CONSTRAINT [FK_Base_CatlogExt_Base_Catalog] FOREIGN KEY([CatalogId])
REFERENCES [dbo].[Base_Catalog] ([Id])
GO
ALTER TABLE [dbo].[Base_CatalogExt] CHECK CONSTRAINT [FK_Base_CatlogExt_Base_Catalog]
GO
ALTER TABLE [dbo].[Dep_DepHistory]  WITH CHECK ADD  CONSTRAINT [FK_Sys_DepHistory_Sys_Department] FOREIGN KEY([DepId])
REFERENCES [dbo].[Dep_Department] ([Id])
GO
ALTER TABLE [dbo].[Dep_DepHistory] CHECK CONSTRAINT [FK_Sys_DepHistory_Sys_Department]
GO
ALTER TABLE [dbo].[Dep_DepPost]  WITH CHECK ADD  CONSTRAINT [FK_Dep_DepPost_Dep_Department] FOREIGN KEY([DepId])
REFERENCES [dbo].[Dep_Department] ([Id])
GO
ALTER TABLE [dbo].[Dep_DepPost] CHECK CONSTRAINT [FK_Dep_DepPost_Dep_Department]
GO
ALTER TABLE [dbo].[Dep_DepPost]  WITH CHECK ADD  CONSTRAINT [FK_Dep_DepPost_Dep_Post] FOREIGN KEY([PostId])
REFERENCES [dbo].[Dep_Post] ([Id])
GO
ALTER TABLE [dbo].[Dep_DepPost] CHECK CONSTRAINT [FK_Dep_DepPost_Dep_Post]
GO
ALTER TABLE [dbo].[Dep_DepUser]  WITH CHECK ADD  CONSTRAINT [FK_Dep_DepUser_Dep_Department] FOREIGN KEY([DepId])
REFERENCES [dbo].[Dep_Department] ([Id])
GO
ALTER TABLE [dbo].[Dep_DepUser] CHECK CONSTRAINT [FK_Dep_DepUser_Dep_Department]
GO
ALTER TABLE [dbo].[Dep_DepUser]  WITH CHECK ADD  CONSTRAINT [FK_Dep_DepUser_UserProfile] FOREIGN KEY([UserId])
REFERENCES [dbo].[UserProfile] ([UserId])
GO
ALTER TABLE [dbo].[Dep_DepUser] CHECK CONSTRAINT [FK_Dep_DepUser_UserProfile]
GO
ALTER TABLE [dbo].[Dep_PostUser]  WITH CHECK ADD  CONSTRAINT [FK_Dep_PostUser_Dep_DepPost] FOREIGN KEY([OperationId])
REFERENCES [dbo].[UserProfile] ([UserId])
GO
ALTER TABLE [dbo].[Dep_PostUser] CHECK CONSTRAINT [FK_Dep_PostUser_Dep_DepPost]
GO
ALTER TABLE [dbo].[Dep_PostUser]  WITH CHECK ADD  CONSTRAINT [FK_Dep_PostUser_Dep_DepPost1] FOREIGN KEY([DepPostId])
REFERENCES [dbo].[Dep_DepPost] ([Id])
GO
ALTER TABLE [dbo].[Dep_PostUser] CHECK CONSTRAINT [FK_Dep_PostUser_Dep_DepPost1]
GO
ALTER TABLE [dbo].[Dep_PostUser]  WITH CHECK ADD  CONSTRAINT [FK_Dep_PostUser_Dep_DepUser] FOREIGN KEY([DepUserId])
REFERENCES [dbo].[Dep_DepUser] ([Id])
GO
ALTER TABLE [dbo].[Dep_PostUser] CHECK CONSTRAINT [FK_Dep_PostUser_Dep_DepUser]
GO
ALTER TABLE [dbo].[webpages_UsersInRoles]  WITH CHECK ADD  CONSTRAINT [fk_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[webpages_Roles] ([RoleId])
GO
ALTER TABLE [dbo].[webpages_UsersInRoles] CHECK CONSTRAINT [fk_RoleId]
GO
ALTER TABLE [dbo].[webpages_UsersInRoles]  WITH CHECK ADD  CONSTRAINT [fk_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[UserProfile] ([UserId])
GO
ALTER TABLE [dbo].[webpages_UsersInRoles] CHECK CONSTRAINT [fk_UserId]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文章基本表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Article', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Article', @level2type=N'COLUMN',@level2name=N'Title'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'关键词' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Article', @level2type=N'COLUMN',@level2name=N'Keywords'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'摘要' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Article', @level2type=N'COLUMN',@level2name=N'Abstract'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URL上的标题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Article', @level2type=N'COLUMN',@level2name=N'UrlTitle'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'作者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Article', @level2type=N'COLUMN',@level2name=N'Author'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编辑者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Article', @level2type=N'COLUMN',@level2name=N'EditorId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Article', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Article', @level2type=N'COLUMN',@level2name=N'EditTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Article', @level2type=N'COLUMN',@level2name=N'State'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文章扩展表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_ArticleExt', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文章ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_ArticleExt', @level2type=N'COLUMN',@level2name=N'ArticleId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'扩展项ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_ArticleExt', @level2type=N'COLUMN',@level2name=N'CatlogExtId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_ArticleExt', @level2type=N'COLUMN',@level2name=N'Value'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'源ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_ArticleRelation', @level2type=N'COLUMN',@level2name=N'SourceId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'目标ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_ArticleRelation', @level2type=N'COLUMN',@level2name=N'TargetId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_ArticleRelation', @level2type=N'COLUMN',@level2name=N'Ord'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'目录基本表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Catalog', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'目录父ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Catalog', @level2type=N'COLUMN',@level2name=N'ParentId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Catalog', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'简介' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Catalog', @level2type=N'COLUMN',@level2name=N'Abstract'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'语言' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Catalog', @level2type=N'COLUMN',@level2name=N'Language'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Catalog', @level2type=N'COLUMN',@level2name=N'Ord'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Catalog', @level2type=N'COLUMN',@level2name=N'State'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'目录扩展表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_CatalogExt', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'目录ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_CatalogExt', @level2type=N'COLUMN',@level2name=N'CatalogId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'数据名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_CatalogExt', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'数据类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_CatalogExt', @level2type=N'COLUMN',@level2name=N'DataType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'默认值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_CatalogExt', @level2type=N'COLUMN',@level2name=N'DefaultValue'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'数据源' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_CatalogExt', @level2type=N'COLUMN',@level2name=N'DataSourceType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'数据源类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_CatalogExt', @level2type=N'COLUMN',@level2name=N'DataSource'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_CatalogExt', @level2type=N'COLUMN',@level2name=N'Ord'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_CatalogExt', @level2type=N'COLUMN',@level2name=N'State'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否可为空' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_CatalogExt', @level2type=N'COLUMN',@level2name=N'AllowNull'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'字段最大长度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_CatalogExt', @level2type=N'COLUMN',@level2name=N'MaxLength'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dep_Department', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'父部门编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dep_Department', @level2type=N'COLUMN',@level2name=N'ParentId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门序号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dep_Department', @level2type=N'COLUMN',@level2name=N'Ord'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dep_Department', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dep_Department', @level2type=N'COLUMN',@level2name=N'Remark'
GO

