using PKS.SZXT.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.SZXT.Core.Domain
{
    public class LoggingWell:Well
    {
        public static LoggingWell Create(string id)
        {
            return new LoggingWell(id);
        }
        public LoggingWell() { }
        private LoggingWell(string id)
        {
            this.Id = id;
        }
        public SearchResultItem GetMontage()
        {
            throw new NotImplementedException();
        }
        public SearchResultItem GetLoggingDraft()
        {
            throw new NotImplementedException();
        }
        public SearchResultItem GetOilGasForm()
        {
            throw new NotImplementedException();
        }
        public SearchResultItem GetDrillingGeoDailyReport()
        {
            throw new NotImplementedException();
        }
        public override IEnumerable<SearchResultItem> GetWellNearby()
        {
            throw new NotImplementedException();
        }
    }
}
