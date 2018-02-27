using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PKS.PageEngine.Query;
using PKS.PageEngine.View;

namespace PKS.PageEngine.EvenHandlers
{
    public class ComponentsEventArg:EventArgs
    {
        public object Data { get; set; }
        public List<ComponentQueryInfo> QueryInfos { get; set; }
    }
}
