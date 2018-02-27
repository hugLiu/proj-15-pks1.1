using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PKS.Core;
using PKS.Data;
using PKS.DbModels;
using PKS.DbModels.Portal;
using PKS.Utils;
using PKS.WebAPI.Services;
using PKS.WebAPI.Models;
using PKS.Models;
using System.Security.Cryptography;

namespace PKS.DbServices.Portal.UserCenter
{
    public class UserCenterService : AppService, IPerRequestAppService
    {
        private IRepository<PKS_FAVORITECATALOG> _favCatalog;
        private IRepository<PKS_USERBEHAVIOR> _userBehavior;
        private IRepository<USERPROFILE> _userProfile;
        private IRepository<WEBPAGES_ROLES> _roles;
        private IRepository<WEBPAGES_USERSINROLES> _userInRoles;
        private ISearchService _searchService;
        private SearchSourceFilter filter = new SearchSourceFilter
        {
            Includes = new List<string>()
            {
                MetadataConsts.IIId,
                MetadataConsts.DataId,
                MetadataConsts.Title,
                MetadataConsts.Author,
                MetadataConsts.IndexedDate,
                MetadataConsts.ShowType
            }
        };
        public UserCenterService(IRepository<PKS_FAVORITECATALOG> favCatalog,
                                     IRepository<PKS_USERBEHAVIOR> userBehavior,
                                     IRepository<USERPROFILE> userProfile,
                                     ISearchService searchService,
                                     IRepository<WEBPAGES_ROLES> roles,
                                     IRepository<WEBPAGES_USERSINROLES> userInRoles)

        {
            this._favCatalog = favCatalog;
            this._userBehavior = userBehavior;
            this._userProfile = userProfile;
            this._searchService = searchService;
            this._roles = roles;
            this._userInRoles = userInRoles;
        }

        public PKS_FAVORITECATALOG AddFavoriteCatalog(PKS_FAVORITECATALOG model)
        {
            _favCatalog.Add(model);
            return model;
        }
        public void DelFavoriteCatalog(int id, PKS_FAVORITECATALOG model = null)
        {
            if (model == null) { model = _favCatalog.Find(id); }

            model.PKS_USERBEHAVIOR.ToList().ForEach(
                b =>
                {
                    b.FAVORITEFLAG = false;
                    _userBehavior.Update(b, false);
                }
            );
            _userBehavior.Submit();

            model.PKS_FAVORITECATALOG1.ToList().ForEach(f => DelFavoriteCatalog(f.Id, f));
            if (model.PARENTID == null) { return; } //根节点不删除
            _favCatalog.DeleteByKey(model);
        }
        public void RenameFavoriteCatalog(int id, string name)
        {
            var model = _favCatalog.Find(id);
            model.FAVNAME = name;
            model.LASTUODATEDDATE = DateTime.Now;
            _favCatalog.Update(model);
        }

        public string GetFavoriteCatalogTree(int userId, string userName)
        {
            var roots = _favCatalog.GetQuery().Where(f => f.USERID == userId).ToList();
            if (roots.Any())
            {
                var root = roots.First(r => r.PARENTID == null);
                return new[] { ConvertFavCatalogToTreeNode(root) }.ToJson();
            }
            var newRoot = AddRootFavCatalog(userId, userName);
            return new[] { ConvertFavCatalogToTreeNode(newRoot) }.ToJson();
        }

