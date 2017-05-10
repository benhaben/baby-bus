using System.Threading.Tasks;
using System.Collections.Generic;


namespace BabyBus.Logic.Shared
{
	public interface ILoginService
	{

		//public async Task<ApiResponser> LoginNew(User crmUser)
		//{
		//    var response = await _restHeler.AsyncUpdate(Constants.LoginNew, crmUser);
		//    return response;
		//}
		//Task<ApiResponser> Login(BabyBus.Model.User crmUser);

		Task<ApiResponser> Login (User crmUser);

		Task<ApiResponser> Register (User crmUser);

		Task<ApiResponser> Checkout (CheckoutModel check);

		Task<User> GetUserByLoginName (string loginName);

		Task<List<CityModel>> GetAllCities ();
		//		Task<ApiResult1> GetAllCities1();
		Task<List<KindergartenModel>> GetKindergartenByCity (string city);

		Task<List<KindergartenClassModel>> GetClassByKgId (int kgId);


	}
}