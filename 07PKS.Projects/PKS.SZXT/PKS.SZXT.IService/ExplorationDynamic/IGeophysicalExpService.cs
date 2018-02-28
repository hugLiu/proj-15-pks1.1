using PKS.SZXT.IService.Common;
using System;

namespace PKS.SZXT.IService.ExplorationDynamic
{
    public interface IGeophysicalExpService: IViewService
    {
        object GetEarthquakeSampling();
        object GetSamplingDynamic(DateTime? date);
        object GetEarthquakeSamplingDesignReport(string swa,DateTime? date);
        object GetEarthquakeSamplingBaseForm(string swa, DateTime? date);
        object GetEarthquakeSamplingAreaPositionPicture(string swa, DateTime? date);
        object GetEarthquakeSamplingDailyReport(string swa, DateTime? date);
    }
}
