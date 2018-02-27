using Jurassic.AppCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jurassic.Com.Tools;

using Jurassic.AppCenter.Resources;
using System.Threading;
using System.Reflection;
using Jurassic.CommonModels.FileRepository;
using System.Data;

namespace Jurassic.CommonModels.Articles
{
    /// <summary>
    /// 网站分类栏目管理业务类, 此类最好初始化为单例
    /// </summary>
    public class CatalogManager : DataManagerBase<Base_Catalog, int>
    {
        public CatalogManager(IDataProvider<Base_Catalog> provider)
        {
            this.Provider = provider;
            //InitData();
        }

        /// <summary>
        /// 获取直接下级
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Base_Catalog> GetChildren(int id)
        {
            return GetAllValid().Where(cat => cat.ParentId == id).ToList();
        }

        /// <summary>
        /// 根据栏目ID和语言ID获取它的所有子栏目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Base_Catalog> GetChildrenByLang(int id)
        {
            return GetAllValid().Where(cat => cat.ParentId == id
                && IsCurrentLangOrEmpty(cat)).OrderBy(cat => cat.Ord).ToList();
        }

        /// <summary>
        /// 获取本栏目有效的扩展属性
        /// 其规则为：和父栏目属性合并，如果同名则以子栏目属性为主
        /// </summary>
        /// <param name="id">栏目ID</param>
        /// <returns>合并以后栏目的总属性表</returns>
        public IEnumerable<Base_CatalogExt> GetAllExts(int id)
        {
            List<Base_CatalogExt> extList = new List<Base_CatalogExt>();
            while (id > 0)
            {
                Base_Catalog cat = GetById(id);
                var pExts = cat.Exts.Where(ext => ext.IsDeleted == false);
                foreach (var pExt in pExts)
                {
                    //找出子栏目没有而父栏目有的属性，加到子栏目的属性表
                    if (!extList.Any(ext => ext.Name == pExt.Name))
                    {
                        extList.Add(pExt);
                    }
                }
                id = CommOp.ToInt(cat.ParentId);
            }
            var exts = extList.OrderBy(ext => ext.Ord);
            return exts;
        }


        /// <summary>
        /// 获取目录的扩展属性ID
        /// </summary>
        /// <param name="catalogId">所在目录的ID</param>
        /// <param name="extName">扩展属性的Name</param>
        /// <returns></returns>
        public int GetExtId(int catalogId, string extName)
        {
            int id = GetAllExts(catalogId).FirstOrDefault(f => f.Name == extName).Id;
            return id;
        }

        /// <summary>
        /// 获取所有后代
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Base_Catalog> GetDescendant(int id)
        {
            List<Base_Catalog> descs = new List<Base_Catalog>();

            var children = GetChildren(id);

            foreach (var child in children)
            {
                descs.Add(child);
                descs.AddRange(GetDescendant(child.Id));
            }

            return descs;
        }

