原Jurassic.WebSignalR合并到Jurassic.WebSchedule中
菜单文件合并
用NinjectDependcyResolver代替原来的NinjectConfig进行控制器解析，不会报出异常只会404

Article.State &=1的删除标志弃用，改用IsDeleted单独标示
Article.State.New 由4变为1

ScheduleEvent

OptionNotice = ArticleState.New+ArticleState.Published 转移到Option字段

将框架中打包的资源重新定向到新的文件夹

重置密码
密码找回
Windows权限认证
邮箱认证

用户扩展的方法
栏目文章数据结构
上传组件
多语言架构：开头带下划线的不被js序列化到前端
如何改写登录逻辑

菜单集成
菜单的多语言字典，静态属性，避免遗漏，减少重复键

启动优化，不再反复写文件

框架帮助文档系统

页面退出的确认