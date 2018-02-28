USE [PKS_DNT3]
GO

--默认管理员为admin,密码为888888
INSERT INTO [dnt_users] ([username],[nickname],[password],[adminid],[groupid],[invisible],[email]) VALUES('admin','admin','21218cca77804d2ba1922c33e0151105','1','1','0','')

INSERT INTO [dnt_userfields] ([uid]) VALUES('1')
GO

--论坛信息
SET IDENTITY_INSERT [dbo].[dnt_forums] ON
INSERT INTO [dbo].[dnt_forums] ([fid], [parentid], [layer], [pathlist], [parentidlist], [subforumcount], [name], [status], [colcount], [displayorder], [templateid], [topics], [curtopics], [posts], [todayposts], [lasttid], [lasttitle], [lastpost], [lastposterid], [lastposter], [allowsmilies], [allowrss], [allowhtml], [allowbbcode], [allowimgcode], [allowblog], [istrade], [allowpostspecial], [allowspecialonly], [alloweditrules], [allowthumbnail], [allowtag], [recyclebin], [modnewposts], [modnewtopics], [jammer], [disablewatermark], [inheritedmod], [autoclose]) 
	  SELECT  1,  0, 0, N'<a href="showforum-1.aspx">勘探目标认识</a>                                       ', N'0 ', 5, N'勘探目标认识', 1, 3,  1, 0, 0, 0, 0, 0, 0, N'', N'1900-01-01 00:00:00', 0, N'', 1, 0, 0, 1, 1, 0, 0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0
UNION SELECT  2,  1, 1, N'<a href="showforum-1.aspx">勘探目标认识</a><a href="showforum-2.aspx">井</a>      ', N'1 ', 0, N'井          ', 1, 1,  2, 0, 0, 0, 0, 0, 0, N'', N'1900-01-01 00:00:00', 0, N'', 1, 0, 0, 1, 1, 0, 0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0
UNION SELECT  3,  1, 1, N'<a href="showforum-1.aspx">勘探目标认识</a><a href="showforum-3.aspx">地震工区</a>', N'1 ', 0, N'地震工区    ', 1, 1,  3, 0, 0, 0, 0, 0, 0, N'', N'1900-01-01 00:00:00', 0, N'', 1, 0, 0, 1, 1, 0, 0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0
UNION SELECT  4,  1, 1, N'<a href="showforum-1.aspx">勘探目标认识</a><a href="showforum-4.aspx">圈闭</a>    ', N'1 ', 0, N'圈闭        ', 1, 1,  4, 0, 0, 0, 0, 0, 0, N'', N'1900-01-01 00:00:00', 0, N'', 1, 0, 0, 1, 1, 0, 0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0
UNION SELECT  5,  1, 1, N'<a href="showforum-1.aspx">勘探目标认识</a><a href="showforum-5.aspx">构造单元</a>', N'1 ', 0, N'构造单元    ', 1, 1,  5, 0, 0, 0, 0, 0, 0, N'', N'1900-01-01 00:00:00', 0, N'', 1, 0, 0, 1, 1, 0, 0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0
UNION SELECT  6,  1, 1, N'<a href="showforum-1.aspx">勘探目标认识</a><a href="showforum-6.aspx">盆地</a>    ', N'1 ', 0, N'盆地        ', 1, 1,  6, 0, 0, 0, 0, 0, 0, N'', N'1900-01-01 00:00:00', 0, N'', 1, 0, 0, 1, 1, 0, 0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0
UNION SELECT  7,  0, 0, N'<a href="showforum-7.aspx">勘探专业研究</a>                                       ', N'0 ', 6, N'勘探专业研究', 1, 3,  6, 0, 0, 0, 0, 0, 0, N'', N'1900-01-01 00:00:00', 0, N'', 1, 0, 0, 1, 1, 0, 0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0
UNION SELECT  8,  7, 1, N'<a href="showforum-7.aspx">勘探专业研究</a><a href="showforum-8.aspx">地层</a>    ', N'7 ', 0, N'地层        ', 1, 1,  7, 0, 0, 0, 0, 0, 0, N'', N'1900-01-01 00:00:00', 0, N'', 1, 0, 0, 1, 1, 0, 0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0
UNION SELECT  9,  7, 1, N'<a href="showforum-7.aspx">勘探专业研究</a><a href="showforum-9.aspx">构造</a>    ', N'7 ', 0, N'构造        ', 1, 1,  8, 0, 0, 0, 0, 0, 0, N'', N'1900-01-01 00:00:00', 0, N'', 1, 0, 0, 1, 1, 0, 0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0
UNION SELECT 10,  7, 1, N'<a href="showforum-7.aspx">勘探专业研究</a><a href="showforum-10.aspx">沉积</a>   ', N'7 ', 0, N'沉积        ', 1, 1,  9, 0, 0, 0, 0, 0, 0, N'', N'1900-01-01 00:00:00', 0, N'', 1, 0, 0, 1, 1, 0, 0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0
UNION SELECT 11,  7, 1, N'<a href="showforum-7.aspx">勘探专业研究</a><a href="showforum-11.aspx">储层</a>   ', N'7 ', 0, N'储层        ', 1, 1, 10, 0, 0, 0, 0, 0, 0, N'', N'1900-01-01 00:00:00', 0, N'', 1, 0, 0, 1, 1, 0, 0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0
UNION SELECT 12,  7, 1, N'<a href="showforum-7.aspx">勘探专业研究</a><a href="showforum-12.aspx">生油</a>   ', N'7 ', 0, N'生油        ', 1, 1, 11, 0, 0, 0, 0, 0, 0, N'', N'1900-01-01 00:00:00', 0, N'', 1, 0, 0, 1, 1, 0, 0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0
UNION SELECT 13,  7, 1, N'<a href="showforum-7.aspx">勘探专业研究</a><a href="showforum-13.aspx">成藏</a>   ', N'7 ', 0, N'成藏        ', 1, 1, 12, 0, 0, 0, 0, 0, 0, N'', N'1900-01-01 00:00:00', 0, N'', 1, 0, 0, 1, 1, 0, 0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0
UNION SELECT 14,  0, 0, N'<a href="showforum-14.aspx">专家问答</a>                                          ', N'0 ', 1, N'专家问答    ', 1, 3, 12, 0, 0, 0, 0, 0, 0, N'', N'1900-01-01 00:00:00', 0, N'', 1, 0, 0, 1, 1, 0, 0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0
UNION SELECT 15, 14, 1, N'<a href="showforum-14.aspx">专家问答</a><a href="showforum-15.aspx">吴专家</a>    ', N'14', 0, N'吴专家      ', 1, 1, 13, 0, 0, 0, 0, 0, 0, N'', N'1900-01-01 00:00:00', 0, N'', 1, 0, 0, 1, 1, 0, 0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0
SET IDENTITY_INSERT [dbo].[dnt_forums] OFF
GO

