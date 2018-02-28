using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PKS.SZXT.Core.DTO;

namespace PKS.SZXT.Core.Domain
{
    public class DrillingWell:Well
    {
        public static DrillingWell Create(string id)
        {
            return new DrillingWell(id);
        }
        public DrillingWell() { }
        private DrillingWell(string id)
        {
            Id = id;
        }
        public virtual SearchResultItem GetBaseForm()
        {
            throw new NotImplementedException();
        }
        public virtual SearchResultItem GetGeoDesignReport()
        {
            throw new NotImplementedException();
        }
        public virtual SearchResultItem GetProjDesignReport()
        {
            throw new NotImplementedException();
        }
        public virtual SearchResultItem GetDesignBodyConstruct()
        {
            throw new NotImplementedException();
        }
        public virtual SearchResultItem GetActualBodyConstruct()
        {
            throw new NotImplementedException();
        }
        public virtual SearchResultItem GetDrilingFluidPerformanceForm()
        {
            throw new NotImplementedException();
        }
        public virtual SearchResultItem GetDrillingArgumentForm()
        {
            throw new NotImplementedException();
        }
        public virtual SearchResultItem GetDrillProgramForm()
        {
            throw new NotImplementedException();
        }
        public virtual SearchResultItem GetWellDeflectionForm()
        {
            throw new NotImplementedException();
        }
        public virtual SearchResultItem GetDailyReport(DateTime date)
        {
            throw new NotImplementedException();
        }
        public override IEnumerable<SearchResultItem> GetWellNearby()
        {
            throw new NotImplementedException();
        }
    }
}
