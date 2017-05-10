using System.Threading.Tasks;
using BabyBus.Models;
using BabyBus.Models.Account;
using BabyBus.Services;

namespace BabyBus.Net.Login
{
    public interface ILoginService
    {

        //public async Task<ApiResponser> LoginNew(User crmUser)
        //{
        //    var response = await _restHeler.AsyncUpdate(Constants.LoginNew, crmUser);
        //    return response;
        //}
        //Task<ApiResponser> Login(BabyBus.Model.User crmUser);

        Task<ApiResponser> Login(User crmUser);

        Task<ApiResponser> Register(User crmUser);

        Task<ApiResponser> Checkout(CheckoutModel check);

        Task<User> GetUserByLoginName(string loginName);

        Task<ApiResult<CityModel>> GetAllCities();
//		Task<ApiResult1> GetAllCities1();
        Task<ApiResult<KindergartenModel>> GetKindergartenByCity(string city);
        Task<ApiResult<KindergartenClassModel>> GetClassByKgId(int kgId);


    }
}
