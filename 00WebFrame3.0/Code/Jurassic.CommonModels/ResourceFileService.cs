using System.Data.Entity;
using Jurassic.AppCenter;
using Jurassic.AppCenter.Logs;
using Jurassic.Com.Tools;
using Jurassic.CommonModels.Articles;
using Jurassic.CommonModels.FileRepository;
using Jurassic.CommonModels.Properties;
using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Jurassic.CommonModels
{
    public class ResourceFileService : IDisposable
    {
        static bool _firstLoad = true;
        public ResourceFileService(ArticleManager article)
        {
            if (_firstLoad)
            {
                var initializer = SiteManager.Kernel.Get<ResourceCatalogInit>();
                _firstLoad = false;
            }
            _article = article;
        }

        public CatalogManager Catalog
        {
            get
            {
                return SiteManager.Catalog;
            }
        }

        ArticleManager _article;

        public ArticleManager Article
        {
            get { return _article; }
        }

        private IAuditDataService<Base_ArticleRelation> ArticleRelation
        {
            get
            {
                return SiteManager.Kernel.Get<IAuditDataService<Base_ArticleRelation>>();
            }
        }

        private IFileRepository mStreamCache;
        public IFileRepository StreamCache
        {
            get
            {
                if (mStreamCache == null)
                {
                    mStreamCache = SiteManager.Kernel.Get<IFileRepository>();

                }
                return mStreamCache;
            }
        }

        /// <summary>
        /// 保存文件信息
        /// </summary>
        /// <param name="info">文件信息</param>
        public ResourceFileInfo SaveFile(ResourceFileInfo info)
        {
            ResourceFileInfo savedInfo = null;

            if (info.EndPos > 0)
            {
                //分块文件流
                savedInfo = InnerSavePartialFile(info);
            }
            else
            {
                //整个文件流
                savedInfo = InnerSaveWholeFile(info);
            }

            return savedInfo;
        }

        private ResourceFileInfo InnerSaveWholeFile(ResourceFileInfo info)
        {
            //保存文件属性信息
            ResourceFileInfo savedInfo = InnerSaveArticleInfo(info);

            //保存文件流
            StreamCache[savedInfo.FileKey] = info.FileStream;

            ChangeArtState(info);
            return savedInfo;
        }
        private ResourceFileInfo InnerSavePartialFile(ResourceFileInfo info)
        {
            ResourceFileInfo savedInfo = new ResourceFileInfo();
            if (info.StartPos == 0)
            {
                //新文件，第一部分流

                //保存文件属性信息
                savedInfo = InnerSaveArticleInfo(info);

                //保存文件流
                StreamCache[savedInfo.FileKey] = info.FileStream;

            }
            else
            {
                //后续流

                savedInfo = GetFileBySizeName(info.FileSize, info.FileName);

                info.Id = savedInfo.Id;

                //保存文件流
                StreamCache.Append(savedInfo.FileKey, info.FileStream, info.StartPos);

            }

            ChangeArtState(info);
            return savedInfo;
        }

        /// <summary>
        /// 当文件上传完整之后修改文件的状态
        /// </summary>
        /// <param name="info"></param>
        private void ChangeArtState(ResourceFileInfo info)
        {
            if (info.Id == 0)
            {
                return;
            }
            if (info.EndPos == info.FileSize - 1 || info.EndPos == 0)
            {
                var a = _article.GetById(info.Id);
                a.Article.State = ArticleState.Published;
                _article.Save(a);
            }
        }

        public int GetUserAvatarCatId()
        {
            return SystemTypes.UserAvatar.Id;
        }

        private int GetCatalogId(string fileType)
        {
            //TODO: 根据文件类型返回对应的系统栏目ID
            return SystemTypes.Root.Id;
        }

        private void SaveExtraExt(Base_CatalogArticle art, ResourceFileInfo info)
        {
            //TODO: 根据文件类型保存额外的扩展属性    
        }


        /// <summary>
        /// 保存文件属性信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private ResourceFileInfo InnerSaveArticleInfo(ResourceFileInfo info)
        {
            //string fileKey = Guid.NewGuid().ToString();
            //文件Key，作为物理文件的文件名称，8位日期 + 年月日时分秒毫秒 by_zjf + 实际的上传文件名称 
            string fileKey = DateTime.Now.ToString("yyyyMMdd") + "-" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-" + info.FileName;

            int catId = GetCatalogId(info.ContentType);
            //保存文件属性信息
            var a = _article.CreateByCatalog(catId);
            a.CreateTime = DateTime.Now;
            a.Article.State = ArticleState.Incomplete;
            a.Article.Title = info.FileName;
            a.Article.EditorId = int.Parse((info.UserId));
            a.Article.Author = info.UserId;
            a.Article.Text = "";
            a.Article.Keywords = info.Keywords;
            a.Article.EditTime = info.CreateTime;
            a.Article.Abstract = info.Abstract;

            //查找扩展属性ID，对应ID赋值
            a.SetExt(SystemTypes.Root.Key, fileKey);
            a.SetExt(SystemTypes.Root.MD5Code, info.MD5Code);
            a.SetExt(SystemTypes.Root.ContentType, info.ContentType);
            a.SetExt(SystemTypes.Root.FileName, info.FileName);
            a.SetExt(SystemTypes.Root.FileSize, info.FileSize);


            SaveExtraExt(a, info);

            //保存文件信息到系统目录
            _article.Save(a);

            info.Id = a.Id;

            var saved = SaveCatalogArticleInfo(info);

            info.Id = saved.Id;
            info.FileKey = fileKey;

            return info;
        }


        /// <summary>
        /// 保存上传记录，其中info.Id不能为空
        /// </summary>
        /// <param name="info"></param>
        public Base_CatalogArticle SaveCatalogArticleInfo(ResourceFileInfo info)
        {
            if (info.Id == 0)
            {
                return null;
            }

            var a = _article.GetById(info.Id);

            //当上传的目录Id为0时表示上传到默认（根目录）
            if (info.CatalogId == 0)
            {
                info.CatalogId = GetUserRootCatalog(a.Article.EditorId).Id;
            }

            //判断是否在相同的文件夹上传重复的文件
            if (_article.GetAllQuery().Where(art => art.ArticleId == a.ArticleId).Select(cat => cat.CatalogId).Contains(info.CatalogId))
            {
                int id = _article.GetAllQuery().FirstOrDefault(art => art.ArticleId == a.ArticleId && art.CatalogId == info.CatalogId).Id;
                _article.Delete(id);
            }

            Base_CatalogArticle ua = new Base_CatalogArticle
            {
                Article = a.Article,
                ArticleId = a.ArticleId,
                CatalogId = info.CatalogId,
                CreateTime = DateTime.Now
            };

            _article.Save(ua);
            return ua;
        }

        /// <summary>
        /// 获取某个用户的所有文件信息（不包括文件流）
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        //public List<ResourceFileInfo> GetAllFileInfo(int userId)
        //{
        //    List<Base_Article> al = Article.GetAllQuery().Where(t => t.EditorId == userId).ToList();

        //    List<ResourceFileInfo> rl = new List<ResourceFileInfo>();
        //    foreach (Base_Article a in al)
        //    {
        //        ResourceFileInfo info = ToResourceFileInfo(a);

        //        rl.Add(info);
        //    }

        //    return rl;
        //}


        /// <summary>
        /// 获取文件信息（包括文件流），不存在返回null
        /// </summary>
        /// <param name="fileId">文件信息ID</param>
        /// <returns></returns>
        public ResourceFileInfo GetFile(int fileId)
        {
            var a = _article.GetById(fileId);
            if (a == null)
            {
                return null;
            }

            ResourceFileInfo info = ToResourceFileInfo(a);

            info.FileStream = StreamCache[info.FileKey];

            return info;
        }

        /// <summary>
        /// 根据一串ID获取文件列表
        /// </summary>
        /// <param name="fileIds">一串文件ID</param>
        /// <param name="includeStream">是否获取文件流数据</param>
        /// <returns></returns>
        public IEnumerable<ResourceFileInfo> GetFiles(IEnumerable<int> fileIds, bool includeStream = false)
        {
            if (fileIds.IsEmpty()) return null;
            var files = _article.GetAllQuery().Where(a => fileIds.Contains(a.Id)).ToList();

            var infos = files.Select(f =>
            {
                var r = ToResourceFileInfo(f);
                if (includeStream)
                {
                    r.FileStream = StreamCache[r.FileKey];
                }
                return r;
            });

            return infos;
        }

        /// <summary>
        /// 获取文件信息（包括文件流），不存在返回null
        /// </summary>
        /// <param name="ca"></param>
        /// <returns></returns>
        public ResourceFileInfo GetFile(Base_CatalogArticle ca)
        {
            if (ca == null)
            {
                return null;
            }

            ResourceFileInfo info = ToResourceFileInfo(ca);

            info.FileStream = StreamCache[info.FileKey];

            return info;
        }

        public ResourceFileInfo SaveArticleInfo(ResourceFileInfo info)
        {
            var fileNameId = Catalog.GetExtId(SystemTypes.Root.Id, SystemTypes.Root.FileName);
            var a = _article.GetQuery().FirstOrDefault(
                t => t.Article.Exts.Any(ext => ext.CatlogExtId == fileNameId
                     && ext.Value.Equals(info.FileName)));

            a.Article.Keywords = info.Keywords;
            a.Article.EditTime = info.CreateTime;
            a.Article.Abstract = info.Abstract;
            _article.Save(a);
            return info;
        }

        /// <summary>
        /// 删除文件信息
        /// </summary>
        /// <param name="fileId">文件信息ID</param>
        /// <returns>被删除的文件信息，如果没有找到文件返回null</returns>
        public ResourceFileInfo DeleteFile(int fileId)
        {
            var a = _article.GetById(fileId);
            if (a == null)
            {
                return null;
            }
            var info = ToResourceFileInfo(a);

            //删除数据库中的相关记录
            int count = _article.Delete(fileId);

            //count的值表示文件被文件夹引用的次数
            if (count == 1)
            {
                //删除实体文件
                StreamCache.Remove(info.FileKey);
            }

            return info;
        }

        /// <summary>
        /// 获取文件信息（不包括文件流），不存在返回null
        /// </summary>
        /// <param name="size">文件大小</param>
        /// <param name="fileMD5">MD5码</param>
        /// <returns></returns>
        public ResourceFileInfo GetFile(long size, string fileMD5)
        {
            var sizeStr = size.ToString();

            var MD5CodeId = Catalog.GetExtId(SystemTypes.Root.Id, SystemTypes.Root.FileName);
            var fileSizeId = Catalog.GetExtId(SystemTypes.Root.Id, SystemTypes.Root.FileName);

            var a = _article.GetAllQuery().FirstOrDefault(
                 t => t.Article.Exts.Any(ext => ext.CatlogExtId == MD5CodeId
                     && ext.Value.Equals(fileMD5))
                     && (t.Article.Exts.Any(ext => ext.CatlogExtId == fileSizeId
                     && ext.Value == sizeStr)));

            if (a == null)
            {
                return null;
            }

            ResourceFileInfo info = ToResourceFileInfo(a);

            return info;
        }

        /// <summary>
        /// 根据名称和大小获取文件信息
        /// </summary>
        /// <param name="size">大小</param>
        /// <param name="fileName">名称</param>
        /// <returns>文件信息</returns>
        public ResourceFileInfo GetFileBySizeName(long size, string fileName, bool getStream = false)
        {
            var sizeStr = size.ToString();

            var fileNameId = Catalog.GetExtId(SystemTypes.Root.Id, SystemTypes.Root.FileName);
            var fileSizeId = Catalog.GetExtId(SystemTypes.Root.Id, SystemTypes.Root.FileSize);
            var a = _article.GetQuery().FirstOrDefault(
              t => t.Article.Exts.Any(ext => ext.CatlogExtId == fileNameId
                  && ext.Value.Equals(fileName))
                  && (t.Article.Exts.Any(ext => ext.CatlogExtId == fileSizeId
                  && ext.Value == sizeStr)));
            if (a == null)
            {
                return null;
            }

            ResourceFileInfo info = ToResourceFileInfo(a);
            if (getStream)
            {
                info.FileStream = StreamCache[info.FileKey];
            }
            return info;
        }

        /// <summary>
        /// 映射
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public ResourceFileInfo ToResourceFileInfo(Base_CatalogArticle a)
        {
            //return ResourceCatalogInit.ToResourceFileInfo(a);

            ResourceFileInfo info = new ResourceFileInfo();
            info.Id = a.Id;
            info.Keywords = a.Article.Keywords;
            info.CreateTime = a.Article.CreateTime;
            info.Abstract = a.Article.Abstract;
            info.UserId = a.Article.EditorId.ToString();
            info.CatalogId = a.CatalogId;
            info.ArticleId = a.ArticleId;

            //获取扩展属性
            var fileKeyId = Catalog.GetExtId(SystemTypes.Root.Id, SystemTypes.Root.Key);
            var MD5CodeId = Catalog.GetExtId(SystemTypes.Root.Id, SystemTypes.Root.MD5Code);
            var contentTypeId = Catalog.GetExtId(SystemTypes.Root.Id, SystemTypes.Root.ContentType);
            var fileNameId = Catalog.GetExtId(SystemTypes.Root.Id, SystemTypes.Root.FileName);
            var fileSizeId = Catalog.GetExtId(SystemTypes.Root.Id, SystemTypes.Root.FileSize);

            info.FileKey = a.Article.Exts.FirstOrDefault(t => t.CatlogExtId == fileKeyId).Value;
            info.MD5Code = a.Article.Exts.FirstOrDefault(t => t.CatlogExtId == MD5CodeId).Value;
            info.ContentType = a.Article.Exts.FirstOrDefault(t => t.CatlogExtId == contentTypeId).Value;
            info.FileName = a.Article.Exts.FirstOrDefault(t => t.CatlogExtId == fileNameId).Value;
            info.FileSize = long.Parse(a.Article.Exts.FirstOrDefault(t => t.CatlogExtId == fileSizeId).Value);

            return info;
        }

        /// <summary>
        /// 获取文件流，不存在返回null，存在返回文件流Stream
        /// </summary>
        /// <param name="fileId">文件信息ID</param>
        /// <returns></returns>
        public Stream GetFileStream(int fileId)
        {
            var a = _article.GetById(fileId);
            if (a == null)
            {
                return null;
            }

            ResourceFileInfo info = ToResourceFileInfo(a);

            return StreamCache[info.FileKey];
        }

        /// <summary>
        /// 获得文件缩略图流
        /// </summary>
        /// <param name="fileId">文件信息ID</param>
        /// <returns>缩略图流</returns>
        public Stream GetFileThumbnail(int fileId)
        {
            Size thumbnailSize = ThumbnailSize.Large;
            return GetFileThumbnail(fileId, thumbnailSize);
        }
        /// <summary>
        /// 获得文件缩略图流
        /// </summary>
        /// <param name="fileId">文件信息ID</param>
        /// <param name="thumbnailSize">缩略图大小</param>
        /// <returns>缩略图流</returns>
        public Stream GetFileThumbnail(int fileId, Size thumbnailSize)
        {
            Stream targetStream = null;

            Stream sourceStream = GetFileStream(fileId);
            if (sourceStream == null)
            {
                return null;
            }

            targetStream = FileExtensionTypeHelper.GetThumbnail(sourceStream, thumbnailSize);

            //所以要手动释放
            sourceStream.Flush();
            sourceStream.Close();

            targetStream.Position = 0;
            return targetStream;
        }


        /// <summary>
        /// 获得头像缩略图流
        /// </summary>
        /// <param name="fileId">文件信息ID（Base_Article中的ID）</param>
        /// <param name="thumbnailSize">缩略图大小</param>
        /// <returns>缩略图流</returns>
        public Stream GetSysAvatarThumbnail(int fileId)
        {
            Stream targetStream = null;

            var myArticle = SiteManager.Kernel.Get<EFAuditDataService<Base_Article>>();
            var art = myArticle.GetQuery().Include(a => a.Exts).FirstOrDefault(a => a.Id == fileId);
            var filekeyId = Catalog.GetExtByName(Catalog.GetRootId(), SystemTypes.Root.Key).Id;

            var filekey = art.Exts.FirstOrDefault(e => e.CatlogExtId == filekeyId).Value;

            Stream sourceStream = StreamCache[filekey];
            if (sourceStream == null)
            {
                return null;
            }

            targetStream = FileExtensionTypeHelper.GetThumbnail(sourceStream, ThumbnailSize.Large);

            //所以要手动释放
            //sourceStream.Flush();
            //sourceStream.Close();
            //targetStream.Position = 0;

            sourceStream.Position = 0;
            return sourceStream;
        }
        /// <summary>
        /// 获取文件的目录
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <returns></returns>
        public ResourceCatalogInfo GetCatalogOfFile(int fileId)
        {
            var a = _article.GetById(fileId);
            if (a == null)
            {
                return null;
            }

            ResourceFileInfo info = ToResourceFileInfo(a);

            int catalogId = info.CatalogId;
            Base_Catalog c = Catalog.GetById(catalogId);

            var rc = ToResourceCatalogInfo(c);

            return rc;
        }


        /// <summary>
        /// 获取目录的所有文件（包括不完整的（Incomplete））
        /// </summary>
        /// <param name="catalogId">目录ID</param>
        /// <returns></returns>
        private List<Base_CatalogArticle> GetFilesInCatalog(int catalogId)
        {
            //return PageQuery(catalogId, 1, 20).ToList();//分页查询不可获取所有文件
            List<Base_CatalogArticle> articles = _article.GetQuery().Where(
             t => t.CatalogId == catalogId).ToList();

            return articles;
        }

        /// <summary>
        /// 映射
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private ResourceCatalogInfo ToResourceCatalogInfo(Base_Catalog c)
        {
            ResourceCatalogInfo d = new ResourceCatalogInfo();
            d.Id = c.Id;
            d.ParentId = (c.ParentId == null) ? 0 : c.ParentId.Value;
            d.Name = c.Name;
            d.OwnerId = c.OwnerId;
            d.OwnerType = d.OwnerType;

            return d;
        }

        /// <summary>
        /// 确定文件目录的根目录ID
        /// 新增：[2017-01-16/汪敏]
        /// </summary>
        /// <returns></returns>
        public int GetFileCatalogRootId()
        {
            return SystemTypes.UserFiles.Id;
        }

        /// <summary>
        /// 取得根目录
        /// 修改：[2017-01-16/汪敏]
        /// </summary>
        /// <returns></returns>
        public Base_Catalog GetUserRootCatalog(int userId)
        {
            //Base_Catalog uc = Catalog.GetAll().FirstOrDefault(c => c.OwnerType == CatalogOwnerType.User
            //    && c.OwnerId == userId);
            Base_Catalog uc = Catalog.GetAll().FirstOrDefault(c => c.OwnerType == CatalogOwnerType.User
                && c.OwnerId == userId && c.ParentId == SystemTypes.UserFiles.Id);
            if (uc == null)
            {
                uc = new Base_Catalog()
                {
                    OwnerId = userId,
                    OwnerType = CatalogOwnerType.User,
                    Name = AppManager.Instance.UserManager.GetById(userId.ToString()).Name,
                    State = ArticleState.Published,
                    ParentId = SystemTypes.UserFiles.Id
                };
                Catalog.Save(uc);
            }
            else
            {
                if (uc.State != ArticleState.Published)
                {
                    uc.State = ArticleState.Published;
                    Catalog.Save(uc);
                }
            }

            return uc;
        }




        /// <summary>
        /// 取得目录的直接下级目录
        /// </summary>
        /// <param name="catalogId">目录Id</param>
        /// <returns></returns>
        public List<ResourceCatalogInfo> GetChildrenCatalogs(int catalogId)
        {
            List<Base_Catalog> cs = Catalog.GetChildren(catalogId);

            var rcs = cs.ConvertAll<ResourceCatalogInfo>(t => ToResourceCatalogInfo(t));

            return rcs;
        }


        /// <summary>
        /// 取得目录的所有下级目录
        /// </summary>
        /// <param name="rootCatalogName"></param>
        /// <returns></returns>
        public List<ResourceCatalogInfo> GetDescendantCatalogs(int catalogId)
        {
            List<ResourceCatalogInfo> infos = InnerGetCatalogTree(catalogId);

            return infos;
        }
        private List<ResourceCatalogInfo> InnerGetCatalogTree(int catalogId)
        {
            List<ResourceCatalogInfo> allInfos = new List<ResourceCatalogInfo>();
            List<ResourceCatalogInfo> infos = GetChildrenCatalogs(catalogId);
            allInfos.AddRange(infos);
            for (int i = 0; i < infos.Count; i++)
            {
                allInfos.AddRange(InnerGetCatalogTree(infos[i].Id));
            }
            return allInfos;
        }


        /// <summary>
        /// 保存一个目录对象
        /// </summary>
        /// <param name="ResourceCatalogInfo">目录</param>
        /// <returns></returns>
        public ResourceCatalogInfo SaveCatalog(ResourceCatalogInfo info)
        {

            Base_Catalog c = new Base_Catalog
            {
                Name = info.Name,
                Ord = 1,
                State = ArticleState.Static,
                EditTime = TimeHelper.ServerTime,
                OwnerId = info.OwnerId,
                OwnerType = info.OwnerType
            };
            if (info.ParentId > 0)
            {
                c.ParentId = info.ParentId;
            }

            Base_Catalog c2 = Catalog.Save(c);

            var returnInfo = ToResourceCatalogInfo(c2);

            return returnInfo;
        }


        /// <summary>
        /// 更新一个目录对象
        /// </summary>
        /// <param name="ResourceCatalogInfo">目录</param>
        /// <returns></returns>
        public ResourceCatalogInfo UpdateCatalog(ResourceCatalogInfo info)
        {
            Base_Catalog c = Catalog.GetById(info.Id);

            if (c == null)
            {
                return null;
            }

            c.ParentId = info.ParentId;
            c.Name = info.Name;

            Base_Catalog c2 = Catalog.Save(c);
            var returnInfo = ToResourceCatalogInfo(c2);

            return returnInfo;
        }


        /// <summary>
        /// 删除一个目录（带删除目录中的子目录以及文件）
        /// </summary>
        /// <param name="catalogId">目录Id</param>
        /// <returns></returns>
        public bool DeleteCatalog(int catalogId)
        {
            //获得所有的子目录
            List<ResourceCatalogInfo> catList = new List<ResourceCatalogInfo>();
            catList.Add(new ResourceCatalogInfo
            {
                Id = catalogId,
            });
            catList.AddRange(GetDescendantCatalogs(catalogId));

            //先从最低级的目录开始删除数据
            for (int i = catList.Count() - 1; i >= 0; i--)
            {
                //如果目录中有文件，必须先删除文件再删除目录
                var files = GetFilesInCatalog(catList[i].Id);
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        DeleteFile(file.Id);
                    }
                }

                Base_Catalog c = Catalog.GetById(catList[i].Id);
                bool b = Catalog.Remove(c);
            }
            return true;
        }


        /// <summary>
        /// 将文件移动到指定的目录中
        /// </summary>
        /// <param name="fileId">将要移动的文件id</param>
        /// <param name="newCatalogId">将要移动到的CatalogId</param>
        /// <param name="isCopy">表示是否是复制文件</param>
        public ResourceFileInfo UpdateCatalogOfFile(int fileId, int newCatalogId, bool isCopy = false)
        {
            //获取文件信息
            var a = _article.GetById(fileId);
            if (a == null)
            {
                return null;
            }

            Base_CatalogArticle newArt = new Base_CatalogArticle
            {
                CatalogId = newCatalogId,
                ArticleId = a.ArticleId,
                Article = a.Article,
                CreateTime = TimeHelper.ServerTime,
                Ord = a.Ord
            };
            //在数据库CatalogArticle中增加一条数据
            var saved = _article.Save(newArt);

            if (!isCopy)
            {
                //移动文件的时候，将数据库中的原来的所在目录记录删除
                DeleteFile(fileId);
            }

            return ToResourceFileInfo(newArt);
        }

        /// <summary>
        /// 通过用户的Id获取其对应可见的目录列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<ResourceCatalogInfo> GetUserVisibleCatalogs(int userId)
        {
            return Catalog.GetUserVisibleCatalogs(userId).Select(c => ToResourceCatalogInfo(c));
        }

        /// <summary>
        /// 排序、分页
        /// </summary>
        /// <param name="catalogId">目录Id</param>
        /// <param name="page">页码数</param>
        /// <param name="pageSize">每页的大小</param>
        /// <param name="key">搜索的关键字</param>
        /// <param name="sortField">将要排序的字段名</param>
        /// <param name="sortOrder">排序的方式（desc、asc）</param>
        /// <returns></returns>
        public Pager<ResourceFileInfo> PageQuery(int catalogId, int page, int pageSize, string key = "", string sortField = null, string sortOrder = null)
        {
            string sortExpression = sortField + " " + sortOrder;
            if (String.IsNullOrWhiteSpace(sortExpression))
            {
                sortExpression = "FileName asc";
            }
            var catIds = SiteManager.Catalog.GetDescendant(catalogId).Select(ca => ca.Id).ToList();
            catIds.Add(catalogId);

            return InnerPageQuery(a => catIds.Contains(a.CatalogId), page, pageSize, key, sortExpression);
        }

        private Pager<ResourceFileInfo> InnerPageQuery(Expression<Func<Base_CatalogArticle, bool>> whereExpression, int page, int pageSize, string key = "", string sortExpression = null)
        {
            IQueryable<Base_CatalogArticle> list = null;
            if (key.IsEmpty())
            {
                list = _article.GetAllQuery()
                     .Where(art => art.Article.IsDeleted == false);
            }
            else
            {
                list = _article.GetAllQuery()
                    .Where(art => (art.Article.IsDeleted == false
                                       && art.Article.Exts.Any(ext => ext.Value.Contains(key))));
            }

            var fileNameID = Catalog.GetExtId(SystemTypes.Root.Id, SystemTypes.Root.FileName);
            var fileSizeID = Catalog.GetExtId(SystemTypes.Root.Id, SystemTypes.Root.FileSize);
            var ContentTypeID = Catalog.GetExtId(SystemTypes.Root.Id, SystemTypes.Root.ContentType);
            var arts = new Pager<ResourceFileInfo>(list.Where(whereExpression)
                       .Select(a => new ResourceFileInfo
                       {
                           Id = a.Id,
                           CatalogId = a.CatalogId,
                           Keywords = a.Article.Keywords,
                           Abstract = a.Article.Abstract,


                           FileName =
                                 a.Article.Exts.FirstOrDefault(
                                   ext => ext.CatlogExtId == fileNameID).Value,
                           FileSizeStr =
                                 a.Article.Exts.FirstOrDefault(
                                   ext => ext.CatlogExtId == fileSizeID).Value,
                           ContentType =
                                 a.Article.Exts.FirstOrDefault(
                                   ext => ext.CatlogExtId == ContentTypeID).Value,
                       })
                       .OrderBy(sortExpression), page, pageSize);
            return arts;
        }

        /// <summary>
        /// 筛选一个目录下的文件与选中的文件列表中的重复文件
        /// </summary>
        /// <param name="files">选中的文件列表</param>
        /// <param name="newCatalogId">将要与之比较的目录Id</param>
        /// <returns></returns>
        public List<ResourceFileInfo> CheckRepeatFiles(List<ResourceFileInfo> files, int newCatalogId)
        {
            //重复文件集合
            List<ResourceFileInfo> repList = new List<ResourceFileInfo>();
            //获得将移动至的文件夹下的所有文件
            var list = _article.GetAllQuery().Where(arts => arts.CatalogId == newCatalogId).Select(a => a.ArticleId);

            //比较此文件夹下的文件是否与待移动的文件有重复，将重复文件记录下来
            foreach (var file in files)
            {
                var art = _article.GetById(file.Id);
                if (list.Contains(art.ArticleId))
                {
                    repList.Add(file);
                }
            }
            return repList;
        }

        /// <summary>
        /// 批量移动、复制文件
        /// </summary>
        /// <param name="files">选中的文件</param>
        /// <param name="repList">重复的文件</param>
        /// <param name="newCatalogId">将要移至的目录Id</param>
        /// <param name="isCover">是否覆盖重复的文件</param>
        /// <param name="isCopy">是否是复制</param>
        public void UpdateCatalogOfFiles(List<ResourceFileInfo> files, List<ResourceFileInfo> repList, int newCatalogId, bool isCover = false, bool isCopy = false)
        {
            if (repList != null)
            {
                //重复文件记录直接重新保存，对其不再进行移动处理
                foreach (var rep in repList)
                {
                    files.Remove(rep);
                }

                if (isCover)
                {
                    //保存新的记录
                    foreach (var rep in repList)
                    {
                        rep.CatalogId = newCatalogId;
                        SaveCatalogArticleInfo(rep);
                        if (!isCopy)
                        {
                            //移动文件的时候，将数据库中的原来的所在目录记录删除
                            DeleteFile(rep.Id);
                        }
                    }
                }
            }

            foreach (var file in files)
            {
                UpdateCatalogOfFile(file.Id, newCatalogId, isCopy);
            }
        }


        /// <summary>
        /// 获得所有的系统默认头像
        /// </summary>
        /// <returns></returns>
        public List<ResourceFileInfo> GetAllSysAvatar()
        {
            var infoList = new List<ResourceFileInfo>();
            var myArticle = SiteManager.Kernel.Get<EFAuditDataService<Base_Article>>();
            var artList = myArticle.GetQuery().Where(a => a.State == (ArticleState.New | ArticleState.ReadOnly)).Include(a => a.Exts).ToList();
            var filekeyId = Catalog.GetExtByName(Catalog.GetRootId(), SystemTypes.Root.Key).Id;
            foreach (var a in artList)
            {
                var filekey = a.Exts.FirstOrDefault(e => e.CatlogExtId == filekeyId).Value;
                infoList.Add(new ResourceFileInfo
                {
                    Id = a.Id,
                    FileKey = filekey
                });
            }
            return infoList;
        }


        /// <summary>
        /// 获得用户的当前头像
        /// </summary>
        /// <param name="userId">用户的Id</param>
        /// <returns></returns>
        public ResourceFileInfo GetUserAvatar(int userId)
        {
            ResourceFileInfo info;
            //获取用户头像目录ID
            var avatarCatId = Catalog.GetAllValid().First(cat => cat.Name == "UserAvatar" && cat.ParentId == Catalog.GetRootId()).Id;
            string uid = userId.ToString();
            var catArt = _article.GetQuery().Where(c => c.CatalogId == avatarCatId).OrderByDescending(c => c.Id).FirstOrDefault(c => c.Article.Author == uid);

            //如果用户没有选择过头像,就给一个默认的头像
            if (catArt == null)
            {
                info = GetAllSysAvatar().First();
                SaveAvatar(new ResourceFileInfo
                {
                    Id = info.Id,
                    UserId = userId.ToString(),
                });
            }
            else
            {
                info = new ResourceFileInfo
                       {
                           //FileStream = GetFileThumbnail(catArt.Id, ThumbnailSize.Small),
                           Id = catArt.Article.Id,
                           ContentType = Convert.ToInt32(catArt.Article.State).ToString()
                       };
            }
            return info;
        }


        /// <summary>
        /// 当选择系统默认头像时，保存头像
        /// </summary>
        /// <param name="info"></param>
        public void SaveAvatar(ResourceFileInfo info)
        {
            var myArticle = SiteManager.Kernel.Get<EFAuditDataService<Base_Article>>();
            Base_Article art = new Base_Article();
            art = myArticle.GetQuery().Include(a => a.Exts).FirstOrDefault(a => a.Id == info.Id);

            Base_Article newart = new Base_Article
            {
                EditorId = Convert.ToInt32(info.UserId),
                Author = info.UserId,
                Title = info.UserId + "的头像",
                State = ArticleState.ReadOnly
            };

            var avatarCatId = Catalog.GetAllValid().First(cat => cat.Name == "UserAvatar" && cat.ParentId == Catalog.GetRootId()).Id;
            Base_CatalogArticle catArt = new Base_CatalogArticle
            {
                Article = newart,
                CatalogId = avatarCatId,
                ArticleId = newart.Id,
            };

            var rootId = Catalog.GetRootId();
            catArt.SetExt(SiteManager.Catalog.GetExtByName(rootId, SystemTypes.Root.Key), art.Exts.FirstOrDefault(e => e.CatlogExtId == SiteManager.Catalog.GetExtByName(rootId, SystemTypes.Root.Key).Id).Value);
            catArt.SetExt(SiteManager.Catalog.GetExtByName(rootId, SystemTypes.Root.FileName), art.Exts.FirstOrDefault(e => e.CatlogExtId == SiteManager.Catalog.GetExtByName(rootId, SystemTypes.Root.FileName).Id).Value);
            catArt.SetExt(SiteManager.Catalog.GetExtByName(rootId, SystemTypes.Root.FileSize), art.Exts.FirstOrDefault(e => e.CatlogExtId == SiteManager.Catalog.GetExtByName(rootId, SystemTypes.Root.FileSize).Id).Value);

            _article.Save(catArt);
            _article.Dispose();
        }

        /// <summary>
        /// 根据Base_Article中的ID返回文件信息 wang
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResourceFileInfo GetFileByArticleId(int id)
        {
            var catalogIds = SiteManager.Catalog.GetDescendant(SystemTypes.Root.Id).Select(c => c.Id);
            var ca = _article.GetByArticleId(id, catalogIds.ToArray());
            return GetFile(ca);
        }

        /// <summary>
        /// 根据文件的ID返回文件的物理路径
        /// </summary>
        /// <param name="fileIds"></param>
        /// <returns></returns>
        public string[] GetFilePath(params int[] fileIds)
        {
            IFileLocator locator = null;
            if (fileIds.IsEmpty())
            {
                return new string[0];
            }

            var files = _article.GetAllInCatalog(SystemTypes.Root.Id)
                .Where(f => fileIds.Contains(f.Id))
                .ToList()
                .Select(f => locator.GetFilePath(f.GetExtStr(SystemTypes.Root.Key)))
                .ToArray();

            return files;
        }

        public void Dispose()
        {
            if (_article != null)
            {
                _article.Dispose();
            }
        }
    }
}

