using PKS.SZXT.IService.Common;
using System;

namespace PKS.SZXT.IService.ExplorationDynamic
{
    public interface IDetectionService:IViewService,IWellNearbyAnalysis
    {
        object GetDetectionInformation();
        object GetDetectionInformation(DateTime date);
        object GetPrimaryExplationForm(string wellId);
        object GetPrimaryExplationPicture(string wellId);
    }
}
