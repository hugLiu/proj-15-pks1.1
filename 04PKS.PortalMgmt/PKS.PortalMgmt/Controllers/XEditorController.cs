using Jurassic.WebFrame;
using PKS.Models;
using PKS.WebAPI.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PKS.DbServices.KManage.Model;
using PKS.DbServices.XEditor;
using PKS.DbServices.XEditor.Model;
using PKS.Utils;
using PKS.Web.MVC;

namespace PKS.PortalMgmt.Controllers
{
    /// <summary>
    /// 编辑器Controller
    /// </summary>
    public class XEditorController : PKSBaseController
    {
        private XEditorService _xEditorService;
        public XEditorController(XEditorService xEditorService)
        {
            _xEditorService = xEditorService;
        }

        public ActionResult Index()
        {
            ViewBag.ShowToolBar = false;
            ViewBag.Token = Token;
            ViewBag.UserName = PKSUser != null ? PKSUser.Identity.Name : CurrentUser.Name;
            return View();
        }

        [HttpGet]
        public ActionResult GetMetadataDefinition()
        {
            return NewtonJson(GetEsMetadatas());
        }

        private List<MetadataDefinition> GetEsMetadatas()
        {
            List<MetadataDefinition> metadataDefinitionList = new List<MetadataDefinition>();
            var service = GetService<ISearchServiceWrapper>();
            MetadataDefinition[] metadataDefinitionCollection = service.GetMetadataDefinitions();

            var query = from u in metadataDefinitionCollection
                        where u.UiType != MetadataUiType.Image.ToString()
                        orderby u.GroupOrder descending, u.ItemOrder descending
                        select u;

            foreach (var item in query)
            {
                metadataDefinitionList.Add(item);
            }
            return metadataDefinitionList;
        }

        public ActionResult GetMetadatas()
        {
            var esMetadatas = GetEsMetadatas().Select(item=>new {item.Id,item.Name,item.Title,item.Type,item.PId});
            var comMetadatas = _xEditorService.GetAllFragmentTypes();
            var data = new
            {
                es=esMetadatas,
                component= comMetadatas
            };
            return new NewtonJsonResult
            {
                Data = data,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Settings = JsonUtil.CamelCaseJsonSerializerSettings
            };
        }

        #region 模板
        [HttpGet]
        public string GetTemplateContent(int templateId)
        {
           var templateInfo= _xEditorService.GetTemplateById(templateId);
            return templateInfo == null ? string.Empty : templateInfo.Template;
        }

        public ActionResult GetTemplateDetailInfos(int templateId)
        {
            TemplateDetailInfo detailInfo = new TemplateDetailInfo();
            detailInfo.TemplateInfo = _xEditorService.GetTemplateById(templateId);
            detailInfo.Fragments= _xEditorService.GetFragmentsByTemplateId(templateId);
            detailInfo.Catalogues = _xEditorService.GetCatalogueInfosByTemplateId(templateId);
            return NewtonJson(detailInfo);
        }

        public ActionResult GetTemplateTree()
        {
            return NewtonJson(_xEditorService.GetTemplateTree());
        }

        [HttpGet]
        public ActionResult GetFragmentsByTemplateId(int templateId)
        {
            return NewtonJson(_xEditorService.GetFragmentsByTemplateId(templateId));
        }

        [HttpGet]
        public ActionResult GetTemplateParamsByTemplateId(int templateId)
        {
            var templateParams = _xEditorService.GetTemplateParamsByTemplateId(templateId);          
            return NewtonJson(templateParams.ToDictionary(item => item.Code, item => item.Name));
        }

        [HttpPost]
        public ActionResult AddTemplate(int pid,string name)
        {
            return NewtonJson(_xEditorService.AddTemplate(pid,name));
        }

