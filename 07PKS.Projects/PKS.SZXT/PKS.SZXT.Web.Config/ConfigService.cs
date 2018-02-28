using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Compilation;

namespace PKS.SZXT.Web.Config
{
    public class ConfigService
    {
        private static readonly string _entry = "Run";
        public static void Run(HttpApplication app)
        {
            var asms = BuildManager.GetReferencedAssemblies();
            foreach (Assembly asm in asms)
            {
                 asm.GetTypes()
                    .Where(t => t.IsSubclassOf(typeof(ConfigService)))
                    .ToList()
                    .ForEach(t =>
                    {
                        t.GetMethod(_entry).Invoke(null, new object[] { app });
                    });
            }
        }
    }
}
