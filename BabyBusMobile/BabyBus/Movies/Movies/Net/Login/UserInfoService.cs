using BabyBus.Helpers;
using BabyBus.Models;
using BabyBus.Models.Account;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BabyBus.Services
{
    public class UserInfoService:IUserInfoService
    {
        private RestHelper _restHeler = new RestHelper();
        public async Task<ApiResponser> UpdateUserInfo(CheckoutModel model) {
            var response = await _restHeler.AsyncUpdate(Constants.UpdateUserInfo, model);
            return response;
        }
    }
}
