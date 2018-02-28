using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jurassic.GeoFeature.Model;

namespace Jurassic.GeoFeature.IDAL
{
    public interface IPendingReview:IInterface<PendingReviewModel>
    {
        IList<PendingReviewModel> GetNotCheckedList();
    }
}
