using AdministratorManagement.Core.DataAccess;
using AdministratorManagement.Core.Services;
using AdministratorManagement.Models;
using BabyBus.AutoModel;
using BabyBus.Core.Helper;
using System;
using System.Web.Http;
using System.Web.OData.Query;
using System.Web;
using System.Web.SessionState;


namespace AdministratorManagement.Controllers
{
    public class AdminLoginController : ApiController
    {

        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(AdminLoginController));


        private readonly IAuthenticationService _authenticationService;
        private readonly IAccessTokenRepository _accessTokenRepository;

        public AdminLoginController(IAuthenticationService authenticationService, IAccessTokenRepository accessTokenRepository)
        {
            _authenticationService = authenticationService;
            _accessTokenRepository = accessTokenRepository;
        }


        [System.Web.Http.HttpPost]
        //TODO: just test , rrmove Complex
        //[System.Web.Http.ActionName("Complex")]
        public ApiResponser PostComplex(Admin loginModel)
        {
            var response = new ApiResponser(true);
            try
            {
                var result = _authenticationService.Authenticate(loginModel);
                if (!result.IsAuthenticated)
                {
                    response.Attach = null;
                }
                else
                {
                    var token = new AccessToken(result.UserLogin.Guid);
                    
                    //可以保存在session
                    //add record for app user in memory, if app user change password,will have many records
                    _accessTokenRepository.Save(token);
                    //response.Attach = token;

                    var userInfo = result.UserLogin;

                    response.Attach = new
                    {
                        Token = token,
                        UserInfo = userInfo,
                    };
                    
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
                Log.Fatal("login failure", ex);
                return response;
            }

            return response;
        }
        [System.Web.Http.Authorize]
        public ApiResult<UserLogin> Get(ODataQueryOptions<Child> options)
        {
            //HttpContext.Current.Session["a"] = "aa";
            var response = new ApiResult<UserLogin>();
            response.Status = true;
            return response;
        }

    }
}