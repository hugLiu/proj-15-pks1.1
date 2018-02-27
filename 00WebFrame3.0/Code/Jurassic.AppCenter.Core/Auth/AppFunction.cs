using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jurassic.Com.Tools;
using System.Text.RegularExpressions;
using Jurassic.AppCenter.Logs;
using Newtonsoft.Json;

namespace Jurassic.AppCenter
{
    /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
    /// <summary>
    /// 系统功能实体类
    /// </summary>
    /// <namespace>
    /// <summary>
    /// Jurassic.AppCenter命名空间为所有应用程序提供公共服务组件，它们不依赖于具体的平台
    /// 和第三方组件。
    /// </summary>
    /// </namespace>
    public class AppFunction : IIdNameParentId<string>
    {
        /// <summary>
        /// 新建一个系统功能实体类
        /// </summary>
        public AppFunction()
        {
            LogType = JLogType.Info;
            Parameters = new List<AppParameter>();
            RelatedIds = new List<string>();
        }

        /// <summary>
        /// 功能的父ID
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 功能ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 功能名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 功能的地址（一般是URL）
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 地址示例
        /// </summary>
        public string LocationSamples { get; set; }

        /// <summary>
        /// 图标的地址
        /// </summary>
        public string IconLocation { get; set; }

        public string IconClass { get; set; }

        /// <summary>
        /// Url后缀的正则表达式
        /// </summary>
        public string RegTail { get; set; }

        /// <summary>
        /// 从该功能的祖先到该功能的逗号分隔的ID路径
        /// </summary>
      //  public string IdPath { get; set; }

        /// <summary>
        /// 发送请求时是Get还是Post
        /// </summary>
        public WebMethod Method { get; set; }

        /// <summary>
        /// 授权类型
        /// </summary>
        public JAuthType AuthType { get; set; }

        /// <summary>
        /// 功能的描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 在同级中，功能的排序位
        /// </summary>
        public int Ord { get; set; }

        /// <summary>
        /// 是否在菜单中显示
        /// </summary>
        public VisibleType Visible
        {
            get;
            set;
        }

        /// <summary>
        /// 使用功能要输入的参数名数组
        /// </summary>
        public List<AppParameter> Parameters { get; set; }

        /// <summary>
        /// 该功能对应的控制器类型全称
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// 该功能对应的方法名称
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// 该功能所在区域名称
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// 本功能被日志记录的级别，如果低于系统规定的级别，则该功能的操作记录不会被记录在日志中，
        /// 除非有异常,默认级别是INFO
        /// </summary>
        public JLogType LogType { get; set; }

        /// <summary>
        /// 关联的ID(不是指它的子级, 主要用于前台菜单和后台服务的关联)
        /// </summary>
        public List<string> RelatedIds { get; set; }

        /// <summary>
        /// 判断用户访问的地址的格式是否与指定AppFunction定义的相同
        /// </summary>
        /// <param name="userLoc">用户的地址</param>
        /// <returns>是/否</returns>
        public bool MatchLocation(string userLoc)
        {
            string funcLoc = Location ?? "";
            string regTail = RegTail;
            if (regTail.IsEmpty())
            {
                userLoc = CommOp.CutStr(userLoc, '?', '#');
                if (!funcLoc.EndsWith("/")) funcLoc += "/";
                if (!userLoc.EndsWith("/")) userLoc += "/";
                if (funcLoc.Equals(userLoc, StringComparison.OrdinalIgnoreCase)) return true;
            }
            else
            {
                string pattern = "^" + funcLoc + "(" + regTail + ")";
                if (Regex.IsMatch(userLoc, pattern, RegexOptions.IgnoreCase)) return true;
            }
            return false;
        }

        /// <summary>
        /// 判断示例地址中的地址是否与给定的地址规则匹配
        /// </summary>
        /// <returns>是/否</returns>
        public bool IsSampleVaild()
        {
            if (LocationSamples.IsEmpty()) return true;
            var samples = LocationSamples.Trim().Replace("\r\n", "\n").Split('\n').Where(s => !s.IsEmpty());

            foreach (var s in samples)
            {
                if (!MatchLocation(s)) return false;
            }
            return true;
        }

        /// <summary>
        /// 返回一个可读的功能名字符串。
        /// </summary>
        /// <returns>功能名字符串</returns>
        public override string ToString()
        {
            string p = "";
            if (!Parameters.IsEmpty())
            {
                p = "(" + String.Join(", ", Parameters.Select(pe => pe.Name)) + ")";
            }
            string f = String.Format("{0}:{1}{2} {3}", Id, Name, p, Method == WebMethod.POST ? "[POST]" : "");
            return f;
        }

        /// <summary>
        /// 用于统一表示参数信息的字符串
        /// </summary>
        [JsonIgnore]
        public string PMark
        {
            get
            {
                return String.Join("\n", Parameters.Select(p => p.Name + "=" + p.ValuePattern));
            }
            set
            {
                if (value.IsEmpty()) return;
                Parameters.Clear();
                string[] ps = value.Split('\n');
                foreach (string p in ps)
                {
                    var pp = p.Split('=');
                    AppParameter ap = new AppParameter();
                    ap.Name = pp[0].ToStr();
                    if (pp.Length >= 2)
                    {
                        ap.ValuePattern = pp[1].ToStr();
                    }
                    if (!ap.Name.IsEmpty())
                    {
                        Parameters.Add(ap);
                    }
                }
            }
        }
        /// <summary>
        /// 判断两个功能点是否实质上等价
        /// </summary>
        /// <param name="other">另一个功能点</param>
        /// <returns>是否实质上等价</returns>
        public bool IsTheSame(AppFunction other)
        {
            return other.ActionName == ActionName
                && other.ControllerName == ControllerName
                && other.Location == Location
                && other.Parameters.Count == Parameters.Count
                && other.Parameters.Except(Parameters).Count() == 0
                && other.RegTail == RegTail
                && other.Method == Method;
        }
    }
}
