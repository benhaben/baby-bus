using System;
using AdministratorManagement.Models;
using BabyBus.AutoModel;

namespace AdministratorManagement.Core.DataAccess
{
    public interface IUserRepository : IRepository<Admin, Guid>
    {
        Admin FindByUsername(string username);

        UV_Teacher FindByUserAppName(string username);
    }
}