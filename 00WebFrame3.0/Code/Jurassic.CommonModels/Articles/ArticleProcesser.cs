using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Jurassic.Com.Tools;
using System.Text.RegularExpressions;
using System.Data.Entity;
using Jurassic.AppCenter;
using Jurassic.Com.DB;

namespace Jurassic.CommonModels.Articles
{

    /// <summary>
    /// 文章点击率加1
    /// </summary>
    public class ArticleProcesser : ProcesserBase
    {
        public override void Process(object artId)
        {
            DBHelper helper = new DBHelper("DefaultConnection");
            helper.ExecNonQuery("UPDATE Base_Article SET Clicks=Clicks+1 WHERE Id=" + artId);
        }
    }
}