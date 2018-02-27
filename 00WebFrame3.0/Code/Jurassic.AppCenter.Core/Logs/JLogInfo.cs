using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jurassic.AppCenter.Logs
{
    /// <summary>
    /// 系统日志数据实体类
    /// </summary>
    public class JLogInfo : IId<int>
    {
        [Column("ID")]
        public int Id { get; set; }

        [Column("MODULENAME")]
        public string ModuleName { get; set; }
        [Column("ACTIONNAME")]
        public string ActionName { get; set; }
        [Column("USERNAME")]
        public string UserName { get; set; }
        [Column("CLIENTIP")]
        public string ClientIP { get; set; }
        [Column("OPTIME")]
        public DateTime OpTime { get; set; }
        [Column("CATALOGID")]
        public int CatalogId { get; set; }
        [Column("OBJECTID")]
        public int ObjectId { get; set; }
        [Column("LOGTYPE")]
        public string LogType { get; set; }
        [Column("REQUEST")]
        public string Request { get; set; }
        [Column("COSTS")]
        public Double Costs { get; set; }
        [Column("MESSAGE")]
        public string Message { get; set; }
        [Column("BROWSER")]
        public string Browser { get; set; }
        [Column("BROWSERVERSION")]
        public decimal BrowserVersion { get; set; }
        [Column("PLATFORM")]
        public string Platform { get; set; }

        public override string ToString()
        {
            return UserName + ":" + Message;
        }

    }
}
