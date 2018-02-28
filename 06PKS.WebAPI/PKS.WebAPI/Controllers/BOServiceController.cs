//using System.Collections.Generic;
//using System.IO;
//using System.Threading.Tasks;
//using System.Web.Http;
//using System.Xml;
//using PKS.Utils;
//using PKS.Web;
//using PKS.WebAPI.Models;
//using PKS.WebAPI.Services;

//namespace PKS.WebAPI.Controllers
//{
//    /// <summary>对象服务控制器</summary>
//    public class BOServiceController : PKSApiController
//    {
//        /// <summary>构造函数</summary>
//        public BOServiceController(IBOService service)
//        {
//            this.ServiceImpl = service;
//        }
//        /// <summary>服务实例</summary>
//        private IBOService ServiceImpl { get; }
//        /// <summary>
//        /// 根据业务对象正式名或别名以及业务对象类型获取正式业务实体信息
//        /// </summary>
//        /// <param name="name">业务对象正式名或别名</param>
//        /// <param name="bot">业务对象类型</param>
//        /// <returns>如果找到，返回<c>BO</c>对象，否则返回<c>null</c></returns>
//        [HttpGet]
//        public BO GetBOByName(string name, string bot)
//        {
//            return ServiceImpl.GetBOByName(name, bot);
//        }

//        /// <summary>
//        /// 根据业务对象Id获取正式业务实体信息
//        /// </summary>
//        /// <param name="boid">业务对象Id</param>
//        /// <returns>如果找到，返回<c>BO</c>对象，否则返回<c>null</c></returns>
//        [HttpGet]
//        public BO GetBOById(string boid)
//        {
//            return ServiceImpl.GetBOById(boid);
//        }

//        /// <summary>
//        /// 根据业务对象别名和应用域获取正式业务对象信息
//        /// </summary>
//        /// <param name="alias">业务对象别名</param>
//        /// <param name="appdomain">业务域</param>
//        /// <returns>如果找到，返回<c>BO</c>对象，否则返回<c>null</c></returns>
//        [HttpGet]
//        public BO GetBOByAlias(string alias, string appdomain)
//        {
//            return ServiceImpl.GetBOByAlias(alias, appdomain);
//        }

//        /// <summary>
//        /// 根据业务对象ID获取业务对象别名
//        /// </summary>
//        /// <returns>别名列表，<c>Alias</c>集合</returns>
//        [HttpPost]
//        public AliasCollection GetBOAliasByID(GetBOAliasRequest request)
//        {
//            var boid = request.BOID;
//            var appdomain = request.AppDomain.ToArray();
//            return ServiceImpl.GetBOAliasByID(boid, appdomain);
//        }

//        /// <summary>
//        /// 根据业务对象ID和数据分类获取3GX数据
//        /// </summary>
//        /// <param name="boid">业务对象ID</param>
//        /// <param name="category"><c>GGGXDataCategory</c>对象，要返回的数据类别</param>
//        /// <returns><c>XmlDocument</c>的一个实例，该实例符合3GX数据规范</returns>
//        [HttpGet]
//        public IHttpActionResult Get3GXById(string boid, GGGXDataCategory category)
//        {
//            var result = ServiceImpl.Get3GXById(boid, category);
//            return new ApiStreamResult(MimeTypeConst._3GX, result.ToStream(), this);
//        }

//        /// <summary>
//        /// 获取指定对象附近的临近对象
//        /// </summary>
//        /// <returns><c>NearBOCollection</c>实例</returns>
//        [HttpPost]
//        public NearBOCollection GetNearBOById(GetNearBOByIdRequest request)
//        {
//            var boid = request.BOID;
//            var distance = request.Distance;
//            var bot = request.BOT;
//            var filter = FilterToJson(request.Filter);
//            return ServiceImpl.GetNearBOById(boid, distance, bot, filter);
//        }

//        /// <summary>
//        /// 获取指定点坐标附近的临近对象
//        /// </summary>
//        /// <returns><c>NearBOCollection</c>实例</returns>
//        [HttpPost]
//        public NearBOCollection GetNearBOByLocation(GetNearBOByLocationRequest request)
//        {
//            var spaceLoaction = request.SpaceLocation;
//            var distance = request.Distance;
//            var bot = request.BOT;
//            var filter = FilterToJson(request.Filter);
//            return ServiceImpl.GetNearBOByLocation(spaceLoaction, distance, bot, filter);
//        }

