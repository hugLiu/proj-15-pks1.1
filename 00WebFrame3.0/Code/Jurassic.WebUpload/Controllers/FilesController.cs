using Jurassic.AppCenter;
using Jurassic.AppCenter.Resources;
using Jurassic.Com.Tools;
using Jurassic.CommonModels;
using Jurassic.CommonModels.Articles;
using Jurassic.CommonModels.FileRepository;
using Jurassic.WebFrame;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Jurassic.WebUpload.Controllers
{
    public class FilesController : BaseController
    {
        private ResourceFileService _resourceFileService;

        public FilesController(ResourceFileService resourceFileService)
        {
            _resourceFileService = resourceFileService;
        }

        #region FileUpload控件相关方法
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            //var filename = id;
            //var filePath = Path.Combine(StorageRoot, filename);

            //if (System.IO.File.Exists(filePath))
            //{
            //    System.IO.File.Delete(filePath);
            //}
            var model = _resourceFileService.DeleteFile(id);
            //删除后要刷新List，使用又请求了数据，文件多的情况会有影响，可以考虑是否可以从界面传过来列表或者缓存下来。
            //return Json(new
            //{
            //    files = GetAllFiles(),
            //});

            return Json(model);
        }

        /// <summary>
        /// 获得缩略图
        /// </summary>
        /// <param name="id">文件信息id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetThumbnail(int id)
        {
            var thumbnail = _resourceFileService.GetFileThumbnail(id);
            var file = thumbnail == null ? null : File(thumbnail, "image/png");//ContentType不应该固定
            return file;
        }

        /// <summary>
        /// 获得原图
        /// </summary>
        /// <param name="id">文件信息id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetImg(int id)
        {
            var fileInfo = _resourceFileService.GetFile(id);
            var file = fileInfo == null ? null : File(fileInfo.FileStream, "image/png");//ContentType不应该固定
            return file;
        }

        /// <summary>
        /// 下载文件，根据Base_CatalogArticle中的ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Download(int id)
        {
            var model = _resourceFileService.GetFile(id);
            Response.HeaderEncoding = Encoding.UTF8;
            string fileName = model.FileName;
            if (model != null && model.FileStream != null)
            {
                Response.Cache.SetOmitVaryStar(true);
                //Response.AddHeader("Cache-Control", "public, s-maxage=600");
                //Response.ContentType = contentType;
                //Response.AddHeader("Content-Length", art.GetExt("Length").ToString());
               // Response.SetFriendFileName(fileName);
                
                return File(model.FileStream, model.ContentType, fileName);
            }
            else
            {
                return new HttpNotFoundResult();
            }
        }

        /// <summary>
        /// 下载文件，根据Base_Article中的ID
        /// </summary>
        /// <param name="id">Base_Article中的ID</param>
        /// <returns>文件流</returns>
        public ActionResult DownloadA(int id)
        {
            var model = _resourceFileService.GetFileByArticleId(id);
            Response.HeaderEncoding = Encoding.UTF8;
            string fileName = model.FileName;
            if (model != null && model.FileStream != null)
            {
                Response.Cache.SetOmitVaryStar(true);
                //Response.AddHeader("Cache-Control", "public, s-maxage=600");
                //Response.ContentType = contentType;
                //Response.AddHeader("Content-Length", art.GetExt("Length").ToString());
                return File(model.FileStream, model.ContentType, fileName);
            }
            else
            {
                return new HttpNotFoundResult();
            }
        }

        private string GetContentTypeByExt(string ext)
        {
            switch ((ext ?? "").ToLower())
            {
                case ".txt":
                    return "text/plain";
                case ".html":
                case ".htm":
                    return "text/html";
                case ".jpg":
                    return "image/jpeg";
                case ".gif":
                    return "image/gif";
                case ".png":
                    return "image/png";
                default:
                    return "application/octet-stream";
            }
        }

        //DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS
        [HttpPost]
        public ActionResult Upload()
        {
            var r = new List<UploadResult>();
            foreach (string file in Request.Files)
            {
                //续传
                r.Add(UploadFile());
            }
            return Json(new
            {
                files = r,

            }, "text/html"); //如果不声明"text/html", 在IE8底下会弹出下载框
        }

        [HttpGet]
        public ActionResult Info(long size, string name)
        {
            var fileInfo = _resourceFileService.GetFileBySizeName(size, name, true);
            if (fileInfo == null || fileInfo.FileStream == null)
            {
                return Json(new
                {
                    file = new UploadResult
                    {
                        Id = 0,
                        size = 0
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                try
                {
                    return Json(new
                    {
                        file = new UploadResult
                        {
                            Id = fileInfo.Id,
                            size = fileInfo.FileStream.Length
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                finally
                {
                    fileInfo.FileStream.Close();
                }
            }
        }

        private UploadResult UploadFile()
        {
            string contentRange = Request.Headers["Content-Range"];
            if (Request.Files.Count != 1 && !contentRange.IsEmpty())
                throw new HttpRequestValidationException("Attempt to upload chunked file containing more than one fragment per request");
            var file = Request.Files[0];
            var inputStream = file.InputStream;
            //ie8会上传完整的带路径文件名
            FileInfo fi = new FileInfo(file.FileName);

            string md5 = Request.Form["md5"];
            int CatalogId = int.Parse(Request.Form["CatalogId"]);

            //在重复文件已经存在不必上传的情况下，会象征性上传一个字节数据，
            //并传重复的文件的ID
            int id = CommOp.ToInt(Request.Form["Id"]);
            int dup = CommOp.ToInt(Request.Form["dup"]);
            ResourceFileInfo retMess = null;
            if (dup == 1 && (id > 0 && inputStream.Length == 1))
            {
                var model = new ResourceFileInfo
                {
                    Id = id,
                    UserId = User.Identity.GetUserId(),
                    MD5Code = md5,
                    FileStream = inputStream,
                    FileName = fi.Name,
                    CatalogId = CatalogId,
                    ContentType = file.ContentType,
                };
                var saved = _resourceFileService.SaveCatalogArticleInfo(model);
                retMess = _resourceFileService.GetFile(saved.Id);
            }
            else
            {
                //bytes 1048576-2097151/5455977
                ChunkInfo chunk = GetBytesChunkInfo(contentRange);

                var model = new ResourceFileInfo
                {
                    Id = id,
                    UserId = User.Identity.GetUserId(),
                    MD5Code = md5,
                    FileStream = inputStream,
                    FileName = fi.Name,
                    CatalogId = CatalogId,
                    FileSize = chunk.Size == 0 ? file.ContentLength : chunk.Size,
                    ContentType = file.ContentType,
                    EndPos = chunk.End,
                    StartPos = chunk.Start
                };

                retMess = _resourceFileService.SaveFile(model);
            }

            return new UploadResult()
            {
                name = retMess.FileName,
                size = retMess.FileSize,
                type = retMess.ContentType,
                Id = retMess.Id,
                UserId = retMess.UserId,
                FileKey = retMess.FileKey,
                CatalogId = retMess.CatalogId,
                url = Url.Action("Download", "Files", new { id = retMess.Id }),
                deleteUrl = Url.Action("Delete", "Files", new { id = retMess.Id }),
                thumbnailUrl = "",//Url.Action("Thumbnail", "Files", new { id = file.FileName }),
                deleteType = "DELETE",
            };
        }

        ChunkInfo GetBytesChunkInfo(string contentRange)
        {
            if (contentRange.IsEmpty())
            {
                return new ChunkInfo();
            }
            Regex reg = new Regex(@"bytes (\d+)-(\d+)/(\d+)");
            var match = reg.Match(contentRange);

            return new ChunkInfo
            {
                Size = CommOp.ToLong(match.Groups[3].Value),
                Start = CommOp.ToLong(match.Groups[1].Value),
                End = CommOp.ToLong(match.Groups[2].Value),
            };
        }

        [HttpGet]
        public FileResult Thumbnail(string id)
        {
            var fullPath = "";// Path.Combine(StorageRoot, id);
            if (GetContentTypeByExt(new FileInfo(fullPath).Extension).Contains("image"))
            {
                //Image.FromFile会独占文件不释放
                Image originImg = Image.FromFile(fullPath);
                Stream ms = DrawingHelper.GetThumbnailStream(originImg, 80, 60);
                //所以要手动释放
                originImg.Dispose();
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, GetContentTypeByExt(new FileInfo(fullPath).Extension));
            }
            return null;
        }

        public object Done(string actionName, string controllerName, string ids)
        {
            var controller = ControllerBuilder.Current.GetControllerFactory()
            .CreateController(this.Request.RequestContext, controllerName) as BaseController;

            controller.ControllerContext = this.ControllerContext;

            int[] idArr = CommOp.ToIntArray(ids, ',');
            ResourceFileInfo[] results = new ResourceFileInfo[idArr.Length];
            int i = 0;
            foreach (int id in idArr)
            {
                //调用业务逻辑得到stream数组
                var file = _resourceFileService.GetFile(id);
                results[i++] = file;
            }

            var obj = RefHelper.CallMethod(controller, actionName, new object[] { results });
            results.Each(r => r.FileStream.Dispose());
            return obj;
        }

        public ActionResult GetArticleId(ResourceFileInfo[] results)
        {
            return Json(results.Select(r => r.Id));
        }

        #endregion

        #region 管理界面方法
        public ActionResult Index()
        {
            //获得用户目录根节点
            var rootCatalog = _resourceFileService.GetUserRootCatalog(int.Parse(User.Identity.GetUserId()));//获得根目录Id
            ViewBag.id = rootCatalog.Id;
            return View();
        }

        public JsonResult GetCatalogs()//得到整个目录树
        {
            var cats = _resourceFileService.GetUserVisibleCatalogs(CommOp.ToInt(CurrentUserId));//得到用户可见目录
            return Json(cats, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取指定栏目下文件列表且分页显示
        /// </summary>
        /// <param name="catId"></param>
        /// <returns></returns>
        public ActionResult GetFiles(int catId, int pageIndex, int pageSize, string key)
        {
            pageIndex++;//MiniUI的分页数字是从0开始，实际的分页是从1开始的，这里+1同步。
            //var data1 = _resourceFileService.PageQuery(catId, pageIndex, pageSize);
            var data1 = _resourceFileService.PageQuery(catId, pageIndex, pageSize, key, "Id", "desc");
            return JsonNT(new
            {
                data = data1,
                total = data1.RecordCount
            });
        }
        #endregion

        #region 菜单栏
        ///<summary>
        ///增加目录 
        ///修改：[2017-01-17/汪敏]
        ///</summary>
        ///<param name="pId">新增的目录的父Id</param>
        ///<param name="catalogName">新增目录的名称</param>
        public JsonResult CreateCatalog(string catalogName, int pId)
        {
            var catalog = new ResourceCatalogInfo();
            if (pId == 0)
            {
                pId = _resourceFileService.GetFileCatalogRootId();
            }
            catalog.ParentId = pId;
            catalog.Name = catalogName;
            catalog.OwnerId = int.Parse(User.Identity.GetUserId());//UserId
            catalog.OwnerType = CommonModels.Articles.CatalogOwnerType.User;//本应该从前台传过来
            return Json(_resourceFileService.SaveCatalog(catalog), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除目录 
        /// 修改：[2017-01-16/汪敏]
        /// </summary>
        /// <param name="catalogId">目录ID</param>
        /// <returns></returns>
        public JsonResult DeleteCatalog(int catalogId)
        {
            //判断是否用户的根目录
            Base_Catalog uc = _resourceFileService.Catalog.GetById(catalogId);
            //if (uc.ParentId == _resourceFileService.GetFileCatalogRootId())
            //{
            //    return JsonTipsLang("error", "RootDirectory_Cannot_Delete");  //用户的根目录不能删除
            //}
            if(uc == null)
            {
                return JsonTipsLang("error", "Directory_IsNull");  //选择的目录不存在
            }

            var catalogsInfo = _resourceFileService.GetChildrenCatalogs(catalogId);

            if (_resourceFileService.DeleteCatalog(catalogId))
            {
                return JsonTips("success", JStr.SuccessDeleted);
            }
            else
            {
                return JsonTips("error", JStr.DeleteFailed);
            }

        }

        /// <summary>
        /// 编辑目录
        /// </summary>
        /// <param name="catalogId"></param>
        /// <param name="catalogName"></param>
        public void RenameCatalog(string model)
        {
            var catalog = JsonConvert.DeserializeObject<ResourceCatalogInfo>(model);//反序列化
            _resourceFileService.UpdateCatalog(catalog);
        }

        /// <summary>
        /// 修改文件所在目录，根据CatalogArticle中的Id，文件原来的目录Id,以及新的目录Id来操作
        /// </summary>
        /// <param name="model">一个文件信息对象</param>
        /// <param name="catalogName">文件原来的目录Id</param>
        public JsonResult MoveToCatalog(string model, int catalogId, bool judge = true)
        {
            var FileInfoModel = JsonConvert.DeserializeObject<List<ResourceFileInfo>>(model);//反序列化
            var checklist = _resourceFileService.CheckRepeatFiles(FileInfoModel, catalogId);
            _resourceFileService.UpdateCatalogOfFiles(FileInfoModel, checklist, catalogId, true, false);
            return JsonTips("success", JStr.OperationSucceed);
        }


        //删除文件
        public JsonResult DeleteFiles(string ids)
        {
            int[] idArr = CommOp.ToIntArray(ids, ',');
            for (var i = 0; i < idArr.Length; i++)
            {
                _resourceFileService.DeleteFile(idArr[i]);
            }
            return JsonTips("success", "删除文件成功！");
        }
        public bool CheckRepeatFile(string model, int catalogId)
        {
            var FileInfoModel = JsonConvert.DeserializeObject<List<ResourceFileInfo>>(model);//反序列化
            var checklist = _resourceFileService.CheckRepeatFiles(FileInfoModel, catalogId);
            if (checklist == null || checklist.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //文件复制
        public JsonResult CopyFile(string model, int catalogId, bool judge = true)
        {
            var FileInfoModel = JsonConvert.DeserializeObject<List<ResourceFileInfo>>(model);//反序列化
            var checklist = _resourceFileService.CheckRepeatFiles(FileInfoModel, catalogId);
            _resourceFileService.UpdateCatalogOfFiles(FileInfoModel, checklist, catalogId, true, true);
            return JsonTips("success",JStr.OperationSucceed);
        }

        public JsonResult GetFileInfo(int fileId)
        {
            var ca = _resourceFileService.Article.GetById(fileId);
            var file = ca.Article;
            var fileInfo = _resourceFileService.GetFile(fileId);
            return Json(new
            {
                Id = file.Id,
                Title = file.Title,
                FileSize = fileInfo.FileSize,
                ContentType = fileInfo.ContentType,
                Keywords = file.Keywords,
                CreateTime = file.CreateTime.ToString(),
                Abstract = file.Abstract,
            }, JsonRequestBehavior.AllowGet);
        }
        #region 修改文件属性（包括扩展属性）

        protected int _innerCatalogId;

        public override int CatalogId
        {
            get
            {
                string catName = Request.QueryString["cat"];
                if (!catName.IsEmpty())
                {
                    Base_Catalog cat = SiteManager.Catalog.GetByName(catName);
                    return cat == null ? _innerCatalogId : cat.Id;
                }
                else
                {
                    return _innerCatalogId;
                }
            }
        }
         
        /// <summary>
        /// 编辑页面
        /// </summary>
        /// <param name="caId">栏目文章ID</param>
        /// <returns>用户编辑页面</returns>
        public virtual ActionResult Edit(int caId = 0)
        {
            var ca = (caId > 0) ? _resourceFileService.Article.GetById(caId) : _resourceFileService.Article.CreateByCatalog(CatalogId);
            _innerCatalogId = ca.CatalogId;
            Session["Editing_" + _innerCatalogId] = ca;
            ViewBag.ShowBreadCrumb = false;
            Base_Catalog cat = SiteManager.Catalog.GetById(_innerCatalogId);
            string catName = cat == null ? null : cat.Name;
            ca.CheckExts();
            return View(ca.Article);
        }
        /// <summary>
        /// 提交编辑
        /// </summary>
        /// <param name="art"></param>
        /// <param name="caId"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)] //因为要传递带html标签的art.Text，所以此处不能验证输入
        public ActionResult Edit(Base_Article art, int caId = 0)
        {
            art.EditorId = CommOp.ToInt(CurrentUserId);
            art.State = ArticleState.Published;
            Base_CatalogArticle ca;
            if (caId == 0)
            {
                ca = new Base_CatalogArticle { CatalogId = CatalogId };
            }
            else
            {
                ca = _resourceFileService.Article.GetById(caId);
                _innerCatalogId = ca.CatalogId;
            }

            ca.Article = art;

            if (art.Id > 0 && caId == 0)
            {
                var oldCaId = _resourceFileService.Article.GetByArticleId(art.Id, CatalogId).Id;
                ca.Id = oldCaId;
            }

            if (true)
            {
                _resourceFileService.Article.Save(ca);
            }
            return JsonTips("success", "",JStr.SuccessSaved0,(object)null, ca.Article.Title);
        }
        #endregion

        #endregion

        #region MD5校验
        public ActionResult VerificateMD5()
        {
            return View();
        }
        #endregion

        #region 修改用户头像的分部视图 2016/2/15
        public ActionResult SetUserAvatar()
        {
            return PartialView();
        }

        public ActionResult UserAvatar()
        {
            return PartialView();
        }
        #endregion

        /// <summary>
        /// 获得所有的系统头像信息，主要获取系统头像ID
        /// </summary>
        /// <returns></returns>
        public ActionResult Avatar()
        {
            List<ResourceFileInfo> lists = _resourceFileService.GetAllSysAvatar();
            ViewBag.ShowToolBar = false;
            return View(lists);
        }

        /// <summary>
        /// 根据userId获得用户选定的系统头像
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public ActionResult GetUserAvatar(string userName)
        {
            var user = AppManager.Instance.UserManager.GetByName(userName);
            if (user == null) return null;
            var file = _resourceFileService.GetUserAvatar(user.Id.ToInt());//根据userId获取文件Id，再用文件Id来提取头像
            var fileAvatar = GetSystemAvatar(file.Id);
            return fileAvatar;
        }

        public JsonResult GetUserAvatarInfo(string userName)
        {
            var user = AppManager.Instance.UserManager.GetByName(userName);
            if (user == null) return null;
            var file = _resourceFileService.GetUserAvatar(user.Id.ToInt());//根据userId获取文件Id，再用文件Id来提取头像
            return Json(file, JsonRequestBehavior.AllowGet);
 
        }

        /// <summary>
        /// 根据系统头像的Id获得系统头像
        /// </summary>
        /// <param name="id">文件信息ID（Base_Article中的ID）</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetSystemAvatar(int id)
        {
            var thumbnail = _resourceFileService.GetSysAvatarThumbnail(id);
            var file = thumbnail == null ? null : File(thumbnail, "image/png");
            return file;
        }
        /// <summary>
        /// 保存系统头像
        /// </summary>
        /// <param name="id">头像文件的Id</param>
        public void SaveSysAvatar(int id)
        {
            ResourceFileInfo file = new ResourceFileInfo();
            file.Id = id;
            file.UserId = User.Identity.GetUserId();
            _resourceFileService.SaveAvatar(file);
        }

        /// <summary>
        /// 获得头像目录
        /// </summary>
        /// <returns></returns>
        public int GetAvatarCatalog()
        {
            var avatarCatalogId = _resourceFileService.GetUserAvatarCatId();
            return avatarCatalogId;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _resourceFileService.Dispose();
        }
    }
}

