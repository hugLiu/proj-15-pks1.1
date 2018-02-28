using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juarssic.Server.Comm
{
    public class MongoCode
    {
        public static IDictionary<string, string> m_MongoCodeDic = new Dictionary<string, string>() { 
        { "$gt", ">" },
        { "$lt", "<" },
        { "$gte", ">=" },
        { "$lte", "<=" },
        { "$eq", "=" },
        { "$ne", "!=" }
        };
    }
}
