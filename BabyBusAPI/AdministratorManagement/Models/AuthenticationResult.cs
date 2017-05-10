using BabyBus.AutoModel;

namespace AdministratorManagement.Models
{
    public class AuthenticationResult
    {
        public bool IsAuthenticated { get; set; }
        public Admin UserLogin { get; set; }

//        public User UserAppLogin { get; set; }

    }
}