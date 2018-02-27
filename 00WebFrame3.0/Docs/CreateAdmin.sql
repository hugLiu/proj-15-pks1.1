--该脚本用于在全新数据库中新建一个管理员账户admin, 密码123456
set identity_insert  UserProfile on
INSERT INTO UserProfile (UserId,UserName) VALUES(1,'admin')
INSERT INTO webpages_Membership( [UserId]
      ,[CreateDate]
      ,[ConfirmationToken]
      ,[IsConfirmed]
      ,[LastPasswordFailureDate]
      ,[PasswordFailuresSinceLastSuccess]
      ,[Password]
      ,[PasswordChangedDate]
      ,[PasswordSalt]
      ,[PasswordVerificationToken])VALUES(1,
	  GetDate(), '9f1cc839-43ef-4929-b8df-b663574d8226',1,null, 0,'8D8C97133C046153F0CEF2199ED3F8C1', null,'4b50d3f4-e5bb-4672-9433-bc84652cf004',null)
set identity_insert  UserProfile off
set identity_insert  webpages_Roles on
	  INSERT INTO webpages_Roles(RoleId,RoleName)VALUES(1,'admin')
set identity_insert  webpages_Roles off
	  INSERT INTO webpages_UsersInRoles(UserId,RoleId)VALUES(1, 1)