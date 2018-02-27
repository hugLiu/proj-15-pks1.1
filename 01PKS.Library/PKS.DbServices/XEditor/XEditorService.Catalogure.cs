using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using PKS.Core;
using PKS.Data;
using PKS.DbModels;
using PKS.DbServices.KManage.Model;
using PKS.DbServices.XEditor.Model;
using PKS.DBModels;

namespace PKS.DbServices.XEditor
{
    public partial class XEditorService : AppService, IPerRequestAppService
    {
        /// <summary>
        /// 根据模板获取所有目录
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public List<CatalogueInfo> GetCatalogueInfosByTemplateId(int templateId)
        {
               var catalogueInfos =
                _kTCatalogueRepository.GetQuery().Where(item => item.KTEMPLATEID == templateId)
                    .Select(item => new CatalogueInfo()
                    {
                        Id = item.Id,
                        Code = item.CODE,
                        Name = item.NAME,
                        LevelNumber = item.LEVELNUMBER,
                        OrderNumber = item.ORDERNUMBER,
                        ParentId = item.PARENTID,
                        TemplateId = item.KTEMPLATEID,
                        PlaceHolderId=item.PLACEHOLDERID
                    }).ToList();
            catalogueInfos.ForEach(item =>
            {
                item.NodeId = Convert.ToString(item.Id);
                item.ParentNodeId = Convert.ToString(item.ParentId);
            });
            return catalogueInfos.ToList();
        }

        /// <summary>
        /// 新增目录
        /// </summary>
        /// <param name="catalogueInfo"></param>
        /// <returns></returns>
        public bool AddCatalogure(CatalogueInfo catalogueInfo)
        {
            PKS_KTEMPLATE_CATALOGUE catalogue = new PKS_KTEMPLATE_CATALOGUE();
            catalogue.CODE = catalogue.CODE;
            catalogue.NAME = catalogue.NAME;
            catalogue.LEVELNUMBER = catalogue.LEVELNUMBER;
            catalogue.ORDERNUMBER = catalogue.ORDERNUMBER;
            catalogue.PARENTID = catalogue.PARENTID;
            catalogue.KTEMPLATEID = catalogue.KTEMPLATEID;
            _kTCatalogueRepository.Add(catalogue);
            return true;
        }


        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="catalogueInfo"></param>
        /// <returns></returns>
        public bool DeleteCatalogure(Expression<Func<PKS_KTEMPLATE_CATALOGUE,bool>> expr)
        {
            _kTCatalogueRepository.DeleteList(expr);
            return true;
        }
    }
}