INSERT INTO [dbo].[dnt_forumfields] ([fid], [password], [icon], [postcredits], [replycredits], [redirect], [attachextensions], [rules], [topictypes], [viewperm], [postperm], [replyperm], [getattachperm], [postattachperm], [moderators], [description], [applytopictype], [postbytopictype], [viewbytopictype], [topictypeprefix], [permuserlist], [seokeywords], [seodescription], [rewritename]) 
	  SELECT  1, N'', N''                                  , N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', 0, 0, 0, 0, N'', N'', N'', N''
UNION SELECT  2, N'', N'/images/地质目标/井.png'           , N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', 0, 0, 0, 0, N'', N'', N'', N''
UNION SELECT  3, N'', N'/images/地质目标/地震工区.png'     , N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', 0, 0, 0, 0, N'', N'', N'', N''
UNION SELECT  4, N'', N'/images/地质目标/圈闭.png'         , N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', 0, 0, 0, 0, N'', N'', N'', N''
UNION SELECT  5, N'', N'/images/地质目标/构造单元.png'     , N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', 0, 0, 0, 0, N'', N'', N'', N''
UNION SELECT  6, N'', N'/images/地质目标/盆地.png'         , N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', 0, 0, 0, 0, N'', N'', N'', N''
UNION SELECT  7, N'', N''                                  , N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', 0, 0, 0, 0, N'', N'', N'', N''
UNION SELECT  8, N'', N'/images/地质目标/地层.png'         , N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', 0, 0, 0, 0, N'', N'', N'', N''
UNION SELECT  9, N'', N'/images/地质目标/构造.jpg'         , N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', 0, 0, 0, 0, N'', N'', N'', N''
UNION SELECT 10, N'', N'/images/地质目标/沉积.png'         , N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', 0, 0, 0, 0, N'', N'', N'', N''
UNION SELECT 11, N'', N'/images/地质目标/储层.png'         , N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', 0, 0, 0, 0, N'', N'', N'', N''
UNION SELECT 12, N'', N'/images/地质目标/生油.png'         , N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', 0, 0, 0, 0, N'', N'', N'', N''
UNION SELECT 13, N'', N'/images/地质目标/成藏.png'         , N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', 0, 0, 0, 0, N'', N'', N'', N''
UNION SELECT 14, N'', N''                                  , N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', 0, 0, 0, 0, N'', N'', N'', N''
UNION SELECT 15, N'', N''                                  , N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', 0, 0, 0, 0, N'', N'', N'', N''
GO

UPDATE [dnt_usergroups] SET [disableperiodctrl]=1