        [HttpPost]
        public bool UpdateTemplateName(int id,string name)
        {
            return _xEditorService.UpdateTemplateName(id, name);
        }
        [HttpPost]
        public bool Deletetemplate(int id)
        {
            return _xEditorService.DeleteTemplate(id);
        }
        [HttpPost]
        [ValidateInput(false)]
        public bool SaveTemplate(string templateInfo, string fragments, string catalogues)
        {
            var templateTree = JsonConvert.DeserializeObject<TemplateTree>(templateInfo);
            var fragmentModels = JsonConvert.DeserializeObject<List<FragmentModel>>(fragments);
            var catalogueInfos = JsonConvert.DeserializeObject<List<CatalogueInfo>>(catalogues);
            return _xEditorService.SaveTemplate(templateTree, fragmentModels, catalogueInfos);
        }
        #endregion

        #region 目录

        public ActionResult GetCatalogueInfosByTemplateId(int templateId)
        {
            return NewtonJson(_xEditorService.GetCatalogueInfosByTemplateId(templateId));
        }

        public bool AddCatalogue(string catalogueInfo)
        {
            var catalogue = JsonConvert.DeserializeObject<CatalogueInfo>(catalogueInfo);
            return _xEditorService.AddCatalogure(catalogue);
        }

        public bool DeleteCatalogure(int catalogureId)
        {
            return _xEditorService.DeleteCatalogure(item=>item.Id== catalogureId);
        }
        #endregion

        #region 组件片段

