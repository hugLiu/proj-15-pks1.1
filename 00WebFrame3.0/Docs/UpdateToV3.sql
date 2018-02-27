--UpdateToV3:用于从已有2.0版本数据库升级到3.0
/*增加options选项和逻辑删除字段*/
alter table Base_Article Add Options int not null default 0;
alter table Base_Article Add IsDeleted bit not null default 0;
alter table Base_Catalog Add Options int not null default 0;
alter table Base_Catalog Add IsDeleted bit not null default 0;
alter table Base_CatalogExt Add Options int not null default 0;
alter table Base_CatalogExt Add IsDeleted bit not null default 0;
go
alter table Base_ArticleExt alter column [Value] nvarchar(400);
/*将State字段中的删除标志移到逻辑删除字段*/
update Base_Article Set IsDeleted=1,State=State-1 WHERE State & 1=1;

/*变按位枚举为常量比较*/
update base_article set options=2,state=state-4 from base_article a, Base_CatalogArticle ca
where ca.ArticleId = a.id
and ca.CatalogId = 21 and state & 6 =6;

update base_article set options=1,state=state-32 from base_article a, Base_CatalogArticle ca
where ca.ArticleId = a.id
and ca.CatalogId = 21 and state & 34 = 34;

/*EventFinished*/
update base_article set state=2 from base_article a, Base_CatalogArticle ca
where ca.ArticleId = a.id
and ca.CatalogId = 21 and state & 258 = 258;

/*EventRead*/
update base_article set state=1 from base_article a, Base_CatalogArticle ca
where ca.ArticleId = a.id
and ca.CatalogId = 21 and state & 66 = 66;
go
/*增加索引优化性能*/
CREATE NONCLUSTERED INDEX [_dta_index_Base_Article_7_1474104292__K13_K1] ON [dbo].[Base_Article]
(
	[IsDeleted] ASC,
	[Id] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
CREATE NONCLUSTERED INDEX [_dta_index_Base_Article_7_1474104292__K10_K7_K13_K1] ON [dbo].[Base_Article]
(
	[State] ASC,
	[EditorId] ASC,
	[IsDeleted] ASC,
	[Id] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
SET ANSI_PADDING ON

CREATE NONCLUSTERED INDEX [_dta_index_Base_Article_7_1474104292__K10_K7_K13_K1_2_12] ON [dbo].[Base_Article]
(
	[State] ASC,
	[EditorId] ASC,
	[IsDeleted] ASC,
	[Id] ASC
)
INCLUDE ( 	[Title],
	[Options]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
	SET ANSI_PADDING ON

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
	[IsDeleted]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
CREATE NONCLUSTERED INDEX [_dta_index_Base_ArticleExt_7_459148681__K3_K2] ON [dbo].[Base_ArticleExt]
(
	[CatlogExtId] ASC,
	[ArticleId] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
CREATE NONCLUSTERED INDEX [_dta_index_Base_ArticleExt_7_459148681__K3_2] ON [dbo].[Base_ArticleExt]
(
	[CatlogExtId] ASC
)
INCLUDE ( 	[ArticleId]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
SET ANSI_PADDING ON

CREATE NONCLUSTERED INDEX [_dta_index_Base_ArticleExt_7_459148681__K2_1_3_4] ON [dbo].[Base_ArticleExt]
(
	[ArticleId] ASC
)
INCLUDE ( 	[Id],
	[CatlogExtId],
	[Value]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
SET ANSI_PADDING ON

CREATE NONCLUSTERED INDEX [_dta_index_Base_ArticleExt_7_459148681__K2_K3_K4] ON [dbo].[Base_ArticleExt]
(
	[ArticleId] ASC,
	[CatlogExtId] ASC,
	[Value] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
CREATE NONCLUSTERED INDEX [_dta_index_Base_ArticleRelation_7_1538104520__K2_K3_K4_1_5] ON [dbo].[Base_ArticleRelation]
(
	[SourceId] ASC,
	[TargetId] ASC,
	[RelationType] ASC
)
INCLUDE ( 	[Id],
	[Ord]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
