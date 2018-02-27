using Ninject;
using System.Data.Entity;
using PKS.Core;
using PKS.Data;
using PKS.DbModels.PortalMgmt;
using PKS.WebAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PKS.DbServices.SysFrame.Model;
using PKS.Utils;
using PKS.Models;

namespace PKS.DbServices.SysFrame
{
    public class RoleMetadataPermissionService : AppService, IInitializable
    {
        /// <summary>初始化</summary>
        void IInitializable.Initialize()
        {
            this.EventBus.Register(this);
        }

        /// <summary>
        /// 从Mongo中将数据导到SQL Server中
        /// </summary>
        public void InitMetas()
        {
            var db = GetService<IRepository<MetadataDefinition>>();
            if (db.GetAll().Count != 0) return;

            List<PKS.Models.MetadataDefinition> metadataDefinitionList = new List<PKS.Models.MetadataDefinition>();
            var service = GetService<ISearchServiceWrapper>();
            var metadataDefinitionCollection = service.GetMetadataDefinitions().ToList();

            var groups = metadataDefinitionCollection
               .GroupBy(r => r.GroupName).Select(r => r.First());

            groups.ToList().ForEach(
                g => db.Add(new MetadataDefinition
                {
                    Name = g.GroupName,
                    Title = g.GroupName,
                    ItemOrder = g.GroupOrder
                }));
            db.Submit();

            var gs = db.GetAll();

            foreach (var item in metadataDefinitionCollection)
            {
                metadataDefinitionList.Add(item);
            }

            metadataDefinitionList.ForEach(m =>
            {
                var me = new MetadataDefinition
                {
                    Name = m.Name,
                    Title = m.Title,
                    Description = m.Description,
                    Required = m.Required,
                    Format = m.Format,
                    InnerTag = m.InnerTag,
                    Type = m.Type.ToString(),
                    UiType = m.UiType.ToString(),
                    GroupCode = m.GroupCode,
                    GroupOrder = m.GroupOrder,
                    GroupName = m.GroupName,
                    ItemOrder = m.ItemOrder,
                    DataSource = "1",
                    SearchWeight = 1,
                    PId = gs.Where(g => g.Name == m.GroupName).FirstOrDefault().Id
                };
                db.Add(me);
            });

            db.Submit();
        }

        /// <summary>
        /// 获取元数据
        /// </summary>
        /// <returns></returns>
        public IList<MetadataDefinition> GetMetas()
        {
            return GetService<IRepository<MetadataDefinition>>()
                        .GetQuery()
                        .OrderBy(r => r.Id)
                        .ToList();
        }

        /// <summary>
        /// 保存元数据
        /// </summary>
        /// <param name="metas"></param>
        public void SaveMetas(IList<MetadataDefinition> metas)
        {
            var db = GetService<IRepository<MetadataDefinition>>();
            metas.ForEach(m =>
            {
                if (m.Id == 0)
                {
                    db.Add(m, false);
                }
                else
                {
                    db.Update(m, false);
                }
            });
            db.Submit();
        }

        public void SaveMetaItems(int metaId, List<string> values)
        {
            var items = values.Distinct().Where(v=>v!="").Select(v => new MetadataValueItem
            {
                MetaId=metaId,
                Text = v,
                Value = v
            }).ToList();

            var db = GetService<IRepository<MetadataValueItem>>();
            db.DeleteList(i => i.MetaId == metaId);
            items.ForEach(i => db.Add(i));
        }

        /// <summary>
        /// 删除元数据
        /// </summary>
        /// <param name="ids"></param>
        public void DeleteMetas(int[] ids)
        {
            var db = GetService<IRepository<MetadataDefinition>>();
            db.DeleteList(m => ids.Contains(m.Id));
        }

        /// <summary>
        /// 获取角色的数据权限相关的元数据
        /// </summary>
        /// <param name="metaId"></param>
        /// <returns></returns>
        public IList<PKS_ROLE_METADATAPERMISSION> GetPermissionRoleMetas(int roleId)
        {
            return GetService<IRepository<PKS_ROLE_METADATAPERMISSION>>()
                         .GetQuery()
                         .Where(m => m.RoleId == roleId)
                         .ToList();
        }

