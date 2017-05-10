using BabyBus.Helpers;
using BabyBus.Models;
using BabyBus.Models.Account;
using BabyBus.Net.Login;
using Cirrious.CrossCore;

//using Cirrious.MvvmCross.Plugins.Messenger;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BabyBus.Services {
    class LoginService : ILoginService {
        private RestHelper _restHeler = new RestHelper();

        //public async Task<ApiResponser> Login(BabyBus.Model.User crmUser)
        //{
        //    var response = await _restHeler.AsyncUpdate(Constants.Login, crmUser);
        //    return response;
        //}


        public async Task<ApiResponser> Login(User crmUser) {
            var response = await _restHeler.AsyncUpdate(Constants.Login, crmUser);
            return response;
        }

        public async Task<User> GetUserByLoginName(string loginName) {
            string url = string.Format(Constants.UserInfoByLoginName, loginName);
            string json = await _restHeler.GetJsonString(url);
            var result = JsonConvert.DeserializeObject<ApiResult<User>>(json);
            if (result.Status && result.Items != null && result.Items.Count > 0) {
                var user = result.Items[0];
                return user;
            } else {
                return new User();
            }
        }

        public async Task<ApiResult<CityModel>> GetAllCities() {
            string url = string.Format(string.Format(Constants.Cities));
            string json = await _restHeler.GetJsonString(url);
            var result = JsonConvert.DeserializeObject<ApiResult<CityModel>>(json);
            if (result == null) {
                return new ApiResult<CityModel> {
                    Status = false,
                    Message = "服务器返回的数据格式错误。"
                };
            }
//			var result = await _restHeler.AsyncQuery<CityModel>(string.Format(Constants.Cities));
            return result;
        }
        //		public async Task<ApiResult1> GetAllCities1()
        //        {
        //			var result = await _restHeler.AsyncQuery1(string.Format(Constants.Cities));
        //            return result;
        //        }

        public async Task<ApiResult<KindergartenModel>> GetKindergartenByCity(string city) {
            string url = string.Format(Constants.Kindergartens, city);
            string json = await _restHeler.GetJsonString(url);
            var result = JsonConvert.DeserializeObject<ApiResult<KindergartenModel>>(json);
            if (result == null) {
                return new ApiResult<KindergartenModel> {
                    Status = false,
                    Message = "服务器返回的数据格式错误。"
                };
            }

//			var result = await _restHeler.AsyncQuery<KindergartenModel>();
            return result;
        }

        public async Task<ApiResult<KindergartenClassModel>> GetClassByKgId(int kgId) {
            string url = string.Format(string.Format(Constants.Classes, kgId));
            string json = await _restHeler.GetJsonString(url);
            var result = JsonConvert.DeserializeObject<ApiResult<KindergartenClassModel>>(json);
            if (result == null) {
                return new ApiResult<KindergartenClassModel> {
                    Status = false,
                    Message = "服务器返回的数据格式错误。"
                };
            }

//			var result = await _restHeler.AsyncQuery<KindergartenClassModel>();
            return result;
        }

        public async Task<ApiResponser> Register(User crmUser) {
            var response = await _restHeler.AsyncUpdate(Constants.Register, crmUser);
            return response;
        }

        public async Task<ApiResponser> Checkout(CheckoutModel check) {
            var response = await _restHeler.AsyncUpdate(Constants.Checkout, check);
            return response;
        }
    }
}
