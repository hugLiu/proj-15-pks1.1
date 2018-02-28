using PKS.SZXT.IService.Common;
using System;

namespace PKS.SZXT.IService
{
    public interface IDrillingService: IViewService,IWellNearbyAnalysis
    {
        object GetComplexWells();
        object GetDynamic(DateTime? date);
        object GetWellBaseForm(string wellId);
        object GetWellGeoDesignReport(string wellId);
        object GetWellProjDesignReport(string wellId);
        object GetWellDesignConstruct(string wellId);
        object GetWellActualConstruct(string wellId);
        object GetDrillingProjArgument(string wellId);
        object GetWellDailyReport(string wellId,DateTime? date);
    }
}
