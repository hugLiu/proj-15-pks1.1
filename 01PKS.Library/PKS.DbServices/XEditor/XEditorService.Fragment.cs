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
       /// 获取所有组件类型
       /// </summary>
       /// <returns></returns>
        public List<FragmentType> GetAllFragmentTypes()
        {
            var fragmentTypeRepository = Bootstrapper.Get<IRepository<PKS_KFRAGMENT_TYPE>>();
            var fragmentTypeParamRepository = Bootstrapper.Get<IRepository<PKS_KFRAGMENT_TYPE_PARAMETER>>();
            var fragmentTypes =
                fragmentTypeRepository.GetQuery()
                    .Select(item => new FragmentType
                    {
                        Id = item.Id,
                        Code = item.CODE,
                        Name = item.NAME,
                        Category = item.Category,
                        VueTag = item.VUETAG,
                        HasTextTemplate = item.HASTEXTTEMPLATE,
                       OrderNumber = item.ORDERNUMBER,
                       ImageUrl=item.IMAGEURL
                    }).OrderBy(item=>item.Category).ThenBy(item=>item.OrderNumber).ToList();
            fragmentTypes.ForEach(fragmentType =>
            {
                fragmentType.ComParams = fragmentTypeParamRepository.GetQuery()
                    .Where(typeparam => typeparam.KFRAGMENTTYPEID
                                        == fragmentType.Id).Select(item => new FragmentTypeParam()
                    {
                        Id = item.Id,
                        Code = item.CODE,
                        DataType = item.DATATYPE,
                        DefaultValue = item.DEFAULTVALUE,
                        FragmentTypeId = item.KFRAGMENTTYPEID,
                        Metadata = item.METADATA,
                        Name = item.NAME
                    }).ToList();
            });
            return fragmentTypes;
        }
        /// <summary>
        /// 根据组件类型获取组件参数
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public List<FragmentTypeParam> GetComponentParamsByFragmentTypeId(List<int> fragmentTypeIds)
        {
            var fragmentTypeParamRepository = Bootstrapper.Get<IRepository<PKS_KFRAGMENT_TYPE_PARAMETER>>();
            var paramQuery =
                fragmentTypeParamRepository.GetQuery().Where(item => fragmentTypeIds.Contains(item.KFRAGMENTTYPEID))
                    .Select(item => new FragmentTypeParam
                    {
                        Id = item.Id,
                        Code = item.CODE,
                        DataType = item.DATATYPE,
                        Name = item.NAME,
                        FragmentTypeId = item.KFRAGMENTTYPEID,
                        Metadata = item.METADATA,
                        DefaultValue = item.DEFAULTVALUE
                    }).ToList();
            return paramQuery;
        }

        /// <summary>
        /// 新增片段
        /// </summary>
        /// <param name="fragmentModel"></param>
        /// <returns></returns>
        public bool AddFragmentInfo(FragmentModel fragmentModel)
        {
            PKS_KFRAGMENT fragmentment = new PKS_KFRAGMENT();
            fragmentment.KTEMPLATEID = fragmentModel.TemplateId;
            fragmentment.KTEMPLATECATALOGUEID = fragmentModel.TemplateCatalogueId;
            fragmentment.TITLE = fragmentModel.Title;
            fragmentment.QUERYPARAMETER = fragmentModel.QueryParameter;
            fragmentment.COMPONENTPARAMETER = fragmentModel.ComponentParameter;

            fragmentment.HTMLTEXT = fragmentModel.Htmltext;
            fragmentment.KFRAGMENTTYPEID = fragmentModel.FragmentTypeId;
            fragmentment.PLACEHOLDERID = fragmentModel.PlaceholderId;
       
            _kFragmentRepository.Add(fragmentment);
            return true;
        }

        /// <summary>
        /// 删除片段
        /// </summary>
        /// <param name="catalogueInfo"></param>
        /// <returns></returns>
        public bool DeleteFragment(Expression<Func<PKS_KFRAGMENT, bool>> expr)
        {
            _kFragmentRepository.DeleteList(expr);
            return true;
        }
    }
}