        /// <summary>
        /// 加载收藏列表
        /// </summary>
        public List<UserCenterListModel> LoadFavoriteList(int userId, int page, int rows, string order, string sort)
        {
            var list = _userBehavior.GetQuery().Where(b => b.USERID == userId && b.FAVORITEFLAG).ToList();
            var dic = list.ToDictionary(k => k.IIID, v => v);
            if (dic.Count == 0)
            {
                return new List<UserCenterListModel>();
            }
            var searchMetaDatas = new SearchMetadatasRequest { Fields = filter };
            searchMetaDatas.IIIds.AddRange(dic.Keys);
            var results = _searchService.GetMetadatas(searchMetaDatas).Select(m => UserCenterListModel.Create(m, dic[m.IIId])).ToList();
            return results;
        }
        public List<UserCenterListModel> LoadDownLoadList(int userId, int page, int rows, string order, string sort)
        {
            var list = _userBehavior.GetQuery().Where(b => b.USERID == userId && b.DOWNLOADCOUNT > 0).ToList();
            var dic = list.ToDictionary(k => k.IIID, v => v);
            if (dic.Count == 0)
            {
                return new List<UserCenterListModel>();
            }
            var searchMetaDatas = new SearchMetadatasRequest { Fields = filter };
            searchMetaDatas.IIIds.AddRange(dic.Keys);
            var results = _searchService.GetMetadatas(searchMetaDatas).Select(m => UserCenterListModel.Create(m, dic[m.IIId])).ToList();
            return results;
        }
        /// <summary>
        /// 添加收藏
        /// </summary>
        public PKS_USERBEHAVIOR AddFavorite(int userId, string IIID, int favoritecatalogId)
        {
            var behavior = _userBehavior.Find(f => f.USERID == userId && f.IIID == IIID);
            if (behavior != null)
            {
                behavior.FAVORITEFLAG = true;
                behavior.FAVORITEDATE = DateTime.Now;
                behavior.FAVORITECATALOGID = favoritecatalogId;
                _userBehavior.Update(behavior);
                return behavior;
            }
            behavior = new PKS_USERBEHAVIOR
            {
                IIID = IIID,
                USERID = userId,
                FAVORITECATALOGID = favoritecatalogId,
                FAVORITEFLAG = true,
                FAVORITEDATE = DateTime.Now,
                DOWNLOADCOUNT = 0
            };
            _userBehavior.Add(behavior);
            return behavior;
        }

        /// <summary>
        /// 取消收藏
        /// </summary>
        public PKS_USERBEHAVIOR CancelFavorite(int userId, string IIID)
        {
            var behavior = _userBehavior.Find(f => f.USERID == userId && f.IIID == IIID);
            if (behavior != null && behavior.FAVORITEFLAG)
            {
                behavior.FAVORITEFLAG = false;
                _userBehavior.Update(behavior);
            }
            return behavior;
        }

        /// <summary>
        /// 添加下载记录
        /// </summary>
        public PKS_USERBEHAVIOR AddDownLoad(int userId, string IIID)
        {
            var behavior = _userBehavior.Find(f => f.USERID == userId && f.IIID == IIID);
            if (behavior != null)
            {
                behavior.DOWNLOADCOUNT = behavior.DOWNLOADCOUNT + 1;
                behavior.DOWNLOADDATE = DateTime.Now;
                _userBehavior.Update(behavior);
                return behavior;
            }
            behavior = new PKS_USERBEHAVIOR
            {
                IIID = IIID,
                USERID = userId,
                DOWNLOADCOUNT = 0,
                DOWNLOADDATE = DateTime.Now
            };
            _userBehavior.Add(behavior);
            return behavior;
        }

        TreeNode ConvertFavCatalogToTreeNode(PKS_FAVORITECATALOG favCatalog)
        {
            var treeNode = new TreeNode
            {
                id = favCatalog.Id,
                text = favCatalog.FAVNAME,
                ChildrenCount = favCatalog.PKS_FAVORITECATALOG1.Count,
            };
            if (treeNode.ChildrenCount > 0)
            {
                treeNode.children = favCatalog.PKS_FAVORITECATALOG1.Select(f => ConvertFavCatalogToTreeNode(f)).ToList();
            }
            if (favCatalog.PARENTID == null) { treeNode.type = 0; treeNode.state = "open"; }
            return treeNode;
        }



        public PKS_FAVORITECATALOG AddRootFavCatalog(int userId, string userName)
        {
            PKS_FAVORITECATALOG m = new PKS_FAVORITECATALOG
            {
                USERID = userId,
                PARENTID = null,
                FAVNAME = "收藏夹",
                CREATEDBY = userName,
                CREATEDDATE = DateTime.Now,
                LASTUPDATEDBY = userName,
                LASTUODATEDDATE = DateTime.Now
            };
            _favCatalog.Add(m);
            return m;
        }

