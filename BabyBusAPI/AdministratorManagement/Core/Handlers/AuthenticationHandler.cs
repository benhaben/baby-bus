using System;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web.Http;
using AdministratorManagement.Controllers;
using AdministratorManagement.Core.DataAccess;

namespace AdministratorManagement.Core.Handlers
{
    public class AuthenticationHandler : DelegatingHandler, IDelegatingHandler
    {
        private readonly IAccessTokenRepository _accessTokenRepository;
        private readonly IUserRepository _userRepository;
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(AuthenticationHandler));

        public AuthenticationHandler()
            //(IAccessTokenRepository accessTokenRepository, IUserRepository userRepository)
        {
            _accessTokenRepository = Bootstrapper.GetService<IAccessTokenRepository>();
            _userRepository = Bootstrapper.GetService<IUserRepository>();
            //_accessTokenRepository = accessTokenRepository;
            //_userRepository = userRepository;
        }

        protected override System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            var content = request.Content;
            string jsonContent = content.ReadAsStringAsync().Result;

            var accessToken = request.Headers.GetCookies("Token");
            if (accessToken.Count == 0)
            {
                Log.Error("Do not have Cookie token!!!");
                return base.SendAsync(request, cancellationToken);

            }

            var tokenValue = accessToken[0]["Token"].Value;
            var token = _accessTokenRepository.FindById(tokenValue);
            if (token == null)
            {
                Log.Error("Do not have Cookie token's value!!!");
                return base.SendAsync(request, cancellationToken);
            }

            var user = _userRepository.FindById(token.UserId);
            if (user == null)
            {
                Log.Error("Do not have the user!!!");
                return base.SendAsync(request, cancellationToken);
            }
            var identity = new GenericIdentity(user.UserName, "Basic");
            
            var principal = new GenericPrincipal(identity,new string[]{ user.Role});
            Thread.CurrentPrincipal = principal;

            if (System.Web.HttpContext.Current != null)
            {
                System.Web.HttpContext.Current.User = principal;
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}