        /// <summary>
        /// 获取所有组件类型
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllFragmentTypes()
        {
            return NewtonJson(_xEditorService.GetAllFragmentTypes());
        }
        /// <summary>
        /// 获取组件参数
        /// </summary>
        /// <param name="fragmentTypeId"></param>
        /// <returns></returns>
        public ActionResult GetComponentParamsByFragmentTypeId(int fragmentTypeId)
        {
            return Json(_xEditorService.GetComponentParamsByFragmentTypeId(new List<int>(){ fragmentTypeId }),JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 添加片段
        /// </summary>
        /// <param name="fragment"></param>
        /// <returns></returns>
        public bool AddFragmentInfo(string fragment)
        {
            var fragmentModel = JsonConvert.DeserializeObject<FragmentModel>(fragment);
            return _xEditorService.AddFragmentInfo(fragmentModel);
        }

        public bool DeleteFragment(int fragmentId)
        {
            return _xEditorService.DeleteFragment(item=>item.Id== fragmentId);
        }
        #endregion

        public void GetServiceMetadata()
        {
            XEditorServiceMetadata metadata = new XEditorServiceMetadata();
            List<XEditorMetadata> metadatas=new List<XEditorMetadata>();
            metadata.Metadatas = metadatas;
            //es元数据
            XEditorMetadata esMetadata=new XEditorMetadata();
            esMetadata.Description = "获取所有Es标签";
            esMetadata.Url = "/Editor/GetMetadataDefinition";
            esMetadata.Method = "Get";
            metadatas.Add(esMetadata);

            //组件
            XEditorMetadata getAllFragmentTypesMetadata = new XEditorMetadata();
            getAllFragmentTypesMetadata.Description = "获取所有组件及组件参数";
            getAllFragmentTypesMetadata.Url = "/Editor/GetAllFragmentTypes";
            getAllFragmentTypesMetadata.Method = "Get";
            metadatas.Add(getAllFragmentTypesMetadata);

            XEditorMetadata templateTreeMetadata = new XEditorMetadata();
            templateTreeMetadata.Description = "获取模板及模板类别树";
            templateTreeMetadata.Url = "/Editor/GetTemplateTree";
            templateTreeMetadata.Method = "Get";
            metadatas.Add(templateTreeMetadata);

            XEditorMetadata addTemplateMetadata = new XEditorMetadata();
            templateTreeMetadata.Description = "添加模板";
            templateTreeMetadata.Url = "/Editor/AddTemplate";
            templateTreeMetadata.Method = "Post";
            List<XEditorParamMetadata> addTemplatEditorParamMetadatas = new List<XEditorParamMetadata>();
            string templateInfoDemo =
                "{ \"id\":5,\"code\":\"Trap1\",\"name\":\"圈闭百科模版1\",\"template\":null,\"isdefault\":true,\"templatecategoryid\":2,\"subsystemid\":4,\"templateurlid\":3,\"instanceclass\":\"圈闭\",\"istemplate\":false,\"nodeid\":\"tem_5\",\"parentnodeid\":\"cat_2\"}";
            addTemplatEditorParamMetadatas.Add(new XEditorParamMetadata("templateInfo", "string", "json串", templateInfoDemo));
            addTemplateMetadata.MetadataParams = addTemplatEditorParamMetadatas;
            metadatas.Add(addTemplateMetadata);

            XEditorMetadata saveTemplateMetadata = new XEditorMetadata();
            saveTemplateMetadata.Description = "保存模板";
            saveTemplateMetadata.Url = "/Editor/SaveTemplate";
            saveTemplateMetadata.Method = "Post";
            List<XEditorParamMetadata> saveTemplatEditorParamMetadatas = new List<XEditorParamMetadata>();
            saveTemplatEditorParamMetadatas.Add(new XEditorParamMetadata("templateInfo", "string", "json串", templateInfoDemo));
            var fragmentsParamDemo =
                "[{\"id\":1,\"templateid\":\"\",\"templatecatalogueid\":10,\"title\":\"\",\"queryparameter\":\"\",\"componentparameter\":\"\",\"htmltext\":\"\",\"fragmenttypeid\":\"\",\"placeholderid\":\"\"}]";
            saveTemplatEditorParamMetadatas.Add(new XEditorParamMetadata("fragments", "string", "json串", fragmentsParamDemo));
            var cataloguesParamDemo =
   "[{ \"id\":1,\"code\":\"\",\"name\":\"\",\"levelnumber\":\"\",\"ordernumber\":\"\",\"parentid\":2,\"templateid\":10,\"nodeid\":\"\",\"parentnodeid\":\"\"}]";
            saveTemplatEditorParamMetadatas.Add(new XEditorParamMetadata("catalogues", "string", "json串", cataloguesParamDemo));
            saveTemplateMetadata.MetadataParams = saveTemplatEditorParamMetadatas;
            metadatas.Add(saveTemplateMetadata);


            XEditorMetadata getCatalogueMetadata = new XEditorMetadata();
            getCatalogueMetadata.Description = "根据模板获取所有目录";
            getCatalogueMetadata.Url = "/Editor/GetCatalogueInfosByTemplateId";
            getCatalogueMetadata.Method = "Get";
            List<XEditorParamMetadata> getCatalogueEditorParamMetadatas = new List<XEditorParamMetadata>();
            getCatalogueEditorParamMetadatas.Add(new XEditorParamMetadata("templateId", "int", "模板Id", null));
            getCatalogueMetadata.MetadataParams = getCatalogueEditorParamMetadatas;
            metadatas.Add(getCatalogueMetadata);



            HttpContext.Response.Clear();
            HttpContext.Response.ContentType = "text/xml";
            HttpContext.Response.Write(ToXmlString(metadata));
            HttpContext.Response.Flush();
        }

        private string ToXmlString(object obj)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            MemoryStream ms = new MemoryStream();
            serializer.Serialize(ms, obj);
            return System.Text.Encoding.UTF8.GetString(ms.ToArray());
        }

        public class XEditorServiceMetadata
        {
            public List<XEditorMetadata> Metadatas { get; set; }
        }

        public class XEditorMetadata
        {
            public string Method { get; set; }
            public string Url { get; set; }
            public string Description { get; set; }

            public List<XEditorParamMetadata> MetadataParams { get; set; }

        }

        public class XEditorParamMetadata
        {
            public XEditorParamMetadata()
            {
                
            }
            public XEditorParamMetadata(string name,string dataType,string description,string dataFormat)
            {
                Name = name;
                DataType = dataType;
                DataFormat = dataFormat;
                Description = description;
            }
            public string Name { get; set; }
            public string DataType { get; set; }
            public string DataFormat { get; set; }
            public string Description { get; set; }
        }
    }
}