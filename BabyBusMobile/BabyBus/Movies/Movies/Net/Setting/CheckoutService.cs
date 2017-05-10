using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyBus.Helpers;
using BabyBus.Models;
using BabyBus.Models.Account;
using BabyBus.Services;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;
using Newtonsoft.Json;

namespace BabyBus.Net.Setting
{
    public class CheckoutService : ICheckoutService
    {
        private RestHelper restHeler = new RestHelper();
        

        public CheckoutService() {
            
        }

        public async Task<ApiResult<CheckoutModel>> GetCheckoutList(User user) {
            string url = string.Format(Constants.CheckoutList, user.KindergartenId, user.ClassId);
            string json = await restHeler.GetJsonString(url);
            var result = JsonConvert.DeserializeObject<ApiResult<CheckoutModel>>(json);
            if (result == null)
            {
                return new ApiResult<CheckoutModel> {
                    Status = false,
                    Message = "服务器返回的数据格式错误。"
                };
            }
            return result;
        }

        public async Task<ApiResponser> Checkout(CheckoutModel check) {
            var response = await restHeler.AsyncUpdate(Constants.Checkout, check);
            return response;
        }
    }
}