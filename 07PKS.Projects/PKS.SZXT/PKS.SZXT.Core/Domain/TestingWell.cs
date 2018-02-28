using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PKS.SZXT.Core.DTO;

namespace PKS.SZXT.Core.Domain
{
    public class TestingWell : Well
    {
        public static TestingWell Create(string id)
        {
            return new TestingWell(id);
        }
        public TestingWell() { }
        private TestingWell(string id)
        {
            Id = id;
        }
        public SearchResultItem GetOilGasDetectGeoDesignReport()
        {
            throw new NotImplementedException();
        }
        public SearchResultItem GetOilGasDetectProjDesignReport()
        {
            throw new NotImplementedException();
        }
        public SearchResultItem GetFormationTestingProductData()
        {
            throw new NotImplementedException();
        }
        public SearchResultItem GetTestingGeoDailyReport()
        {
            throw new NotImplementedException();
        }
        public override IEnumerable<SearchResultItem> GetWellNearby()
        {
            throw new NotImplementedException();
        }
    }
}
