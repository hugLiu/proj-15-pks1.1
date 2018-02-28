using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PKS.DBModels;
using PKS.DbModels.Portal;
using PKS.DbServices.Portal.UserCenter;
using PKS.Utils;
using PKS.Web;
using PKS.Web.Controllers;
using PKS.Models;

namespace PKS.Portal.Controllers
{
    public class UserCenterController : PKSBaseController
    {
        private UserCenterService _userCenterService;
        public UserCenterController(UserCenterService userCenterService)
        {
            this._userCenterService = userCenterService;
        }

        public ActionResult Index(int userId)
        {
            string roleName;
            var user = _userCenterService.GetUserProfile(userId, out roleName);
            var favCatalogTree = _userCenterService.GetFavoriteCatalogTree(user.USERID, user.USERNAME);
            ViewBag.treeData = favCatalogTree;
            ViewBag.roleName = roleName;
            ViewBag.isFormsAuthentication = this.PKSUser.Identity.AuthenticationType == AuthenticationType.Forms.ToString();
            return View(user);
        }
        public ActionResult FavoriteList(int userId)
        {
            ViewBag.userId = userId;
            return View();
        }

        public ActionResult DownLoadList(int userId)
        {
            ViewBag.userId = userId;
            return View();
        }

        public JsonResult ChangPassword(int userId, string oldPassword, string newPassword)
        {
            ChangePasswordResultType type = _userCenterService.ChangePassword(userId, oldPassword, newPassword);
            return Json(new { code = type.ToString() }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加收藏弹框
        /// </summary>
        public ActionResult FavoriteCatalogList(int userId, string IIID)
        {
            string roleName;
            var user = _userCenterService.GetUserProfile(userId, out roleName);
            var favCatalogTree = _userCenterService.GetFavoriteCatalogTree(user.USERID, user.USERNAME);
            ViewBag.treeData = favCatalogTree;
            ViewBag.userId = userId;
            ViewBag.IIID = IIID;
            ViewBag.userName = user.USERNAME;
            return View();

        }

        public ActionResult AddFavorite(int userId, string IIID, int? favoritecatalogId = null)
        {
            if (!favoritecatalogId.HasValue || favoritecatalogId <= 0)
            {
                return Json(new { message = "请选择文件夹", code = 0 });
            }
            _userCenterService.AddFavorite(userId, IIID, favoritecatalogId.Value);
            return Json(new { message = "添加成功", code = 1 }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CancelFavorite(int userId, string IIID)
        {
            _userCenterService.CancelFavorite(userId, IIID);
            return Json(new { message = "取消", code = 1 }, JsonRequestBehavior.AllowGet);
        }

        public ContentResult LoadFavoriteList(int userId, int page, int rows, string order, string sort)
        {
            var datas = _userCenterService.LoadFavoriteList(userId, page, rows, order, sort);
            var json = new
            {
                rows = datas.OrderByDescending(o => o.FavoriteDate)
                .Skip(rows * (page - 1)).Take(rows)
                .ToList(),
                total = datas.Count
            }.ToJson();
            return Content(json);
        }
        public ContentResult LoadDownLoadList(int userId, int page, int rows, string order, string sort)
        {
            var datas = _userCenterService.LoadDownLoadList(userId, page, rows, order, sort);
            var json = new
            {
                rows = datas.OrderByDescending(o => o.DownLoadDate)
                .Skip(rows * (page - 1)).Take(rows)
                .ToList(),
                total = datas.Count
            }.ToJson();
            return Content(json);
        }
        #region 收藏文件夹的增删改查

        public ActionResult AddFavCatalog(int? pId, string folderName, int userId, string userName)
        {
            if (pId <= 0) { pId = null; }
            PKS_FAVORITECATALOG m = new PKS_FAVORITECATALOG
            {
                USERID = userId,
                PARENTID = pId,
                FAVNAME = folderName,
                CREATEDBY = userName,
                CREATEDDATE = DateTime.Now,
                LASTUPDATEDBY = userName,
                LASTUODATEDDATE = DateTime.Now
            };
            m = _userCenterService.AddFavoriteCatalog(m);
            return Json(new { id = m.Id, text = m.FAVNAME });
        }

        public void DelFavCatalog(int id)
        {
            _userCenterService.DelFavoriteCatalog(id);
        }
        public void RenameFavCatalog(int id, string name)
        {
            _userCenterService.RenameFavoriteCatalog(id, name);
        }

        #endregion

    }
}