using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PKS.PageEngine.Param;

namespace PKS.PageEngine.Query
{
    public interface IQueryPlanTranslator
    {
        string Translate(QueryPlan queryPlan, List<QueryOutputParam> outputParams);
    }
}
