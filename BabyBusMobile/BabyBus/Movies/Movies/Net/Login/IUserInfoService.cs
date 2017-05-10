using BabyBus.Models;
using BabyBus.Helpers;
using BabyBus.Models.Account;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BabyBus.Services
{
    public interface IUserInfoService
    {
        Task<ApiResponser> UpdateUserInfo(CheckoutModel model);

    }
}
