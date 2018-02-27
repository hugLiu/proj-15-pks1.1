using Jurassic.AppCenter.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jurassic.WebFrame
{
    /// <summary>
    /// WebFrame框架本身用到的多语言Key集合
    /// </summary>
    public class FStr : IStartupStr
    {
        /// <summary>
        /// 已恢复默认角色授权
        /// </summary>
        public static string DefaultRoleReseted
        {
            get { return ResHelper.GetStr("DefaultRoleReseted"); }
        }

        /// <summary>
        /// 请先选择一个用户
        /// </summary>
        public static string PlzSelectUser
        {
            get { return ResHelper.GetStr("PlzSelectUser"); }
        }

        /// <summary>
        /// 用户名有重复
        /// </summary>
        public static string DuplicatedName
        {
            get { return ResHelper.GetStr("DuplicatedName"); }
        }

        /// <summary>
        /// 重置密码失败，带1个参数
        /// </summary>
        public static string ResetPasswordFailed0
        {
            get { return ResHelper.GetStr("ResetPasswordFailed{0}"); }
        }

        /// <summary>
        /// 成功新增一个用户，带1个参数（用户名）
        /// </summary>
        public static string UserSuccessAdded0
        {
            get { return ResHelper.GetStr("UserSuccessAdded{0}"); }
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        public static string ResetPassword
        {
            get { return ResHelper.GetStr("ResetPassword"); }
        }

        /// <summary>
        /// 是否重置密码到123456
        /// </summary>
        public static string ConfirmResetPassword
        {
            get { return ResHelper.GetStr("ConfirmResetPassword"); }
        }

        /// <summary>
        /// 新用户
        /// </summary>
        public static string NewUser
        {
            get { return ResHelper.GetStr("NewUser"); }
        }

        /// <summary>
        /// 成功更新用户信息
        /// </summary>
        public static string UpdateUserInfoSucceed
        {
            get { return ResHelper.GetStr("UpdateUserInfoSucceed"); }
        }
        /// <summary>
        /// 成功更新用户信息
        /// </summary>
        public static string UpdateUserInfoSucceed0
        {
            get { return ResHelper.GetStr("UpdateUserInfoSucceed{0}"); }
        }

        /// <summary>
        /// 成功更新用户信息
        /// </summary>
        public static string EditUser
        {
            get { return ResHelper.GetStr("EditUser"); }
        }

        /// <summary>
        /// 角色名称
        /// </summary>
        public static string RoleNames
        {
            get { return ResHelper.GetStr("RoleNames"); }
        }

        /// <summary>
        /// 登录成功
        /// </summary>
        public static string LoginSucceed
        {
            get { return ResHelper.GetStr("LoginSucceed"); }
        }

        /// <summary>
        /// 第一次登录强制修改密码
        /// </summary>
        public static string MustChangePassword
        {
            get { return ResHelper.GetStr("MustChangePassword"); }
        }
        /// <summary>
        /// 成功修改密码
        /// </summary>
        public static string PasswordChangeSucceed
        {
            get { return ResHelper.GetStr("PasswordChangeSucceed"); }
        }

        /// <summary>
        /// 重置密码的邮件已发送
        /// </summary>
        public static string ResetPasswordEmailSent
        {
            get { return ResHelper.GetStr("ResetPasswordEmailSent"); }
        }
        /// <summary>
        /// 重置密码成功！
        /// </summary>
        public static string ResetPasswordSucceed
        {
            get { return ResHelper.GetStr("ResetPasswordSucceed"); }
        }
        /// <summary>
        /// 密码为空
        /// </summary>
        public static string PasswordEmpty
        {
            get { return ResHelper.GetStr("PasswordEmpty"); }
        }
        /// <summary>
        /// 记住我
        /// </summary>
        public static string RememberMe
        {
            get { return ResHelper.GetStr("RememberMe"); }
        }
        /// <summary>
        /// 忘记密码
        /// </summary>
        public static string ForgotPassword
        {
            get { return ResHelper.GetStr("ForgotPassword"); }
        }
        /// <summary>
        /// 正在登录...
        /// </summary>
        public static string Loging
        {
            get { return ResHelper.GetStr("Loging"); }
        }
        /// <summary>
        /// 请输入忘记密码的用户名
        /// </summary>
        public static string PlzInputUserNameForgetPwd
        {
            get { return ResHelper.GetStr("PlzInputUserNameForgetPwd"); }
        }

        /// <summary>
        /// 请重启站点
        /// </summary>
        public static string PlzRestartWebSite
        {
            get
            {
                return ResHelper.GetStr("PlzRestartWebSite");
            }
        }

        /// <summary>
        /// 用户基本信息
        /// </summary>
        public static string UserBaseInfo
        {
            get
            {
                return ResHelper.GetStr("UserBaseInfo");
            }
        }

        /// <summary>
        /// 编辑用户信息
        /// </summary>
        public static string EditUserInfo
        {
            get
            {
                return ResHelper.GetStr("EditUserInfo");
            }
        }

        /// <summary>
        /// 角色
        /// </summary>
        public static string Roles
        {
            get
            {
                return ResHelper.GetStr("Roles");
            }
        }

        /// <summary>
        /// 重置
        /// </summary>
        public static string RestoreToDefault
        {
            get
            {
                return ResHelper.GetStr("RestoreToDefault");
            }
        }

        /// <summary>
        /// 非法的请求数据
        /// </summary>
        public static string InvalidRequestData
        {
            get { return ResHelper.GetStr("InvalidRequestData"); }
        }

        /// <summary>
        /// 重复的机构名称
        /// </summary>
        public static string DuplicatedOrgCode
        {
            get { return ResHelper.GetStr("DuplicatedOrgCode"); }
        }
        public static string NoDataToSave
        {
            get { return ResHelper.GetStr("NoDataToSave"); }
        }

        /// <summary>
        /// 上级组织机构
        /// </summary>
        public static string UpOrg
        {
            get { return ResHelper.GetStr("UpOrg"); }
        }

        /// <summary>
        /// 组织机构名称
        /// </summary>
        public static string OrgName
        {
            get { return ResHelper.GetStr("OrgName"); }
        }
        /// <summary>
        /// 组织机构编码
        /// </summary>
        public static string OrgCode
        {
            get { return ResHelper.GetStr("OrgCode"); }
        }
        /// <summary>
        /// 组织机构编码
        /// </summary>
        public static string OrgType
        {
            get { return ResHelper.GetStr("OrgType"); }
        }

        /// <summary>
        /// 单位
        /// </summary>
        public static string Company
        {
            get { return ResHelper.GetStr("Company"); }
        }

        /// <summary>
        /// 部门
        /// </summary>
        public static string Department
        {
            get { return ResHelper.GetStr("Department"); }
        }
        /// <summary>
        /// 部门相关岗位
        /// </summary>
        public static string DepPost
        {
            get { return ResHelper.GetStr("DepPost"); }
        }

        /// <summary>
        /// 岗位名称
        /// </summary>
        public static string PostName
        {
            get { return ResHelper.GetStr("PostName"); }
        }

        /// <summary>
        /// 岗位人数
        /// </summary>
        public static string PostNumber
        {
            get { return ResHelper.GetStr("PostNumber"); }
        }

        /// <summary>
        /// 岗位人数
        /// </summary>
        public static string PostDesc
        {
            get { return ResHelper.GetStr("PostDesc"); }
        }
        /// <summary>
        /// 岗位责任
        /// </summary>
        public static string PostDuty
        {
            get { return ResHelper.GetStr("PostDuty"); }
        }
        /// <summary>
        /// 岗位要求
        /// </summary>
        public static string PostRequire
        {
            get { return ResHelper.GetStr("PostRequire"); }
        }
        /// <summary>
        /// 部门相关人员
        /// </summary>
        public static string DepartmentEmploy
        {
            get { return ResHelper.GetStr("DepartmentEmploy"); }
        }
        /// <summary>
        /// 合同类型
        /// </summary>
        public static string ContractType
        {
            get { return ResHelper.GetStr("ContractType"); }
        }

        /// <summary>
        /// 合同期限(年)
        /// </summary>
        public static string ContractYear
        {
            get { return ResHelper.GetStr("ContractYear"); }
        }

        /// <summary>
        /// 是否主管
        /// </summary>
        public static string IsLeader
        {
            get { return ResHelper.GetStr("IsLeader"); }
        }

        /// <summary>
        /// 是否主部门
        /// </summary>
        public static string IsMainDept
        {
            get { return ResHelper.GetStr("IsMainDept"); }
        }
        /// <summary>
        /// 用户账号筛选
        /// </summary>
        public static string UserAccountFilter
        {
            get { return ResHelper.GetStr("UserAccountFilter"); }
        }

        /// <summary>
        /// 加入部门
        /// </summary>
        public static string AddToDept
        {
            get { return ResHelper.GetStr("AddToDept"); }
        }

        /// <summary>
        /// 当前所属主部门
        /// </summary>
        public static string CurrentMainDept
        {
            get { return ResHelper.GetStr("CurrentMainDept"); }
        }

        /// <summary>
        /// 选择添加该部门用户
        /// </summary>
        public static string ChoiceToAddThisUser
        {
            get { return ResHelper.GetStr("ChoiceToAddThisUser"); }
        }
        /// <summary>
        /// 新增岗位
        /// </summary>
        public static string AddPost
        {
            get { return ResHelper.GetStr("AddPost"); }
        }
        /// <summary>
        /// 系统概况
        /// </summary>
        public static string SystemStatus
        {
            get { return ResHelper.GetStr("SystemStatus"); }
        }

        /// <summary>
        /// 语言管理
        /// </summary>
        public static string ResourceManager
        {
            get { return ResHelper.GetStr("ResourceManager"); }
        }
        /// <summary>
        /// 删除条目
        /// </summary>
        public static string DeleteItem
        {
            get { return ResHelper.GetStr("DeleteItem"); }
        }
        /// <summary>
        /// 新增条目
        /// </summary>
        public static string AddItem
        {
            get { return ResHelper.GetStr("AddItem"); }
        }

        /// <summary>
        /// 清空资源
        /// </summary>
        public static string ClearAllResouce
        {
            get { return ResHelper.GetStr("ClearAllResouce"); }
        }
        /// <summary>
        /// 确定清空资源
        /// </summary>
        public static string ConfirmClearAllResource
        {
            get { return ResHelper.GetStr("ConfirmClearAllResource"); }
        }

        /// <summary>
        /// 删除语种
        /// </summary>
        public static string DeleteCulture
        {
            get { return ResHelper.GetStr("DeleteCulture"); }
        }

        /// <summary>
        /// 请先将光标定位在要删除的语种所在列
        /// </summary>
        public static string PlzPointCultureColumnForDelete
        {
            get { return ResHelper.GetStr("PlzPointCultureColumnForDelete"); }
        }

        /// <summary>
        /// 请输入语种简称(如zh-cn)
        /// </summary>
        public static string PlzInputCultueSimpleName
        {
            get { return ResHelper.GetStr("PlzInputCultueSimpleName"); }
        }

        /// <summary>
        /// 新增语种
        /// </summary>
        public static string AddCulture
        {
            get { return ResHelper.GetStr("AddCulture"); }
        }

        /// <summary>
        /// 角色列表
        /// </summary>
        public static string RolesList
        {
            get { return ResHelper.GetStr("RolesList"); }
        }

        /// <summary>
        /// 角色权限
        /// </summary>
        public static string RolePermission
        {
            get { return ResHelper.GetStr("RolePermission"); }
        }
        /// <summary>
        /// 角色名称重复
        /// </summary>
        public static string DuplicatedRoleName
        {
            get { return ResHelper.GetStr("DuplicatedRoleName"); }
        }
        /// <summary>
        /// 功能管理
        /// </summary>
        public static string FunctionManager
        {
            get { return ResHelper.GetStr("FunctionManager"); }
        }
        /// <summary>
        /// 功能名称不能为空
        /// </summary>
        public static string FunctionNameRequired
        {
            get { return ResHelper.GetStr("FunctionNameRequired"); }
        }
        /// <summary>
        /// ID不能为空
        /// </summary>
        public static string IDRequried
        {
            get { return ResHelper.GetStr("IDRequried"); }
        }
        /// <summary>
        /// 图标地址
        /// </summary>
        public static string IconAddress
        {
            get { return ResHelper.GetStr("IconAddress"); }
        }
        /// <summary>
        /// 图标CSS样式名称
        /// </summary>
        public static string IconCSSName
        {
            get { return ResHelper.GetStr("IconCSSName"); }
        }
        /// <summary>
        /// 授权类型
        /// </summary>
        public static string AuthorizeType
        {
            get { return ResHelper.GetStr("AuthorizeType"); }
        }
        /// <summary>
        /// 显示类型
        /// </summary>
        public static string VisibleType
        {
            get { return ResHelper.GetStr("VisibleType"); }
        }
        /// <summary>
        /// 类名
        /// </summary>
        public static string ClassName
        {
            get { return ResHelper.GetStr("ClassName"); }
        }
        /// <summary>
        /// 方法名
        /// </summary>
        public static string MethodName
        {
            get { return ResHelper.GetStr("MethodName"); }
        }
        /// <summary>
        /// 新增平级
        /// </summary>
        public static string AddBrother
        {
            get { return ResHelper.GetStr("AddBrother"); }
        }
        /// <summary>
        /// 新增子级
        /// </summary>
        public static string AddChild
        {
            get { return ResHelper.GetStr("AddChild"); }
        }
        /// <summary>
        /// 没有结点被勾选
        /// </summary>
        public static string NoNodeSelected
        {
            get { return ResHelper.GetStr("NoNodeSelected"); }
        }

        /// <summary>
        /// 请输入要移动到的父结点ID，0代表移动到顶级
        /// </summary>
        public static string PlzInputParentNodeId
        {
            get { return ResHelper.GetStr("PlzInputParentNodeId"); }
        }
        /// <summary>
        /// 结点不能移动到自身
        /// </summary>
        public static string NodeCannotMoveToItself
        {
            get { return ResHelper.GetStr("NodeCannotMoveToItself"); }
        }
        /// <summary>
        /// 没找到要移动到的父级节点
        /// </summary>
        public static string ParentNodeNotFound
        {
            get { return ResHelper.GetStr("ParentNodeNotFound"); }
        }
        /// <summary>
        /// 有结点的ID为空值
        /// </summary>
        public static string NodeIdIsEmpty
        {
            get { return ResHelper.GetStr("NodeIdIsEmpty"); }
        }
        /// <summary>
        /// 有结点的ID重复
        /// </summary>
        public static string DupliatedNodeId
        {
            get { return ResHelper.GetStr("DupliatedNodeId"); }
        }

        /// <summary>
        /// 修改的密码不能与原密码相同
        /// </summary>
        public static string NewPassowrdMustNotEqualToOld
        {
            get { return ResHelper.GetStr("NewPassowrdMustNotEqualToOld"); }
        }
        /// <summary>
        /// 成功清除
        /// </summary>
        public static string ClearSucceed
        {
            get { return ResHelper.GetStr("ClearSucceed"); }
        }
        /// <summary>
        /// 登录失败
        /// </summary>
        public static string LoginFailed
        {
            get { return ResHelper.GetStr("LoginFailed"); }
        }
        /// <summary>
        /// 岗位类型
        /// </summary>
        public static string PostType
        {
            get { return ResHelper.GetStr("PostType"); }
        }
        /// <summary>
        /// 岗位雇佣类型
        /// </summary>
        public static string PostHireType
        {
            get { return ResHelper.GetStr("PostHireType"); }
        }
        /// <summary>
        /// 岗位信息
        /// </summary>
        public static string PostInfo
        {
            get { return ResHelper.GetStr("PostInfo"); }
        }
        /// <summary>
        /// 岗位信息
        /// </summary>
        public static string ChangePassword
        {
            get { return ResHelper.GetStr("ChangePassword"); }
        }
        /// <summary>
        /// 登出
        /// </summary>
        public static string Login
        {
            get { return ResHelper.GetStr("Login"); }
        }
        /// <summary>
        /// 登出
        /// </summary>
        public static string Logout
        {
            get { return ResHelper.GetStr("Logout"); }
        }
        /// <summary>
        /// 用户配置页
        /// </summary>
        public static string UserConfigPage
        {
            get { return ResHelper.GetStr("UserConfigPage"); }
        }
        /// <summary>
        /// 登录历史
        /// </summary>
        public static string LoginHistory
        {
            get { return ResHelper.GetStr("LoginHistory"); }
        }
        /// <summary>
        /// 客户端IP
        /// </summary>
        public static string ClientIP
        {
            get { return ResHelper.GetStr("ClientIP"); }
        }
        /// <summary>
        /// 偏好设置
        /// </summary>
        public static string FavoriteSettings
        {
            get { return ResHelper.GetStr("FavoriteSettings"); }
        }
        /// <summary>
        /// 皮肤
        /// </summary>
        public static string Skin
        {
            get { return ResHelper.GetStr("Skin"); }
        }
        /// <summary>
        /// 多标签
        /// </summary>
        public static string TabShow
        {
            get { return ResHelper.GetStr("TabShow"); }
        }
        /// <summary>
        /// 表格线
        /// </summary>
        public static string GridLine
        {
            get { return ResHelper.GetStr("GridLine"); }
        }
        /// <summary>
        /// 横线
        /// </summary>
        public static string Horizontal
        {
            get { return ResHelper.GetStr("Horizontal"); }
        }
        /// <summary>
        /// 竖线
        /// </summary>
        public static string Vertical
        {
            get { return ResHelper.GetStr("Vertical"); }
        }
        /// <summary>
        /// 布局设置
        /// </summary>
        public static string LayoutConfig
        {
            get { return ResHelper.GetStr("LayoutConfig"); }
        }
        /// <summary>
        /// 布局已经重置
        /// </summary>
        public static string LayoutReseted
        {
            get { return ResHelper.GetStr("LayoutReseted"); }
        }
        /// <summary>
        /// 提交方法
        /// </summary>
        public static string SubmitMethod
        {
            get { return ResHelper.GetStr("SubmitMethod"); }
        }
        /// <summary>
        /// 所有用户
        /// </summary>
        public static string AllLoginUsers
        {
            get { return ResHelper.GetStr("AllLoginUsers"); }
        }
        /// <summary>
        /// 所有人
        /// </summary>
        public static string EveryOne
        {
            get { return ResHelper.GetStr("EveryOne"); }
        }
        /// <summary>
        /// 授权用户
        /// </summary>
        public static string NeedAuth
        {
            get { return ResHelper.GetStr("NeedAuth"); }
        }
        /// <summary>
        /// 按钮
        /// </summary>
        public static string Button
        {
            get { return ResHelper.GetStr("Button"); }
        }
        /// <summary>
        /// 用户菜单
        /// </summary>
        public static string UserMenu
        {
            get { return ResHelper.GetStr("UserMenu"); }
        }
        /// <summary>
        /// 权限菜单
        /// </summary>
        public static string RoleMenu
        {
            get { return ResHelper.GetStr("RoleMenu"); }
        }
        /// <summary>
        /// 分组第一项
        /// </summary>
        public static string GroupBegin
        {
            get { return ResHelper.GetStr("GroupBegin"); }
        }
        /// <summary>
        /// 自定义容器
        /// </summary>
        public static string UserContainer
        {
            get { return ResHelper.GetStr("UserContainer"); }
        }
        /// <summary>
        /// 快速访问项
        /// </summary>
        public static string QuckAccessBar
        {
            get { return ResHelper.GetStr("QuckAccessBar"); }
        }

        /// <summary>
        /// 自定义容器
        /// </summary>
        public static string SetTop
        {
            get { return ResHelper.GetStr("SetTop"); }
        }
        /// <summary>
        /// 首页部件
        /// </summary>
        public static string Widget
        {
            get { return ResHelper.GetStr("Widget"); }
        }
        /// <summary>
        /// 说明：参数格式是 参数名=值(或匹配值的正则表达式)，每行填一个
        /// </summary>
        public static string HowToDefineParameter
        {
            get { return ResHelper.GetStr("HowToDefineParameter"); }
        }

        /// <summary>
        /// 系统已完成初始化并正常运行
        /// </summary>
        public static string SystemInitFinishedAndRuningPerfect
        {
            get { return ResHelper.GetStr("SystemInitFinishedAndRuningPerfect"); }
        }
        /// <summary>
        /// 重新写资源文件
        /// </summary>
        public static string RewriteAllResFiles
        {
            get { return ResHelper.GetStr("RewriteAllResFiles"); }
        }

        /// <summary>
        /// 重新生成功能列表
        /// </summary>
        public static string ReGenerateFunctions
        {
            get { return ResHelper.GetStr("ReGenerateFunctions"); }
        }
        /// <summary>
        /// 功能数
        /// </summary>
        public static string FunctionsCount
        {
            get { return ResHelper.GetStr("SystemFunctionsCount"); }
        }
        /// <summary>
        /// 角色数
        /// </summary>
        public static string RolesCount
        {
            get { return ResHelper.GetStr("SystemRolesCount"); }
        }
        /// <summary>
        /// 用户数
        /// </summary>
        public static string UsersCount
        {
            get { return ResHelper.GetStr("SystemUsersCount"); }
        }

        /// <summary>
        /// 应用程序管理起始页
        /// </summary>
        public static string ApplicationManagerStartPage
        {
            get { return ResHelper.GetStr("ApplicationManagerStartPage"); }
        }

        /// <summary>
        /// 设置
        /// </summary>
        public static string Settings
        {
            get { return ResHelper.GetStr("Settings"); }
        }

        /// <summary>
        /// 用户头像
        /// </summary>
        public static string UserAvatar
        {
            get { return ResHelper.GetStr("UserAvatar"); }
        }

        /// <summary>
        /// 控制器
        /// </summary>
        public static string Controller
        {
            get { return ResHelper.GetStr("Controller"); }
        }
        /// <summary>
        /// 方法
        /// </summary>
        public static string Method
        {
            get { return ResHelper.GetStr("Method"); }
        }
        /// <summary>
        /// 开销(ms)
        /// </summary>
        public static string Costs
        {
            get { return ResHelper.GetStr("Costs"); }
        }

        /// <summary>
        /// 角色管理
        /// </summary>
        public static string RolesManager
        {
            get { return ResHelper.GetStr("RolesManager"); }
        }

        /// <summary>
        /// 您所选择的上级数据节点与当前节点重名
        /// </summary>
        public static string ParentAndCurrentSameName
        {
            get { return ResHelper.GetStr("ParentAndCurrentSameName"); }
        }
        /// <summary>
        /// 提示:您所删除的服务节点,与客户组存在服务授权关系,请先删除该服务授权关系.
        /// </summary>
        public static string PlzDeleteAuthroizeRelationFirst
        {
            get { return ResHelper.GetStr("PlzDeleteAuthroizeRelationFirst"); }
        }
        /// <summary>
        /// 你设置的客户组账号或授权Key重复
        /// </summary>
        public static string DuplicatedAccountOrKey
        {
            get { return ResHelper.GetStr("DuplicatedAccountOrKey"); }
        }
        /// <summary>
        /// 数据状态
        /// </summary>
        public static string DataStatus
        {
            get { return ResHelper.GetStr("DataStatus"); }
        }
        /// <summary>
        /// 删除数据节点
        /// </summary>
        public static string DeleteDataNode
        {
            get { return ResHelper.GetStr("DeleteDataNode"); }
        }
        /// <summary>
        /// 新增数据节点
        /// </summary>
        public static string AddDataNode
        {
            get { return ResHelper.GetStr("AddDataNode"); }
        }
        /// <summary>
        /// 数据节点编码
        /// </summary>
        public static string DataNodeCode
        {
            get { return ResHelper.GetStr("DataNodeCode"); }
        }
        /// <summary>
        /// 数据节点名称
        /// </summary>
        public static string DataNodeName
        {
            get { return ResHelper.GetStr("DataNodeName"); }
        }
        /// <summary>
        /// 上级节点名称
        /// </summary>
        public static string ParentNodeName
        {
            get { return ResHelper.GetStr("ParentNodeName"); }
        }
        /// <summary>
        /// 服务描述名称
        /// </summary>
        public static string ServiceInfoName
        {
            get { return ResHelper.GetStr("ServiceInfoName"); }
        }
        /// <summary>
        /// 服务方法名称
        /// </summary>
        public static string ServiceMethodName
        {
            get { return ResHelper.GetStr("ServiceMethodName"); }
        }
        /// <summary>
        /// 服务方法全称
        /// </summary>
        public static string ServiceMethodFullName
        {
            get { return ResHelper.GetStr("ServiceMethodFullName"); }
        }
        /// <summary>
        /// 服务状态
        /// </summary>
        public static string ServiceStatus
        {
            get { return ResHelper.GetStr("ServiceStatus"); }
        }
        /// <summary>
        /// 新增服务节点
        /// </summary>
        public static string AddServiceNode
        {
            get { return ResHelper.GetStr("AddServiceNode"); }
        }
        /// <summary>
        /// 删除服务节点
        /// </summary>
        public static string DeleteServiceNode
        {
            get { return ResHelper.GetStr("DeleteServiceNode"); }
        }
        /// <summary>
        /// 授权状态
        /// </summary>
        public static string AuthorizeStatus
        {
            get { return ResHelper.GetStr("AuthorizeStatus"); }
        }
        /// <summary>
        /// 授权人
        /// </summary>
        public static string AccreditBy
        {
            get { return ResHelper.GetStr("AccreditBy"); }
        }
        /// <summary>
        /// 有效期
        /// </summary>
        public static string ValidityDate
        {
            get { return ResHelper.GetStr("ValidityDate"); }
        }
        /// <summary>
        /// 授权Key
        /// </summary>
        public static string AuthorizeKey
        {
            get { return ResHelper.GetStr("AuthorizeKey"); }
        }
        /// <summary>
        /// 客户组账号
        /// </summary>
        public static string ClientId
        {
            get { return ResHelper.GetStr("ClientId"); }
        }
        /// <summary>
        /// 客户组名称
        /// </summary>
        public static string ClientName
        {
            get { return ResHelper.GetStr("ClientName"); }
        }
        /// <summary>
        /// 授驻有效期
        /// </summary>
        public static string AuthorizeDate
        {
            get { return ResHelper.GetStr("AuthorizeDate"); }
        }
        /// <summary>
        /// 新增授权
        /// </summary>
        public static string AddAuthorize
        {
            get { return ResHelper.GetStr("AddAuthorize"); }
        }
        /// <summary>
        /// 编辑授权
        /// </summary>
        public static string EditAuthorize
        {
            get { return ResHelper.GetStr("EditAuthorize"); }
        }
        /// <summary>
        /// 数据授权
        /// </summary>
        public static string DataAuthorize
        {
            get { return ResHelper.GetStr("DataAuthorize"); }
        }
        /// <summary>
        /// 服务授权
        /// </summary>
        public static string ServiceAuthorize
        {
            get { return ResHelper.GetStr("ServiceAuthorize"); }
        }
        /// <summary>
        /// 请选择客户组名称
        /// </summary>
        public static string PlzInputClientName
        {
            get { return ResHelper.GetStr("PlzInputClientName"); }
        }
        /// <summary>
        /// 日志管理
        /// </summary>
        public static string LogManager
        {
            get { return ResHelper.GetStr("LogManager"); }
        }
        /// <summary>
        /// 日志管理
        /// </summary>
        public static string CultureManager
        {
            get { return ResHelper.GetStr("CultureManager"); }
        }

        /// <summary>
        /// 组织管理
        /// </summary>
        public static string OrgManager
        {
            get { return ResHelper.GetStr("OrgManager"); }
        }


        /// <summary>
        /// 岗位维护
        /// </summary>
        public static string PostMaintance
        {
            get { return ResHelper.GetStr("PostMaintance"); }
        }

        /// <summary>
        /// 请输入关键字
        /// </summary>
        public static string PlzEnterKeyword
        {
            get { return ResHelper.GetStr("PlzEnterKeyword"); }
        }
        /// <summary>
        /// 请输入账号
        /// </summary>
        public static string AccountRequired
        {
            get { return ResHelper.GetStr("AccountRequired"); }
        }
        /// <summary>
        /// 密码最少要5个字符
        /// </summary>
        public static string PasswordAtLeast5Chars
        {
            get { return ResHelper.GetStr("PasswordAtLeast5Chars"); }
        }

        /// <summary>
        /// 用户管理
        /// </summary>
        public static string UserManager
        {
            get { return ResHelper.GetStr("UserManager"); }
        }
        /// <summary>
        /// 你所选择的岗位已经添加
        /// </summary>
        public static string PostAlreadyAdded
        {
            get { return ResHelper.GetStr("PostAlreadyAdded"); }
        }
        /// <summary>
        /// 只允许输入英文或数字
        /// </summary>
        public static string OnlyLetterOrNumberAllowed
        {
            get { return ResHelper.GetStr("OnlyLetterOrNumberAllowed"); }
        }
        /// <summary>
        /// 该用户已设置主部门
        /// </summary>
        public static string UserMainDeptExists
        {
            get { return ResHelper.GetStr("UserMainDeptExists"); }
        }
        /// <summary>
        /// 是否调整为当前所设置的部门
        /// 是=以当前部门做主部门 否=不调整主部门
        /// </summary>
        public static string ConfirmAdjToCurrentDept
        {
            get { return ResHelper.GetStr("ConfirmAdjToCurrentDept"); }
        }
        /// <summary>
        /// 请选择一个组织机构
        /// </summary>
        public static string PlzSelectADept
        {
            get { return ResHelper.GetStr("PlzSelectADept"); }
        }
        /// <summary>
        /// 请先删该组织机构所对应的子节点!
        /// </summary>
        public static string PlzDeleteChildNodesFirst
        {
            get { return ResHelper.GetStr("PlzDeleteChildNodesFirst"); }
        }
        /// <summary>
        /// 确定删除所选择组织机构以及相关岗位人员信息?
        /// </summary>
        public static string ConfrimDeleteSelectedDeptPostEmployInfo
        {
            get { return ResHelper.GetStr("ConfrimDeleteSelectedDeptPostEmployInfo"); }
        }
        /// <summary>
        /// 岗位基础信息
        /// </summary>
        public static string PostBaseInfo
        {
            get { return ResHelper.GetStr("PostBaseInfo"); }
        }

        /// <summary>
        /// 部门人员
        /// </summary>
        public static string DeptEmployee
        {
            get { return ResHelper.GetStr("DeptEmployee"); }
        }
        /// <summary>
        /// 请输入名称
        /// </summary>
        public static string NameRequired
        {
            get { return ResHelper.GetStr("NameRequired"); }
        }

        /// <summary>
        /// 属性集
        /// </summary>
        public static string Properties
        {
            get { return ResHelper.GetStr("Properties"); }
        }

        /// <summary>
        /// 是否复制结点和子结点
        /// </summary>
        public static string ConfirmCloneNodeAndChildren
        {
            get { return ResHelper.GetStr("ConfirmCloneNodeAndChildren"); }
        }

        /// <summary>
        /// 确定清空所有日志
        /// </summary>
        public static string ConfirmClearAllLogs
        {
            get { return ResHelper.GetStr("ConfirmClearAllLogs"); }
        }

        /// <summary>
        /// 搜索导航栏
        /// </summary>
        public static string SearchNav
        {
            get { return ResHelper.GetStr("SearchNav"); }
        }

        /// <summary>
        /// 欢迎第一次使用本系统，请您修改密码
        /// </summary>
        public static string LoginChangePasswordFirst
        {
            get { return ResHelper.GetStr("LoginChangePasswordFirst", "欢迎第一次使用本系统，请您修改密码"); }
        }
        /// <summary>
        /// 修改您的登录密码
        /// </summary>
        public static string ChangeYourPassword
        {
            get { return ResHelper.GetStr("ChangeYourPassword", "修改您的登录密码"); }
        }

        /// <summary>
        /// 旧密码
        /// </summary>
        public static string OldPassword
        {
            get { return ResHelper.GetStr("OldPassword", "旧密码"); }
        }
        /// <summary>
        /// 新密码
        /// </summary>
        public static string NewPassword
        {
            get { return ResHelper.GetStr("NewPassword", "新密码"); }
        }
        /// <summary>
        /// 确认新密码
        /// </summary>
        public static string ConfirmPassword
        {
            get { return ResHelper.GetStr("ConfirmPassword", "确认新密码"); }
        }
    }
}