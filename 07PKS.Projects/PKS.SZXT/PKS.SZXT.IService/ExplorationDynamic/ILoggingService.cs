using PKS.SZXT.IService.Common;
using System;

namespace PKS.SZXT.IService.ExplorationDynamic
{
    public interface ILoggingService: IViewService,IWellNearbyAnalysis
    {
        object GetLoggingDiscovery();
        object GetLoggingDynamic(DateTime? date);
        object GetMontage(string wellId);
        object GetLoggingDraft(string wellId);
        object GetOilGasForm(string wellId);
        object GetDrillingGeoDailyReport(string wellId,DateTime? date);
    }
}
