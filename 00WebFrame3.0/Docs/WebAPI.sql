USE [GeoBankDB]
GO
/****** Object:  Table [dbo].[API_AUTH_TOKEN]    Script Date: 2017/6/2 10:14:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[API_AUTH_TOKEN](
	[TokeyID] [nvarchar](36) NOT NULL,
	[ClientName] [nvarchar](50) NULL,
	[ClientId] [nvarchar](50) NULL,
	[TokeyCode] [nvarchar](50) NULL,
	[Memo] [nvarchar](1000) NULL,
	[IsValid] [int] NULL,
	[ValidityDate] [datetime] NULL,
	[AccreditDate] [datetime] NULL,
	[AccreditBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_API_AUTH_TOKEN] PRIMARY KEY NONCLUSTERED 
(
	[TokeyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[API_DATA_NODE_INFO]    Script Date: 2017/6/2 10:14:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[API_DATA_NODE_INFO](
	[DataID] [nvarchar](36) NOT NULL,
	[DataParentID] [nvarchar](50) NULL,
	[DataNodeName] [nvarchar](50) NULL,
	[DataNodeID] [nvarchar](50) NULL,
	[Memo] [nvarchar](1000) NULL,
	[IsValid] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_API_DATA_NODE_INFO] PRIMARY KEY CLUSTERED 
(
	[DataID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[API_DATA_RELATION]    Script Date: 2017/6/2 10:14:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[API_DATA_RELATION](
	[RID] [nvarchar](36) NOT NULL,
	[DataID] [nvarchar](50) NULL,
	[TokeyID] [nvarchar](50) NULL,
 CONSTRAINT [PK_API_DATA_RELATION] PRIMARY KEY CLUSTERED 
(
	[RID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[API_SERVICE_INFO]    Script Date: 2017/6/2 10:14:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[API_SERVICE_INFO](
	[ServiceID] [nvarchar](36) NOT NULL,
	[ParentID] [nvarchar](36) NULL,
	[ServiceName] [nvarchar](50) NULL,
	[ServiceFunctionName] [nvarchar](50) NULL,
	[ServiceFullName] [nvarchar](255) NULL,
	[RequestWay] [nvarchar](50) NULL,
	[AuthWay] [nvarchar](50) NULL,
	[Memo] [nvarchar](1000) NULL,
	[IsValid] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_API_SERVICE_INFO] PRIMARY KEY CLUSTERED 
(
	[ServiceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[API_SERVICE_RELATION]    Script Date: 2017/6/2 10:14:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[API_SERVICE_RELATION](
	[SID] [nvarchar](36) NOT NULL,
	[ServiceID] [nvarchar](50) NULL,
	[TokeyID] [nvarchar](50) NULL,
 CONSTRAINT [PK_API_SERVICE_RELATION2] PRIMARY KEY CLUSTERED 
(
	[SID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'API_AUTH_TOKEN', @level2type=N'COLUMN',@level2name=N'TokeyID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客户组名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'API_AUTH_TOKEN', @level2type=N'COLUMN',@level2name=N'ClientName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客户组ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'API_AUTH_TOKEN', @level2type=N'COLUMN',@level2name=N'ClientId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'授权Key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'API_AUTH_TOKEN', @level2type=N'COLUMN',@level2name=N'TokeyCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'API_AUTH_TOKEN', @level2type=N'COLUMN',@level2name=N'Memo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0无效 1有效' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'API_AUTH_TOKEN', @level2type=N'COLUMN',@level2name=N'IsValid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'授权日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'API_AUTH_TOKEN', @level2type=N'COLUMN',@level2name=N'AccreditDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'授权人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'API_AUTH_TOKEN', @level2type=N'COLUMN',@level2name=N'AccreditBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'记录所发布的安全令牌以及安全令牌的用户组' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'API_AUTH_TOKEN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'API_DATA_NODE_INFO', @level2type=N'COLUMN',@level2name=N'DataID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上级数据节点ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'API_DATA_NODE_INFO', @level2type=N'COLUMN',@level2name=N'DataParentID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'数据节点名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'API_DATA_NODE_INFO', @level2type=N'COLUMN',@level2name=N'DataNodeName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'数据节点ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'API_DATA_NODE_INFO', @level2type=N'COLUMN',@level2name=N'DataNodeID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'API_DATA_NODE_INFO', @level2type=N'COLUMN',@level2name=N'Memo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否启用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'API_DATA_NODE_INFO', @level2type=N'COLUMN',@level2name=N'IsValid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'API_DATA_NODE_INFO', @level2type=N'COLUMN',@level2name=N'CreatedDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'API_DATA_NODE_INFO', @level2type=N'COLUMN',@level2name=N'CreatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'根据数据分类划分各个数据节点的详细信息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'API_DATA_NODE_INFO'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'API_DATA_RELATION', @level2type=N'COLUMN',@level2name=N'RID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'数据节点主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'API_DATA_RELATION', @level2type=N'COLUMN',@level2name=N'DataID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'数据节点名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'API_DATA_RELATION', @level2type=N'COLUMN',@level2name=N'TokeyID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'根据数据分类划分各个数据节点的详细信息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'API_DATA_RELATION'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'API_SERVICE_INFO', @level2type=N'COLUMN',@level2name=N'ServiceID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上级数据节点ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'API_SERVICE_INFO', @level2type=N'COLUMN',@level2name=N'ParentID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'数据节点名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'API_SERVICE_INFO', @level2type=N'COLUMN',@level2name=N'ServiceName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务函数名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'API_SERVICE_INFO', @level2type=N'COLUMN',@level2name=N'ServiceFunctionName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'数据节点ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'API_SERVICE_INFO', @level2type=N'COLUMN',@level2name=N'ServiceFullName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'API_SERVICE_INFO', @level2type=N'COLUMN',@level2name=N'RequestWay'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否启用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'API_SERVICE_INFO', @level2type=N'COLUMN',@level2name=N'IsValid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'API_SERVICE_INFO', @level2type=N'COLUMN',@level2name=N'CreatedDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'API_SERVICE_INFO', @level2type=N'COLUMN',@level2name=N'CreatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'根据服务分类划分各个数据节点的详细信息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'API_SERVICE_INFO'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'API_SERVICE_RELATION', @level2type=N'COLUMN',@level2name=N'SID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务节点主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'API_SERVICE_RELATION', @level2type=N'COLUMN',@level2name=N'ServiceID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'数据节点名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'API_SERVICE_RELATION', @level2type=N'COLUMN',@level2name=N'TokeyID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'建立服务与客户组的授权关系' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'API_SERVICE_RELATION'
GO
