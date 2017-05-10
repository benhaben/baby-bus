using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BabyBus.Logic.Shared
{
    public class StatsService:IStatsService
    {
        private RestHelper restHelper = new RestHelper();

        public StatsService()
        {
        }

        #region IStatsService implementation

        public async Task<ApiResponser> EnterLog(UserReport report)
        {
            var response = await restHelper.AsyncUpdate(Constants.UserReport, report);
            return response;
        }


        public async Task<ApiResponser> UploadPageReport(IList<PageReport> list)
        {
            var response = await restHelper.AsyncUpdate(Constants.PageReport, list);
            return response;
        }

        #endregion
    }
}

