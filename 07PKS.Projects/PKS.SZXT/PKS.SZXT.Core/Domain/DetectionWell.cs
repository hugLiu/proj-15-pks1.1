using PKS.SZXT.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.SZXT.Core.Domain
{
    public class DetectionWell:Well
    {
        public static DetectionWell Create(string id)
        {
            return new DetectionWell(id);
        }
        public DetectionWell() { }
        private DetectionWell(string id)
        {
            Id = id;
        }
        public SearchResultItem GetPrimaryExplationForm()
        {
            throw new NotImplementedException();
        }
        public SearchResultItem GetPrimaryExplationPicture()
        {
            throw new NotImplementedException();
        }
        public override IEnumerable<SearchResultItem> GetWellNearby()
        {
            throw new NotImplementedException();
        }
    }
}
