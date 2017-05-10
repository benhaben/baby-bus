using AdministratorManagement.Models;
using BabyBus.AutoModel;

namespace AdministratorManagement.Core.Services
{
    public interface IAuthenticationService
    {
        AuthenticationResult Authenticate(Admin loginModel);
    }
}