        public USERPROFILE GetUserProfile(int id, out string roleName)
        {
            roleName = (from ur in _userInRoles.GetQuery()
                        join r in _roles.GetQuery()
                        on ur.ROLEID equals r.ROLEID
                        where ur.USERID == id
                        select r.ROLENAME).First();
            return _userProfile.Find(id);
        }

        public ChangePasswordResultType ChangePassword(int userId, string oldPassword, string newPassword)
        {
            var memberShipService = GetService<IRepository<WEBPAGES_MEMBERSHIP>>();
            var now = DateTime.Now;
            WEBPAGES_MEMBERSHIP member = memberShipService.Find(userId);
            if (member == null)
            {
                return ChangePasswordResultType.NotLoginIn;
            }
            if (member.PASSWORD != GenarateEncryptPassword(oldPassword, member.PASSWORDSALT))
            {
                member.LASTPASSWORDFAILUREDATE = now;
                memberShipService.Update(member);
                return ChangePasswordResultType.PasswordError;
            }
            member.PASSWORD = GenarateEncryptPassword(newPassword, member.PASSWORDSALT);
            member.PASSWORDCHANGEDDATE = now;
            memberShipService.Update(member);
            return ChangePasswordResultType.Success;
        }

        private string GenarateEncryptPassword(string password, string passwordSalt)
        {
            var originalString = password + passwordSalt;
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] md5Bytes = md5.ComputeHash(UnicodeEncoding.UTF8.GetBytes(originalString));
            return BitConverter.ToString(md5Bytes).Replace("-", "");
        }

        public bool HasFavorite(int userId, string iiid)
        {
            var behavior = _userBehavior.Find(f => f.USERID == userId && f.IIID == iiid && f.FAVORITEFLAG);
            return behavior != null;
        }

    }

    public enum ChangePasswordResultType
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 1,

        /// <summary>
        /// 未登录
        /// </summary>
        NotLoginIn = -1,

        /// <summary>
        /// 密码错误
        /// </summary>
        PasswordError = -2,
    }

    public class TreeNode
    {
        public int id { get; set; }
        public string text { get; set; }
        public string iconCls { get; set; } = "icon-folder"; //自定义一个文件夹图标样式，该样式可能还有必要美化
        public int type { get; set; } = 1;

        [Newtonsoft.Json.JsonIgnore]
        public int ChildrenCount { get; set; }
        public string state { get; set; } = "";

        public List<TreeNode> children { get; set; } = new List<TreeNode>();
    }

    public class UserCenterListModel
    {
        public int Id { get; set; }
        public string FavoriteDate { get; set; }
        public string IIID { get; set; }
        public bool FavoriteFlag { get; set; }
        public int DownLoadCount { get; set; }
        public string DownLoadDate { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string DataId { get; set; }
        public string IndexedDate { get; set; }
        public IndexShowType ShowType { get; set; }
        private static readonly IndexShowType[] AllowedShowTypes = new[] { IndexShowType.Image, IndexShowType.Pdf };
        public bool CanDownLoad { get { return AllowedShowTypes.Any(e => e == this.ShowType); } }

        public static UserCenterListModel Create(Metadata m, PKS_USERBEHAVIOR p)
        {
            return new UserCenterListModel
            {
                Id = p.Id,
                FavoriteFlag = p.FAVORITEFLAG,
                FavoriteDate = p.FAVORITEDATE.ToStandardString(),
                IIID = p.IIID,
                DownLoadCount = p.DOWNLOADCOUNT,
                DownLoadDate = p.DOWNLOADDATE.ToStandardString(),
                Title = m.Title,
                Author = m.Author,
                DataId = m.DataId,
                ShowType = m.ShowType.ToEnum<IndexShowType>(true),
                IndexedDate = m.IndexedDate.ToStandardString()
            };
        }
    }
}
