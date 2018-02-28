using PKS.SZXT.Web.Config.Model;
using PKS.SZXT.Web.Config.PageConfig;
using System.Collections.Generic;
using System.Web;
using System.Web.Script.Serialization;
using System.Linq;
using System.IO;
using static System.Web.Configuration.WebConfigurationManager;
using static Newtonsoft.Json.JsonConvert;
using System.Threading.Tasks;

namespace PKS.SZXT.Web.Config.PageSearchService
{
    public class PageSearchService:ConfigService
    {
        private readonly static char[] splitor = new char[] { ',', ';' };
        public static HttpServerUtility Server { get; private set; }
        public static Dictionary<string, Dictionary<string,string>> PageSearchConfigDic { get; private set; }
        public new static void Run(HttpApplication app)
        {
            Server = app.Server;
            PageSearchConfigDic = GetRequestQueryDic();
        }
        private static Dictionary<string,Dictionary<string,string>>  GetRequestQueryDic()
        {
            var sec = GetPageSearchConfigFromMananger();
            var t1 = Task.Run(() => GetRequestFileMap(sec));
            var t2 = Task.Run(() => GetFileQueryMap(sec));
            Task.WaitAll(t1, t2);
            return BuildRequestQueryDic(t1.Result, t2.Result);
        }
        private static Dictionary<string, string> GetRequestFileMap(PageSearchConfigSection sec)
        {
            var res = new Dictionary<string, string>();
            var configItems = sec.PageSearchConfigs;
            foreach (PageSearchConfigElement o in configItems)
            {
                foreach (var key in BuildSearch(o))
                {
                    res.Add(key, o.FileName);
                }
            }
            return res;
        }
        private static Dictionary<string, Dictionary<string, string>> GetFileQueryMap(PageSearchConfigSection sec)
        {
            var basePath = sec.ConfigFileBasePath;
            var configItems = sec.PageSearchConfigs;
            return  GetFileQueryMap(basePath, configItems);
        }
        private static  Dictionary<string, Dictionary<string, string>> GetFileQueryMap(string basePath, PageSearchConfigCollection configItems)
        {
            var res = new Dictionary<string, Dictionary<string, string>>();
            var filePath = string.Empty;
            var tasks = new List<Task<KeyValuePair<string, Dictionary<string, string>>>>();
            foreach (PageSearchConfigElement o in configItems)
            {
                tasks.Add(Task.Run(() => GetFileQuery(basePath, o.FileName)));
            }
            Task.WaitAll(tasks.ToArray());
            foreach (var t in tasks)
            {
                res.Add(t.Result.Key, t.Result.Value);
            }
            return res;
        }

        private static KeyValuePair<string,Dictionary<string,string>> GetFileQuery(string basePath, string fileName)
        {
            var filePath = GetPath(basePath, fileName);
            var fileContent = ReadFileContentAsync(filePath);
            var queries = GetQueryDic(fileContent);
            return new KeyValuePair<string, Dictionary<string, string>>(fileName, queries);
        }
        private static string ReadFileContentAsync(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"页面搜索条件配置节中配置的文件{ filePath }不存在");
            return File.ReadAllText(filePath);
        }
        private static Dictionary<string, string> GetQueryDic(string fileContent)
        {
            var res = new Dictionary<string, string>();
            var lst = DeserializeObject<List<SearchItem>>(fileContent);
            foreach (var o in lst)
            {
                res.Add(o.Grid, SerializeObject(o.Query));
            }
            return res;
        }
        /// <summary>
        /// 从Web.config中获取页面的搜索配置
        /// </summary>
        /// <returns></returns>
        public static PageSearchConfigSection GetPageSearchConfigFromMananger()
        {
            var secName = GetPageSearchConfigSecName();
            var sec = GetSection(secName);
            if (sec == null)
                throw new System.Exception($"Web.config文件中{secName}配置节不存在");
            return sec as PageSearchConfigSection;
        }
        /// <summary>
        /// 生成http请求与es查询条件的字典
        /// </summary>
        /// <param name="searchConfigDic">Http请求与搜索配置文件</param>
        /// <param name="queryDic">Es查询字典</param>
        /// <returns></returns>
        private static Dictionary<string, Dictionary<string, string>> BuildRequestQueryDic(Dictionary<string, string> searchConfigDic,
                                                                     Dictionary<string, Dictionary<string, string>> queryDic)
        {
            var res = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, string> esQuery;
            foreach (var o in searchConfigDic)
            {
                esQuery = queryDic[o.Value];
                res.Add(o.Key, esQuery);
            }
            return res;
        }
        /// <summary>
        /// 生成搜索与配置文件键值对
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private static IEnumerable< string> BuildSearch(PageSearchConfigElement o)
        {
            return o.ActionName
                    .Split(splitor)
                    .Select(s => GetUniformName(o.ControllerName, s));
        }
        /// <summary>
        /// 获取页面搜索配置节点名称
        /// </summary>
        /// <returns></returns>
        private static string GetPageSearchConfigSecName()
        {
            return ConfigSection.PageSearchConfig.ToString();
        }
        /// <summary>
        /// 生成文件对应服务器路径
        /// </summary>
        /// <param name="basePath"></param>
        /// <param name="relative"></param>
        /// <returns></returns>
        private static string GetPath(string basePath, string relative)
        {
            var path = Path.Combine(basePath, relative);
            return Server.MapPath(path);
        }
        /// <summary>
        /// 将controller名称与action名称组合生成统一查询名称
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="actionName"></param>
        /// <param name="splitor"></param>
        /// <returns></returns>
        private static string GetUniformName(string controllerName, string actionName,string splitor = ".")
        {
            return $"{controllerName}{splitor}{actionName}";
        }
        public Dictionary<string, string> GetPageFilterByRoute(string controllerName, string actionName)
        {
            var key = GetUniformName(controllerName, actionName);
            var res = PageSearchConfigDic
                      .FirstOrDefault(p => p.Key == key)
                      .Value;
            //如果不能精确匹配,则进行模糊匹配
            if(res == null)
                res = PageSearchConfigDic
                      .FirstOrDefault(p => p.Key.ToLower() == key.ToLower())
                      .Value;
            if (res == null)
                throw new KeyNotFoundException($"找不到{key}对应的查询配置");
            return res;
        }
    }
}
