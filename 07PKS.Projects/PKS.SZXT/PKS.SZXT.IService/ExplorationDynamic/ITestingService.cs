using PKS.SZXT.IService.Common;
using System;

namespace PKS.SZXT.IService.ExplorationDynamic
{
    public interface ITestingService: IViewService,IWellNearbyAnalysis
    {
        object GetOilGasTesting();
        object GetTestingDynamic(DateTime? date);
        object GetOilGasDetectGeoDesignReport(string wellId);
        object GetOilGasDetectProjDesignReport(string wellId);
        object GetFormationTestingProductData(string wellId);
        object GetTestingGeoDailyReport(string wellId,DateTime? date);
    }
}