//        /// <summary>
//        /// 获取指定业务对象名称/类型附近的临近对象
//        /// </summary>
//        /// <returns><c>NearBOCollection</c>实例</returns>
//        [HttpPost]
//        public NearBOCollection GetNearBOByName(GetNearBOByNameRequest request)
//        {
//            var boName = request.BOName;
//            var boType = request.BOType;
//            var distance = request.Distance;
//            var bot = request.BOT;
//            var filter = FilterToJson(request.Filter);
//            return ServiceImpl.GetNearBOByName(boName, boType, distance, bot, filter);
//        }

//        /// <summary>
//        /// 获取指定业务对象类型/类别的业务对象
//        /// </summary>
//        /// <returns>业务对象集合<c>BOCollection</c></returns>
//        [HttpPost]
//        public BOCollection GetBOListByBOT(GetBOListByBOTRequest request)
//        {
//            var bot = request.BOT;
//            var filter = FilterToJson(request.Filter);
//            return ServiceImpl.GetBOListByType(bot, filter);
//        }

//        /// <summary>
//        /// 获取指定业务对象类型和符合属性过滤条件的业务对象
//        /// </summary>
//        /// <returns>业务对象集合<c>BOCollection</c></returns>
//        [HttpPost]
//        public BOCollection GetBOListByFilter(GetBOListByFilterRequest request)
//        {
//            var bot = request.BOT;
//            var bbox = request.BBox;
//            var filter = FilterToJson(request.Filter);
//            return ServiceImpl.GetBOListByFilter(bot, bbox, filter);
//        }

//        /// <summary>
//        /// 根据业务对象树模板获取业务对象列表
//        /// </summary>
//        /// <param name="template">树模板<c>BOTreeTemplate</c></param>
//        /// <returns>业务对象集合<c>BOCollection</c></returns>
//        [HttpPost]
//        public TreeBOCollection GetBOTree(BOTreeTemplate template)
//        {
//            return ServiceImpl.GetBOTree(template);
//        }
//        /// <summary>
//        /// 获得对象词库
//        /// </summary>
//        [HttpGet]
//        public TermBOCollection GetCCTermOfBO()
//        {
//            return ServiceImpl.GetCCTermOfBO();
//        }

//        /// <summary>
//        /// 根据业务对象类型和应用域获取属性定义信息
//        /// </summary>
//        /// <param name="request">请求参数</param>
//        /// <returns>返回属性定义集合</returns>
//        [HttpPost]
//        public List<PropertySchema> GetPropertySchema(GetPropertySchemaRequest request)
//        {
//            var bot = request.BOT;
//            var appdomain = request.AppDomain;
//            var names = request.Names;
//            return ServiceImpl.GetPropertySchema(bot, appdomain, names);
//        }

//        /// <summary>
//        /// 根据filter获得3GX数据
//        /// </summary>
//        [HttpPost]
//        public IHttpActionResult Get3GXByFilter(Get3GxByFilterRequest request)
//        {
//            var result = ServiceImpl.Get3GXByFilter(request.BOT, request.BOs, FilterToJson(request.Filter), request.Category);
//            return new ApiStreamResult(MimeTypeConst._3GX, result.ToStream(), this);
//        }

//        private string FilterToJson(object objFilter)
//        {
//            var filter = string.Empty;
//            if (objFilter != null && objFilter.ToJson() != "{}")
//                filter = objFilter.ToJson();
//            return filter;
//        }

//        /// <summary>获得服务信息</summary>
//        protected override ServiceInfo GetServiceInfo()
//        {
//            var dbInfo = ServiceImpl.GetDbInfo();
//            return new BOServiceInfo()
//            {
//                CRS = dbInfo.CRS,
//                CSParam = dbInfo.CSParam,
//                DBSerName = dbInfo.DBSerName,
//                Description = "对象服务"
//            };
//        }
//        /// <summary>获得服务能力数据</summary>
//        protected override async Task<ServiceCapabilities> GetServiceCapabilities()
//        {
//            var appdomains = ServiceImpl.GetAppDomains();
//            return await Task.FromResult(new BOServiceCapabilities { AppDomains = appdomains });
//        }
//    }
//}
