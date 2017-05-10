using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using BabyBus.AutoModel;
using BabyBus.Core.Helper;
using System.Web.SessionState;
using AdministratorManagement.Core;

namespace AdministratorManagement.Controllers
{
    public enum NoticeViewType
    {
        Notice = 1,
        GrowMemory,
        NoticeOnlyKg,
        All
    }
    
    [Authorize]
    public class NoticeController : BabyBusApiController
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(NoticeController));
       
        private RestHelper restHeler = new RestHelper();
        private async Task<ApiResponser> SendNotice(Notice model)
        {
            var response = await restHeler.AsyncUpdate(Constants.SendNotice, model);
            return response;
        }
        public async Task<ApiResponser> Post(Notice model)
        {
            var response = new ApiResponser(true);
            model.CreateTime = DateTime.Now;
            try
            {
               response = await  SendNotice(model);
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
                return response;
            }

            return response;
        }

    }
}