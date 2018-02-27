using System.Text;
using Jurassic.AppCenter;
using Jurassic.Com.DB;
using Jurassic.CommonModels;
using Jurassic.CommonModels.Articles;
using Jurassic.WebRepeater.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using Jurassic.CommonModels.EFProvider;
using Jurassic.WebSchedule;

namespace Jurassic.WebTemplate.Controllers
{
    /// <summary>
    /// 该示例以一个留言板演示框架内容管理
    /// </summary>
    public class NoteBookController : Jurassic.WebRepeater.Controllers.ArticleController
    {
        public NoteBookController()
        {
            //使用定义的NoteBook栏目类来初始化一个NoteBook栏目
            //如栏目已存在，则只读取栏目
            SiteManager.Catalog.InitStaticCatalogs(typeof(NoteBook));
        }

        public override int CatalogId
        {
            get
            {
                return NoteBook.NoteBookCatalog.Id;
            }
        }

        public void AddNoteBookTest(int count, string SqlOrEF)
        {

            if (SqlOrEF == "ef")
            {
                SignalRProcesserFactory.Instance.Register("NoteBookTest", new NoteBookTransProcesser());
            }
            else
            {
                SignalRProcesserFactory.Instance.Register("NoteBookTest", new NoteBookSQLTransProcesser());
            }
            //SignalRProcesserFactory.Instance.Register("NoteBookTest", new NoteBookProcesser());
            //SignalRProcesserFactory.Instance.Register("NoteBookTest", new NoteBookTransProcesser());

            //将任务加入到队列中
            SignalRProcesserFactory.Instance.Add("NoteBookTest", count);
            //开始执行队列任务
            SignalRProcesserFactory.Instance.Start("NoteBookTest");
        }

        public void StopAddNoteBookTest()
        {
            SignalRProcesserFactory.Instance.Stop("NoteBookTest");
        }
    }

    /// <summary>
    /// 该类是为了定义栏目的扩展属性
    /// 在SiteManager.Catalog.InitStaticCatalogs方法初始化后，
    /// 数据库中会增加同类名的栏目。
    /// </summary>
    class NoteBook
    {
        /// <summary>
        /// Id是必须的。它的值会在初始化时被数据库的"NoteBook"所在的栏目替代
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name是必须的。它的值会在初始化时被"NoteBook"替代
        /// </summary>
        public string Name { get; set; }

        //以下的属性定义会存放在Base_CatalogExt中
        //它们的值会被自动初始化为等同于属性名
        public string QQ { get; set; }

        public string Tel { get; set; }

        public string Email { get; set; }

        /// <summary>
        /// 以下的静态对象方便程序调用来获取对应的属性名称
        /// </summary>
        public static NoteBook NoteBookCatalog { get; set; }

        public static FAQ FAQ { get; set; }
    }

    /// <summary>
    /// 还可以定义NoteBook下面的子栏目
    /// </summary>
    class FAQ : NoteBook
    {
        /// <summary>
        /// 使用;号分隔的项来表示下拉列表选项
        /// </summary>
        [CatalogExt(DataSource = "WebFrame;WinFrame;WcfFrame", DataSourceType = ExtDataSourceType.DirectList)]
        public string QType { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }

    /// <summary>
    /// 使用事务批量插入数据效率比不使用事务快
    /// </summary>
    public class NoteBookTransProcesser : ProcesserBase
    {
        public override void Process(object cnt)
        {
            int count = (int)cnt;
            int div = count / 1000;
            if (div < 1) div = 1;
            if (div > 100) div = 100;
            var provider = SiteManager.Get<ArticleProvider>();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var catId = NoteBook.NoteBookCatalog.Id;
            var exts = SiteManager.Catalog.GetAllExts(catId);
            var user = AppManager.Instance.UserManager.GetAll().FirstOrDefault();
            provider.BeginTrans();
            for (int i = 0; i < count && this.Enabled; i++)
            {
                //todo:此处做增加Base_Article数据的操作
                var ca = SiteManager.Get<ArticleManager>().CreateByCatalog(catId);
                var art = ca.Article;
                art.Id = 0;
                art.Title = i + "+" + DateTime.Now;
                art.Text = i + "+这是内容" + DateTime.Now;
                art.State = ArticleState.Published;
                art.EditorId = user == null ? 1 : Convert.ToInt32(user.Id);
                if (exts.Count() != 0)
                {
                    foreach (var ex in exts)
                    {
                        ca.Article.Exts.FirstOrDefault(e => e.CatlogExtId == ex.Id).Value = i + "+扩展属性+" + ex.Name;
                    }
                }

                provider.Add(ca);

                //为防止事务过长造成数据库锁死内存溢出
                if (i % 100 == 0 && i > 0)
                {
                    provider.EndTrans();
                    provider.Dispose();
                    provider = SiteManager.Get<ArticleProvider>();

                    provider.BeginTrans();
                }
                //当每插入一定量的数据时报告一次进度
                if (i % div == 0)
                {
                    SignalRProcesserFactory.Instance.Group("NoteBookTest")
                        .reportMyProgress(new { proc = i * 100.0 / count, cost = sw.ElapsedMilliseconds / 1000 });
                }
                //数据插入完毕
            }
            provider.EndTrans();
            if (this.Enabled)
            {
                SignalRProcesserFactory.Instance.Group("NoteBookTest").done();
            }
        }
    }

