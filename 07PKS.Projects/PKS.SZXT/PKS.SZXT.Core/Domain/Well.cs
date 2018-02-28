using PKS.SZXT.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.SZXT.Core.Domain
{
    public abstract class Well
    {
        public string Id { get; set; }
        public abstract IEnumerable<SearchResultItem> GetWellNearby();
    }
}
