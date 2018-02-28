using System.Web;

namespace PKS.SZXT.Web.Config.Model
{
    public static class AppStateHelper
    {
      
        public static object Get(string key,object defaultValue=null)
        {
            if (HttpRuntime.Cache[key] == null&& defaultValue != null)
            {
                HttpRuntime.Cache[key] = defaultValue;
            }
            return HttpRuntime.Cache[key];
        }
        public static object Set(string key, object value)
        {
            return HttpRuntime.Cache[key] = value;
        }
    }
}
