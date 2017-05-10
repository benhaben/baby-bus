using System;
using Cirrious.CrossCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using BabyBusSSApi.ServiceModel.Enumeration;
using BabyBusSSApi.ServiceModel.DTO.Create;
using BabyBusSSApi.ServiceModel.DTO.Create.Report;

namespace BabyBus.Logic.Shared
{
    /// <summary>
    /// statistic utilities.
    /// </summary>
    public static class StatsUtils
    {
        private static readonly IRemoteService _service;


        static StatsUtils()
        {
            _service = Mvx.Resolve<IRemoteService>();
        }

      
        public static void LogPageReport(PageReportType type)
        {
            var report = new CreatePageReport
            {
                UserId = BabyBusContext.UserId,
                PageReportType = type,
                CreateDate = DateTime.Now
            };
            
            Task.Run(async () =>
                {
                    try
                    {
                        await _service.UploadPageReport(report);
                    }
                    catch (Exception ex)
                    {
                        Xamarin.Insights.Report(ex);
                    }
                });
					
        }
    }
}