        /// <summary>
        /// 获取角色的数据权限相关的元数据
        /// </summary>
        /// <param name="metaId"></param>
        /// <returns></returns>
        public IList<RoleMetadataItemPermission> GetMetadataItemPermissions(int[] roleId)
        {
            var metadataDefinitionRepository = GetService<IRepository<MetadataDefinition>>().GetQuery();
            var metadataPermissionRepository = GetService<IRepository<PKS_ROLE_METADATAPERMISSION>>().GetQuery();
            var metadataItemPermissionRepository = GetService<IRepository<PKS_ROLE_METADATAITEMPERMISSION>>().GetQuery();
            var itemPermissionQuery = from metadataItemPermission in metadataItemPermissionRepository
                join metadataPermission in metadataPermissionRepository on metadataItemPermission.RoleMetaId equals
                metadataPermission.Id
                join metadataDefinition in metadataDefinitionRepository on metadataPermission.MetadataId equals
                metadataDefinition.Id
                where roleId.Contains(metadataPermission.RoleId)&& metadataPermission.IsValid
                select new RoleMetadataItemPermission
                {
                    IsAble = metadataItemPermission.IsAble,
                    IsValid = metadataPermission.IsValid,
                    MetadataId = metadataDefinition.Id,
                    MetadataItemId = metadataItemPermission.Id,
                    MetadataItemName = metadataItemPermission.MetadataItemName,
                    MetadataName = metadataDefinition.Name,
                    MetadataPermissionId = metadataPermission.Id,
                    MetaDataType = metadataDefinition.Type,
                    RoleId = metadataPermission.RoleId
                };
            return itemPermissionQuery.ToList();
        }

        /// <summary>
        /// 获取一个元数据下的项
        /// </summary>
        /// <param name="metaId"></param>
        /// <returns></returns>
        public IList<MetadataValueItem> GetMetaItems(int metaId)
        {
            return GetService<IRepository<MetadataValueItem>>()
                         .GetQuery()
                         .Where(m => m.MetaId == metaId)
                         .ToList();
        }

        /// <summary>
        /// 获取角色的某个元数据下的项（包括黑白名单）
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="metaId"></param>
        /// <returns></returns>
        public IList<PKS_ROLE_METADATAITEMPERMISSION> GetPermissionRoleMetaItems(int roleId, int metaId)
        {
            return GetService<IRepository<PKS_ROLE_METADATAPERMISSION>>()
                         .GetQuery()
                         .Where(m => m.RoleId == roleId)
                         .Where(m => m.MetadataId == metaId)
                         .FirstOrDefault()?.MetadataItemPermissioin;
        }

        /// <summary>
        /// 保存Role_Metadata之间的关系
        /// </summary>
        /// <param name="metas"></param>
        /// <param name="roleId"></param>
        public List<PKS_ROLE_METADATAPERMISSION> SaveRoleMetas(int roleId, List<PKS_ROLE_METADATAPERMISSION> roleMetas)
        {
            var db = GetService<IRepository<PKS_ROLE_METADATAPERMISSION>>();

            var temp = new List<PKS_ROLE_METADATAPERMISSION>();
            var temp2 = new List<PKS_ROLE_METADATAPERMISSION>();
            roleMetas.ForEach(p =>
            {
                var d = db.GetQuery().Where(m => m.RoleId == roleId && m.MetadataId == p.MetadataId).ToList();
                if (d.Count > 0)
                {
                    temp.Add(d[0]);
                    d[0].IsValid = p.IsValid;
                    db.Update(d[0]);
                }

                if (db.Exist(m => m.RoleId == roleId && m.MetadataId == p.MetadataId))
                {
                    temp2.Add(p);
                }
            });


            //删除原本有且当前保存时被删除的
            var delete = db.GetQuery().Where(m => m.RoleId == roleId).ToList().Except(temp);
            var dd = GetService<IRepository<PKS_ROLE_METADATAITEMPERMISSION>>();
            //删除Role_metaItems
            delete.ToList().ForEach(d =>
            {
                dd.DeleteList(r => r.RoleMetaId == d.Id);
            });
            //删除Role_Metas
            db.DeleteList(delete);

            //添加界面上添加的
            var add = roleMetas.Except(temp2);
            add.ToList().ForEach(p =>
            {
                db.Add(p, false);
            });
            db.Submit();

            var result = db.GetQuery().Where(p => p.RoleId == roleId).ToList();
            return result;
        }

        /// <summary>
        /// 保存Role_MetaItem之间的关系
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="metaId"></param>
        /// <param name="whiteList"></param>
        /// <param name="blackList"></param>
        public void SaveRoleMetaItems(int roleMetaId, List<PKS_ROLE_METADATAITEMPERMISSION> whiteList, List<PKS_ROLE_METADATAITEMPERMISSION> blackList)
        {
            var db = GetService<IRepository<PKS_ROLE_METADATAITEMPERMISSION>>();
            var query = db.GetQuery();
            db.DeleteList(r => r.RoleMetaId == roleMetaId);

            if (blackList != null)
            {
                blackList.ForEach(p =>
                {
                    p.RoleMetaId = roleMetaId;
                    p.IsAble = false;
                    db.Add(p, false);
                });
            }

            if (whiteList != null)
            {
                whiteList.ForEach(p =>
                {
                    p.RoleMetaId = roleMetaId;
                    p.IsAble = true;
                    db.Add(p, false);
                });
            }
            db.Submit();
        }

    }

    public static class JsonEx
    {
        public static T ToViewModel<T>(this object ob)
        {
            return ob.ToJson().JsonTo<T>();
        }
    }

}