    /// <summary>
    /// 使用常规方法批量新增数据
    /// </summary>
    public class NoteBookProcesser : ProcesserBase
    {
        public override void Process(object cnt)
        {
            int count = (int)cnt;
            int div = count / 1000;
            if (div < 1) div = 1;
            if (div > 100) div = 100;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var catId = NoteBook.NoteBookCatalog.Id;
            var exts = SiteManager.Catalog.GetAllExts(catId);
            var user = AppManager.Instance.UserManager.GetAll().FirstOrDefault();
            int i = 0;
            using (var article = SiteManager.Get<ArticleManager>())
            {
                for (i = 0; i < count && this.Enabled; i++)
                {
                    //todo:此处做增加Base_Article数据的操作
                    var ca = article.CreateByCatalog(catId);
                    var art = ca.Article;
                    art.Id = 0;
                    art.Title = i + "+" + DateTime.Now;
                    art.Text = i + "+这是内容" + DateTime.Now;
                    art.State = ArticleState.Published;
                    art.EditorId = user == null ? 1 : Convert.ToInt32(user.Id);
                    if (exts.Count() != 0)
                    {
                        foreach (var ex in exts)
                        {
                            ca.Article.Exts.FirstOrDefault(e => e.CatlogExtId == ex.Id).Value = i + "+扩展属性+" + ex.Name;
                        }
                    }

                    article.Save(ca);
                }

                //当每插入一定量的数据时报告一次进度
                if (i % div == 0)
                {
                    SignalRProcesserFactory.Instance.Group("NoteBookTest")
                        .reportMyProgress(new { proc = i * 100.0 / count, cost = sw.ElapsedMilliseconds / 1000 });//没有使用ProcessedPercent，因为精度不够
                }
                //数据插入完毕
            }
            if (this.Enabled)
            {
                SignalRProcesserFactory.Instance.Group("NoteBookTest").done();
            }
        }
    }

    /// <summary>
    /// 使用SQl语句插入数据
    /// </summary>
    public class NoteBookSQLTransProcesser : ProcesserBase
    {
        public override void Process(object cnt)
        {
            int count = (int)cnt;
            int div = count / 1000;
            if (div < 1) div = 1;
            if (div > 100) div = 100;
            DBHelper helper = new DBHelper("DefaultConnection");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var catId = NoteBook.NoteBookCatalog.Id;
            var exts = SiteManager.Catalog.GetAllExts(catId);
            var user = AppManager.Instance.UserManager.GetAll().FirstOrDefault();
            helper.BeginTrans();
            int i = 0;
            using (var article = SiteManager.Get<ArticleManager>())
            {
                for ( i = 0; i < count && this.Enabled; i++)
                {
                    //todo:此处做增加Base_Article数据的操作
                    var ca = article.CreateByCatalog(catId);
                    var art = ca.Article;
                    art.Id = 0;
                    art.Title = i + "+" + DateTime.Now;
                    art.Text = i + "+这是内容" + DateTime.Now;
                    art.State = ArticleState.Published;
                    art.EditorId = user == null ? 1 : Convert.ToInt32(user.Id);

                    //插入Article数据的sql语句
                    string insertArtSql = string.Format("insert into Base_Article(Title,State,EditorId,CreateTime,EditTime) values('{0}',{1},{2},'{3}','{4}') select @@identity",
                        art.Title, Convert.ToInt32(art.State), art.EditorId, DateTime.Now, DateTime.Now);
                    //获得现在插入的数据的Id
                    string artId = helper.TransGetObject(insertArtSql).ToString();


                    StringBuilder insertSql = new StringBuilder();

                    //插入扩展属性的值
                    if (exts.Count() != 0)
                    {
                        foreach (var ex in exts)
                        {
                            insertSql.AppendFormat(
                                    "insert into Base_ArticleExt(ArticleId,CatlogExtId,Value) values({0},{1},'{2}')", artId,
                                    ex.Id, i + "+扩展属性+" + ex.Name);
                        }
                    }

                    //插入Text的值
                    insertSql.AppendFormat("insert into Base_ArticleText values({0},'{1}')", artId, art.Text);

                    //插入CatalogArticle的数据
                    insertSql.AppendFormat("insert into Base_CatalogArticle(CatalogId,ArticleId,CreateTime) values({0},{1},'{2}')",
                            catId, artId, DateTime.Now);

                    //插入到表中。ArticleExt、ArticleText、CatalogArticle
                    helper.TransNonQuery(insertSql.ToString());

                    //为防止事务过长造成数据库锁死内存溢出
                    if (i % 1000 == 0 && i > 0)
                    {
                        helper.EndTrans();

                        helper = new DBHelper("DefaultConnection");

                        helper.BeginTrans();
                    }
                }
                //当每插入一定量的数据时报告一次进度
                if (i % div == 0)
                {
                    SignalRProcesserFactory.Instance.Group("NoteBookTest")
                        .reportMyProgress(new { proc = i * 100.0 / count, cost = sw.ElapsedMilliseconds / 1000 });
                }
                //数据插入完毕
            }
            helper.EndTrans();
            if (this.Enabled)
            {
                SignalRProcesserFactory.Instance.Group("NoteBookTest").done();
            }
        }
    }
}
