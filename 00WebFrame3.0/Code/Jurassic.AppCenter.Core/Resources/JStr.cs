using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.AppCenter.Resources
{
    /// <summary>
    /// 将常用的资源字符串转换成强类型的名称
    /// 方便查找和调用
    /// </summary>
    public class JStr : IStartupStr
    {
        /// <summary>
        /// 用户
        /// </summary>
        public static string User
        {
            get { return ResHelper.GetStr("User"); }
        }

        /// <summary>
        /// 用户
        /// </summary>
        public static string Users
        {
            get { return ResHelper.GetStr("Users"); }
        }

        /// <summary>
        /// 保存
        /// </summary>
        public static string Save
        {
            get { return ResHelper.GetStr("Save"); }
        }

        /// <summary>
        /// 保存失败
        /// </summary>
        public static string SaveFailed
        {
            get { return ResHelper.GetStr("SaveFailed"); }
        }

        /// <summary>
        /// 删除失败
        /// </summary>
        public static string DeleteFailed
        {
            get { return ResHelper.GetStr("DeleteFailed"); }
        }

        /// <summary>
        /// 删除标记
        /// </summary>
        public static string IsDeleted
        {
            get { return ResHelper.GetStr("IsDeleted"); }
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        public static string UserID
        {
            get { return ResHelper.GetStr("UserID"); }
        }

        /// <summary>
        /// 克隆
        /// </summary>
        public static string Clone
        {
            get { return ResHelper.GetStr("Clone"); }
        }

        /// <summary>
        /// 上移
        /// </summary>
        public static string MoveUp
        {
            get { return ResHelper.GetStr("MoveUp"); }
        }

        /// <summary>
        /// 下移
        /// </summary>
        public static string MoveDown
        {
            get { return ResHelper.GetStr("MoveDown"); }
        }

        /// <summary>
        /// 移动
        /// </summary>
        public static string Move
        {
            get { return ResHelper.GetStr("Move"); }
        }

        /// <summary>
        /// 查找
        /// </summary>
        public static string Search
        {
            get { return ResHelper.GetStr("Search"); }
        }

        /// <summary>
        /// 替换
        /// </summary>
        public static string Replace
        {
            get { return ResHelper.GetStr("Replace"); }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        public static string Refresh
        {
            get { return ResHelper.GetStr("Refresh"); }
        }

        public static string Name
        {
            get { return ResHelper.GetStr("Name"); }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public static string Close
        {
            get { return ResHelper.GetStr("Close"); }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public static string Level
        {
            get { return ResHelper.GetStr("Level"); }
        }

        /// <summary>
        /// 长度
        /// </summary>
        public static string Length
        {
            get { return ResHelper.GetStr("Length"); }
        }

        /// <summary>
        /// 时间
        /// </summary>
        public static string Time
        {
            get { return ResHelper.GetStr("Time"); }
        }

        /// <summary>
        /// 确认删除？
        /// </summary>
        public static string ConfirmDelete
        {
            get { return ResHelper.GetStr("ConfirmDelete"); }
        }

        /// <summary>
        /// 不同语言单词间的分隔符，比如英文是半角空格,中文没有，是空
        /// </summary>
        public static string WordSpc
        {
            get { return ResHelper.GetStr("WordSpc", " "); }
        }

        /// <summary>
        /// 新增
        /// </summary>
        public static string Add
        {
            get { return ResHelper.GetStr("Add"); }
        }

        /// <summary>
        /// 编辑
        /// </summary>
        public static string Edit
        {
            get { return ResHelper.GetStr("Edit"); }
        }

        /// <summary>
        /// 授权
        /// </summary>
        public static string Authorize
        {
            get { return ResHelper.GetStr("Authorize"); }
        }

        /// <summary>
        /// 删除
        /// </summary>
        public static string Delete
        {
            get { return ResHelper.GetStr("Delete"); }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public static string UserName
        {
            get { return ResHelper.GetStr("UserName"); }
        }

        /// <summary>
        /// Email
        /// </summary>
        public static string Email
        {
            get { return ResHelper.GetStr("Email"); }
        }

        /// <summary>
        /// 电话
        /// </summary>
        public static string PhoneNumber
        {
            get { return ResHelper.GetStr("PhoneNumber"); }
        }

        /// <summary>
        /// 保存成功
        /// </summary>
        public static string SuccessSaved
        {
            get { return ResHelper.GetStr("SuccessSaved"); }
        }

        /// <summary>
        /// 保存成功(带1个参数）
        /// </summary>
        public static string SuccessSaved0
        {
            get { return ResHelper.GetStr("SuccessSaved0"); }
        }

        /// <summary>
        /// 添加成功（带1个参数）
        /// </summary>
        public static string SuccessAdded0
        {
            get { return ResHelper.GetStr("SuccessAdded0"); }
        }

        /// <summary>
        /// 添加成功
        /// </summary>
        public static string SuccessAdded
        {
            get { return ResHelper.GetStr("SuccessAdded"); }
        }

        /// <summary>
        /// 删除成功
        /// </summary>
        public static string SuccessDeleted
        {
            get { return ResHelper.GetStr("SuccessDeleted"); }
        }

        /// <summary>
        /// 带一个参数的删除成功信息
        /// </summary>
        public static string SuccessDeleted0
        {
            get { return ResHelper.GetStr("SuccessDeleted0"); }
        }

        /// <summary>
        /// 成功更新
        /// </summary>
        public static string SuccessUpdated
        {
            get
            {
                return ResHelper.GetStr("SuccessUpdated");
            }
        }

        /// <summary>
        /// 带一个参数的成功更新
        /// </summary>
        public static string SuccessUpdated0
        {
            get
            {
                return ResHelper.GetStr("SuccessUpdated0");
            }
        }

        /// <summary>
        /// 操作成功
        /// </summary>
        public static string OperationSucceed
        {
            get
            {
                return ResHelper.GetStr("OperationSucceed");
            }
        }

        /// <summary>
        /// 操作失败
        /// </summary>
        public static string OperationFailed
        {
            get
            {
                return ResHelper.GetStr("OperationFailed");
            }
        }

        /// <summary>
        /// 成功移动
        /// </summary>
        public static string SuccessMoved
        {
            get { return ResHelper.GetStr("SuccessMoved"); }
        }
        /// <summary>
        /// 状态
        /// </summary>
        public static string Status
        {
            get { return ResHelper.GetStr("Status"); }
        }

        /// <summary>
        /// 成功移动,带一个参数
        /// </summary>
        public static string SuccessMoved0
        {
            get { return ResHelper.GetStr("SuccessMoved0"); }
        }
        /// <summary>
        /// 排序
        /// </summary>
        public static string Sort
        {
            get { return ResHelper.GetStr("Sort"); }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public static string Remark
        {
            get { return ResHelper.GetStr("Remark"); }
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        public static string CreatedDate
        {
            get { return ResHelper.GetStr("CreatedDate"); }
        }
        /// <summary>
        /// 角色
        /// </summary>
        public static string Role
        {
            get { return ResHelper.GetStr("Role"); }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public static string Description
        {
            get { return ResHelper.GetStr("Description"); }
        }

        /// <summary>
        /// 基本信息
        /// </summary>
        public static string BaseInfo
        {
            get { return ResHelper.GetStr("BaseInfo"); }
        }

        /// <summary>
        /// 地址
        /// </summary>
        public static string Address
        {
            get { return ResHelper.GetStr("Address"); }
        }
        /// <summary>
        /// 参数
        /// </summary>
        public static string Parameter
        {
            get { return ResHelper.GetStr("Parameter"); }
        }
        /// <summary>
        /// 父ID
        /// </summary>
        public static string ParentID
        {
            get { return ResHelper.GetStr("ParentID"); }
        }
        /// <summary>
        /// 顺序
        /// </summary>
        public static string Order
        {
            get { return ResHelper.GetStr("Order"); }
        }

        /// <summary>
        /// 消息
        /// </summary>
        public static string Message
        {
            get { return ResHelper.GetStr("Message"); }
        }
        /// <summary>
        /// 请求
        /// </summary>
        public static string Request
        {
            get { return ResHelper.GetStr("Request"); }
        }
        /// <summary>
        /// 操作人
        /// </summary>
        public static string Operator
        {
            get { return ResHelper.GetStr("Operator"); }
        }

        /// <summary>
        /// 请先选择要删除的行
        /// </summary>
        public static string PlzSelectDataRowsToDelete
        {
            get { return ResHelper.GetStr("PlzSelectDataRowsToDelete"); }
        }

        /// <summary>
        /// 预览
        /// </summary>
        public static string Preview
        {
            get { return ResHelper.GetStr("Preview"); }
        }

        /// <summary>
        /// 确定
        /// </summary>
        public static string OK
        {
            get { return ResHelper.GetStr("OK"); }
        }

        /// <summary>
        /// 取消
        /// </summary>
        public static string Cancel
        {
            get { return ResHelper.GetStr("Cancel"); }
        }

        /// <summary>
        /// 标题
        /// </summary>
        public static string Title
        {
            get { return ResHelper.GetStr("Title"); }
        }

        /// <summary>
        /// 关键字
        /// </summary>
        public static string Keyword
        {
            get { return ResHelper.GetStr("Keyword"); }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public static string Abstract
        {
            get { return ResHelper.GetStr("Abstract"); }
        }
        /// <summary>
        /// 文件名
        /// </summary>
        public static string FileName
        {
            get { return ResHelper.GetStr("FileName"); }
        }

        /// <summary>
        /// 请选择一个结点
        /// </summary>
        public static string PlzSelectANode
        {
            get { return ResHelper.GetStr("PlzSelectANode"); }
        }
        /// <summary>
        /// 请选择一条记录
        /// </summary>
        public static string PlzSelectARecord
        {
            get { return ResHelper.GetStr("PlzSelectARecord"); }
        }
        /// <summary>
        /// 内容
        /// </summary>
        public static string Content
        {
            get { return ResHelper.GetStr("Content"); }
        }
        /// <summary>
        /// 提交
        /// </summary>
        public static string Submit
        {
            get { return ResHelper.GetStr("Submit"); }
        }
        /// <summary>
        /// 开始时间
        /// </summary>
        public static string StartTime
        {
            get { return ResHelper.GetStr("StartTime"); }
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        public static string EndTime
        {
            get { return ResHelper.GetStr("EndTime"); }
        }

        /// <summary>
        /// 错误
        /// </summary>
        public static string Error
        {
            get { return ResHelper.GetStr("Error"); }
        }
        /// <summary>
        /// 关键字
        /// </summary>
        public static string Key
        {
            get { return ResHelper.GetStr("Key"); }
        }
         /// <summary>
        /// 文本
        /// </summary>
        public static string Text
        {
            get { return ResHelper.GetStr("Text"); }
        }

        /// <summary>
        /// 系统名称
        /// </summary>
        public static string SystemName
        {
            get { return ResHelper.GetStr("SystemName"); }
        }
        /// <summary>
        /// 系统简称
        /// </summary>
        public static string SystemShortName
        {
            get { return ResHelper.GetStr("SystemShortName"); }
        }
        /// <summary>
        /// 系统版权信息
        /// </summary>
        public static string SystemCopyright
        {
            get { return ResHelper.GetStr("SystemCopyright"); }
        }
        /// <summary>
        /// 公司名称
        /// </summary>
        public static string SystemCompanyName
        {
            get { return ResHelper.GetStr("SystemCompanyName"); }
        }

        /// <summary>
        /// 公司信息，地址，电话等
        /// </summary>
        public static string SystemCompanyInfo
        {
            get { return ResHelper.GetStr("SystemCompanyInfo"); }
        }
        /// <summary>
        /// 请求数据无效
        /// </summary>
        public static string InvalidRequestData
        {
            get { return ResHelper.GetStr("InvalidRequestData"); }
        }

        /// <summary>
        /// 启用
        /// </summary>
        public static string Enabled
        {
            get { return ResHelper.GetStr("Enabled"); }
        }
        
        /// <summary>
        /// 禁用
        /// </summary>
        public static string Disabled
        {
            get { return ResHelper.GetStr("Disabled"); }
        }

        /// <summary>
        /// 打开
        /// </summary>
        public static string Open
        {
            get { return ResHelper.GetStr("Disabled"); }
        }

        /// <summary>
        /// 清空
        /// </summary>
        public static string Clear
        {
            get { return ResHelper.GetStr("Clear"); }
        }

        /// <summary>
        /// 语言
        /// </summary>
        public static string Language
        {
            get { return ResHelper.GetStr("Language"); }
        }
        /// <summary>
        /// 只读
        /// </summary>
        public static string ReadOnly
        {
            get { return ResHelper.GetStr("ReadOnly"); }
        }
        /// <summary>
        /// 操作
        /// </summary>
        public static string Operation
        {
            get { return ResHelper.GetStr("Operation"); }
        }
    }
}
