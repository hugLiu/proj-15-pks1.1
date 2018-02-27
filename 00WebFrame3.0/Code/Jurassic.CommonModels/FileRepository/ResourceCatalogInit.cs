using System.IO;
using System.Web;
using Jurassic.AppCenter;
using Jurassic.CommonModels.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;

namespace Jurassic.CommonModels.FileRepository
{
    public class ResourceCatalogInit
    {

        public ResourceCatalogInit()
        {
            //使用定义的SystemTypes栏目类来初始化一个SystemTypes栏目
            //如栏目已存在，则只读取栏目
            SiteManager.Catalog.InitStaticCatalogs(typeof(SystemTypes));

            //初始化头像数据
            var Article = SiteManager.Kernel.Get<EFAuditDataService<Base_Article>>();
            var artList = Article.GetQuery().Where(a => a.State == (ArticleState.New + ArticleState.ReadOnly)).ToList();
            if (artList.Count == 0 && HttpContext.Current != null)
            {
                InnerInitSysAvatar();
            }
        }


        /// <summary>
        /// 初始化系统自带的头像
        /// </summary>
        private void InnerInitSysAvatar()
        {
            var rootpath = HttpContext.Current.Server.MapPath("/");
            var filepath = Path.Combine(rootpath, "Avatar");
            DirectoryInfo folder = new DirectoryInfo(filepath);
            //目录的根节点
            var rootId = SiteManager.Catalog.GetAllValid().First(cat => cat.Name == "SystemTypes" && cat.ParentId == null).Id;
            var Article = SiteManager.Kernel.Get<EFAuditDataService<Base_Article>>();
            foreach (FileInfo file in folder.GetFiles())
            {
                Base_Article art = new Base_Article
                {
                    State = ArticleState.ReadOnly | ArticleState.New,//表示是头像 的状态
                    Title = file.Name,
                    EditorId = Convert.ToInt32(AppManager.Instance.UserManager.GetAll().FirstOrDefault(a => a.Id != null).Id),
                };
                //查找扩展属性ID，对应ID赋值
                art.SetExt(SiteManager.Catalog.GetExtByName(rootId, SystemTypes.Root.Key), "~/Avatar/" + file.Name);
                art.SetExt(SiteManager.Catalog.GetExtByName(rootId, SystemTypes.Root.FileName), file.Name);
                art.SetExt(SiteManager.Catalog.GetExtByName(rootId, SystemTypes.Root.FileSize), file.Length);
                Article.Add(art);
            }
        }
    }

    class SystemTypes
    {
        /// <summary>
        /// Id是必须的。它的值会在初始化时被数据库的"SystemTypes"所在的栏目替代
        /// </summary>

        public int Id { get; set; }

        /// <summary>
        /// Name是必须的。它的值会在初始化时被"SystemTypes"替代
        /// </summary>
        public string Name { get; set; }

        //以下的属性定义会存放在Base_CatalogExt中
        //它们的值会被自动初始化为等同于属性名
        public string MD5Code { get; set; }
        public string Key { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
        [CatalogExt(DataType = ExtDataType.SingleNumber)]
        public string FileSize { get; set; }

        /// <summary>
        /// 以下的静态对象方便程序调用来获取对应的属性名称
        /// </summary>
        public static SystemTypes Root { get; set; }
        public static Files Files { get; set; }
        public static Pictures Pictures { get; set; }
        public static UserFiles UserFiles { get; set; }
        public static UserAvatar UserAvatar { get; set; }


    }

    /// <summary>
    /// 还可以定义SystemTypes下面的子栏目
    /// </summary>
    class Files : SystemTypes
    {

    }
    class Pictures : SystemTypes
    {

    }

    class UserFiles : SystemTypes
    {

    }
    class UserAvatar : SystemTypes
    {

    }

}