        /// <summary>
        /// 判断某个栏目是否是另一个栏目的子孙或它自己
        /// </summary>
        /// <param name="rootId">上级栏目ID</param>
        /// <param name="id">下级栏目ID</param>
        /// <returns>是/否</returns>
        public bool IsDescendant(int rootId, int id)
        {
            if (rootId == id) return true;
            var children = GetChildren(rootId);
            foreach (var chd in children)
            {
                if (IsDescendant(chd.Id, id))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 判断某个栏目是否另一栏目的直接下级或它自身
        /// </summary>
        /// <param name="rootId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsChild(int rootId, int id)
        {
            if (rootId == id) return true;
            var children = GetChildren(rootId);
            return children.Select(chd => chd.Id).Contains(id);
        }

        /// <summary>
        /// 获取所有后代，栏目语言为当前语言
        /// </summary>
        /// <param name="ids">栏目ID列表</param>
        /// <returns>所有指定ID栏目的当前语言子栏目</returns>
        public List<Base_Catalog> GetDescendantByLang(params int[] ids)
        {
            List<Base_Catalog> descs = new List<Base_Catalog>();
            var children = new List<Base_Catalog>();
            foreach (var id in ids)
            {
                children.AddRange(GetChildrenByLang(id));
            }

            foreach (var child in children)
            {
                descs.Add(child);
                descs.AddRange(GetDescendantByLang(child.Id));
            }

            return descs;
        }

        /// <summary>
        /// 根据栏目Id获取所有的子栏目Id集合
        /// </summary>
        /// <param name="catId"></param>
        /// <returns></returns>
        public List<int> GetChildrenIdsByCatId(int catId)
        {
            var childrenIds = GetDescendantByLang(catId)
                  .Where(cat => IsCurrentLangOrEmpty(cat))
                  .Select(cat => cat.Id)
                  .ToList();
            childrenIds.Add(catId);
            return childrenIds;
        }

        internal bool IsCurrentLangOrEmpty(Base_Catalog cat)
        {
            return cat.Language.IsEmpty() || cat.Language.Equals(GetCurrentLanguage(), StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 获取所有用户可见的栏目列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<Base_Catalog> GetUserVisibleCatalogs(int userId)
        {
            return GetUserOwneredCatalogs(userId)
                .Union(GetUserSharedCatalogs(userId));
        }

        /// <summary>
        /// 获取所有用户共享的栏目列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<Base_Catalog> GetUserSharedCatalogs(int userId)
        {
            return GetAllValid().Where(cat => (cat.State & ArticleState.Shared) == ArticleState.Shared);
        }

        /// <summary>
        /// 获取用户所属的栏目列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<Base_Catalog> GetUserOwneredCatalogs(int userId)
        {
            return GetAllValid().Where(cat => cat.OwnerType == CatalogOwnerType.User && cat.OwnerId == userId);
        }

        /// <summary>
        /// 获取根目录ID
        /// </summary>
        /// <param name="name">根目录名称</param>
        /// <returns></returns>
        public virtual int GetRootId()
        {
            return GetAllValid().First(cat => cat.Name == "SystemTypes" && cat.ParentId == null).Id;
        }

        /// <summary>
        /// 获取所有未删除的栏目
        /// </summary>
        /// <returns></returns>
        public virtual List<Base_Catalog> GetAllValid()
        {
            return GetAll()
                .Where(cat => cat.IsDeleted == false)
                .ToList();
        }

        /// <summary>
        /// 提交修改并返回树结点信息
        /// </summary>
        /// <param name="cat">要修改的功能</param>
        /// <returns>修改后的功能信息Json</returns>
        public Base_Catalog Save(Base_Catalog cat)
        {
            cat.EditTime = TimeHelper.ServerTime;
            if (cat.Id == 0)
            {
                Add(cat);
            }
            else
            {
                Change(cat);
            }

            return cat;
        }

        public List<int> Delete(params int[] idArr)
        {
            if (idArr == null) return null;
            List<int> deleted = new List<int>();

            foreach (var id in idArr)
            {
                var func = GetById(id);
                if (func == null) continue;
                var childIds = GetAllValid()
                    .Where(f => f.ParentId == id).Select(f => f.Id);

                //ztree会把半选的父结点也传过来，所以要判断，如果所选父结点的子结点
                //有一个不删除，则父结点不删
                if (childIds.Any(cid => !idArr.Contains(cid)))
                {
                    continue;
                }
                Provider.Delete(func);
                deleted.Add(func.Id);
            }
            return deleted;
        }

        /// <summary>
        /// 移动结点
        /// </summary>
        /// <param name="id">要移动的结点ID</param>
        /// <param name="dir">移动方向</param>
        /// <returns>移动以后的结点信息</returns>
        public Base_Catalog Sort(int id, int dir)
        {
            var cat = GetById(id);
            var cats = GetAll().Where(c => c.ParentId == cat.ParentId)
                .OrderBy(c => c.Ord).ToList();

            int index = cats.IndexOf(cat);

            cats.Remove(cat);
            cats.Insert(index + dir, cat);
            int ord = 1;
            cats.Each(c => { c.Ord = ord++; Change(c); });
            return cat;
        }

        int parentId;
        List<int> idArr;

        /// <summary>
        /// 将选中结点移到另一个结点下
        /// </summary>
        /// <param name="idArr">要移动的结点ID列表</param>
        /// <param name="pId">要移到的新结点ID，0代表移动到根结点</param>
        public void Move(IEnumerable<int> ids, int pId)
        {
            parentId = pId;
            idArr = ids.ToList();
            if (parentId != 0)
            {
                var func = GetById(parentId);
                if (func == null)
                {
                    throw new JException("结点未找到");
                }
            }
            else
            {
                parentId = 0;
            }
            if (idArr.Contains(pId))
            {
                throw new JException("结点不能移动到自身.");
            }
            while (idArr.Count > 0)
            {
                var id = idArr[0];
                CascadeMove(id);
            }
        }

        void CascadeMove(int id, bool move = true)
        {
            var childIds = GetAll()
                        .Where(f => f.ParentId == id).Select(f => f.Id).ToList();
            var func = GetById(id);

            idArr.Remove(id);

            //ztree会把半选的父结点也传过来，所以要判断，如果所选父结点的子结点
            //全选，则是对父结点操作，否则是对子结点操作
            if (move && childIds.All(cid => idArr.Contains(cid)))
            {
                func.ParentId = parentId == 0 ? null : (int?)parentId;
                Save(func);
                childIds.Each(cid => CascadeMove(cid, false));
            }
        }

        /// <summary>
        /// 获取上级由系统初始化时就固定的栏目(静态栏目)
        /// </summary>
        /// <param name="catId"></param>
        /// <returns></returns>
        public Base_Catalog GetStaticParent(int catId)
        {
            Base_Catalog cat;
            cat = GetById(catId);
            while (cat.ParentId != null)
            {
                var parent = GetById(cat.ParentId.Value);
                if (parent == null) break;
                cat = parent;
                if (cat.State == ArticleState.Static)
                {
                    break;
                }
            }
            return cat;
        }

        /// <summary>
        /// 获取某文章所属的静态栏目，它决定了文章的类型,它可能是某个文章栏目的上级栏目
        /// </summary>
        /// <param name="art"></param>
        /// <returns></returns>
        public Base_Catalog GetStaticCatalog(Base_Article art)
        {
            using (var article = SiteManager.Get<ArticleManager>())
            {
                var catIds = article.GetQuery().Where(ca => ca.ArticleId == art.Id)
                    .Select(ca => ca.CatalogId);
                foreach (var catId in catIds)
                {
                    var cat = GetStaticParent(catId);
                    if (cat != null)
                    {
                        return cat;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 根据栏目ID和扩展属属性ID获取栏目扩展属性对象
        /// 它有可能找到该栏目的父栏目的扩展属性
        /// </summary>
        /// <param name="catId">栏目ID</param>
        /// <param name="extId">扩展属性ID</param>
        /// <returns>栏目扩展属性对象，没找到则返回null</returns>
        public Base_CatalogExt GetExtById(int catId, int extId)
        {
            Base_Catalog cat = GetById(catId);
            return GetAllExts(cat.Id).FirstOrDefault(ext => ext.Id == extId);
        }

        /// <summary>
        /// 根据栏目扩展属性的ID获取栏目扩展属性对象
        /// </summary>
        /// <param name="extId">扩展属性ID</param>
        /// <returns>栏目扩展属性对象，没找到则返回null</returns>
        public Base_CatalogExt GetExtById(int extId)
        {
            foreach (var cat in GetAll())
            {
                var ext1 = GetAllExts(cat.Id).FirstOrDefault(ext => ext.Id == extId);
                if (ext1 != null)
                {
                    return ext1;
                }
            }
            return null;
        }

        /// <summary>
        /// 根据栏目扩展属性名称查找扩展属性对象
        /// </summary>
        /// <param name="catId">所在栏目ID</param>
        /// <param name="extName">扩展属性名称</param>
        /// <returns>扩展属性对象</returns>
        public Base_CatalogExt GetExtByName(int catId, string extName)
        {
            return GetAllExts(catId).FirstOrDefault(ext => ext.Name.Equals(extName, StringComparison.OrdinalIgnoreCase));
        }


        /// <summary>
        /// 获取当前区域语言的名称（如zh-CN)
        /// </summary>
        /// <returns></returns>
        public string GetCurrentLanguage()
        {
            return Thread.CurrentThread.CurrentCulture.Name;
        }

        /// <summary>
        /// 获取指定栏目到根栏目的层级数
        /// </summary>
        /// <param name="catId"></param>
        /// <param name="rootId"></param>
        /// <returns></returns>
        public int GetDeepth(int catId, int rootId)
        {
            Base_Catalog cat = GetById(catId);
            int deepth = 0;
            while (cat.ParentId != null && cat.Id != rootId)
            {
                cat = GetById(cat.ParentId.Value);
                deepth++;
            }
            return deepth;
        }

        /// <summary>
        /// 获取指定栏目到根栏目的ID集合
        /// </summary>
        /// <param name="catId"></param>
        /// <param name="rootId"></param>
        /// <returns></returns>
        public List<int> GetIds(int catId, int rootId)
        {
            List<int> listIds = new List<int>();
            Base_Catalog cat = GetById(catId);
            while (cat.ParentId != null && cat.Id != rootId)
            {
                listIds.Add(cat.Id);
                cat = GetById(cat.ParentId.Value);
            }
            return listIds;
        }

        #region 用于初始化栏目的方法

        JTree<object> _catalogTree = new JTree<object>(new object());

        /// <summary>
        /// 初始化一个类的静态成员属性到Base_Catalog表中
        /// 该类的静态成员属性类型都包括了Id (int)和Name (string)两个公共属性。
        /// 根据指定类型中的公共属性来初始化栏目。
        /// 该类型必须有Id和Name两个属性。
        /// 该类型的Name属性将会作为Base_Catalog的Name。Id属性取Base_Catalog对应的记录ID。
        /// 除Id和Name属性以外的其他属性都将在Base_Catalog表的扩展属性表Base_CatalogExt中的定义。
        /// 每个属性的类型必须为String
        /// </summary>
        /// <example>
        ///  //类型的定义举例：
        /// public class SampleCatalog
        /// {
        ///     public int Id {get;set;}
        ///     public string Name {get;set;}
        ///     
        ///    public string ExtProp1 {get;set;}
        ///    
        ///    public string ExtProp2 {get; set;}
        /// }
        /// 
        /// public class SampleStaticCatalogs
        /// {
        ///     public static SampleCatalog Sample {get;set;}
        /// }
        /// 
        /// public static class Main()
        /// {
        ///     InitStaticCatalogs(typeof(SampleStaticCatalogs));
        ///     Console.WriteLine(SampleStaticCatalogs.Sample.ExtProp);
        /// }
        /// </example>
        /// <param name="type">容纳一系列静态属性的类型</param>
        public void InitStaticCatalogs(Type type)
        {
            var varProps = type.GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Where(p => p.PropertyType.GetProperty("Id") != null && p.PropertyType.GetProperty("Name") != null)
                .ToList();

            //根据继承关系进行排序，父类排前面
            varProps.Sort(new Comparison<PropertyInfo>((p1, p2) => p1 == null || p2 == null || p1.PropertyType.IsSubclassOf(p2.PropertyType) ? 1 : -1));

            foreach (PropertyInfo pi in varProps)
            {
                var obj = Init(pi.PropertyType);
                pi.SetValue(null, obj, null);
            }
        }

        object Init(Type catType)
        {
            int? tParentId = null;
            object catDef;
            catDef = Activator.CreateInstance(catType);

            AssignNameValue(catDef);

            var parentTree = _catalogTree.FindTree(n => n.GetType() == catType.BaseType);

            //如果在树中找不到父结点，则它为根结点的一级子结点
            if (parentTree == null)
            {
                var tree = _catalogTree.Add(catDef);

                //判断有无其它先期加入的结点应该是它的子结点（根据类继承关系来判断）
                //判断过于复杂，先不做，这就要求初始化顺序一定要按父子关系来
                //foreach (object node in _catalogTree)
                //{
                //    if (node.GetType().IsAssignableFrom(catType))
                //    {
                //        tree.Add(node);
                //        var id = (int)RefHelper.GetValue(node, "Id");

                //        var nodeCat = SiteManager.Catalog.GetById(id);

                //    }
                //}
            }
            else
            {
                tParentId = RefHelper.GetValue(parentTree.Node, "Id") as int?;
                parentTree.Add(catDef);
            }


            var catalog = SiteManager.Catalog.GetAll().FirstOrDefault(cat => cat.ParentId == tParentId && cat.Name == (string)RefHelper.GetValue(catDef, "Name"));
            if (catalog == null)
            {
                catalog = new Base_Catalog()
                {
                    Name = (string)RefHelper.GetValue(catDef, "Name"),
                    ParentId = tParentId,
                    State = ArticleState.Static + ArticleState.Published,
                    OwnerType = CatalogOwnerType.System,
                    EditTime = DateTime.Now,
                    CreateTime = DateTime.Now
                };

                ChangeExts(catalog, catType);
                SiteManager.Catalog.Add(catalog);
            }
            else if (ChangeExts(catalog, catType))
            {
                SiteManager.Catalog.Change(catalog);
            }

            PropertyInfo pi = catType.GetProperty("Id", BindingFlags.Public | BindingFlags.Instance);
            pi.SetValue(catDef, catalog.Id, null);

            return catDef;
        }

        bool ChangeExts(Base_Catalog catalog, Type catType)
        {
            bool changed = false;
            //反射找到对象所有扩展属性, 这里忽略了对象的父属性，避免在Base_CatalogExt重复定义
            var extProps = catType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Where(p => p.PropertyType == typeof(string)
                && p.Name != "Name" && p.Name != "Id");


            //删除数据表的原有的扩展属性，它们在新的实体属性中不存在
            foreach (var ext in catalog.Exts.ToList())
            {
                if (!extProps.Select(p => p.Name).Contains(ext.Name))
                {
                    ext.IsDeleted = true;
                    changed = true;
                }
            }

            //添加数据表中原来没有的扩展属性
            foreach (var prop in extProps)
            {
                var extAttr = prop.GetCustomAttributes(typeof(CatalogExtAttribute), false).FirstOrDefault() as CatalogExtAttribute
                    ?? new CatalogExtAttribute();
                var ext1 = catalog.Exts.FirstOrDefault(e => e.Name == prop.Name);
                bool canUpdate = ext1 == null || extAttr.ForceUpdate;
                if (ext1 == null)
                {
                    ext1 = new Base_CatalogExt
                    {
                        Name = prop.Name,
                        State = ArticleState.Static + ArticleState.Published,
                    };

                    catalog.Exts.Add(ext1);
                }
                if (canUpdate)
                {
                    //修改ext1中各属性的状态
                    ext1.DataType = extAttr.DataType;
                    ext1.AllowNull = extAttr.AllowNull;
                    ext1.DataSource = extAttr.DataSource;
                    ext1.DataSourceType = extAttr.DataSourceType;
                    ext1.DefaultValue = extAttr.DefaultValue;
                    ext1.MaxLength = extAttr.MaxLength;
                    ext1.Ord = extAttr.Ord;
                    changed = true;
                }
                if (ext1.State != (ArticleState.Static + ArticleState.Published))
                {
                    ext1.State = ArticleState.Static + ArticleState.Published;
                    canUpdate = true;
                }
                if (canUpdate && ext1.Id > 0 && Provider is EFAuditDataService<Base_Catalog>)
                {
                    ((EFAuditDataService<Base_Catalog>)Provider).MarkState(ext1, System.Data.Entity.EntityState.Modified);
                }
            }
            return changed;
        }

        void AssignNameValue(object t)
        {
            var type = t.GetType();
            RefHelper.SetValue(t, "Name", t.GetType().Name);
            foreach (PropertyInfo prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (prop.PropertyType == typeof(string) && prop.Name != "Name")
                {
                    prop.SetValue(t, prop.Name, null);
                }
            }
        }

        #endregion
    }
}
