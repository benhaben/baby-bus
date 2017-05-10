using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyBus.Helpers;
using BabyBus.Models.Communication;
using BabyBus.Models.Enums;
using BabyBus.Net.Communication;
using BabyBus.Services;
using BabyBus.Utilities;
using Cirrious.CrossCore;

namespace BabyBus.Manager
{
    public static class UpdateManager
    {
        static UpdateManager() {
            IsUpdating = false;
        }

        public static bool IsUpdating { get; set; }

        public static event EventHandler UpdateAllFinished;

        public async static void UpdateAll() {
            var user = BabyBusContext.UserAllInfo;
            if (user == null)
                return;
            Debug.WriteLine("###Start Update all data from Web API###");
            var service = Mvx.Resolve<INoticeService>();
            
            IsUpdating = true;
            //Notice
            try
            {
                //Local DB MaxId
                var list = BabyBusContext.NoticeList;
                long maxId = 0;
                if(list.Count > 0)
                    maxId = list.OrderByDescending(x => x.NoticeId).FirstOrDefault().NoticeId;
                var result = await service.GetNewNoticeList(user, NoticeViewType.Notice,maxId);
                if (result.Status) {
                    BabyBusContext.InsertAll(result.Items);
                }
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.Message);
            }

            //GrowMemory
            try {
                //Local DB MaxId
                var list = BabyBusContext.GrowMemoryList;
                long maxId = 0;
                if (list.Count > 0)
                    maxId = list.OrderByDescending(x => x.NoticeId).FirstOrDefault().NoticeId;
                var result = await service.GetNewNoticeList(user, NoticeViewType.GrowMemory, maxId);
                if (result.Status) {
                    BabyBusContext.InsertAll(result.Items);
                }
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.Message);
            }
            if (UpdateAllFinished != null)
                UpdateAllFinished(null, null);
            Debug.WriteLine("###End Update all data from Web API###");
            IsUpdating = false;
        }
    }
}
