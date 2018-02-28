using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.GF.Server.DBUtility
{
    /// <summary>
    /// 获得连接字符串ConnectingString
    /// </summary>
    public class SqlConnString
    {
        public static string ReturnConnString()
        {
            string _dal = System.Configuration.ConfigurationManager.AppSettings["GFDAL"];//获取App.Config中DAL
            string _conn = System.Configuration.ConfigurationManager.AppSettings["GFSqlConn"];//获取App.Config中DAL
            return _conn;
        }
    }
}
